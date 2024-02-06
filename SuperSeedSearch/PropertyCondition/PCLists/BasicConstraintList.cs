using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.WorldBuilding;
using SuperSeedSearch.WorldGenMod;
using SuperSeedSearch.Storage;
using SuperSeedSearch.Helpers;
using SuperSeedSearch.ConstantEnum;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using SuperSeedSearch.PropertyCondition.PCPoint;


namespace SuperSeedSearch.PropertyCondition
{
    public static class BasicConstraintList
    {
        //public static readonly List<string> TrueFalseList = new List<string>{"123", "1234" };//(Helpers.EnumHelper<TrueFalse>.GetAllEnumVariablesAsString()); //doestn work here but it works if it is inside ContainsListItem.cs oO
        
        //todo unify with POI
        public static readonly List<Constraint> pcBasicDistanceConstraintList = new List<Constraint>(){
                ConditionElmWithValues(ConstraintNames.DistanceToMidWorldHorizontal, (a,b) => Helpers.BasicFunctions.DistanceToMidWorldHorizontal(a.posX) ,WorldGenPass.Reset  ),
                ConditionElmWithValues(ConstraintNames.DistanceToPredictedSpawndHorizontal, (a,b) => Math.Abs(a.posX-WorldInfo.PredictedSpawnX) ,WorldGenPass.Terrain  ),
                ConditionElmWithValues(ConstraintNames.DistanceToSpawndHorizontal, (a,b) => Helpers.BasicFunctions.DistanceToSpawnHorizontal (a.posX)  ,WorldGenPass.SpawnPoint ),                
                ConditionElmWithValues(ConstraintNames.DistanceToDungeonEarlyHorizontal, (a,b) => Helpers.BasicFunctions.DistanceToDungeonXHorizontal (a.posX)  ,WorldGenPass.Reset ),                
                ConditionElmWithValues(ConstraintNames.DistanceToDungeonHorizontal, (a,b) => Math.Abs(a.posX-GenVars.dungeonX)  ,WorldGenPass.Dungeon ),                
                ConditionElmWithValues(ConstraintNames.DistanceToJungleMainEntranceHorizontal, (a,b) => Helpers.BasicFunctions.DistanceToJungleMainEntranceHorizontal(a.posX)  ,WorldGenPass.Jungle ),                
                ConditionElmWithValues(ConstraintNames.CloserToEndOfWorld + " " + ConstraintNames.DungeonFirstRoomEarly, (a,b) => Helpers.BasicFunctions.EndOfWorldThan2nd(a.posX, GenVars.dungeonLocation)  ,WorldGenPass.Reset ),                
                ConditionElmWithValues(ConstraintNames.CloserToEndOfWorld + " " + ConstraintNames.Dungeon, (a,b) => Helpers.BasicFunctions.EndOfWorldThan2nd(a.posX, GenVars.dungeonX)  ,WorldGenPass.Dungeon ),                                
                ConditionElmWithValues(ConstraintNames.DistanceToSpawndEuclid, (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY,Main.spawnTileX, Main.spawnTileY, b.ParameterValue1.IsParameterSet && b.ParameterValue1.name.Equals(ConstraintNames.DistanceToSpawndEuclidParam1)?(double)b.ParameterValue1:1 ) ,WorldGenPass.SpawnPoint  ),
                ConditionElmWithValues(ConstraintNames.DistanceToPredSpawndEuclid, (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY, WorldInfo.PredictedSpawnX, WorldInfo.PredictedSpawnHeight, b.ParameterValue1.IsParameterSet && b.ParameterValue1.name.Equals(ConstraintNames.DistanceToSpawndEuclidParam1)?(double)b.ParameterValue1:1 ) ,WorldGenPass.Terrain  ),
                ConditionElmWithValues(ConstraintNames.DistanceToJungleMainEntranceEuclid, (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY, WorldInfo.JungleMainEntranceX, WorldInfo.JungleMainEntranceYapprox, b.ParameterValue1.IsParameterSet && b.ParameterValue1.name.Equals(ConstraintNames.DistanceToSpawndEuclidParam1)?(double)b.ParameterValue1:1 ) ,WorldGenPass.Jungle  ),
                ConditionElmWithValues(ConstraintNames.HeightAboveUndergroundLayer, (a,b) => Helpers.BasicFunctions.HeightAbove2nd (a.posY, (int)Main.worldSurface) ,WorldGenPass.Terrain  ),
                ConditionElmWithValues(ConstraintNames.HeightAboveCavernUndergroundLayer, (a,b) => Helpers.BasicFunctions.HeightAbove2nd (a.posY, (int) Main.rockLayer) ,WorldGenPass.Terrain  ),
                ConditionElmWithValues(ConstraintNames.DepthBelowPredictedSpawn, (a,b) => Helpers.BasicFunctions.DepthBelowPredictedSpawn (a.posY)  ,WorldGenPass.Terrain ),
                ConditionElmWithValues(ConstraintNames.DepthBelowSpawn, (a,b) => (a.posY-Main.spawnTileY )  ,WorldGenPass.SpawnPoint ),
                ConditionElmWithValues(ConstraintNames.DepthBelowJungleMainEntrance, (a,b) => (a.posY- WorldInfo.JungleMainEntranceYapprox )  ,WorldGenPass.Jungle ),
                ConditionElmWithValues(ConstraintNames.PathlengthSpawn, (a,b) =>  DataExtractor.pathlength.get(a.posX,a.posY) ,WorldGenPass.FinalCleanup ),
                ConditionElmWithValues(ConstraintNames.PathlengthJungleMain, (a,b) =>  DataExtractor.pathlengthFromJungleMainEntrance.get(a.posX,a.posY) ,WorldGenPass.FinalCleanup ),
                ConditionElmWithValues(ConstraintNames.PathlengthSpawnReducedDepth, (a,b) =>  DataExtractor.pathlength.getReducedByBestDepthPl(a.posX,a.posY) ,WorldGenPass.FinalCleanup ),
                ConditionElmWithValues(ConstraintNames.PathlengthJungleMainReducedDepth, (a,b) =>  DataExtractor.pathlengthFromJungleMainEntrance.getReducedByBestDepthPl(a.posX,a.posY) ,WorldGenPass.FinalCleanup ), 

                ConditionElmWithValues(ConstraintNames.PathlengthQuickPredictedSpawn, (a,b) =>  DataExtractor.quickPathlength.get(a.posX,a.posY) ,WorldGenPass.SmallHoles ),
                ConditionElmWithValues(ConstraintNames.PathlengthQuickJungleMain, (a,b) =>  DataExtractor.quickPathlengthJungleMainEntrance.get(a.posX,a.posY) ,WorldGenPass.Jungle ),
                ConditionElmWithValues(ConstraintNames.PathlengthQuickSnowCSurface, (a,b) =>  DataExtractor.quickPathlengthSnowCSurface.get(a.posX,a.posY) ,WorldGenPass.GenerateIceBiome ),
                ConditionElmWithValues(ConstraintNames.PathlengthQuickDesertCSurface, (a,b) =>  DataExtractor.quickPathlengthDesertSurface.get(a.posX,a.posY) ,WorldGenPass.FullDesert ),
                ConditionElmWithValues(ConstraintNames.PathlengthQuickShimmer, (a,b) =>  DataExtractor.quickPathlengthJungleBeach.get(a.posX,a.posY) ,WorldGenPass.Shimmer ),



                ConditionElmWithValues(ConstraintNames.TilesOnDungeonSide, (a,b) =>  GenVars.dungeonSide<0?Main.maxTilesX/2-a.posX:a.posX-Main.maxTilesX/2  ,WorldGenPass.Reset ),
                ConditionElmWithValues(ConstraintNames.TilesOnTempleSide, (a,b) =>  GenVars.dungeonSide<0?a.posX-Main.maxTilesX/2:Main.maxTilesX/2-a.posX  ,WorldGenPass.Reset ),
                ConditionElmWithValues(ConstraintNames.HasTilesNearby, HasTilesNearbyList.HasTilesNearby ,WorldGenPass.Oasis ),//Todo selection related last pass
                ConditionElmWithValues(ConstraintNames.HasTilesAbove, HasTilesNearbyList.HasTilesAbove ,WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasTilesBelow, HasTilesNearbyList.HasTilesBelow ,WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasTilesAboveBelow, HasTilesNearbyList.HasTilesAboveBelow ,WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasTilesLeft, HasTilesNearbyList.HasTilesLeft ,WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasTilesRight, HasTilesNearbyList.HasTilesRight ,WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasTilesLeftRight, HasTilesNearbyList.HasTilesLeftRight, WorldGenPass.Oasis ),//..
                ConditionElmWithValues(ConstraintNames.HasBGWallNearby, HasBGWallNearbyList.HasWallNearby ,WorldGenPass.Oasis ),//Todo selection related last pass
                ConditionElmWithValues(ConstraintNames.HasBGWallDifferentNearby, HasBGWallNearbyList.HasWallDifferentThanNearby ,WorldGenPass.Oasis ),//Todo selection related last pass
                ConditionElmWithValues(ConstraintNames.WestLeftOfMid, (a,b) =>  Main.maxTilesX/2 - a.posX ,WorldGenPass.Reset ),
                ConditionElmWithValues(ConstraintNames.EastRightOfMid, (a,b) =>  -Main.maxTilesX/2 + a.posX ,WorldGenPass.Reset ),


                ConditionElmWithValues(ConstraintNames.XCoordinate, (a,b) =>  a.posX ,WorldGenPass.Reset ),
                ConditionElmWithValues(ConstraintNames.YCoordinate, (a,b) =>  a.posY ,WorldGenPass.Reset ),


                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.StartOfSnowBiomeGen , (a,b) => BasicFunctions.HorizontalDistToStartOfIceBiomeGen(a.posX), WorldGenPass.Reset   ),
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestMountaincaveEntrance, (a,b) => BasicFunctions.HorizontalDistToMountainCaveEntrance(a.posX), WorldGenPass.MountCaves   ),
                ConditionElmWithValues(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestMountaincaveEntrance , (a,b) => BasicFunctions.HorizontalDistToMountainCaveEntrance(a.posX, true),  WorldGenPass.MountCaves   ),
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.StartOfBeach , (a,b) => BasicFunctions.HorizontalDistToBeach(a.posX), WorldGenPass.Reset    ),
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.EndofWorld , (a,b) => BasicFunctions.HorizontalDistToEndOfWorld(a.posX), WorldGenPass.Reset ),

                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestPyramid, (a,b) => BasicFunctions.HorizontalDistToPyramid(a.posX), WorldGenPass.Pyramids   ),
                ConditionElmWithValues(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestPyramid , (a,b) => BasicFunctions.HorizontalDistToPyramid(a.posX, true),  WorldGenPass.Pyramids   ),

                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestPotentialPyramid, (a,b) => BasicFunctions.HorizontalDistToPyramid(a.posX, false, true), WorldGenPass.Dunes   ),
                ConditionElmWithValues(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestPotentialPyramid , (a,b) => BasicFunctions.HorizontalDistToPyramid(a.posX, true, true),  WorldGenPass.Dunes   ),


                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + ConstraintNames.ClosestOasis, (a,b) => BasicFunctions.HorizontalDisToOasis(a.posX), WorldGenPass.Oasis   ),
                ConditionElmWithValues(ConstraintNames.DifferenceHorizontalToMid + " " + "for " + ConstraintNames.ClosestOasis , (a,b) => BasicFunctions.HorizontalDisToOasis(a.posX, true),  WorldGenPass.Oasis   ),

            
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + PoI.ShimmerLocation , (a,b) => Math.Abs(GenVars.shimmerPosition.X - a.posX), WorldGenPass.Shimmer ),
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + PoI.SnowSurfaceCenter , (a,b) => Math.Abs(WorldInfo.SnowTopOfSurfaceCenterX - a.posX), WorldGenPass.GenerateIceBiome ),
                ConditionElmWithValues(ConstraintNames.DistanceHorizontal + " " + PoI.DesertHiveSurfaceCenter , (a,b) => Math.Abs(WorldInfo.DesertTopOfSurfaceCenterX - a.posX), WorldGenPass.FullDesert ),

                ConditionElmWithValues(ConstraintNames.DistanceEuclid + " " + PoI.ShimmerLocation , (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY, (int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y), WorldGenPass.Shimmer ),
                ConditionElmWithValues(ConstraintNames.DistanceEuclid + " " + PoI.SnowSurfaceCenter , (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY, WorldInfo.SnowTopOfSurfaceCenterX, WorldInfo.SnowTopOfSurfaceCenterY), WorldGenPass.GenerateIceBiome ),
                ConditionElmWithValues(ConstraintNames.DistanceEuclid + " " + PoI.DesertHiveSurfaceCenter , (a,b) => Helpers.BasicFunctions.DistanceEuclid(a.posX,a.posY, WorldInfo.DesertTopOfSurfaceCenterX, WorldInfo.DesertTopOfSurfaceCenterYapprox), WorldGenPass.FullDesert ),

                ConditionElmWithValues(ConstraintNames.DepthBelow + " " + PoI.ShimmerLocation , (a,b) => -(GenVars.shimmerPosition.Y - a.posY), WorldGenPass.Shimmer ),
                ConditionElmWithValues(ConstraintNames.DepthBelow + " " + PoI.SnowSurfaceCenter , (a,b) => -(WorldInfo.SnowTopOfSurfaceCenterY - a.posY), WorldGenPass.GenerateIceBiome ),
                ConditionElmWithValues(ConstraintNames.DepthBelow + " " + PoI.DesertHiveSurfaceCenter , (a,b) => -(WorldInfo.DesertTopOfSurfaceCenterYapprox - a.posY), WorldGenPass.FullDesert ),


            };

        public static readonly List<Constraint> pcTileConstraintList = new List<Constraint>(){
            ConditionElmWithValues(ConstraintNames.IDTile, (a,b) => a.IDTile ,WorldGenPass.Reset, DisplayFunEach ),
            ConditionElmWithValues(ConstraintNames.IDWall, (a,b) => a.IDWall ,WorldGenPass.Reset, DisplayFunEach ),
            ConditionElmWithValues(ConstraintNames.FrameX, (a,b) => a.frameX ,WorldGenPass.Reset, DisplayFunEach ),            
            ConditionElmWithValues(ConstraintNames.FrameY, (a,b) => a.frameY ,WorldGenPass.Reset, DisplayFunEach ),            
        };

        public static readonly List<Constraint> pcRNGNValueConstraintList = new List<Constraint>(){
                ConditionElmWithValues(ConstraintNames.RNGN1, (a,b) => WorldInfo.RNGNumbers.RNGN1 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN2, (a,b) => WorldInfo.RNGNumbers.RNGN2 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN3, (a,b) => WorldInfo.RNGNumbers.RNGN3 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN4, (a,b) => WorldInfo.RNGNumbers.RNGN4 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN5, (a,b) => WorldInfo.RNGNumbers.RNGN5 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN6, (a,b) => WorldInfo.RNGNumbers.RNGN6 ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.RNGN7, (a,b) => WorldInfo.RNGNumbers.RNGN7 ,WorldGenPass.PreWorldGen, DisplayFunEach ),                
                ConditionElmWithValues(ConstraintNames.RNGN8, (a,b) => WorldInfo.RNGNumbers.RNGN8 ,WorldGenPass.PreWorldGen, DisplayFunEach ),                
                ConditionElmWithValues(ConstraintNames.RNGN9, (a,b) => WorldInfo.RNGNumbers.RNGN9 ,WorldGenPass.PreWorldGen, DisplayFunEach ),                
            };

        public static readonly List<Constraint> pcRNGNValueSeriesConstraintList = new List<Constraint>(){
                ConditionElmWithValues(ConstraintNames.RNGNidelta, (a,b) => b.targetValue ,WorldGenPass.PreWorldGen, DisplayFunEach ),                    
                ConditionElmWithValues(ConstraintNames.RNGNiParameters, (a,b) => b.targetValue ,WorldGenPass.PreWorldGen, DisplayFunEach ),                    
            };



        public static readonly List<Constraint> pcWorldStyleConstraintList = new List<Constraint>(){
                ConditionElmWithValues(ConstraintNames.DungeonColor, (a,b) => PropertyCondition.WorldStyles.pcDungeonColor[(int)(WorldInfo.RNGNumbers.RNGN2*3)] ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.DungeonSide, (a,b) => PropertyCondition.WorldStyles.pcLeftRightSide[(GenVars.dungeonSide+1)/2 ] ,WorldGenPass.Reset, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.HardmodeHallowSide, (a,b) => PropertyCondition.WorldStyles.pcLeftRightSide[1-(int)(WorldInfo.RNGNumbers.RNGN3*2)] ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.HardmodeHallowAtJungleSnow, (a,b) => PropertyCondition.WorldStyles.pcJungelDungeonSide[GenVars.dungeonSide*(1-2*(int)(WorldInfo.RNGNumbers.RNGN3*2))<0?0:1] ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.MoonType, (a,b) => PropertyCondition.WorldStyles.pcMoonType[Main.moonType] ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.OreCopperOrTin, (a,b) => PropertyCondition.WorldStyles.pcOreCopperOrTin[WorldGen.SavedOreTiers.Copper==TileID.Copper?0:1] ,WorldGenPass.Reset, DisplayFunOres ),
                ConditionElmWithValues(ConstraintNames.OreIronOrLead, (a,b) => PropertyCondition.WorldStyles.pcOreIronOrLead[WorldGen.SavedOreTiers.Iron==TileID.Iron?0:1] ,WorldGenPass.Reset, DisplayFunOres ),
                ConditionElmWithValues(ConstraintNames.OreSilverOrTungsten, (a,b) => PropertyCondition.WorldStyles.pcOreSilverOrTungsten[WorldGen.SavedOreTiers.Silver==TileID.Silver?0:1] ,WorldGenPass.Reset, DisplayFunOres ),
                ConditionElmWithValues(ConstraintNames.OreGoldOrPlatinum, (a,b) => PropertyCondition.WorldStyles.pcOreGoldOrPlatinum[WorldGen.SavedOreTiers.Gold==TileID.Gold?0:1] ,WorldGenPass.Reset, DisplayFunOres ),
                ConditionElmWithValues(ConstraintNames.UndergroundLayerSize, (a,b) => Main.rockLayer-Main.worldSurface ,WorldGenPass.Terrain, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.CavernLayerSize, (a,b) => Main.maxTilesY-200-Main.rockLayer ,WorldGenPass.Terrain, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.UndergroundLayerTilesFromTopWorld, (a,b) => Main.worldSurface ,WorldGenPass.Terrain, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.CavernLayerTilesFromTopWorld, (a,b) => Main.rockLayer ,WorldGenPass.Terrain, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.UndergroundLayerTilesToSkyNPCSpawn, (a,b) => Main.worldSurface -  BasicFunctions.SkyNPCSpawn ,WorldGenPass.Terrain, DisplayFunEach ),
                
                ConditionElmWithValues(ConstraintNames.LavaLineDepth, (a,b) => Main.maxTilesY-GenVars.lavaLine ,WorldGenPass.Terrain, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.WorldSurface2BuriedChestsStart, (a,b) => PCPointOfInterestList.getStartDepthOfBuriedChest()-Main.worldSurface ,WorldGenPass.Terrain, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.GlowingMossType, (a,b) => PropertyCondition.WorldStyles.pcGlowingMossType[(int)(WorldInfo.RNGNumbers.RNGN1*4)] ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.JungleShrineType, (a,b) => PropertyCondition.WorldStyles.pcJungleShrineType[(int)(WorldInfo.RNGNumbers.RNGN1*5)] ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.OceanCaveDungeon, (a,b) => PropertyCondition.WorldStyles.pcOceanCaveDungeon[((int)(WorldInfo.RNGNumbers.RNGN1*( (WorldGen.drunkWorldGen || WorldGen.tenthAnniversaryWorldGen) ? 1:(WorldGen.remixWorldGen?2:3)) ))>0?1:0 ] ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                                
                ConditionElmWithValues(ConstraintNames.SalamanderGiantShellyCrawdad_Salamander, (a,b) => (SalamanderGiantShellyCrawdad_Stats.HasType(b.targetValue.valueString)?"":"not ")+ b.targetValue.valueString  ,WorldGenPass.PreWorldGen, DisplayFunEach ),                
                ConditionElmWithValues(ConstraintNames.SalamanderGiantShellyCrawdad_Crawdad, (a,b) => (SalamanderGiantShellyCrawdad_Stats.HasType(b.targetValue.valueString)?"":"not ")+ b.targetValue.valueString  ,WorldGenPass.PreWorldGen, DisplayFunEach ),                
                ConditionElmWithValues(ConstraintNames.SalamanderGiantShellyCrawdad_GiantShelly, (a,b) => (SalamanderGiantShellyCrawdad_Stats.HasType(b.targetValue.valueString)?"":"not ")+ b.targetValue.valueString  ,WorldGenPass.PreWorldGen, DisplayFunEach ),                

                ConditionElmWithValues(ConstraintNames.SalamanderGiantShellyCrawdad_UniqueCount, (a,b) => SalamanderGiantShellyCrawdad_Stats.HasCount() ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.SalamanderGiantShellyCrawdad_Extinct, (a,b) => (SalamanderGiantShellyCrawdad_Stats.IsExtinct(b.targetValue.valueString)?"":"has ")+b.targetValue.valueString ,WorldGenPass.PreWorldGen, DisplayFunEach ),
                
               
                ConditionElmWithValues(ConstraintNames.BackgroundStyle, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForest1, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,1)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForest2, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,2)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForest3, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,3)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForest4, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,4)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavern1, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,1)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavern2, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,2)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavern3, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,3)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavern4, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,4)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForestChange1, (a,b) => ((float)Main.treeX[0]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForestChange2, (a,b) => ((float)Main.treeX[1]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleForestChange3, (a,b) => ((float)Main.treeX[2]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavernChange1, (a,b) => ((float)Main.caveBackX[0]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavernChange2, (a,b) => ((float)Main.caveBackX[1]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.BackgroundStyleCavernChange3, (a,b) => ((float)Main.caveBackX[2]*100)/Main.maxTilesX ,WorldGenPass.Reset, DisplayFunEach ),


                ConditionElmWithValues(ConstraintNames.TreeStyle, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,-1,true)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.TreeStyle1, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,1,true)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.TreeStyle2, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,2,true)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.TreeStyle3, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,3,true)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.TreeStyle4, (a,b) => (Backgrounds.HasStyle(b.targetValue.valueString,4,true)?"":"not ")+b.targetValue.valueString ,WorldGenPass.Reset, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.DesertStyleGuess, (a,b) => GuessDesertStyle() ,WorldGenPass.FullDesert, DisplayFunEach ),

                ConditionElmWithValues(ConstraintNames.ExtensionDesertlPyramid, (a,b) => (b.targetValue<=BasicFunctions.HorizontalDistToPyramid(WorldInfo.DesertTopOfSurfaceCenterX, returnNumWithDiff:(GenVars.desertHiveRight-GenVars.desertHiveLeft) )?"":"not ")+b.targetValue.valueDouble ,WorldGenPass.Pyramids, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.ExtensionDesertOasis, (a,b) => (b.targetValue<=BasicFunctions.HorizontalDisToOasis(WorldInfo.DesertTopOfSurfaceCenterX, returnNumWithDiff:(GenVars.desertHiveRight-GenVars.desertHiveLeft) )?"":"not ")+b.targetValue.valueDouble ,WorldGenPass.Oasis, DisplayFunEach ),
                ConditionElmWithValues(ConstraintNames.ExtensionDesertPotentialPyramid, (a,b) => (b.targetValue<=BasicFunctions.HorizontalDistToPyramid(WorldInfo.DesertTopOfSurfaceCenterX,false,true,  returnNumWithDiff:(GenVars.desertHiveRight-GenVars.desertHiveLeft)  )?"":"not ")+b.targetValue.valueDouble ,WorldGenPass.Dunes, DisplayFunEach ),
            };

       

        public static class Backgrounds{     

            public static bool HasStyle(string name, int posID = -1, bool treeStyle= false){
                string[] nameAndNumber = name.Split(WorldStyles.bgStyleDivider);
                if(nameAndNumber.Length<2) return false;
                string bgname = nameAndNumber[0].Trim();
                string bgidstr = nameAndNumber[1].Trim();
                int bgid = -1;
                if(!Int32.TryParse(bgidstr,out bgid)) return false;

                if(posID>0){
                        if(posID>2 && Main.maxTilesX == 4200) return false;
                        if(posID>3 && Main.maxTilesX == 6400) return false;
                        if(posID>4) return false;
                }

                if(treeStyle){
                    if(!bgname.Equals(WorldStyles.TreeStyle)) return false;

                    if(posID>0){                    
                        return Main.treeStyle[posID-1] ==bgid ;

                    }else{
                        return (Main.treeStyle[0] ==bgid || Main.treeStyle[1] ==bgid || (Main.treeStyle[2]==bgid && Main.maxTilesX!=4200) || (Main.treeStyle[3]==bgid && Main.maxTilesX!=6400 && Main.maxTilesX!=4200 ));

                    }                    
                }

                if(posID>0){
                    if(bgname.Equals(WorldStyles.ForestBackground)){
                        if( (posID == 1 && bgid==WorldGen.treeBG1) ||
                            (posID == 2 && bgid==WorldGen.treeBG2) ||
                            (posID == 3 && bgid==WorldGen.treeBG3) ||
                            (posID == 4 && bgid==WorldGen.treeBG4))
                            return true;
                    }
                    if(bgname.Equals(WorldStyles.CavernBackground)){
                        if( (posID <= 4 && bgid==Main.caveBackStyle[posID-1]))
                            return true;
                    }
                    return false;
                }

                if(bgname.Equals(WorldStyles.ForestBackground) && (WorldGen.treeBG1 ==bgid || WorldGen.treeBG2 ==bgid || (WorldGen.treeBG3==bgid && Main.maxTilesX!=4200) || (WorldGen.treeBG4==bgid && Main.maxTilesX!=6400 && Main.maxTilesX!=4200 ))) return true;
                if(bgname.Equals(WorldStyles.CavernBackground) && ((Main.caveBackStyle[0]==bgid) || (Main.caveBackStyle[1]==bgid) || (Main.caveBackStyle[2]==bgid && Main.maxTilesX!=4200) || (Main.caveBackStyle[3]==bgid && Main.maxTilesX!=6400 && Main.maxTilesX!=4200 ))) return true;
                if(bgname.Equals(WorldStyles.CavernIceBackground) && Main.iceBackStyle==bgid) return true;
                if(bgname.Equals(WorldStyles.CavernHellBackground) && Main.hellBackStyle==bgid) return true;
                if(bgname.Equals(WorldStyles.CavernJungleBackground) && Main.jungleBackStyle==bgid) return true;
                if(bgname.Equals(WorldStyles.CorruptionBackground) && WorldGen.corruptBG==bgid) return true;
                if(bgname.Equals(WorldStyles.JungleBackground) && WorldGen.jungleBG==bgid) return true;
                if(bgname.Equals(WorldStyles.SnowBackground) && WorldGen.snowBG==bgid) return true;
                if(bgname.Equals(WorldStyles.HallowBackground) && WorldGen.hallowBG==bgid) return true;
                if(bgname.Equals(WorldStyles.CrimsonBackground) && WorldGen.crimsonBG==bgid) return true;
                if(bgname.Equals(WorldStyles.DesertBackground) && WorldGen.desertBG==bgid) return true;
                if(bgname.Equals(WorldStyles.OceanBackground) && WorldGen.oceanBG==bgid) return true;
                if(bgname.Equals(WorldStyles.MushroomBackground) && WorldGen.mushroomBG==bgid) return true;
                if(bgname.Equals(WorldStyles.UnderworldBackground) && WorldGen.underworldBG==bgid) return true;
                

                return false;
            }

        }


        public static class SalamanderGiantShellyCrawdad_Stats{            
            static int seedComp = -1;
            static HashSet<string> types = new HashSet<string>();        
            static HashSet<string> notExtinct = new HashSet<string>();
            static void ProofAndGenIfNot(){
                if ( WorldInfo.RNGNumbers.forSeed != seedComp || NPC.cavernMonsterType[0,0] == 0 || types.Count == 0){
                    types.Clear();notExtinct.Clear();

                    int WorldIDComp = (int)(( 1341148412L*(WorldInfo.RNGNumbers.forSeed)+ 65212616L)%((1L<<31)-1));// 16th value
                    Main.worldID = WorldIDComp;

                    NPC.SetWorldSpecificMonstersByWorldID();
                    for(int i=0;i<2;i++)
                    for(int j=0;j<3;j++){
                        int id  = NPC.cavernMonsterType[i,j];
                        if(id>493 && id<496){
                            types.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Crawdad[id-494] );
                            notExtinct.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Extinct[0] );
                        }
                        else if(id>495 && id<498)
                        {
                            types.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_GiantShelly[id-496] );
                            notExtinct.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Extinct[1] );
                        }
                        else if(id>497 && id<507){
                            types.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Salamander[id-498] );
                            notExtinct.Add(PropertyCondition.WorldStyles.pcSalamanderGiantShellyCrawdad_Extinct[2] );
                        }
                    }
                    seedComp = WorldInfo.RNGNumbers.forSeed ;
                }
            }    
            public static bool IsExtinct(string type){
                ProofAndGenIfNot();
                return !notExtinct.Contains(type);
            }           

            public static bool HasType(string type){
                ProofAndGenIfNot();
                return types.Contains(type);
            }
            public static int HasCount(){
                ProofAndGenIfNot();
                return types.Count;
            }

        }
     
        public static string GuessDesertStyle() {
            
            int x = WorldInfo.DesertTopOfSurfaceCenterX;
            int y = WorldInfo.DesertTopOfSurfaceCenterYapprox;
            int ht = GenVars.desertHiveHigh-1;
            int hl = GenVars.desertHiveLeft+5;
            int hr = GenVars.desertHiveRight-5;
            

            int conxspread = 0;
            int Maxconxspread = 0;
            for(int yi=y;yi<ht;yi++){
                conxspread = 0;
                for(int xi=hl;xi<hr;xi++){
                    if(Main.tile[xi,yi].HasTile && Main.tile[xi,yi].TileType == TileID.HardenedSand) conxspread++;
                    else {
                        Maxconxspread = Math.Max(Maxconxspread,conxspread); conxspread=0;}                
                }
                if(conxspread>0) Maxconxspread = Math.Max(Maxconxspread,conxspread);
            }
            
            if(Maxconxspread>8) return WorldStyles.pcDesertStyles[1]; //todo need to change to enum?


            int conyspread = 0;
            int Maxconyspread = 0;
            int lastX = 0;
            List<int> whereX = new List<int>();
            List<int> whereY = new List<int>();
            for(int xi=hl;xi<hr;xi++){
                conyspread=0;               
                for(int yi=y;yi<ht;yi++){            
                    if(Main.tile[xi,yi].HasTile && Main.tile[xi,yi].TileType == TileID.HardenedSand) conyspread++;
                    else {
                        Maxconyspread = Math.Max(Maxconyspread,conyspread);                 
                        if(conyspread>5){
                            if(xi-lastX>7){
                                whereX.Add(xi);                        
                                whereY.Add(yi);
                                ;
                            }                 
                            lastX = xi;   
                            
                            conyspread=0;
                            break;
                        }
                    }      
                }   
                if(conyspread>0){
                    Maxconyspread = Math.Max(Maxconyspread,conyspread);                 
                        if(conyspread>5){
                            if(xi-lastX>7){
                                whereX.Add(xi);                        
                                whereY.Add(ht-1);
                                
                            }                 
                            lastX = xi;   
                            
                            conyspread=0;                            
                        }                        
                    
                }
            }
            


            if(Maxconyspread<8) return WorldStyles.pcDesertStyles[0];

            if(whereX.Count == 2 && (whereX[1]-whereX[0])< 30 )return WorldStyles.pcDesertStyles[4];

            int c2hill = 0;
            int c3hole = 0;
            for(int i = 0; i < whereX.Count; i++){
                int cx = whereX[i]+1;
                int cy = whereY[i]-1;
               

                bool found = true;
                int lx = cx;
                int ly = cy;
                while(found && cy-->42){
                    found = false;
                    for(int xi = cx-12; xi < cx+13; xi++){
                        if(Main.tile[xi,cy].HasTile){
                            found = true; 
                        }
                        if(Main.tile[xi,cy].HasTile && Main.tile[xi,cy].TileType == TileID.HardenedSand                                 
                                && !Main.tile[xi+1,cy].HasTile&& ((Main.tile[xi+2,cy].HasTile && Main.tile[xi+2,cy].TileType == TileID.HardenedSand) ||
                                (!Main.tile[xi+2,cy].HasTile && Main.tile[xi+3,cy].HasTile && Main.tile[xi+3,cy].TileType == TileID.HardenedSand ))                        
                             ){                            
                            lx = cx;
                            ly = cy;                            
                        }
                    } 
                    cx = lx;
                }
                cx = lx;
                cy = ly;                
                found = true;
                while(found && cy-->0){
                    found = false;
                    for(int w=cx-4;w<cx+5;w++) if(Main.tile[cx+w,cy].HasTile && Main.tile[cx+w,cy].TileType == TileID.HardenedSand){ found = true; break;}
                }
                ly-=2;                
                int sc=0;
                for(int xi = cx-20; xi < cx+21; xi++){
                for(int yi = ly; yi > ly -20; yi--){
                if(Main.tile[xi,yi].HasTile && Main.tile[xi,yi].TileType == TileID.Sand ){ sc++;
                }}}
                       
                if(sc<42) c2hill++;
                else c3hole++;                
            }          

            return WorldStyles.pcDesertStyles[c3hole>c2hill?3:2];
        }


        //public static readonly List<Constraint> pcPredictedSpawn = new List<Constraint>(){
            //ConditionElmWithValues(ConstraintNames.TilesAboveUndergroundLayer, (a,b) => Helpers.BasicFunctions.DepthBelowPredictedSpawn((int)Main.worldSurface) ,WorldGenPass.Terrain ),
            //ConditionElmWithValues(ConstraintNames.TilesAboveCavernLayer, (a,b) => Helpers.BasicFunctions.DepthBelowPredictedSpawn((int)Main.rockLayer) ,WorldGenPass.Terrain ),
            //ConditionElmWithValues(ConstraintNames.TilesAboveGraniteMarbleDetonator, (a,b) => Helpers.BasicFunctions.DepthBelowPredictedSpawn((int)GenVars.rockLayer) ,WorldGenPass.Terrain ),                
        //};

        public static Constraint ConditionElmWithValues(string name, Func<PropertyElement, Constraint, ValueStringAndOrDouble> fun, WorldGenPass firstPossibleUsage, Func<Constraint, bool, string> customDisplayFun = null)
        {
            return new Constraint(name, fun, ConditionElementValueList.FindValueValueList4ValueName(name), firstPossibleUsage,
                                         ConditionElementValueList.FindDefaultValue4ValueName(name),
                                         GetCondParameterValue(name, 1), GetCondParameterValue(name, 2), GetCondParameterValue(name, 3),
                                         customDisplayFun);

        }

        private static string DisplayFunEach(Constraint con, bool isFirst)
        {
            string txt = "";
            string goleAsText = con.gole == GreaterOrLessEqual.Equal?"":con.gole == GreaterOrLessEqual.NotEqual?"not ": GreaterOrLessEqualClass.ToDString(con.gole)+" ";
            txt += (!isFirst ? ", " : "") + $"{goleAsText}";
            txt += $"{con.targetValue.GetValueAsString()} ";
            txt += con.name;
            return txt;
        }
        private static string DisplayFunOres(Constraint con, bool isFirst)
        {
            string txt = "";
            string goleAsText = con.gole == GreaterOrLessEqual.Equal?"":con.gole == GreaterOrLessEqual.NotEqual?"not ": GreaterOrLessEqualClass.ToDString(con.gole)+" ";
            txt += (!isFirst ? ", " : "") + $"{goleAsText}";            
            txt += $"{con.targetValue.GetValueAsString()}";
            return txt;
        }

        public static Constraint.CondParameterValue GetCondParameterValue(string name, int paramID)
        {
            name = ConstraintNames.GetParameterNameTextfromFunctionNameText(name, paramID);
            Constraint.CondParameterValue cel = null;
            if (name.Length > 0)
            {

                ConditionElementValueList.KeyWords? ff = Helpers.EnumHelper<ConditionElementValueList.KeyWords>.Try2FindValue4Name(name);
                if (ff == null) return cel;

                cel = new Constraint.CondParameterValue(name, ConditionElementValueList.lookUpDefaultValueList[
                     (ConditionElementValueList.KeyWords)Helpers.EnumHelper<ConditionElementValueList.KeyWords>.Try2FindValue4Name(name)]);
            }
            return cel;
        }
    }
}