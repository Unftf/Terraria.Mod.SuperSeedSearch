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
using System.Threading;

namespace SuperSeedSearch.WorldGenMod
{
    public static class WorldGenModifier{

        static bool[] sseedarrayBFWG = new bool[Enum.GetNames(typeof(SecretSeed)).Length];
        static bool[] sseedarrayAWG = new bool[Enum.GetNames(typeof(SecretSeed)).Length];

        public static bool[] sseedarrayEmpty = null;
        public static string activeSSeedInMain ="";

        public static bool constantSecretSeedModifier = false;
        public static bool randomSecretSeedModifier = false;
        public static int SecretSeedModifierValue = -1;
        public static string SecretSeedModifierText = "";

        public static void ResetSetSecretSeeds(ref bool[] setToSecrets, bool changeBackTime = false){//todo replace with || num                    

                    bool changeTimeBack = WorldGen.drunkWorldGen;
                    
                    WorldGen.drunkWorldGen = false;
                    Main.drunkWorld = false;
                    WorldGen.getGoodWorldGen = false;
                    Main.getGoodWorld = false;			
                    WorldGen.tenthAnniversaryWorldGen = false;
                    Main.tenthAnniversaryWorld = false;			
                    WorldGen.notTheBees = false;	
                    Main.notTheBeesWorld = false;            
                    Main.dontStarveWorld = false;
                    WorldGen.dontStarveWorldGen = false;
                    Main.noTrapsWorld = false;
                    WorldGen.noTrapsWorldGen= false;
                    Main.remixWorld = false;
                    WorldGen.remixWorldGen = false;
                    WorldGen.everythingWorldGen = false;
                    WorldGen.tempTenthAnniversaryWorldGen = false;
                    WorldGen.tempRemixWorldGen = false;
                    Main.zenithWorld = false;
                    Main.starGame = false;
	                    
                    if(setToSecrets==null) return;
                   

                    if(setToSecrets[(int) SecretSeed.Drunk]){
                        WorldGen.drunkWorldGen = true;
                        Main.drunkWorld = true;
                        //if(changeBackTime)
                        //    Main.time = WGstaringDayTime;//clouds missing
                    }   

                    if(setToSecrets[(int) SecretSeed.Worthy]){
                        WorldGen.getGoodWorldGen = true;
                        Main.getGoodWorld = true;		
                    }

                    if(setToSecrets[(int) SecretSeed.Celebration]){
                        WorldGen.tenthAnniversaryWorldGen = true;
                        Main.tenthAnniversaryWorld = true;		
                    }

                    if(setToSecrets[(int) SecretSeed.Constant]){
                        Main.dontStarveWorld = true;
                        WorldGen.dontStarveWorldGen = true;
                    }

                    if(setToSecrets[(int) SecretSeed.Bees]){
                        WorldGen.notTheBees = true;	
                        Main.notTheBeesWorld = true;            
                    } 

                    if(setToSecrets[(int) SecretSeed.DontDigUp]){
                        WorldGen.remixWorldGen = true;	
                        Main.remixWorld = true;            
                    } 
                    
                    if(setToSecrets[(int) SecretSeed.NoTraps]){
                        WorldGen.noTrapsWorldGen= true;	
                        Main.noTrapsWorld = true;            
                    } 
                    
                    if(setToSecrets[(int) SecretSeed.Zenith]){
                        WorldGen.everythingWorldGen = true;
                        WorldGen.tempTenthAnniversaryWorldGen = true;
                        WorldGen.tempRemixWorldGen = true;         
                        Main.zenithWorld = true;
                        Main.starGame = true;
                        
                        string caught = WorldInfo.GetValueAsStringOrEmptyIfNotExists(WGHacksEnum.SSStarsCaught);
                        
     				    Main.starsHit = caught.Length==0?0:Int32.Parse(caught);
                        

                    } 
            }
            public static void InitSecretSeedArry(){
                
                activeSSeedInMain = WorldInfo.GetValueAsString(MainSettingEnum.SecretSeed); 

                sseedarrayBFWG = new bool[Enum.GetNames(typeof(SecretSeed)).Length];
                sseedarrayAWG = new bool[Enum.GetNames(typeof(SecretSeed)).Length];       
                               

                
                if(WorldInfo.HasKeyValue( WGHacksEnum.WGHacksIsActive,  OnOffenable.On)){
                    bool every = sseedarrayBFWG [(int)SecretSeed.Zenith] = WorldInfo.HasKeyValue( WGHacksEnum.SSZenithIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSZenithIsActive,  OnOffenable.OnOff);
                    sseedarrayBFWG[(int)SecretSeed.Drunk] = WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.Worthy] = WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.Celebration] = WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.Constant] = WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.Bees] = WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.DontDigUp] = WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.OffOn)));
                    sseedarrayBFWG[(int)SecretSeed.NoTraps] = WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.OnOff) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.OffOn)));
                }
                SecretSeed sseedInMain = Helpers.EnumHelper<SecretSeed>.GetValueFromName(activeSSeedInMain);
                sseedarrayBFWG[(int) sseedInMain ] = true;
                constantSecretSeedModifier = false;
                randomSecretSeedModifier = false;
                SecretSeedModifierValue = -1;
                bool SecretSeedModifierFitFirst = WorldInfo.HasKeyValue( MainSettingEnum.SecretSeedModifier, Helpers.EnumHelper<ConstantEnum.SecretSeedModifier>.GetNameOFEnumVariable(ConstantEnum.SecretSeedModifier.FirtFit)  );
                if(SecretSeedModifierFitFirst){
                    SecretSeedModifier? modi = sseedInMain switch{SecretSeed.Drunk => SecretSeedModifier.drunk,
                                                                 SecretSeed.Worthy => SecretSeedModifier.ftw1, 
                                                                 SecretSeed.Celebration => SecretSeedModifier.cele1,
                                                                 SecretSeed.Constant => SecretSeedModifier.constant1,
                                                                 SecretSeed.Bees => SecretSeedModifier.bees1,
                                                                 SecretSeed.DontDigUp => SecretSeedModifier.dig1,
                                                                 SecretSeed.NoTraps => SecretSeedModifier.traps1,
                                                                 SecretSeed.Zenith => SecretSeedModifier.fixed1,   
                                                                  _   => null};
                    if(modi!=null){
                        constantSecretSeedModifier = true;
                        SecretSeedModifierText =  Helpers.EnumHelper<ConstantEnum.SecretSeedModifier>.GetNameOFEnumVariable((SecretSeedModifier)modi);
                    }
                    //Main.ActiveWorldFileData.SetSeed(SecretSeedModifierValue.Trim());
                }else if( WorldInfo.HasKeyValue( MainSettingEnum.SecretSeedModifier, Helpers.EnumHelper<ConstantEnum.SecretSeedModifier>.GetNameOFEnumVariable(ConstantEnum.SecretSeedModifier.random)  ) ){
                    randomSecretSeedModifier = true;
                }else if( !WorldInfo.HasKeyValue( MainSettingEnum.SecretSeedModifier, Helpers.EnumHelper<ConstantEnum.SecretSeedModifier>.GetNameOFEnumVariable(ConstantEnum.SecretSeedModifier.seed)  ) ){
                    constantSecretSeedModifier = true;
                    SecretSeedModifierText = WorldInfo.GetValueAsString( MainSettingEnum.SecretSeedModifier);
                }

                if(sseedInMain == SecretSeed.Zenith){
                        sseedarrayBFWG[(int)SecretSeed.Drunk] = true;
                        sseedarrayBFWG[(int)SecretSeed.Worthy] = true;
                        sseedarrayBFWG[(int)SecretSeed.Celebration] = true;
                        sseedarrayBFWG[(int)SecretSeed.Constant] = true;
                        sseedarrayBFWG[(int)SecretSeed.Bees] = true;
                        sseedarrayBFWG[(int)SecretSeed.DontDigUp] = true;
                        sseedarrayBFWG[(int)SecretSeed.NoTraps] = true;
                        sseedarrayBFWG[(int)SecretSeed.Zenith] = true;
                }

                //after wg
                              
             
                if(WorldInfo.HasKeyValue( WGHacksEnum.WGHacksIsActive,  OnOffenable.On)){
                    bool every = sseedarrayAWG[(int)SecretSeed.Zenith] = WorldInfo.HasKeyValue( WGHacksEnum.SSZenithIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSZenithIsActive,  OnOffenable.OffOn);
                
                    sseedarrayAWG[(int)SecretSeed.Drunk] = WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSdrunkIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.Worthy] = WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSworthyIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.Celebration] = WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SScelebrIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.Constant] = WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSconstantIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.Bees] = WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSbeesIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.DontDigUp] = WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSDontDigUpIsActive,  OnOffenable.OnOff)));
                    sseedarrayAWG[(int)SecretSeed.NoTraps] = WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.On) || WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.OffOn) || (every && !(WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.Off) ||  WorldInfo.HasKeyValue( WGHacksEnum.SSNoTrapsIsActive,  OnOffenable.OnOff)));
                }

                sseedarrayAWG[(int) sseedInMain ] = true;
                if(sseedInMain == SecretSeed.Zenith){
                        sseedarrayAWG[(int)SecretSeed.Drunk] = true;
                        sseedarrayAWG[(int)SecretSeed.Worthy] = true;
                        sseedarrayAWG[(int)SecretSeed.Celebration] = true;
                        sseedarrayAWG[(int)SecretSeed.Constant] = true;
                        sseedarrayAWG[(int)SecretSeed.Bees] = true;
                        sseedarrayAWG[(int)SecretSeed.DontDigUp] = true;
                        sseedarrayAWG[(int)SecretSeed.NoTraps] = true;
                }


            }



            public static void SetSecretSeedsBeforeWorldGen(){  
                ResetSetSecretSeeds(ref sseedarrayBFWG,true);
            }

            public static bool SetSecretSeedsAndOtherAfterWorldGen(){                  

                ResetSetSecretSeeds(ref sseedarrayAWG,true);

                if (WorldInfo.HasKeyDifferentToValue( WGHacksEnum.HardmodeOreTier1,  Helpers.EnumHelper<HardmodeOreTier1>.GetNameOFEnumVariable(HardmodeOreTier1.doNotChange))) WorldGen.SavedOreTiers.Cobalt = TileID.Search.GetId(WorldInfo.GetValueAsString(WGHacksEnum.HardmodeOreTier1));                
                if (WorldInfo.HasKeyDifferentToValue( WGHacksEnum.HardmodeOreTier2,  Helpers.EnumHelper<HardmodeOreTier2>.GetNameOFEnumVariable(HardmodeOreTier2.doNotChange))) WorldGen.SavedOreTiers.Mythril = TileID.Search.GetId(WorldInfo.GetValueAsString(WGHacksEnum.HardmodeOreTier2));                
                if (WorldInfo.HasKeyDifferentToValue( WGHacksEnum.HardmodeOreTier3,  Helpers.EnumHelper<HardmodeOreTier3>.GetNameOFEnumVariable(HardmodeOreTier3.doNotChange))) WorldGen.SavedOreTiers.Adamantite = TileID.Search.GetId(WorldInfo.GetValueAsString(WGHacksEnum.HardmodeOreTier3));
                
                bool alsoUpdate = WorldInfo.HasKeyValue( WGHacksEnum.StartInHardmode,  Helpers.EnumHelper<StartInHardmode>.GetNameOFEnumVariable(StartInHardmode.onMechsKilledAndUpdate)); 
                bool killMechs = alsoUpdate || WorldInfo.HasKeyValue( WGHacksEnum.StartInHardmode,  Helpers.EnumHelper<StartInHardmode>.GetNameOFEnumVariable(StartInHardmode.onMechsKilled));

                if(WorldInfo.HasKeyValue( WGHacksEnum.StartInHardmode,  Helpers.EnumHelper<StartInHardmode>.GetNameOFEnumVariable(StartInHardmode.on))
                    || killMechs
                         ) {
                             WorldGen._genRandSeed = -1;
                             WorldGen._lastSeed = ModMenuMod.wGPassChanger.currentSeed;
                             Main.rand = new UnifiedRandom(WorldGen._lastSeed);
                             WorldGen.gen = false;
                             WorldGen.generatingWorld = false;
                             //WorldGen.StartHardmode();
                             Main.hardMode = true;				             
                             WorldGen.smCallBack(2);
                             //while(!WorldGen.IsGeneratingHardMode){Thread.Sleep(10);}; 
                             while(WorldGen.IsGeneratingHardMode){Thread.Sleep(100);} 
                             
                             WorldGen.gen = true;
                             WorldGen.generatingWorld = true;

                             }
                else if (WorldInfo.HasKeyValue( WGHacksEnum.StartInHardmode,  Helpers.EnumHelper<StartInHardmode>.GetNameOFEnumVariable(StartInHardmode.onNoBiomes))) {Main.hardMode = true;}

                if(killMechs) {
                    NPC.downedMechBoss1 = true;
                    NPC.downedMechBoss2 = true;
                    NPC.downedMechBoss3 = true;

                    if(alsoUpdate){
                        if(!RandomNumberAnalyzer.RandomNumberAnalyzer.HasBeenUsedForCurrentSeed()) RandomNumberAnalyzer.RandomNumberAnalyzer.generateNewRNG( (int)(500*RandomNumberAnalyzer.RandomNumberAnalyzer.BulbPredictionTickCount*((double)Main.maxTilesX/4200)));
                        //RandomNumberAnalyzer.RandomNumberAnalyzer.BulbSpawnHelper();
                        Main.rand = new UnifiedRandom(WorldGen._lastSeed);
                        
                        Main.ActiveWorldFileData.SetSeed(""+ModMenuMod.wGPassChanger.currentSeed);
                        
                        

                        
                        //WorldGen._genRand = new UnifiedRandom(ModMenuMod.wGPassChanger.currentSeed);
       
                        Main.rand = new UnifiedRandom(ModMenuMod.wGPassChanger.currentSeed);                        
                        const int loops = 2;
                        WorldGen.gen = false;
                        WorldGen.generatingWorld = false;
                        for(int l=0; l<loops;l++){
                            WorldGen._genRandSeed = -1;
                            WorldGen._lastSeed = ModMenuMod.wGPassChanger.currentSeed;

                            
                            //Thread.Sleep(1000);
                            for(int i=0;i<RandomNumberAnalyzer.RandomNumberAnalyzer.BulbPredictionTickCount ;i++) WorldGen.UpdateWorld();
                            
                        }
                        WorldGen._genRandSeed = -1;
                        WorldGen._lastSeed = ModMenuMod.wGPassChanger.currentSeed;

                        WorldGen.gen = true;
                        WorldGen.generatingWorld = true;


                        for (int y = 0; y < Main.maxTilesY; y += 1)
                            for (int x = 0; x < Main.maxTilesX; x += 1)                               
                                if(Main.tile[x, y].TileType == TileID.PlanteraBulb )
                                    return true;   
                            
                        

                        return false;


                    }

                }

                return true;
            }




            public static void RestoreWGConstants(){
                WorldGen.noTileActions = false;
				Main.tileSolid[TileID.CrackedBlueDungeonBrick] = true;
                Main.tileSolid[TileID.CrackedGreenDungeonBrick] = true;
                Main.tileSolid[TileID.CrackedPinkDungeonBrick] = true;                
				Main.tileSolid[TileID.RollingCactus] = true;

                //some more which are used somewhwere in worldgen
                Main.tileSolid[TileID.Spikes] = true;
                Main.tileSolid[TileID.Obsidian] = true;
                Main.tileSolid[TileID.ActiveStoneBlock] = true;
                Main.tileSolid[TileID.Traps] = true;
                Main.tileSolid[TileID.BreakableIce] = true;
                Main.tileSolid[TileID.Cloud] = true;
                Main.tileSolid[TileID.MushroomBlock] = true;
                Main.tileSolid[TileID.LivingWood] = true;
                Main.tileSolid[TileID.LeafBlock] = true;
                Main.tileSolid[TileID.RainCloud] = true;
                Main.tileSolid[TileID.Sunplate] = true;
                Main.tileSolid[TileID.Hive] = true;
                Main.tileSolid[TileID.LihzahrdBrick] = true;
                Main.tileSolid[TileID.HoneyBlock] = true;
                Main.tileSolid[TileID.WoodenSpikes] = true;                
                Main.tileSolid[TileID.SnowCloud] = true;
            }




    }



}