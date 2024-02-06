using Terraria;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria.IO;
using Terraria.WorldBuilding;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections;

using Terraria.ID;
using Terraria.Audio;

namespace SuperSeedSearch.WorldGenMod
{
    public class WGPassChanger
    {

        public event Action onWorldGenStart;
        public event Action onWorldGenEnd;
        UI.UISuperSeed uiSuperSeed;
        public DataExtractor dataExtractor = null;
        public Analyser analyser = null;

        Random rng = new Random();

        ConditionScheduler conditionSched;

        public enum WGtype {normal =0, FloatingChestsOnly, PartialFloatingChestsMountCave};
        public List<WGtype> WGqueue = new List<WGtype>{WGtype.normal};
        public int WGQind = 0;

        public WGPassChanger(UI.UISuperSeed uiSuperSeed)
        {
            
            this.uiSuperSeed = uiSuperSeed;
            if(uiSuperSeed!=null){
                uiSuperSeed.condInterface.mainPropsPanel.OnSelected += (tab) => {uiSuperSeed.statsText.Show();if (uiSuperSeed.statsText.isEmpty()) uiSuperSeed.ShowTextPanel();else uiSuperSeed.ShowStatsPanel();};
                uiSuperSeed.condInterface.mainPropsPanel.OnDeSelected += (tab) => {uiSuperSeed.statsText.Clear();uiSuperSeed.ShowConstraintPanel();};
            }
            conditionSched = new ConditionScheduler();
            dataExtractor = new DataExtractor();
            analyser = new Analyser(conditionSched);
            Storage.LoadStoreConfigSeedsTxt.LoadConfig(ConstantsStrings.DefaultConfig,true);
            
        }

        public bool isGen = false;

        public void StartWorldGen()
        {
            if (isGen) return;
            
            isGen = true;
            uiSuperSeed.generationProgress = new GenerationProgress();
            
            conditionSched.Reset();//need to be for init WorldSettings
            InitWorldSettings();
            

            InitStats();    
            
            onWorldGenStart?.Invoke();
            Task.Factory.StartNew(new Action(StartWorldGenThread));
        }

        public int StartSeed;
        public int currentSeed;
        public string currentSeedAsText;
        internal string startingSeedAsText;
        internal bool customTextSeed = false;
        internal bool storedSeedFile = false;
        internal string[] seedList;

        internal bool expendTextSeedWithCount = false;

        int stepSize;
        int maxSeedCountStopAfter;
        int maxFoundSeedCountStopAfter;
        public bool stopWorldGen = false;

        public static class Stats{
                public static int lastSeedEnteredWorldGen = -1;
                public static int lastSeedFinishedWorldGen = -1;
                public static int seedsFound = 0;
                public static int seedsChecked = 0;
                public static Dictionary<WorldGenPass, int> wgStepPassed = null;

                public static void Reset(){
                    lastSeedEnteredWorldGen = -1;
                    lastSeedFinishedWorldGen = -1;
                    seedsFound = 0;
                    seedsChecked = 0;
                    wgStepPassed = new Dictionary<WorldGenPass, int>();
                }

        };
        bool fastMode = false;//1e7 in 25sec vs 31sec

        StoreAsPicture StoreWorldAsPicture;
        StoreWorldFile StoreWorldFile;
        StoreStatsInfo StoreWorldStatsInfo;
        
        public WorldSize worldSize;
        Difficulty difficulty;
        EvilType evilType;

        string worldName = "";

        private void InitWorldSettings()
        {
            worldName = WorldInfo.GetValueAsString(MainSettingEnum.WorldName);
            
            worldSize = Helpers.EnumHelper<WorldSize>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.WorldSize]);
            Main.maxTilesX = worldSize switch { WorldSize.small => 4200, WorldSize.medium => 6400, _ or WorldSize.large => 8400 };
            Main.maxTilesY = worldSize switch { WorldSize.small => 1200, WorldSize.medium => 1800, _ or WorldSize.large => 2400 };
            difficulty = Helpers.EnumHelper<Difficulty>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.Difficulty]);
            Main.GameMode = (int)difficulty;


            WGqueue = new List<WGtype>{WGtype.normal};
            WGQind = 0;
            if(WorldInfo.HasKeyValue( WGHacksEnum.WGHacksIsActive,  OnOffenable.On) ){
                    
                    int dummy = 0;
                    if(Int32.TryParse(WorldInfo.GetValueAsString(WGHacksEnum.OverwriteWorldTileSizeX), out dummy)) Main.maxTilesX = Int32.Parse(WorldInfo.GetValueAsString(WGHacksEnum.OverwriteWorldTileSizeX));
                    if(Int32.TryParse(WorldInfo.GetValueAsString(WGHacksEnum.OverwriteWorldTileSizeY), out dummy)) Main.maxTilesY = Int32.Parse(WorldInfo.GetValueAsString(WGHacksEnum.OverwriteWorldTileSizeY));
                    //if(Main.maxTilesX>8400 || Main.maxTilesY>2400);
                    if(WorldInfo.HasKey(WGHacksEnum.PartialWorldGenPreStep)) {
                        if(  WorldInfo.Info[WGHacksEnum.PartialWorldGenPreStep].Equals(Helpers.EnumHelper<PartialWorldGenPreStep>.GetNameOFEnumVariable(PartialWorldGenPreStep.FloatingIslandChestMountCave)) ){
                            WGqueue = new List<WGtype>{WGtype.normal, WGtype.PartialFloatingChestsMountCave};                            
                        }
                        else if(  WorldInfo.Info[WGHacksEnum.PartialWorldGenPreStep].Equals(Helpers.EnumHelper<PartialWorldGenPreStep>.GetNameOFEnumVariable(PartialWorldGenPreStep.FloatingIslandChestOnly)) ){
                            WGqueue = new List<WGtype>{WGtype.normal, WGtype.FloatingChestsOnly};                            
                        }else if(  WorldInfo.Info[WGHacksEnum.PartialWorldGenPreStep].Equals(Helpers.EnumHelper<PartialWorldGenPreStep>.GetNameOFEnumVariable(PartialWorldGenPreStep.FloatingIslandChest1stPos2nd)) ){
                            WGqueue = new List<WGtype>{WGtype.normal, WGtype.PartialFloatingChestsMountCave, WGtype.FloatingChestsOnly};                            
                        }
                        WGQind = WGqueue.Count-1;
                    }
            }
           
            stopWorldGen = false;

            double holder = 0;
            maxSeedCountStopAfter = (holder = Helpers.BasicFunctions.ParseAnyDoubleNumber(WorldInfo.Info[MainSettingEnum.StopSearchAfter].ToString()))<0?0:holder>Int32.MaxValue?Int32.MaxValue:(int)holder;
            maxFoundSeedCountStopAfter = (holder = Helpers.BasicFunctions.ParseAnyDoubleNumber(WorldInfo.Info[MainSettingEnum.StopSearchIfFound].ToString()))<1?1:holder>Int32.MaxValue?Int32.MaxValue:(int)holder;
            stepSize = (holder = Helpers.BasicFunctions.ParseAnyDoubleNumber(WorldInfo.Info[MainSettingEnum.NextSeedIsOldPlus].ToString()))<0?(holder<-Int32.MaxValue?-Int32.MaxValue:(int)holder):holder>Int32.MaxValue?Int32.MaxValue:(int)holder;
            
            StoreWorldFile = Helpers.EnumHelper<StoreWorldFile>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.StoreWorldFile]) ;
            string checkForOldValues = "";
            StoreWorldAsPicture = Helpers.EnumHelper<StoreAsPicture>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.StoreWorldMapAsPicture], true, out checkForOldValues) ;            
            if(checkForOldValues.Length>0){ WorldInfo.Info[MainSettingEnum.StoreWorldMapAsPicture] = checkForOldValues; uiSuperSeed?.condInterface?.mainPropsPanel.ChangeValue( MainSettingEnum.StoreWorldMapAsPicture, checkForOldValues );  }
            

            StoreWorldStatsInfo = Helpers.EnumHelper<StoreStatsInfo>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.StoreStatsInfo]);
            
            startingSeedAsText = WorldInfo.Info[MainSettingEnum.StartSeed].ToString();
            currentSeedAsText = (startingSeedAsText.Equals(ConstantsStrings.RandomStartingSeed))? Main.rand.Next(Int32.MaxValue).ToString() :startingSeedAsText;
            Main.ActiveWorldFileData.SetSeed(currentSeedAsText);
            StartSeed = Main.ActiveWorldFileData.Seed;
            currentSeed = StartSeed;
            currentSeedAsText = currentSeedAsText.ToString();
            
            
            storedSeedFile = false;
            expendTextSeedWithCount = false;
            customTextSeed = false;
            expendTextSeedWithCount = false;
            if(WorldInfo.GetValueAsString(MainSettingEnum.SeedReplaceText).Equals(Helpers.EnumHelper<SeedReplaceText>.GetNameOFEnumVariable(SeedReplaceText.SeedFile) )) {
                storedSeedFile = true;
                seedList = LoadSeedsTxt.LoadSeedFile();
                
                if(seedList.Length>0){
                    startingSeedAsText = seedList[0];                    
                    currentSeedAsText = startingSeedAsText;
                    Main.ActiveWorldFileData.SetSeed(currentSeedAsText.Trim());
                    StartSeed = Main.ActiveWorldFileData.Seed;
                    currentSeed = StartSeed;  
                } 
                    maxSeedCountStopAfter = seedList.Length;

            }else
            if(WorldInfo.GetValueAsString(MainSettingEnum.SeedReplaceText).Equals(Helpers.EnumHelper<SeedReplaceText>.GetNameOFEnumVariable(SeedReplaceText.WorldID2Seed)) ) {
                startingSeedAsText = "" + ((316080314L   * currentSeed + 75192552L)% ((1L<<31)-1)  );
                currentSeedAsText = startingSeedAsText;
                Main.ActiveWorldFileData.SetSeed(currentSeedAsText);
                StartSeed = Main.ActiveWorldFileData.Seed;
                currentSeed = StartSeed;
                currentSeedAsText = currentSeedAsText.ToString();

            }else if(!WorldInfo.GetValueAsString(MainSettingEnum.SeedReplaceText).Equals(Helpers.EnumHelper<SeedReplaceText>.GetNameOFEnumVariable(SeedReplaceText.Donot) )
               ) {
                customTextSeed = true;
                startingSeedAsText = WorldInfo.GetValueAsString(MainSettingEnum.SeedReplaceText);                
                string name = Helpers.Helpers.EncodeNameString(startingSeedAsText, StartSeed,currentSeedAsText, 0, worldSize, difficulty, WorldGen.crimson?EvilType.crimson:EvilType.corruption);
                if( name.Equals(startingSeedAsText) ){
                    expendTextSeedWithCount = true;
                }
                currentSeedAsText = name;
                Main.ActiveWorldFileData.SetSeed(currentSeedAsText.Trim());
                currentSeed = Main.ActiveWorldFileData.Seed;                
            }
            evilType = Helpers.EnumHelper<EvilType>.GetValueFromName((string)WorldInfo.Info[MainSettingEnum.EvilType]);
            WorldGen.WorldGenParam_Evil = evilType==EvilType.both?(int)EvilType.corruption:(evilType==EvilType.randomInverse ? RandomNumberAnalyzer.RandomNumberAnalyzer.PredictAntiRandom(currentSeed) :(int)evilType);

            WorldGenModifier.InitSecretSeedArry();


            

            //Main.rand = new UnifiedRandom(Main.ActiveWorldFileData.Seed);
            
        }

        private void StartWorldGenThread()
        {   
            WorldGen.gen = true;    
            PassMemory.Clear();       

            while (!stopWorldGen && maxSeedCountStopAfter>0)
            {                
                bool doNormalWorldGen = true;
                this.lastWorldGenPassID = ConstantEnum.WorldGenPass.PreWorldGen;
                conditionSched.ResetConditionsAfterPass(false, ConstantEnum.WorldGenPass.PreWorldGen);//reset everything
                WorldInfo.RNGNumbers.Clear();

                doNormalWorldGen = AnalysePreWorldGen();
                //if(doNormalWorldGen) doNormalWorldGen = PredictWG();
                
                if (doNormalWorldGen)
                {   

                    Stats.lastSeedEnteredWorldGen = currentSeed;
                    UpdateStats();
                    ContinuteWorldGen = true;
                    
                    DoWorldGen();                
                    //Helpers.Helpers.writeDebugFile($"{currentSeed} goS {(int)(-WorldInfo.GetValueAsInt(DataExtractor.DetermineDataNames.PredictedSpawnHeight)+WorldGen.rockLayer )} str {(int)(WorldGen.rockLayerLow-WorldGen.worldSurfaceHigh)} RNG {(int)(100.0*WorldInfo.RNGNumbers.RNGN1)} {(int)(100.0*WorldInfo.RNGNumbers.RNGN2)} {(int)(100.0*WorldInfo.RNGNumbers.RNGN3)} PSH {WorldInfo.GetValueAsInt(DataExtractor.DetermineDataNames.PredictedSpawnHeight)}"
                    //+$" MWS {(int)Main.worldSurface} MRL {(int)Main.rockLayer} WRL {(int)WorldGen.rockLayer} WRH {(int)WorldGen.rockLayerHigh} WRo {(int)WorldGen.rockLayerLow} WWS {(int)WorldGen.worldSurface} WWH {(int)WorldGen.worldSurfaceHigh} WWo {(int)WorldGen.worldSurfaceLow} Wat {(int)WorldGen.waterLine} Wla {(int)WorldGen.lavaLine}",false,"./layer");
                    
                    if(WGqueue[WGQind] == WGtype.normal)
                    if (ContinuteWorldGen && !stopWorldGen){      
                        bool sschange = false;                  
                        if(WorldInfo.HasKeyValue( WGHacksEnum.WGHacksIsActive,  OnOffenable.On)){
                            sschange = true;
                            ContinuteWorldGen = WorldGenModifier.SetSecretSeedsAndOtherAfterWorldGen();
                        }
                        if(ContinuteWorldGen){
                            --maxFoundSeedCountStopAfter;
                            Stats.seedsFound++;                        
                            StoreWorld();
                        }
                        if(sschange)WorldGenModifier.SetSecretSeedsBeforeWorldGen();
                    }   


                    uiSuperSeed.generationProgress = new GenerationProgress();                    
                }
               
                if(!stopWorldGen){ 

                    if(ContinuteWorldGen && WGqueue[WGQind] != WGtype.normal){
                        WGQind--;
                    }else{
                        WGQind = WGqueue.Count-1;

                        if(evilType==EvilType.both && WorldGen.WorldGenParam_Evil==(int)EvilType.corruption) WorldGen.WorldGenParam_Evil = (int)EvilType.crimson;
                        else{
                            Stats.seedsChecked++;                            
                            maxSeedCountStopAfter--;                        
                            if(evilType==EvilType.both && WorldGen.WorldGenParam_Evil==(int)EvilType.crimson) WorldGen.WorldGenParam_Evil = (int)EvilType.corruption;
                        
                            UpdateSeed();                        
                        }


                        if (maxSeedCountStopAfter <= 0 || maxFoundSeedCountStopAfter <= 0) break;
                        if(doNormalWorldGen || !fastMode)UpdateStats();     

                    }

                }                
            }
            PostWorldGen();
        }

        private void PostWorldGen()
        {
            isGen = false;
            onWorldGenEnd?.Invoke();
            UpdateStats();
            ResetSecrets();
            DataExtractor.Reset();            

            WorldGen.gen = false;
            WorldGen.generatingWorld = false;  
            //uiSuperSeed?.condInterface?.mainPropsPanel.GetValue(MainSettings.StartSeed).Equals(StartSeed.ToString())==true
            bool rngIsSet = startingSeedAsText.Equals(ConstantsStrings.RandomStartingSeed);
            if((Stats.seedsChecked>0  )&& !customTextSeed) uiSuperSeed?.condInterface?.mainPropsPanel.ChangeValue( MainSettingEnum.StartSeed,GetValidSeed(Stats.lastSeedFinishedWorldGen+(rngIsSet?0:stepSize)).ToString(), rngIsSet );
            
        }

        public void StoreWorld()
        {            


            string seed = customTextSeed? currentSeedAsText: (currentSeed.ToString()).PadLeft(10, '0');
            string name = (Helpers.Helpers.EncodeNameString(worldName, currentSeed,currentSeedAsText, Stats.seedsFound, worldSize, difficulty, WorldGen.crimson?EvilType.crimson:EvilType.corruption ));            
            Main.worldName = name.Trim();            
            Main.ActiveWorldFileData = WorldFile.CreateMetadata(Main.worldName, false, Main.GameMode);
            Main.ActiveWorldFileData.SetSeed(seed);


            if (StoreWorldAsPicture != StoreAsPicture.Off )
            {
                uiSuperSeed.generationProgress.Message = "Store world as picture";
                StoreWorldAsImage.StoreWorldAsImage.StoreCurrent(StoreWorldAsPicture );
                uiSuperSeed.generationProgress.Message = "";
            }
            if(StoreWorldStatsInfo != StoreStatsInfo.Off){
                uiSuperSeed.generationProgress.Message = "Compute and store stats";
                WorldInfo.SetValue("seed",currentSeed);
                WorldInfo.SetValue("seed as text",currentSeedAsText);
                WorldInfo.SetValue("seed modifier",WorldGenModifier.SecretSeedModifierText.Length>0?WorldGenModifier.SecretSeedModifierValue:currentSeed);
                WorldInfo.SetValue("seed modifier as text",WorldGenModifier.SecretSeedModifierText.Length>0?WorldGenModifier.SecretSeedModifierText:currentSeedAsText);
                Storage.Stats.StoreStats(conditionSched.GetConditionStateAsString(), StoreWorldStatsInfo);                
            }
            uiSuperSeed.generationProgress.Message = "Store world";

            if (StoreWorldFile != StoreWorldFile.Off)
                WorldFile.SaveWorld(false, resetTime: true);
            uiSuperSeed.generationProgress.Message = "";
            SoundEngine.PlaySound(SoundID.CoinPickup );
            //uiSuperSeed.generationProgress.Message = "Compute Pathlength";            
            //Pathlength.Pathlength plen = new Pathlength.Pathlength();
            //plen.ComputePathlength(currentSeed);
            //uiSuperSeed.generationProgress.Message = "";
        }
        //public double WGstaringDayTime = 0;

        private void ClearWorld(){
            WorldGen.clearWorld();
            //WGstaringDayTime = Main.time;            
            ResetSecrets();
            PassMemory.Disable();

            if( WGqueue[WGQind] == WGtype.FloatingChestsOnly ){                
                GenVars.numIslandHouses = (int)((double)Main.maxTilesX * 0.0008);
                GenVars.skyIslandHouseCount = 0;
                for(int i=0; i <GenVars.numIslandHouses; i++){
                    GenVars.floatingIslandHouseX[i] = 850+(i*(Main.maxTilesX-1700))/GenVars.numIslandHouses;
                    GenVars.floatingIslandHouseY[i] = 100;
                    GenVars.floatingIslandStyle[i] = 0;
                    GenVars.skyLake[i] = false;
                }
            }


        }
        public void ResetSecrets(){
            WorldGenModifier.ResetSetSecretSeeds(ref WorldGenModifier.sseedarrayEmpty );
        }
        

        private void DoWorldGen()
        {      
            uiSuperSeed.generationProgress = new GenerationProgress();
            WorldGen.generatingWorld = true;
            WorldGen.gen = true;
            Main.ActiveWorldFileData.SetSeed(currentSeed.ToString());
            currentSeed = Main.ActiveWorldFileData.Seed;            
            ClearWorld();  
            DataExtractor.Reset();
            
            
            if( WorldGenModifier.constantSecretSeedModifier ){
                Main.ActiveWorldFileData.SetSeed(WorldGenModifier.SecretSeedModifierText);
                WorldGenModifier.SecretSeedModifierValue = Main.ActiveWorldFileData.Seed;
            }else if( WorldGenModifier.randomSecretSeedModifier )
            {
                Main.ActiveWorldFileData.SetSeed( rng.Next(0,Int32.MaxValue).ToString() );
                WorldGenModifier.SecretSeedModifierValue = Main.ActiveWorldFileData.Seed;                
            }
            WorldGen.GenerateWorld(currentSeed, uiSuperSeed.generationProgress);   
                       
            
        }
        private void InitStats(){
            Stats.Reset();
            uiSuperSeed.statsText.ClearAllStats();
            UpdateStats();
            if(conditionSched.HasAnyThing4WGPass(WorldGenPass.PreWorldGen))     {               
                    Stats.wgStepPassed.Add(WorldGenPass.PreWorldGen,0);
                    uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.SeedsPassedWGstep+" "+ WorldGenPassDict.AsPreConstraintText(WorldGenPass.PreWorldGen) + " " + WorldGenPass.PreWorldGen.ToString(), 0, WorldGenPassDict.AsPreConstraintText(WorldGenPass.PreWorldGen));
                    
            }
            foreach(WorldGenPass wgpass in Enum.GetValues(typeof(WorldGenPass))  ){
                if(conditionSched.HasAnyThing4WGPass(wgpass) && wgpass!=WorldGenPass.PreWorldGen){  
                    Stats.wgStepPassed.Add(wgpass,0);                  
                    uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.SeedsPassedWGstep+" "+ WorldGenPassDict.AsPreConstraintText(wgpass) + " " + wgpass.ToString(), 0, WorldGenPassDict.AsPreConstraintText(wgpass));
                }
            }

            if(uiSuperSeed.condInterface.currentTab.IsMainTab()){
                uiSuperSeed.statsText.Show();
                uiSuperSeed.ShowStatsPanel();
            }

        }

        private void UpdateStats(){

            if(uiSuperSeed==null || uiSuperSeed.statsText==null)            return;
            uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.SeedsPassed, Stats.seedsFound);
            uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.StartingSeed, StartSeed);
            uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.LastSeed, Stats.lastSeedEnteredWorldGen);
            uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.SeedsChecked, Stats.seedsChecked);
            uiSuperSeed.statsText.SetStat(ConstantEnum.Statistics.SeedsToDo, maxSeedCountStopAfter);              
            foreach(var passpassed in Stats.wgStepPassed){
                uiSuperSeed.statsText.SetStat(WorldGenPassDict.AsPreConstraintText(passpassed.Key), passpassed.Value);
            }
        }
        private void UpdateSeed()
        {
            Stats.lastSeedFinishedWorldGen = currentSeed;
                        
            if(customTextSeed == true) {                                                
                if(expendTextSeedWithCount) currentSeedAsText = startingSeedAsText + Stats.seedsChecked;
                else{
                    int counter = GetValidSeed(Stats.seedsChecked*stepSize);
                    currentSeedAsText = Helpers.Helpers.EncodeNameString(startingSeedAsText, GetValidSeed(StartSeed+counter), currentSeedAsText, Stats.seedsChecked, worldSize, difficulty, WorldGen.crimson?EvilType.crimson:EvilType.corruption, false );                                      
                }
                Main.ActiveWorldFileData.SetSeed(currentSeedAsText.Trim());
                currentSeed = Main.ActiveWorldFileData.Seed;                
            }else if(storedSeedFile){
                
                currentSeedAsText = seedList.Length==0?"":seedList[Math.Min(Stats.seedsChecked,seedList.Length-1)];                                
                Main.ActiveWorldFileData.SetSeed(currentSeedAsText.Trim());
                currentSeed = Main.ActiveWorldFileData.Seed; 
            }            
            else{
                currentSeed += stepSize;            
                currentSeed = GetValidSeed(currentSeed);  
            }      
            if(evilType==EvilType.randomInverse) WorldGen.WorldGenParam_Evil = RandomNumberAnalyzer.RandomNumberAnalyzer.PredictAntiRandom(currentSeed);
        }
        private int GetValidSeed(int currentSeed) {
            if (currentSeed < 0 && stepSize<0) { currentSeed = (Int32.MaxValue)+currentSeed;}
            else if (currentSeed < 0 && stepSize>0) { currentSeed = (currentSeed +Int32.MaxValue); currentSeed++;}
            return currentSeed;
        }

        public void StopWorldGen()
        {
            stopWorldGen = true;
            ContinuteWorldGen = false;
            ResetSecrets();
            PassMemory.Clear();
        }

        public ConstantEnum.WorldGenPass lastWorldGenPassID;
        public bool ContinuteWorldGen;
      
        public void ExtactDataFromCurWGPass(GenPass targetPass,string lastPassName, ConstantEnum.WorldGenPass lastPassNameID){
            this.lastWorldGenPassID = lastPassNameID;
            //if(!conditionSched.HasAnyThing4WGPass(lastPassNameID) && lastPassNameID!=ConstantEnum.WorldGenPass.FinalCleanup) return;//todo: dont miss to extract some needed data
            if(lastPassNameID > conditionSched.lastPassUsedPassive) return;//todo: dont miss to extract some needed data
            dataExtractor.ExtactDataFromCurWGPass(targetPass,lastPassName,lastPassNameID);
        }

        public bool AnalyseWGPassAndCheckIfContinue(string lastPassName, ConstantEnum.WorldGenPass lastPassNameID)
        {
            this.lastWorldGenPassID = lastPassNameID;
            if(conditionSched == null || (!conditionSched.HasAnyThing4WGPass(lastPassNameID) && lastPassNameID!=ConstantEnum.WorldGenPass.FinalCleanup)) return true;//mods may generate somehtign after this todo5
            uiSuperSeed.generationProgress.Message = "Analyse world generation after " + lastPassName;
            PassMemory.Disable();
            conditionSched.ResetConditionsAfterPass(true, lastPassNameID);
            
            
            analyser.AnalyseUniqueProps(lastPassNameID);
            analyser.AnalyseChests(lastPassNameID);
            analyser.AnalyseTiles(lastPassNameID);            

            ContinuteWorldGen = conditionSched.CheckOutIfAnyConditionTrueAfterFullWorldAnalysis(lastWorldGenPassID);            
            if(ContinuteWorldGen && Stats.wgStepPassed.ContainsKey(lastPassNameID) ){Stats.wgStepPassed[lastPassNameID]++; UpdateStats();}
            return ContinuteWorldGen;
        }


        private bool AnalysePreWorldGen(){            
            analyser.SetRNGValues(currentSeed);

            if(!conditionSched.HasAnyThing4WGPass(ConstantEnum.WorldGenPass.PreWorldGen)) return true;

            analyser.AnalyseSeedRNG(currentSeed);                
            analyser.AnalyseUniquePropsBeforeWG();

            ContinuteWorldGen = conditionSched.CheckOutIfAnyConditionTrueAfterFullWorldAnalysis(ConstantEnum.WorldGenPass.PreWorldGen);
            if(ContinuteWorldGen){ Stats.wgStepPassed[ConstantEnum.WorldGenPass.PreWorldGen]++;if(!fastMode)UpdateStats();}

            return ContinuteWorldGen;
        }






    }


}
