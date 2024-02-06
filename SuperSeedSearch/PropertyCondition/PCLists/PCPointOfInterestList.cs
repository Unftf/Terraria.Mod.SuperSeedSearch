using Terraria;
using Terraria.WorldBuilding;
using System;
using System.Collections.Generic;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;
using SuperSeedSearch.Helpers;
using SuperSeedSearch.WorldGenMod;
using Steamworks;
using SuperSeedSearch.UI;
using System.Linq;
using System.ComponentModel;
using Terraria.ID;
using System.Security.Cryptography;
using Terraria.ModLoader;

namespace SuperSeedSearch.PropertyCondition.PCPoint
{
    public static class PCPointOfInterestList
    {
        public static List<Condition> pcPointOfInteerestConditionList = null;
        class PoI{
            public string name="";
            Func<int> getX = () =>  -1;
            Func<int>  getY = () =>  -1;

            public int X => getX();
            public int Y => getY();
            public WorldGenPass firstPossibleEntryPoint = WorldGenPass.PostWorldGen;
            
            public PoI(string name, WorldGenPass firstPossibleEntryPoint, Func<int> GetPosX, Func<int>  GetposY = null, Func<List<PoIFunSingle>> privateFunList = null){
                getX = GetPosX;
                getY = GetposY;
                this.name = name;
                this.firstPossibleEntryPoint = firstPossibleEntryPoint;
                this.privateFunList = privateFunList;
            }
            public bool IsOnlyHori => getY == null;

            public Func<List<PoIFunSingle>> privateFunList = null;
            
        }
        class PoIFun{
            public string name = "";
            public Func<PoI,PoI,int> poiFun = null;
            public Func< GreaterOrLessEqual, Constraint.TargetValue,Constraint.TargetValue, bool> extraEvalFun = null;
            public PoIFun(string name,  Func<PoI,PoI,int> poiFun, Func< GreaterOrLessEqual, Constraint.TargetValue,Constraint.TargetValue, bool> extraEvalFun = null){
                this.name =name;
                this.poiFun = poiFun;
                this.extraEvalFun = extraEvalFun;
            }
        }
        class PoIFunSingle{
            public string name = "";
            public Func<PoI,int> poiFun = null;
            public WorldGenPass firstPossibleEntryPoint;
            public PoIFunSingle(string name, WorldGenPass firstPossibleEntryPoint, Func<PoI,int> poiFun){
                this.name =name;
                this.poiFun = poiFun;
                this.firstPossibleEntryPoint = firstPossibleEntryPoint;
            }
        }


        static List<PoI> PoIList= new List<PoI>{
            new PoI(ConstantEnum.PoI.PredictedSpawn, WorldGenPass.Terrain, ()=>WorldInfo.PredictedSpawnX, ()=> WorldInfo.PredictedSpawnHeight, () => SinglePoIPathlengthPrivatePredicSpawn ),            
            new PoI(ConstantEnum.PoI.Dungeon, WorldGenPass.Dungeon, ()=> Main.dungeonX, () => Main.dungeonY ),
            new PoI(ConstantEnum.PoI.DesertHiveCenter, WorldGenPass.FullDesert, ()=> (GenVars.desertHiveLeft+GenVars.desertHiveRight)/2, () => (GenVars.desertHiveLow+GenVars.desertHiveHigh)/2 ),
            new PoI(ConstantEnum.PoI.DesertHiveSurfaceCenter, WorldGenPass.FullDesert, ()=> WorldInfo.DesertTopOfSurfaceCenterX, () => WorldInfo.DesertTopOfSurfaceCenterYapprox, () => SinglePoIPathlengthPrivateDesertSurf  ),
            new PoI(ConstantEnum.PoI.SnowSurfaceCenter, WorldGenPass.GenerateIceBiome, ()=> WorldInfo.SnowTopOfSurfaceCenterX, () => WorldInfo.SnowTopOfSurfaceCenterY, () => SinglePoIPathlengthPrivateSnowCSurf  ),
            new PoI(ConstantEnum.PoI.Spawn, WorldGenPass.SpawnPoint, ()=> Main.spawnTileX, () => Main.spawnTileY, () => SinglePoIPathlengthSpawnPrivate ),            
            new PoI(ConstantEnum.PoI.MidofWorld, WorldGenPass.Reset, ()=>Main.maxTilesX/2, null ),
            new PoI(ConstantEnum.PoI.CenterOfTheWorld, WorldGenPass.Reset, ()=>Main.maxTilesX/2, ()=>Main.maxTilesY/2 ),
            new PoI(ConstantEnum.PoI.TopLeftOfTheWorld, WorldGenPass.Reset, ()=>0, ()=>0 ),
            new PoI(ConstantEnum.PoI.TopAboveMid, WorldGenPass.Reset, ()=>Main.maxTilesX/2, ()=>0 ),
            new PoI(ConstantEnum.PoI.Dungeon1stGen, WorldGenPass.Reset, ()=> GenVars.dungeonLocation, null ),            
            new PoI(ConstantEnum.PoI.Jungle1stGen, WorldGenPass.Reset, ()=> GenVars.jungleOriginX, () => (int)((double)Main.maxTilesY + Main.rockLayer) / 2  ) ,
            new PoI(ConstantEnum.PoI.JungleMainEntrance, WorldGenPass.Jungle, ()=> WorldInfo.JungleMainEntranceX, () => WorldInfo.JungleMainEntranceYapprox, () => SinglePoIPathlengthPrivateJungleMain)    , 
            new PoI(ConstantEnum.PoI.ShimmerLocation, WorldGenPass.Shimmer, ()=> (int) GenVars.shimmerPosition.X , () => (int)GenVars.shimmerPosition.Y , () => SinglePoIPathlengthPrivateShimmer  ) ,            
            new PoI(ConstantEnum.PoI.DungeonBeach, WorldGenPass.Terrain, ()=> (int) WorldInfo.DungeonBeachX , () => (int)WorldInfo.DungeonBeachHeight  ) ,
            new PoI(ConstantEnum.PoI.JungleBeach, WorldGenPass.Terrain, ()=> (int) WorldInfo.JungleBeachX , () => (int)WorldInfo.JungleBeachHeight  , () => SinglePoIPathlengthPrivateJungleBeach) ,


            //new PoI("Snow start", WorldGenPass.Reset, ()=> GenVars.dungeonLocation, () => Main.dungeonY )            
        };

        static List<PoIFun> PoIFunListXY= new List<PoIFun>{
            new PoIFun(ConstraintNames.DistanceEuclid, (a,b) => (int)BasicFunctions.DistanceEuclid(a.X,a.Y,b.X,b.Y )  ),
            new PoIFun(ConstraintNames.HeightAbove, (a,b) => b.Y-a.Y ),                                    
        };
        static List<PoIFun> PoIFunListHori= new List<PoIFun>{                        
            new PoIFun(ConstraintNames.DistanceHorizontal, (a,b) => Math.Abs(b.X-a.X) ),
            new PoIFun(ConstraintNames.WestLeftOf, (a,b) => b.X-a.X ),
            new PoIFun(ConstraintNames.EastRigthOf, (a,b) => a.X-b.X ),
            new PoIFun(ConstraintNames.WestLeftOfGreater0, (a,b) => b.X-a.X, (gole,target,value) => value>=0 && target>=0 ),
            new PoIFun(ConstraintNames.EastRigthOfGreater0, (a,b) => a.X-b.X, (gole,target,value) => value>=0 && target>=0 ),            
            new PoIFun(ConstraintNames.CloserToEndOfWorld, (a,b) => BasicFunctions.EndOfWorldThan2nd(a.X,b.X)  ),
        };

        static List<PoIFunSingle> SinglePoIHeightAbove= new List<PoIFunSingle>{
            new PoIFunSingle(ConstraintNames.HeightAbove + " Underground layer", WorldGenPass.Terrain , (a) => { return (int) (Main.worldSurface-a.Y);}  ),
            new PoIFunSingle(ConstraintNames.HeightAbove + " Cavern layer" , WorldGenPass.Terrain, (a) => (int) (Main.rockLayer-a.Y)  ),
            new PoIFunSingle(ConstraintNames.HeightAbove + " " + ConstraintNames.GraniteMarbleDetonator, WorldGenPass.Terrain, (a) => ((int)GenVars.rockLayer)-a.Y ),            
            new PoIFunSingle(ConstraintNames.HeightAbove + " " + ConstraintNames.LifeCrystalAltarStatue, WorldGenPass.Terrain, (a) => ((int)( (Main.worldSurface * 2.0 + Main.rockLayer) / 3.0 ))-a.Y ), 
            new PoIFunSingle(ConstraintNames.HeightAbove + " " + ConstraintNames.SilverCabinHolesSurfCavesEnd, WorldGenPass.Terrain, (a) => ((int)GenVars.worldSurfaceHigh)-a.Y ),                          
            new PoIFunSingle(ConstraintNames.HeightAbove + " " + ConstraintNames.BuriedChestsStart, WorldGenPass.Terrain, (a) => getStartDepthOfBuriedChest()-a.Y ),                          
        };
        static List<PoIFunSingle> SinglePoIDepthBelow= new List<PoIFunSingle>{
            new PoIFunSingle(ConstraintNames.DepthBelow + " " + ConstraintNames.SkyNPCSpawn , WorldGenPass.Terrain, (a) => a.Y-BasicFunctions.SkyNPCSpawn ),
            new PoIFunSingle(ConstraintNames.DepthBelow + " " + ConstraintNames.WorldSurfaceLow , WorldGenPass.Terrain, (a) => a.Y-(int)GenVars.worldSurfaceLow ),
        };
        static List<PoIFunSingle> SinglePoIPathlength = new List<PoIFunSingle>{
            new PoIFunSingle(ConstraintNames.PathlengthSpawn, WorldGenPass.FinalCleanup, (a) => DataExtractor.pathlength.get(a.X,a.Y) ),
            new PoIFunSingle(ConstraintNames.PathlengthJungleMain, WorldGenPass.FinalCleanup, (a) => DataExtractor.pathlengthFromJungleMainEntrance.get(a.X,a.Y) ),
        };
        static List<PoIFunSingle> SinglePoIPathlengthSpawnPrivate = new List<PoIFunSingle>{
            new PoIFunSingle("pathlength to Underworld", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, Main.maxTilesY-200 )  ),            
            new PoIFunSingle("pathlength to Underworld +40", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, Main.maxTilesY-200+40 )  ),            
            new PoIFunSingle("pathlength to start of lava line", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, (int)GenVars.lavaLine )  ),            
            new PoIFunSingle("pathlength to Cavern layer", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, (int)Main.rockLayer  )  ),            
            new PoIFunSingle("pathlength to Cavern layer +50%", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer))  )  ),            
            new PoIFunSingle("pathlength to Underground layer", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, (int)Main.worldSurface  )  ),            
            new PoIFunSingle("pathlength to Dungeon side beach", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, 100, (int)Main.worldSurface,  GenVars.dungeonSide<0? GenVars.leftBeachEnd: GenVars.rightBeachStart, -1  )  ),     
            new PoIFunSingle("pathlength to Jungle side beach", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlength, a, 100, (int)Main.worldSurface,  GenVars.dungeonSide>0? GenVars.leftBeachEnd: GenVars.rightBeachStart, -1  )  ),     
        };



        static List<PoIFunSingle> SinglePoIPathlengthPrivateJungleMain = new List<PoIFunSingle>{
            new PoIFunSingle("pathlength to Underworld", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, Main.maxTilesY-200 )  ),            
            new PoIFunSingle("pathlength to Underworld +40", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, Main.maxTilesY-200+40 )  ),            
            new PoIFunSingle("pathlength to start of lava line", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, (int)GenVars.lavaLine )  ),            
            new PoIFunSingle("pathlength to Cavern layer", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, (int)Main.rockLayer  )  ),            
            new PoIFunSingle("pathlength to Cavern layer +50%", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer))  )  ),            
            new PoIFunSingle("pathlength to Underground layer", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, (int)Main.worldSurface  )  ),            
            new PoIFunSingle("pathlength to Dungeon side beach", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, 100, (int)Main.worldSurface,  GenVars.dungeonSide<0? GenVars.leftBeachEnd: GenVars.rightBeachStart, -1  )  ),     
            new PoIFunSingle("pathlength to Jungle side beach", WorldGenPass.FinalCleanup, (a) => findMin(ref DataExtractor.pathlengthFromJungleMainEntrance, a, 100, (int)Main.worldSurface,  GenVars.dungeonSide>0? GenVars.leftBeachEnd: GenVars.rightBeachStart, -1  )  ),     

            new PoIFunSingle("near Underworld path early gen. (Jungle)", WorldGenPass.Jungle, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleMainEntrance, Main.maxTilesY-200)  ),
            new PoIFunSingle("near Underworld path early gen. (Hives)", WorldGenPass.Hives, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleMainEntrance, Main.maxTilesY-200)  ),
            new PoIFunSingle("near Cavern layer path early gen. (Jungle)", WorldGenPass.Jungle, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleMainEntrance,(int)Main.rockLayer)  ),
            new PoIFunSingle("near Cavern layer path early gen. (Hives)", WorldGenPass.Hives, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleMainEntrance, (int)Main.rockLayer)  ),
            new PoIFunSingle("near Shimmer path early gen. (Shimmer)", WorldGenPass.Hives, (a) => DataExtractor.quickPathlengthJungleMainEntrance.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y -1 )),
        };
        static List<PoIFunSingle> SinglePoIPathlengthPrivatePredicSpawn = new List<PoIFunSingle>{
            new PoIFunSingle("near Underworld path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, Main.maxTilesY-200 )  ),            
            new PoIFunSingle("near Underworld path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, Main.maxTilesY-200)  ),     
            new PoIFunSingle("near Cavern layer path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.rockLayer )  ),            
            new PoIFunSingle("near Cavern layer path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.rockLayer)  ),      
            new PoIFunSingle("near Underground layer path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.worldSurface )  ),            
            new PoIFunSingle("near Underground layer path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.worldSurface)  ),      
            new PoIFunSingle("near Underground layer path early gen. (SmallHoles)", WorldGenPass.SmallHoles, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.worldSurface)  ),      
            new PoIFunSingle("near Underground layer +20 path early gen. (SmallHoles)", WorldGenPass.SmallHoles, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlength, (int)Main.worldSurface+20)  ),      
        };
        static List<PoIFunSingle> SinglePoIPathlengthPrivateDesertSurf = new List<PoIFunSingle>{
            new PoIFunSingle("near Underground layer path early gen. (FullDesert)", WorldGenPass.FullDesert, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.worldSurface, GenVars.desertHiveLeft, GenVars.desertHiveRight )  ),            
            new PoIFunSingle("near Underground layer path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.worldSurface, GenVars.desertHiveLeft, GenVars.desertHiveRight)  ),     
            new PoIFunSingle("near Underground layer path early gen. (Oasis)", WorldGenPass.Oasis, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.worldSurface, GenVars.desertHiveLeft, GenVars.desertHiveRight)  ),     
            new PoIFunSingle("near Cavern layer path early gen. (FullDesert)", WorldGenPass.FullDesert, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.rockLayer, GenVars.desertHiveLeft, GenVars.desertHiveRight )  ),            
            new PoIFunSingle("near Cavern layer path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.rockLayer, GenVars.desertHiveLeft, GenVars.desertHiveRight )  ),            
            new PoIFunSingle("near Cavern layer path early gen. (Oasis)", WorldGenPass.Oasis, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int)Main.rockLayer, GenVars.desertHiveLeft, GenVars.desertHiveRight)  ),           
            new PoIFunSingle("near deep Item depth path early gen. (FullDesert)", WorldGenPass.FullDesert, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int) ((GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7), GenVars.desertHiveLeft, GenVars.desertHiveRight   )  ),           
            new PoIFunSingle("near deep Item depth path early gen. (LivingTrees)", WorldGenPass.LivingTrees, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int) ((GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7), GenVars.desertHiveLeft, GenVars.desertHiveRight   )  ),           
            new PoIFunSingle("near deep Item depth path early gen. (Oasis)", WorldGenPass.Oasis, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthDesertSurface, (int) ((GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7), GenVars.desertHiveLeft, GenVars.desertHiveRight   )  ),           
        };
        static List<PoIFunSingle> SinglePoIPathlengthPrivateSnowCSurf = new List<PoIFunSingle>{
            new PoIFunSingle("near Underworld path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepthSnow(ref DataExtractor.quickPathlengthSnowCSurface,  Main.maxTilesY-200 )  ),                        
            new PoIFunSingle("near Snow biome end path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepthSnow(ref DataExtractor.quickPathlengthSnowCSurface,  GenVars.lavaLine-140 )  ),
            new PoIFunSingle("near Cavern +50% until end path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepthSnow(ref DataExtractor.quickPathlengthSnowCSurface,  (int)(Main.rockLayer+(GenVars.lavaLine-140))/2 )  ),                        
            new PoIFunSingle("near Cavern layer path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepthSnow(ref DataExtractor.quickPathlengthSnowCSurface, (int)Main.rockLayer )  ),                        
            new PoIFunSingle("near Underground layer path early gen. (MountainCaves)", WorldGenPass.MountainCaves, (a) => FindMinPlToDeepthSnow(ref DataExtractor.quickPathlengthSnowCSurface, (int)Main.worldSurface )  ),                                  
        };
        

        static List<PoIFunSingle> SinglePoIPathlengthPrivateShimmer = new List<PoIFunSingle>{                                                  
            new PoIFunSingle("near path from Jungle beach early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => DataExtractor.quickPathlengthJungleBeach.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y -1 )),
            new PoIFunSingle("near path from Jungle beach early gen. (Hives)", WorldGenPass.Hives, (a) => DataExtractor.quickPathlengthJungleBeach.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y -1 )),            
        };



        static List<PoIFunSingle> SinglePoIPathlengthPrivateJungleBeach = new List<PoIFunSingle>{                                                  
            new PoIFunSingle("near Shimmer path early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => DataExtractor.quickPathlengthJungleBeach.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y -1 )),
            new PoIFunSingle("near Shimmer path early gen. (Hives)", WorldGenPass.Hives, (a) => DataExtractor.quickPathlengthJungleBeach.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y -1 )),
            new PoIFunSingle("near Underworld path early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach,  Main.maxTilesY-200 )),                                                
            new PoIFunSingle("near Underworld path early gen. (Hives)", WorldGenPass.Hives, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach,  Main.maxTilesY-200 )),                                                
            
            new PoIFunSingle("near Cavern layer +50% path early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => findMin(ref DataExtractor.quickPathlengthJungleBeach, a, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer))  )  ),            
            new PoIFunSingle("near Cavern layer +50% path early gen. (Hives)", WorldGenPass.Hives, (a) => findMin(ref DataExtractor.quickPathlengthJungleBeach, a, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer))  )  ),            

            new PoIFunSingle("near Cavern layer path early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach, (int)Main.rockLayer )  ),            
            new PoIFunSingle("near Cavern layer path early gen. (Hives)", WorldGenPass.Hives, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach, (int)Main.rockLayer)  ),      
            new PoIFunSingle("near Underground layer path early gen. (Shimmer)", WorldGenPass.Shimmer, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach, (int)Main.worldSurface )  ),            
            new PoIFunSingle("near Underground layer path early gen. (Hives)", WorldGenPass.Hives, (a) => FindMinPlToDeepth(ref DataExtractor.quickPathlengthJungleBeach, (int)Main.worldSurface)  ),      
        };



        public static int FindMinPlToDeepth(ref Pathlength.Pathlength pl, int fy, int restrictXLeft = -1, int restrictXRight = -1){
            int min = Int32.MaxValue;
            int from = restrictXLeft<0?50:restrictXLeft; from = Math.Max(from,20);
            int to = restrictXRight<0?Main.maxTilesX-50:restrictXRight; to = Math.Min(Main.maxTilesX-20, to);

            for(int xi=from; xi < to;xi+=1){                               
                    min = Math.Min(pl.at(xi,fy), min);
            }
            return min;
        }

        public static int FindMinPlToDeepthSnow(ref Pathlength.Pathlength pl, int fy){ 
            int min = Int32.MaxValue;
            int from = fy > GenVars.lavaLine-140? GenVars.snowMinX[GenVars.lavaLine-140]-(fy-(GenVars.lavaLine-140) )  : GenVars.snowMinX[fy];  from = Math.Max(from,20);
            int to = fy > GenVars.lavaLine-140? GenVars.snowMaxX[GenVars.lavaLine-140]+(fy-(GenVars.lavaLine-140) )  : GenVars.snowMaxX[fy];  to = Math.Min(Main.maxTilesX-20, to);
            
            for(int xi=from; xi < to;xi+=1){                               
                    min = Math.Min(pl.at(xi,fy), min);
            }
            return min;
        }

        public static int findMin(ref Pathlength.Pathlength pl, int fy, int ty = -5, int fx = 40, int tx = 0  ){
            if(tx<0) tx = fx -tx;
            else if(tx==0) tx = Main.maxTilesX -40;
            if(ty<0) ty = fy -ty;

            int min = Int32.MaxValue;
            for(int yi=fy;yi<ty;yi+=1)
            for(int xi=fx;xi<tx;xi+=1){                    
                min = Math.Min(pl.at(xi,yi), min);
            }
            return min;
        }

        static int findMin(ref Pathlength.Pathlength pl, PoI a, int fy, int ty = -5, int fx = 40, int tx = 0  ){
                                   
            return findMin(ref pl, fy, ty, fx, tx  );            
        }

        public static int getStartDepthOfBuriedChest(){
            return ((int)((GenVars.worldSurfaceHigh + 20.0 + Main.rockLayer) / 2.0));

        }



        static List<PoIFunSingle> SinglePoIDHorizontal = new List<PoIFunSingle>{
            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.StartOfSnowBiomeGen , WorldGenPass.Reset, (a) => BasicFunctions.HorizontalDistToStartOfIceBiomeGen(a.X)   ),
            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestMountaincaveEntrance, WorldGenPass.MountCaves, (a) => BasicFunctions.HorizontalDistToMountainCaveEntrance(a.X)   ),
            new PoIFunSingle(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestMountaincaveEntrance , WorldGenPass.MountCaves, (a) => BasicFunctions.HorizontalDistToMountainCaveEntrance(a.X,true)   ),

            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestOasis, WorldGenPass.Oasis, (a) => BasicFunctions.HorizontalDisToOasis(a.X)   ),
            new PoIFunSingle(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestOasis , WorldGenPass.Oasis, (a) => BasicFunctions.HorizontalDisToOasis(a.X, true)   ),
            
            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestPyramid, WorldGenPass.Pyramids, (a) => BasicFunctions.HorizontalDistToPyramid(a.X)   ),
            new PoIFunSingle(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestPyramid , WorldGenPass.Pyramids, (a) => BasicFunctions.HorizontalDistToPyramid(a.X, true)   ),

            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestPotentialPyramid, WorldGenPass.Dunes, (a) => BasicFunctions.HorizontalDistToPyramid(a.X, false, true)   ),
            new PoIFunSingle(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestPotentialPyramid , WorldGenPass.Dunes, (a) => BasicFunctions.HorizontalDistToPyramid(a.X, true, true)   ),


            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.StartOfBeach , WorldGenPass.Reset, (a) => BasicFunctions.HorizontalDistToBeach(a.X)   ),
            new PoIFunSingle(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.EndofWorld , WorldGenPass.Reset, (a) => BasicFunctions.HorizontalDistToEndOfWorld(a.X)   ),
        };


        static PCPointOfInterestList()
        {               
            //AddToList(ConstantEnum.PCPointOfInterestTypes.All, ConstantEnum.WorldGenPass.Reset, PoIList );
            for(int i=0; i< PoIList.Count;i++){
                AddToList(PoIList[i].name, PoIList[i].firstPossibleEntryPoint, new List<PoI>{PoIList[i] } );
            }
        }
     
        static void AddToList(string name, ConstantEnum.WorldGenPass FirstEntryPoint, List<PoI> poiList)
        {
            if (pcPointOfInteerestConditionList == null) pcPointOfInteerestConditionList = new List<Condition>();
            PropertyElement propel = new PropertyElement { CondType = ConstantEnum.PropertyType.Unique };

            List<Constraint> condelli = new List<Constraint>();

            foreach (PoI poi in poiList)            
            {
                foreach (PoI poi2 in PoIList)            
                {
                    if(poi.name.Equals(poi2.name)) continue;
                    WorldGenPass entry = poi.firstPossibleEntryPoint > poi2.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poi2.firstPossibleEntryPoint;

                    if(!poi.IsOnlyHori && !poi2.IsOnlyHori)
                    foreach(PoIFun poif in PoIFunListXY){
                        Constraint condel = new Constraint(poi.name + " " + poif.name + " " + poi2.name, (p,c) =>{return poif.poiFun(poi,poi2);}, ConditionElementValueList.defaultDistFunX, entry,0, extraEvalFun:poif.extraEvalFun );
                        condelli.Add(condel);
                    }

                    foreach(PoIFun poif in PoIFunListHori){
                        Constraint condel = new Constraint(poi.name + " " + poif.name + " " + poi2.name, (p,c) =>{return poif.poiFun(poi,poi2);}, ConditionElementValueList.defaultDistFunX, entry,0, extraEvalFun:poif.extraEvalFun );
                        condelli.Add(condel);
                    }
                }
                //todo use same foreach
                if(!poi.IsOnlyHori){
                    foreach(PoIFunSingle poifs in SinglePoIHeightAbove){                                            
                        WorldGenPass entryThis = poi.firstPossibleEntryPoint > poifs.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poifs.firstPossibleEntryPoint;
                        Constraint condel = new Constraint(poi.name + " " + poifs.name, (p,c) => {return poifs.poiFun(poi);}, ConditionElementValueList.defaultDistFunY, entryThis,0  );
                        condelli.Add(condel);
                    }
                    foreach(PoIFunSingle poifs in SinglePoIDepthBelow){                        
                        WorldGenPass entryThis = poi.firstPossibleEntryPoint > poifs.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poifs.firstPossibleEntryPoint;
                        Constraint condel = new Constraint(poi.name + " " + poifs.name, (p,c) => {return poifs.poiFun(poi);}, ConditionElementValueList.defaultDistFunY, entryThis,0);
                        condelli.Add(condel);
                    }
                    foreach(PoIFunSingle poifs in SinglePoIPathlength){     
                        if(poifs.name.Contains(poi.name) )           continue;        
                        WorldGenPass entryThis = poi.firstPossibleEntryPoint > poifs.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poifs.firstPossibleEntryPoint;
                        Constraint condel = new Constraint(poi.name + " " + poifs.name, (p,c) => {return poifs.poiFun(poi);}, ConditionElementValueList.defaultDistFunY, entryThis,0);
                        condelli.Add(condel);
                    }
                    
                }

                foreach(PoIFunSingle poifs in SinglePoIDHorizontal){                        
                    WorldGenPass entryThis = poi.firstPossibleEntryPoint > poifs.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poifs.firstPossibleEntryPoint;
                    Constraint condel = new Constraint(poi.name + " " + poifs.name, (p,c) => {return poifs.poiFun(poi);}, ConditionElementValueList.defaultDistFunX, entryThis,0  );
                    condelli.Add(condel);
                }
                
                if( poi.privateFunList != null)
                foreach(PoIFunSingle poifs in poi.privateFunList()){                        
                    WorldGenPass entryThis = poi.firstPossibleEntryPoint > poifs.firstPossibleEntryPoint ? poi.firstPossibleEntryPoint: poifs.firstPossibleEntryPoint;
                    Constraint condel = new Constraint(poi.name + " " + poifs.name, (p,c) => {return poifs.poiFun(poi);}, ConditionElementValueList.defaultDistFunX, entryThis,0  );                
                    condelli.Add(condel);
                }


            }




            Condition cond = new Condition(ConditionType.PointOfInterest, name, propel,condelli){isPersistent=true};//all Persistent?
            //cond.firstEntryPoint = FirstEntryPoint; 

            pcPointOfInteerestConditionList.Add(cond);
        }


    }
}