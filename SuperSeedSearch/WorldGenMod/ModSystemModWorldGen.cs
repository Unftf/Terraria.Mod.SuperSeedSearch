using Terraria.ModLoader;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using System.Reflection;
using SuperSeedSearch.UI;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;



namespace SuperSeedSearch.WorldGenMod
{
    public class ModSystemModWorldGen : ModSystem
    {
        
        

        public static Dictionary<string, string> tiles = new Dictionary<string, string>();
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
                        
            if (ModMenuMod.uiSuperSeed == null || (ModMenuMod.uiSuperSeed.startPause == null || ModMenuMod.uiSuperSeed.startPause.isInSearchMode != true) ) return;
            //TempleSizeReduced = false;    

            List<String> exceptions = null;
            
            if(ModMenuMod.wGPassChanger != null) {
                if(ModMenuMod.wGPassChanger.WGqueue[ModMenuMod.wGPassChanger.WGQind] == WGPassChanger.WGtype.PartialFloatingChestsMountCave )
                    exceptions =  new List<string>{ WorldGenPassName.Reset,WorldGenPassName.Terrain,WorldGenPassName.MountCaves,WorldGenPassName.FloatingIslands,WorldGenPassName.FloatingIslandHouses } ;
                else if(ModMenuMod.wGPassChanger.WGqueue[ModMenuMod.wGPassChanger.WGQind] == WGPassChanger.WGtype.FloatingChestsOnly )
                    exceptions =  new List<string>{WorldGenPassName.FloatingIslandHouses } ;
                if(exceptions!=null) ClearPasses(tasks, exceptPasses: exceptions);     
            }


           
            GenPass resetPass = null;//, jungleTemplePass = null;
            for (int i = tasks.Count; i > 0; i--)
            {
                GenPass lastPass = tasks[i - 1];       
                string lastPassName = lastPass.Name;  
                

                if (!ConstantEnum.WorldGenPassDict.worldGenPassDict.ContainsKey(lastPassName) || (exceptions!=null && !exceptions.Contains(lastPassName))  ) continue;                
                
                ConstantEnum.WorldGenPass wgpID = ConstantEnum.WorldGenPassDict.worldGenPassDict[lastPassName];
                if(wgpID == ConstantEnum.WorldGenPass.Reset) resetPass=lastPass;
                //else if(wgpID == ConstantEnum.WorldGenPass.JungleTemple) jungleTemplePass=lastPass;
                tasks.Insert(i, new PassLegacy("Analyse last World gen. pass", delegate {  PostWorldGenPassAnalysis(tasks, lastPass, lastPassName, wgpID); }));//generates memory leak                
            }    
                  
            if(resetPass!=null) tasks.Insert(0, new PassLegacy("Restore secret seeds", delegate { RevertSecretSeedsAndApplyNew(resetPass); }));
            //int jungleTempleInd = tasks.IndexOf(jungleTemplePass);

            /*
            if( WorldInfo.HasKeyValue( WGHacksEnum.FixDoubleTempleIncrease,  Helpers.EnumHelper<FixDoubleTempleIncrease>.GetNameOFEnumVariable(FixDoubleTempleIncrease.on) )){
                tasks.Insert(jungleTempleInd+1, new PassLegacy("Fix drunk worthy Jungle Temple size af", delegate { FixDrunkWorthyJungleTempleSizeAfter(); }));
                tasks.Insert(jungleTempleInd, new PassLegacy("Fix drunk worthy Jungle Temple size bf", delegate { FixDrunkWorthyJungleTempleSizeBefore(); }));
            }*/

        }

        public static void PostWorldGenPassAnalysis(List<GenPass> tasks, GenPass lastPass, string lastPassName, ConstantEnum.WorldGenPass lastPassNameID)
        {            
            if (ModMenuMod.uiSuperSeed == null || ModMenuMod.wGPassChanger == null || ModMenuMod.wGPassChanger.stopWorldGen == true) { ClearPasses(tasks); return; }
            
            //if( WorldInfo.GetValueAsStringOrEmptyIfNotExists( WGHacks.StoreWorldImageAfter).Equals(lastPassNameID.ToString()) ) ModMenuMod.wGPassChanger?.StoreWorld();
            ModMenuMod.wGPassChanger?.ExtactDataFromCurWGPass(lastPass,lastPassName, lastPassNameID);
            bool ContinueWG = ModMenuMod.wGPassChanger==null? false :ModMenuMod.wGPassChanger.AnalyseWGPassAndCheckIfContinue(lastPassName, lastPassNameID);

            /*if(lastPassNameID ==  ConstantEnum.WorldGenPass.FloatingIslands){
                for(int f=0; f< GenVars.numIslandHouses; f++){
                    Console.WriteLine($" cloud at {GenVars.floatingIslandHouseX[f]} {GenVars.floatingIslandHouseY[f]} stayle {GenVars.floatingIslandStyle[f]} lake {GenVars.skyLake[f]}  totalt {GenVars.numIslandHouses} {GenVars.skyIslandHouseCount} ");
                }
            }*/


            if (!ContinueWG) ClearPasses(tasks);
            else if( WorldInfo.GetValueAsStringOrEmptyIfNotExists( WGHacksEnum.EndWorldGenAfterWGPass).Equals(lastPassNameID.ToString()) ) ClearPasses(tasks);
            //StoreWorldAsImage.StoreWorldAsImage.StoreCurrent(ConstantEnum.StoreAsPicture.PictureDefault,true);            
        }

        public static void RevertSecretSeedsAndApplyNew(GenPass resetPass){
            string activeSSeedInMain = WorldGenModifier.activeSSeedInMain;
            if(!(Main.drunkWorld || Main.getGoodWorld ||  Main.tenthAnniversaryWorld || Main.dontStarveWorld || Main.notTheBeesWorld ) &&
             activeSSeedInMain.Equals(SecretSeed.None.ToString()) &&  !WorldInfo.HasKeyValue( WGHacksEnum.WGHacksIsActive,  OnOffenable.On)) return;

			int seed = ModMenuMod.wGPassChanger.currentSeed;
			WorldGen._lastSeed = seed;
            WorldGen._genRandSeed = seed;
            //WorldGen._genRand = new UnifiedRandom(seed);
            RestoreSeed(seed);
            GenVars.jungleHut = (ushort)(WorldInfo.RNGNumbers.RNGN1*5);
            //DataInsertion.InsertVar<ushort>(resetPass, DataInsertion.VanillaVarNames.JungleHut, (ushort)WorldGen.genRand.Next(5));            

		    //Main.rand = new UnifiedRandom(seed);  
            
            
            WorldGenModifier.SetSecretSeedsBeforeWorldGen();
                       
        }
        

        //bool TempleSizeReduced = false;
        /*private void FixDrunkWorthyJungleTempleSizeBefore(){                
                if(WorldGen.drunkWorldGen && WorldGen.getGoodWorldGen &&  WorldInfo.HasKeyValue( WGHacksEnum.FixDoubleTempleIncrease,  Helpers.EnumHelper<FixDoubleTempleIncrease>.GetNameOFEnumVariable(FixDoubleTempleIncrease.on) )  ){
                    WorldGen.getGoodWorldGen = false; TempleSizeReduced =true;
                }
        }
        private void FixDrunkWorthyJungleTempleSizeAfter(){
                if(TempleSizeReduced){
                    WorldGen.getGoodWorldGen = true; TempleSizeReduced =false;
                }
        }*/

        private static void RestoreSeed(int seed){
                FieldInfo WorldGeneratorInfo = typeof(WorldGen).GetField("_generator", BindingFlags.NonPublic | BindingFlags.Static);
                WorldGenerator woge = (WorldGenerator)WorldGeneratorInfo.GetValue(null);
                FieldInfo worldGeneratorSeedInfo = typeof(WorldGenerator).GetField("_seed", BindingFlags.Instance | BindingFlags.NonPublic);
                worldGeneratorSeedInfo.SetValue(woge, seed);
                
        }

        public static void ClearPasses(List<GenPass> tasks, int start = 0, int end = -1, List<string> exceptPasses = null)
        {          

            if (end == -1 || end > tasks.Count) end = tasks.Count;
            FieldInfo WorldGeneratorInfo = typeof(WorldGen).GetField("_generator", BindingFlags.NonPublic | BindingFlags.Static);
            WorldGenerator woge = (WorldGenerator)WorldGeneratorInfo.GetValue(null);
            FieldInfo worldGeneratorPassesInfo = typeof(WorldGenerator).GetField("_passes", BindingFlags.Instance | BindingFlags.NonPublic);
            List<GenPass> gpl = (List<GenPass>)worldGeneratorPassesInfo.GetValue(woge);
            FieldInfo GenPassArrayInfo = typeof(List<GenPass>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic);
            GenPass[] GenPassArray = (GenPass[])GenPassArrayInfo.GetValue(gpl);
            for (int passid = start; passid < end; passid++)
            {               
                //produces memory leaks here                
                if(exceptPasses!=null && exceptPasses.Contains(GenPassArray[passid].Name)) continue;
                GenPassArray[passid] = new PassLegacy("Clear Pass", delegate (GenerationProgress progress, GameConfiguration passConfig) { }, GenPassArray[passid].Weight);                
            }
            if(end == tasks.Count &&  (exceptPasses == null || exceptPasses.Count==0)   ){                
                WorldGenModifier.RestoreWGConstants();
            }

        }





    }
}