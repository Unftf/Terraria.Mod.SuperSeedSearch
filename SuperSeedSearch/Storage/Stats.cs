using Terraria;
using Terraria.ID;
using Terraria.WorldBuilding;
using System;
using System.IO;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Reflection;

using System.Collections.Generic;
using SuperSeedSearch;
using Terraria.Utilities;
using System.Linq;
using Terraria.ObjectData;
using SuperSeedSearch.ConstantEnum;
using Terraria.ModLoader;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SuperSeedSearch.PropertyCondition.PCPoint;
using SuperSeedSearch.WorldGenMod;
using SuperSeedSearch.UI;


namespace SuperSeedSearch.Storage{


    public class Stats{
        internal class Obj2Str : IComparer {
            int IComparer.Compare(Object a, Object b)
            {
                return ((new CaseInsensitiveComparer()).Compare(a.ToString(), b.ToString()));
            }
        }

        public static void StoreStats(string condText, StoreStatsInfo statsType = StoreStatsInfo.On){
            if(statsType == StoreStatsInfo.Off) return;

            if(DataExtractor.pathlength == null || DataExtractor.quickPathlength == null || DataExtractor.quickPathlengthJungleBeach == null ) DataExtractor.computePredSpawnHeightAndBeachHeight(DataExtractor.DetermineDataNames.PredictedSpawnHeight);
            if(DataExtractor.pathlengthFromJungleMainEntrance == null ||  DataExtractor.quickPathlengthJungleMainEntrance == null) DataExtractor.ApproximatedJungleMainEntranceHeight(WorldGenMod.DataExtractor.DetermineDataNames.JungleMainEntranceX);                
            if(DataExtractor.quickPathlengthDesertSurface == null ) DataExtractor.ApproxSnowSurfCenterXY(DataExtractor.DetermineDataNames.DesertTopOfSurfaceCenterYapprox);
            if(DataExtractor.quickPathlengthSnowCSurface == null ) DataExtractor.ApproxDesertCenterSurfaceY(DataExtractor.DetermineDataNames.DesertTopOfSurfaceCenterYapprox);

            string filename = Main.worldPathName.Substring(0, Main.worldPathName.Length - 4) + "_stats.txt";      
                using StreamWriter file = new(filename);
                
                string wgInfoBase = "## Condition info (beta) ## "+Environment.NewLine + condText + Environment.NewLine+Environment.NewLine;
                string wgInfoStart = "## World info (beta) ## " + Environment.NewLine + Environment.NewLine;
                string wgInfo = wgInfoBase+wgInfoStart;


                var keys = WorldInfo.Info.Keys;
                ArrayList Keys = new ArrayList(WorldInfo.Info.Keys);
                IComparer obj2strcomp = new Obj2Str();
                Keys.Sort( obj2strcomp );

                
                foreach(var info in Keys ){
                    string entry = "";
                    if(WorldInfo.Info[info].GetType().IsArray ){                            
                            Type valueType = WorldInfo.Info[info].GetType();
                            int i = 5;
                            if(  typeof(Int32).IsAssignableFrom(valueType.GetElementType()) == true){                                
                                foreach(var v in  (Int32[])WorldInfo.Info[info]){ if(--i<0){entry += "..."; break;}  entry += v.ToString()+ " ";  }
                            }    
                            else if(  typeof(Vector2).IsAssignableFrom(valueType.GetElementType()) == true){                                
                                foreach(var v in  ((Vector2[])WorldInfo.Info[info]) ){if(--i<0){entry += "..."; break;} entry += v.ToString()+ " "; }
                            }    
                            else{
                                entry = WorldInfo.Info[info].ToString();//continue;
                            }                        
                    }else{
                        if(info is string && ((string)info).Equals(Statistics.FoundMatchingChestItems)) {
                            //wgInfo = wgInfoBase + Statistics.FoundMatchingChestItems + Environment.NewLine;
                            //foreach(var v in (List<Tuple<string,int,int,int>>) WorldInfo.Info[Statistics.FoundMatchingChestItems]){
                            //    wgInfo += v.Item1 + " at " + "(" +v.Item3+","+v.Item4+")" + Environment.NewLine;
                            //}
                            //wgInfo += Environment.NewLine + Environment.NewLine + wgInfoStart;
                            continue;
                        }
                        else
                            entry = WorldInfo.Info[info].ToString();
                    }
                    wgInfo += info.ToString() + " : " + entry + Environment.NewLine;
                }  
                wgInfo += Environment.NewLine;

                

                
                wgInfo += "# WorldGen variables"+Environment.NewLine;

                UnifiedRandom rand = new UnifiedRandom(WorldGen._genRandSeed);            
                foreach(var rngn in typeof(WorldInfo.RNGNumbers).GetFields(BindingFlags.Static | BindingFlags.Public ) ){
                    if( rngn.FieldType != typeof(System.Double)) continue;                    
                    double val = (double)typeof(WorldInfo.RNGNumbers).GetField(rngn.Name, BindingFlags.Static | BindingFlags.Public).GetValue(null);                   
                    
                    double rngNext = rand.NextDouble();
                    wgInfo += (rngn.Name + ": "+ rngNext+ (WorldInfo.RNGNumbers.predicted && Math.Abs(rngNext-val)>0? (" pred: " + val + " diff:"+Math.Abs(rngNext-val)  ):"") + Environment.NewLine );
                }
                wgInfo += "Evil biome type: " + (WorldGen.drunkWorldGen? "Both": (WorldGen.crimson? "Crimson":"Corruption")) +  Environment.NewLine;
                wgInfo += ConstantEnum.ConstraintNames.HardmodeHallowSide + ": " + PropertyCondition.WorldStyles.pcLeftRightSide[1-(int)(WorldInfo.RNGNumbers.RNGN3*2)] + Environment.NewLine;
                wgInfo += ConstantEnum.ConstraintNames.HardmodeHallowAtJungleSnow + ": " + PropertyCondition.WorldStyles.pcJungelDungeonSide[GenVars.dungeonSide*(1-2*(int)(WorldInfo.RNGNumbers.RNGN3*2))<0?0:1] + Environment.NewLine;
                wgInfo += "Spawn depth: " + Main.spawnTileY + Environment.NewLine;
                wgInfo += "Spawn height above underground: " + (int)(-Main.spawnTileY+Main.worldSurface) + Environment.NewLine;
                wgInfo += "Spawn height above cavern layer: " + (int)(-Main.spawnTileY+Main.rockLayer) + Environment.NewLine;
                wgInfo += "Spawn height above GenVars.rockLayer: " + (int)(-Main.spawnTileY+GenVars.rockLayer) + Environment.NewLine;
                wgInfo += "Spawn height above GenVars.worldSurfaceHigh: " + (int)(-Main.spawnTileY+GenVars.worldSurfaceHigh) + Environment.NewLine;
                wgInfo += "Spawn height above start of buried chest variable: " + (int)(-Main.spawnTileY+PCPointOfInterestList.getStartDepthOfBuriedChest() ) + Environment.NewLine;
                
                
                int surfx = Main.spawnTileX;
                int surfy = Helpers.BasicFunctions.AllignToNearbySurfaceLevel(surfx,Main.spawnTileY);
                
                wgInfo += "Surface depth around Spawn: " + surfy + Environment.NewLine;
                wgInfo += "Spawn-surface height above Spawn: " + (int)(-surfy+Main.spawnTileY) + Environment.NewLine;
                wgInfo += "Spawn-surface height above underground: " + (int)(-surfy+Main.worldSurface) + Environment.NewLine;
                wgInfo += "Spawn-surface height above cavern layer: " + (int)(-surfy+Main.rockLayer) + Environment.NewLine;
                wgInfo += "Spawn-surface height above GenVars.rockLayer: " + (int)(-surfy+GenVars.rockLayer) + Environment.NewLine;
                wgInfo += "Spawn-surface height above GenVars.worldSurfaceHigh: " + (int)(-surfy+GenVars.worldSurfaceHigh) + Environment.NewLine;                
                wgInfo += "Spawn-surface height above start of buried chest variable: " + (int)(-surfy+PCPointOfInterestList.getStartDepthOfBuriedChest() ) + Environment.NewLine;

                wgInfo += "Main.worldSurface: " +(int)(Main.worldSurface)+  Environment.NewLine;
                wgInfo += "Main.rockLayer: " +(int)(Main.rockLayer)+  Environment.NewLine;
                wgInfo += "Underground layer height: " +(int)(-Main.worldSurface+Main.rockLayer)+  Environment.NewLine;
                wgInfo += "Start of buried chest variable: " +PCPointOfInterestList.getStartDepthOfBuriedChest()+  Environment.NewLine;
                wgInfo += "Buried chest variable below start of Underground: " +(int)(-Main.worldSurface + PCPointOfInterestList.getStartDepthOfBuriedChest() )+  Environment.NewLine;


                int j = 0;
                string strtxt = "";
                foreach(var info in typeof(GenVars).GetMembers(BindingFlags.Static | BindingFlags.Public) ){
                    if(j++<1) continue;
                    var val = typeof(GenVars).GetField(info.Name, BindingFlags.Static | BindingFlags.Public).GetValue(null);
                    string txt = "";
                    
                    if(val.GetType().IsArray){
                        txt = "[";
                        int c = 0;
                        for (int ind = 0; ind < ((Array)val).Length; ind++)
                        {
                            if (++c>5){ txt += ((Array)val).GetValue(ind) + "..."; break;}
                            txt += ((Array)val).GetValue(ind) + ", ";                            
                        }
                        txt += "]";
                        val = txt;
                    }else if( val.GetType() == typeof(System.Collections.Generic.List<System.Int32>)){
                        txt = "[";
                        int c = 0;                        
                        foreach(var ele in (System.Collections.Generic.List<System.Int32>) val){
                            if (++c>5){ txt += ele + "..."; break;}
                        }
                        txt += "]";
                        val = txt;
                    }else if(val.GetType() == typeof(StructureMap) ) {

                        List<Rectangle> str = (List<Rectangle>)typeof(StructureMap).GetField("_structures", BindingFlags.NonPublic | BindingFlags.Instance ).GetValue((StructureMap)val );
                        List<Rectangle> pstr = (List<Rectangle>)typeof(StructureMap).GetField("_protectedStructures", BindingFlags.NonPublic | BindingFlags.Instance).GetValue((StructureMap)val);
                        //int c = 0;
                        txt = "structures: ";
                        if(str != null)
                        foreach(var ele in  str){
                            txt += "("+ele.X + " " + ele.Y + " "+ ele.Width+") , ";
                        }
                        //c = 0;
                        txt += Environment.NewLine;
                        txt += "protected structures: ";
                        if(pstr != null)
                        foreach(var ele in  pstr){
                            txt += "("+ele.X + " " + ele.Y + " "+ ele.Width+") , ";
                        }
                        strtxt = strtxt + txt + Environment.NewLine;
                        continue;
                    }

                    wgInfo += (info.Name + ": " + val + Environment.NewLine );
                }

                string bgInfo = "## Background & tree styles ## "+Environment.NewLine;
                bgInfo+="Tree style: "+ Main.treeStyle[0]+", "+ Main.treeStyle[1]+(Main.maxTilesX!=4200?(", "+Main.treeStyle[2]):"")+(Main.maxTilesX>=8400?(", "+Main.treeStyle[3]):"")+Environment.NewLine;
                bgInfo+="Forest background/Tree transition x-pos: "+ Main.treeX[0]+(Main.maxTilesX!=4200?(", "+Main.treeX[1]):"")+(Main.maxTilesX>=8400?(", "+Main.treeX[2]):"")+Environment.NewLine;
                bgInfo+="Cavern background transition x-pos: "+ Main.caveBackX[0]+(Main.maxTilesX!=4200?(", "+Main.caveBackX[1]):"")+(Main.maxTilesX>=8400?(", "+Main.caveBackX[2]):"")+Environment.NewLine;
                bgInfo+="Forest biome: "+ WorldGen.treeBG1+", "+ WorldGen.treeBG2+(Main.maxTilesX!=4200?(", "+WorldGen.treeBG3):"")+(Main.maxTilesX>=8400?(", "+WorldGen.treeBG4):"")+Environment.NewLine;
                bgInfo+="Cavern layer: "+ Main.caveBackStyle[0]+", "+ Main.caveBackStyle[1]+(Main.maxTilesX!=4200?(", "+Main.caveBackStyle[2]):"")+(Main.maxTilesX>=8400?(", "+Main.caveBackStyle[3]):"")+Environment.NewLine;
                bgInfo+="Cavern Ice: " + Main.iceBackStyle + Environment.NewLine;
                bgInfo+="Cavern Hell: " + Main.hellBackStyle + Environment.NewLine;
                bgInfo+="Cavern Jungle: " + Main.jungleBackStyle + Environment.NewLine;
                bgInfo+="Corruption: " + WorldGen.corruptBG + Environment.NewLine;
                bgInfo+="Jungle: " + WorldGen.jungleBG + Environment.NewLine;
                bgInfo+="Snow: " + WorldGen.snowBG + Environment.NewLine;
                bgInfo+="Hallow: " + WorldGen.hallowBG + Environment.NewLine;
                bgInfo+="Crimson: " + WorldGen.crimsonBG + Environment.NewLine;
                bgInfo+="Desert: " + WorldGen.desertBG + Environment.NewLine;
                bgInfo+="Ocean: " + WorldGen.oceanBG + Environment.NewLine;
                bgInfo+="Mushroom: " + WorldGen.mushroomBG + Environment.NewLine;
                bgInfo+="Underworld: " + WorldGen.underworldBG + Environment.NewLine;
                
                
                string exstats = statsType is StoreStatsInfo.Extended or StoreStatsInfo.TooMany? ExtendendStats(statsType):"";

                string bulbtxt = RandomNumberAnalyzer.RandomNumberAnalyzer.GetBulbText();
                file.Write(wgInfo+Environment.NewLine  + strtxt+Environment.NewLine+Environment.NewLine+ bgInfo + Environment.NewLine + Environment.NewLine 
                    + (exstats.Length>0?exstats+Environment.NewLine + Environment.NewLine:"")
                    + (bulbtxt.Length>0?bulbtxt+Environment.NewLine + Environment.NewLine:"") );
                file.Close();
        }

        enum Vartype {BiomeSpread, Tile, ChestItem, Modifier, Wire, Liquid, Wall, Length};

        //Todo statsType with name
        enum Statstype {Count, PathlengthSpawn, PathlengthJungleMain, PathlengthJungleMainReduced, QuickPathSpawn, QuickPathJungleMain, QuickPathDesert, QuickPathSnow, CloToMid, CloToBeach, mostJungSide, mostDunSide, deepest, highest, highestRelSpawn, highestRelUnderg, highestRelCavern, CloToJungCX, CloToJungCY, CloToTempCX, CloToTempCY, CloToSnowCX, CloToSnowCY, CloToDesertCX, CloToDesertCY, CloToShimCX, CloToShimmCY,  CloToDungCX, CloToDungCY, CloToMountainCaveX, CloToPyramidX, CloToOasisX, CloToLivingTreeX, XpYdist2ShimmerMin, XpYdist2ShimmerMax, XpYdist2TempleMin, XpYdist2TempleMax, QuickPathJungleBeach, Length };
        enum StatstypeBiomeSpread {Jungle, Snow, Dungeon, Temple, Length};
        enum bsSide {center, left, right, Length};
        readonly static string[] StatsNames = {"Quantity ", "Pathlenght to Spawn", "Pathlength to Jungle main entrance", "Pathlength to Jungle main entrance reduced by depth cost", "Near Spawn pathlength (max dx = y)", "Near Jungle main entrance pathlength (max dx = y)", "Near Desert center surface pathlength (max dx = y)", "Near Snow center surface pathlength (max dx = y +100)", "Closest to Mid (x-dist)", "Closest to a Beach (x-dist)", "Most Jungle side (x)", "Most Dungeon side (x)", "deepest (y)", "highest (y)", "highest related to spawn (y-dist)", "highest related to start underground (y-dist)", "highest related to start cavern layer (y-dist)", "Closest to Jungle center (x-dist)", "Closest to Jungle center (y-dist)", "Closest to Temple center (x-dist)", "Closest to Temple center (y-dist)", "Closest to Snow center (x-dist)", "Closest to Snow center (y-dist)", "Closest to main Desert center (x-dist)", "Closest to main Desert center (y-dist)", "Closest to Shimmer center (x-dist)", "Closest to Shimmer center (y-dist)",  "Closest to Dungeon entrance (x-dist)", "Closest to Dungeon entrance (y-dist)", "Closest to a mountain cave (x-dist)", "Closest to a Pyramid (x-dist)", "Closest to a Oasis (x-dist)", "Closest to a Living tree (x-dist)", "X+Y dist to Shimmer center min", "X+Y dist to Shimmer center max", "X+Y dist to Temple center min","X+Y dist to Temple center max", "Near Jungle beach pathlength (max dx = y +200)", "Count"};
        

        public static string ExtendendStats(StoreStatsInfo statsType ){
            //todo unify different pathlenghts and texts
            if (statsType is StoreStatsInfo.On or StoreStatsInfo.Off ) return "";

           
            if(DataExtractor.pathlength == null) WorldGenMod.DataExtractor.SetupPathlength(WorldGenMod.DataExtractor.DetermineDataNames.ComputePathLength);
            
            if(statsType == StoreStatsInfo.TooMany){
                
                

                if(DataExtractor.pathlengthFromJungleMainEntrance == null) DataExtractor.SetupPathlengthJungle(DataExtractor.DetermineDataNames.ComputePathLengthJungle);
                DataExtractor.SetupQuickPathlengthSpawn(DataExtractor.DetermineDataNames.ComputeQuickPathLengthSpawn);
                DataExtractor.SetupQuickPathlengthJungleMainEntrance(DataExtractor.DetermineDataNames.ComputeQuickPathLengthJungleMain);
                DataExtractor.SetupQuickPathlengthSnowCSurface(DataExtractor.DetermineDataNames.ComputeQuickPathLengthSnowCSurf);
                DataExtractor.SetupQuickPathlengthDesertSurface(DataExtractor.DetermineDataNames.ComputeQuickPathLengthDesertCSurf);
                DataExtractor.SetupQuickPathlengthJungleBeach(DataExtractor.DetermineDataNames.ComputeQuickPathLengthJungleBeach);
            }

            

            int[][][][] stats = null;
            int[] livingTrees = new int[Main.maxTilesX];
            
            int StatsNum = statsType == StoreStatsInfo.TooMany?  (int)Statstype.Length: (int)Statstype.PathlengthSpawn+1;

            string StatsTableNames = "Name: ";
            for(int i = 0; i < StatsNum; i ++){
                StatsTableNames += (i+1) + "." + StatsNames[i] + (i==StatsNum-1?" ":" | ");
            }
            String EXstats = "## Chest Items ##" + Environment.NewLine + StatsTableNames + Environment.NewLine;
            
            stats = new int[(int)Vartype.Length][][][];
            
            for(int i=0; i < (int)Vartype.Length; i++){                
            
                if(i== (int)Vartype.BiomeSpread){
            
                    if (statsType != StoreStatsInfo.TooMany ) continue;
            
                    stats[0] = new int[(int)StatstypeBiomeSpread.Length][][];
            
                    for(StatstypeBiomeSpread j=0; j < StatstypeBiomeSpread.Length; j++){
            
                        stats[i][(int)j] = new int[Main.maxTilesY][];
                    }    
            
                    continue;
                }
                else stats[i] = new int[StatsNum][][];
                    
                for(int j=0; j < StatsNum; j++){
                    stats[i][j] = new int[ (Vartype)i switch { Vartype.Tile => TileID.Count, Vartype.ChestItem => ItemID.Count, Vartype.Modifier =>  PrefixID.Count, Vartype.Wire => 1, Vartype.Liquid => LiquidID.Count, Vartype.Wall => WallID.Count, _ => 0 }  ][];
                
                }
            }
            
            int StatstypeBiomeSpreadDesertid = (int)StatstypeBiomeSpread.Length;
            int StatstypeBiomeSpreadShimerid = (int)StatstypeBiomeSpread.Length+1;
            int StatstypeBiomeSpreadDungeonEntranceid = (int)StatstypeBiomeSpread.Length+2;

            Vector2[] BiomeCenter = null;
            if(statsType == StoreStatsInfo.TooMany){            
                BiomeCenter = new Vector2[(int)StatstypeBiomeSpread.Length+3];

                BiomeCenter[StatstypeBiomeSpreadDesertid] = new Vector2((GenVars.desertHiveLeft+GenVars.desertHiveRight)/2, (GenVars.desertHiveHigh+GenVars.desertHiveLow)/2 );
                BiomeCenter[StatstypeBiomeSpreadShimerid] = new Vector2((float)GenVars.shimmerPosition.X, (float)GenVars.shimmerPosition.Y );
                BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid] = new Vector2((float)GenVars.dungeonX, (float)GenVars.dungeonY );
            }

            //Todo add pl also inside this
            Action<Vartype, int, int, int> UpdateStats = (Vartype t, int x, int y, int id ) => {                    
                    int style = 0;
                    int wrapLimit = 1;

                    if(t == Vartype.Tile){
                       
                        Tile tile = Main.tile[x,y];
                        ushort tild = tile.TileType;
                        short frameX = tile.TileFrameX;
                        short frameY = tile.TileFrameY;
            

                        TileObjectData tdata = TileObjectData.GetTileData(tild,0);
                        
                        style = TileObjectData.GetTileStyle(tile);
                        style = style<0? 0:style;
                        wrapLimit = tdata != null && tdata.StyleWrapLimit!=0?tdata.StyleWrapLimit :1;
                    }
                    if(t == Vartype.ChestItem){
                        int cid = Chest.FindChest(x,y); 
                        if(cid >= 0){
                            for(int itemi =0; itemi < 40 && Main.chest[cid].item[itemi] !=null ; itemi++){
                
                                int iid = Main.chest[cid].item[itemi].type;                    
                                if( iid == id ){
                                    if(Main.chest[cid].item[itemi].prefix > 0 && Main.chest[cid].item[itemi].prefix<PrefixID.Count){ 
                                        style = Main.chest[cid].item[itemi].prefix;                        
                                    }
                                    break;
                                }
                                if(iid == ItemID.None) break;                    
                            }


                        }
                    }

                    
                    if(t == Vartype.BiomeSpread){
                        
                        if( id != y && stats[(int)t][-id-y][-id-y] == null ) return;
                        for(int si=0; si < (int)StatstypeBiomeSpread.Length;si++)
                            if(stats[(int)t][si][y] == null ||  stats[(int)t][(int)si][y].Length == 0) stats[(int)t][(int)si][y]  = new int[(int)bsSide.Length]{-1,Main.maxTilesX,-1};

                        if(Main.tile[x,y] == null || !Main.tile[x,y].HasTile) return;
                        ushort tt = Main.tile[x,y].TileType;
                        if( !Main.tileSolid[tt ] )  return;

                        StatstypeBiomeSpread bst = StatstypeBiomeSpread.Length;
                        
                        if(tt == TileID.JungleGrass) bst = StatstypeBiomeSpread.Jungle;
                        else if(tt == TileID.SnowBlock || tt == TileID.IceBlock ) bst = StatstypeBiomeSpread.Snow;
                        else if(TileID.Sets.DungeonBiome[tt]>0 ) bst = StatstypeBiomeSpread.Dungeon;
                        else if(tt == TileID.LihzahrdBrick ) bst = StatstypeBiomeSpread.Temple;
                        
                        if(bst == StatstypeBiomeSpread.Length) return;
                        
                        ref int value2chC = ref stats[(int)t][(int) bst][y][(int) bsSide.center ];                        
                        ref int value2chL = ref stats[(int)t][(int) bst][y][(int) bsSide.left ];
                        ref int value2chR = ref stats[(int)t][(int) bst][y][(int) bsSide.right ];
                        
                        
                        value2chL = Math.Min(value2chL, x);
                        value2chR = Math.Max(value2chR, x);
                        value2chC = (value2chL+value2chR)/2;
                        
                        
                    }
                    else
                    for(int si=0; si<(int)StatsNum ;si++){                                            
                        if(stats[(int)t][(int)si][id] == null ||  stats[(int)t][(int)si][id].Length == 0) {
                            stats[(int)t][(int)si][id]  = Enumerable.Repeat( (Statstype)si switch{ Statstype.PathlengthSpawn or Statstype.PathlengthJungleMain or Statstype.PathlengthJungleMainReduced or Statstype.QuickPathSpawn or Statstype.QuickPathJungleMain or Statstype.QuickPathDesert or Statstype.QuickPathSnow or Statstype.QuickPathJungleBeach or Statstype.CloToBeach or Statstype.CloToMid or Statstype.CloToJungCX or Statstype.CloToJungCY or Statstype.CloToTempCX or Statstype.CloToTempCY or Statstype.CloToSnowCX or Statstype.CloToSnowCY or Statstype.CloToDesertCX or Statstype.CloToDesertCY or Statstype.CloToShimCX or Statstype.CloToShimmCY or Statstype.CloToDungCX or Statstype.CloToDungCY or Statstype.CloToMountainCaveX or Statstype.CloToPyramidX or Statstype.CloToOasisX or Statstype.CloToLivingTreeX or Statstype.XpYdist2ShimmerMin or Statstype.XpYdist2TempleMin => Int32.MaxValue, Statstype.highest or Statstype.highestRelSpawn or Stats.Statstype.highestRelUnderg or Stats.Statstype.highestRelCavern or Statstype.deepest or Statstype.mostDunSide or Statstype.mostJungSide or Statstype.XpYdist2ShimmerMax or Statstype.XpYdist2TempleMax => Int32.MinValue, Statstype.Count or _=> 0}  , t == Vartype.ChestItem? PrefixID.Count: style+1 ).ToArray();                                                        
                        }
                      
                         if(style >= stats[(int)t][(int)si][id].Length){          // tiles only           
                                 
                                int news = style/wrapLimit;
                                news = wrapLimit*(news+1);
                                int[] tmp = Enumerable.Repeat( (Statstype)si switch{ Statstype.PathlengthSpawn or Statstype.PathlengthJungleMain or Statstype.PathlengthJungleMainReduced or Statstype.QuickPathSpawn or Statstype.QuickPathJungleMain or Statstype.QuickPathDesert or Statstype.QuickPathSnow or Statstype.QuickPathJungleBeach or Statstype.CloToBeach or Statstype.CloToMid or Statstype.CloToJungCX or Statstype.CloToJungCY or Statstype.CloToTempCX or Statstype.CloToTempCY or Statstype.CloToSnowCX or Statstype.CloToSnowCY or Statstype.CloToDesertCX or Statstype.CloToDesertCY or Statstype.CloToShimCX or Statstype.CloToShimmCY or Statstype.CloToDungCX or Statstype.CloToDungCY or Statstype.CloToMountainCaveX or Statstype.CloToPyramidX or Statstype.CloToOasisX or Statstype.CloToLivingTreeX or Statstype.XpYdist2ShimmerMin or Statstype.XpYdist2TempleMin => Int32.MaxValue, Statstype.highest or Statstype.highestRelSpawn or Stats.Statstype.highestRelUnderg or Stats.Statstype.highestRelCavern or Statstype.deepest or Statstype.mostDunSide or Statstype.mostJungSide or Statstype.XpYdist2ShimmerMax or Statstype.XpYdist2TempleMax  => Int32.MinValue, Statstype.Count or _ => 0}  , news).ToArray();
                                
                                Array.Copy(stats[(int)t][(int)si][id], tmp, stats[(int)t][(int)si][id].Length);
                                stats[(int)t][(int)si][id] = tmp;
                              
                         }
                 
                        for(int r = 0; r<1 + (t == Vartype.ChestItem && style!=0 ?1:0) ;r++){                        
                                
                            ref int value2ch = ref stats[(int)t][(int)si][id][r==0 || t != Vartype.ChestItem ?style:0];
                            
                            switch((Statstype)si){
                                case Statstype.PathlengthSpawn:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.pathlength.get(x,y) ); break;
                                case Statstype.PathlengthJungleMain:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.pathlengthFromJungleMainEntrance.get(x,y) ); break;
                                case Statstype.PathlengthJungleMainReduced:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.pathlengthFromJungleMainEntrance.getReducedByBestDepthPl(x,y) ); break;
                                case Statstype.QuickPathSpawn:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.quickPathlength.get(x,y) ); break;
                                case Statstype.QuickPathJungleMain:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.quickPathlengthJungleMainEntrance.get(x,y) ); break;
                                case Statstype.QuickPathDesert:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.quickPathlengthDesertSurface.get(x,y) ); break;
                                case Statstype.QuickPathSnow:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.quickPathlengthSnowCSurface.get(x,y) ); break;       
                                case Statstype.QuickPathJungleBeach:
                                    value2ch = Math.Min(value2ch, WorldGenMod.DataExtractor.quickPathlengthJungleBeach.get(x,y) ); break;                              
                  

                                case Statstype.CloToBeach:
                                    value2ch = Math.Min(value2ch, Math.Min(Math.Abs(x-GenVars.leftBeachEnd), Math.Abs(x-GenVars.rightBeachStart))  );break;
                                case Statstype.CloToMid:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-Main.maxTilesX/2)  );break;
                                case Statstype.highest:
                                    value2ch = Math.Max(value2ch, Main.maxTilesY-y);break; 
                                case Statstype.highestRelSpawn:
                                    value2ch = Math.Max(value2ch, Main.spawnTileY-y);break; 
                                case Statstype.highestRelUnderg:
                                    value2ch = Math.Max(value2ch, (int)Main.worldSurface-y);break; 
                                case Statstype.highestRelCavern:
                                    value2ch = Math.Max(value2ch, (int)Main.rockLayer-y);break; 
                                case Statstype.mostDunSide:
                                    value2ch = Math.Max(value2ch, GenVars.dungeonSide<0? Main.maxTilesX-1-x : x );break;
                                case Statstype.mostJungSide:
                                    value2ch = Math.Max(value2ch, GenVars.dungeonSide>0? Main.maxTilesX-1-x : x );break;
                                case Statstype.deepest:
                                    value2ch = Math.Max(value2ch, y);break;
                                case Statstype.Count:
                                    value2ch += 1;break;                                

                                case Statstype.CloToJungCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[(int) StatstypeBiomeSpread.Jungle].X )  );break;
                                case Statstype.CloToJungCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[(int) StatstypeBiomeSpread.Jungle].Y )  );break;
                                case Statstype.CloToTempCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].X )  );break;
                                case Statstype.CloToTempCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].Y )  );break;
                                case Statstype.CloToSnowCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[(int) StatstypeBiomeSpread.Snow].X )  );break;
                                case Statstype.CloToSnowCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[(int) StatstypeBiomeSpread.Snow].Y )  );break;
                                case Statstype.CloToDesertCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[StatstypeBiomeSpreadDesertid].X )  );break;
                                case Statstype.CloToDesertCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[StatstypeBiomeSpreadDesertid].Y )  );break;
                                case Statstype.CloToShimCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].X )  );break;
                                case Statstype.CloToShimmCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].Y )  );break;
                                case Statstype.CloToDungCX:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].X )  );break;
                                case Statstype.CloToDungCY:
                                    value2ch = Math.Min(value2ch, Math.Abs(y-(int)BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].Y )  );break;
                                case Statstype.CloToMountainCaveX:
                                    value2ch = Math.Min(value2ch,  Helpers.BasicFunctions.HorizontalDistToMountainCaveEntrance(x)); break;  
                                case Statstype.CloToPyramidX:
                                    value2ch = Math.Min(value2ch,  Helpers.BasicFunctions.HorizontalDistToPyramid(x)); break;  
                                case Statstype.CloToOasisX:
                                    value2ch = Math.Min(value2ch,  Helpers.BasicFunctions.HorizontalDisToOasis(x)); break;  
                                case Statstype.CloToLivingTreeX:
                                    value2ch = Math.Min(value2ch,  livingTrees[x]); break;  
                                case Statstype.XpYdist2ShimmerMin:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].X )+ Math.Abs(y-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].Y )  );break;
                                case Statstype.XpYdist2ShimmerMax:
                                    value2ch = Math.Max(value2ch, Math.Abs(x-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].X )+ Math.Abs(y-(int)BiomeCenter[StatstypeBiomeSpreadShimerid].Y )  );break;
                                case Statstype.XpYdist2TempleMin:
                                    value2ch = Math.Min(value2ch, Math.Abs(x-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].X )+ Math.Abs(y-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].Y )  );break;
                                case Statstype.XpYdist2TempleMax:
                                    value2ch = Math.Max(value2ch, Math.Abs(x-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].X )+ Math.Abs(y-(int)BiomeCenter[(int) StatstypeBiomeSpread.Temple].Y )  );break;

                                  
                                   
                            }

                        }                        
                    }
            };

    


            Func<Vartype, int, int, string> GetStatsText = (Vartype t, int id, int style) => {
                const string sep = " | ";
                string text = "";
                for(Statstype si=0; si< (Statstype) StatsNum; si++){
                    if(stats[(int)t] == null || stats[(int)t][(int)si]==null || stats[(int)t][(int)si][id] == null ||  stats[(int)t][(int)si][id].Length <= style ) return "";
                    int value = stats[(int)t][(int)si][id][style] ;
                    int overlapValue = si switch{Statstype.PathlengthSpawn => WorldGenMod.DataExtractor.pathlength.maxValue, Statstype.PathlengthJungleMain => WorldGenMod.DataExtractor.pathlengthFromJungleMainEntrance.maxValue,Statstype.PathlengthJungleMainReduced => WorldGenMod.DataExtractor.pathlengthFromJungleMainEntrance.maxValue, 
                                                        
                                                        Statstype.QuickPathSpawn => WorldGenMod.DataExtractor.quickPathlength.maxValue,
                                                        Statstype.QuickPathJungleMain => WorldGenMod.DataExtractor.quickPathlengthJungleMainEntrance.maxValue,
                                                        Statstype.QuickPathDesert => WorldGenMod.DataExtractor.quickPathlengthDesertSurface.maxValue,
                                                        Statstype.QuickPathSnow => WorldGenMod.DataExtractor.quickPathlengthSnowCSurface.maxValue,
                                                        Statstype.QuickPathJungleBeach => WorldGenMod.DataExtractor.quickPathlengthJungleBeach.maxValue,

                                                        Statstype.CloToBeach or Statstype.CloToMid => Main.maxTilesX/2, Statstype.deepest or Statstype.highest => 0, Statstype.highestRelSpawn => -(Main.maxTilesY-Main.spawnTileY), Statstype.highestRelUnderg => -(int)(Main.maxTilesY-Main.worldSurface),Statstype.highestRelCavern => -(int)(Main.maxTilesY-Main.rockLayer),   Statstype.mostDunSide or Statstype.mostJungSide => 0, //jungle reducved max size too high
                                                        Statstype.CloToJungCX => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Jungle].X, Main.maxTilesX-BiomeCenter[(int)StatstypeBiomeSpread.Jungle].X),
                                                        Statstype.CloToJungCY => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Jungle].Y, Main.maxTilesY-BiomeCenter[(int)StatstypeBiomeSpread.Jungle].Y),
                                                        Statstype.CloToTempCX => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Temple].X, Main.maxTilesX-BiomeCenter[(int)StatstypeBiomeSpread.Temple].X),
                                                        Statstype.CloToTempCY => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Temple].Y, Main.maxTilesY-BiomeCenter[(int)StatstypeBiomeSpread.Temple].Y),
                                                        Statstype.CloToSnowCX => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Snow].X, Main.maxTilesX-BiomeCenter[(int)StatstypeBiomeSpread.Snow].X),
                                                        Statstype.CloToSnowCY => (int)Math.Max(BiomeCenter[(int)StatstypeBiomeSpread.Snow].Y, Main.maxTilesY-BiomeCenter[(int)StatstypeBiomeSpread.Snow].Y),
                                                        Statstype.CloToDesertCX => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadDesertid].X, Main.maxTilesX-BiomeCenter[StatstypeBiomeSpreadDesertid].X),
                                                        Statstype.CloToDesertCY => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadDesertid].Y, Main.maxTilesY-BiomeCenter[StatstypeBiomeSpreadDesertid].Y),
                                                        Statstype.CloToShimCX => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadShimerid].X, Main.maxTilesX-BiomeCenter[StatstypeBiomeSpreadShimerid].X),
                                                        Statstype.CloToShimmCY => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadShimerid].Y, Main.maxTilesY-BiomeCenter[StatstypeBiomeSpreadShimerid].Y),
                                                        Statstype.CloToDungCX => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].X, Main.maxTilesX-BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].X),
                                                        Statstype.CloToDungCY => (int)Math.Max(BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].Y, Main.maxTilesY-BiomeCenter[StatstypeBiomeSpreadDungeonEntranceid].Y),
                                                        Statstype.CloToMountainCaveX => Main.maxTilesX, //could be smaller
                                                        Statstype.CloToPyramidX => Main.maxTilesX, //could be smaller
                                                        Statstype.CloToOasisX => Main.maxTilesX, //could be smaller
                                                        Statstype.CloToLivingTreeX => Main.maxTilesX, //could be smaller
                                                        Statstype.XpYdist2ShimmerMin => Main.maxTilesX+Main.maxTilesY, //could be smaller
                                                        Statstype.XpYdist2ShimmerMax => 0, //could be bigger
                                                        Statstype.XpYdist2TempleMin => Main.maxTilesX+Main.maxTilesY, //could be smaller
                                                        Statstype.XpYdist2TempleMax => 0, //could be bigger

                                                        Statstype.Count or _=> Int32.MaxValue-1 };
                    bool over = si switch{Statstype.PathlengthSpawn or Statstype.PathlengthJungleMain or Statstype.PathlengthJungleMainReduced or Statstype.QuickPathSpawn or Statstype.QuickPathJungleMain or Statstype.QuickPathDesert or Statstype.QuickPathSnow or Statstype.QuickPathJungleBeach or Statstype.CloToBeach or Statstype.CloToMid or Statstype.CloToJungCX or Statstype.CloToJungCY or Statstype.CloToTempCX or Statstype.CloToTempCY or Statstype.CloToSnowCX or Statstype.CloToSnowCY or Statstype.CloToDesertCX or Statstype.CloToDesertCY or Statstype.CloToShimCX or Statstype.CloToShimmCY or Statstype.CloToDungCX or Statstype.CloToDungCY or Statstype.CloToMountainCaveX or Statstype.CloToPyramidX or Statstype.CloToOasisX or Statstype.CloToLivingTreeX or Statstype.XpYdist2ShimmerMin or Statstype.XpYdist2TempleMin  => (value>overlapValue) , Statstype.deepest or Statstype.highest or Statstype.highestRelSpawn or Statstype.highestRelUnderg or Statstype.highestRelCavern or Statstype.mostDunSide or Statstype.mostJungSide or Statstype.XpYdist2ShimmerMax or Statstype.XpYdist2TempleMax => (value<overlapValue), Statstype.Count or _=> (value>overlapValue) };
                    string combined = si switch{Statstype.PathlengthSpawn or Statstype.PathlengthJungleMain or Statstype.PathlengthJungleMainReduced or Statstype.QuickPathSpawn or Statstype.QuickPathJungleMain or Statstype.QuickPathDesert or Statstype.QuickPathSnow or Statstype.QuickPathJungleBeach or Statstype.CloToBeach or Statstype.CloToMid or Statstype.CloToJungCX or Statstype.CloToJungCY or Statstype.CloToTempCX or Statstype.CloToTempCY or Statstype.CloToSnowCX or Statstype.CloToSnowCY or Statstype.CloToDesertCX or Statstype.CloToDesertCY or Statstype.CloToShimCX or Statstype.CloToShimmCY or Statstype.CloToDungCX or Statstype.CloToDungCY or Statstype.CloToMountainCaveX or Statstype.CloToPyramidX or Statstype.CloToOasisX or Statstype.CloToLivingTreeX or Statstype.XpYdist2ShimmerMax or Statstype.XpYdist2TempleMax => (over?">"+overlapValue:""+value ), Statstype.deepest or Statstype.highest or Statstype.highestRelSpawn or Statstype.highestRelUnderg or Statstype.highestRelCavern or Statstype.mostDunSide or Statstype.mostJungSide or Statstype.XpYdist2ShimmerMax or Statstype.XpYdist2TempleMax or Statstype.Count or _=> (over?"<"+overlapValue:""+value)  };
                    text += combined + ((si==(Statstype) StatsNum-1)?"":sep);
                }

                return text;
            };

            Func<Vartype, int, int, int> GetCount = (Vartype t, int id, int style) =>{
                if (stats[(int) t][(int)Statstype.Count][id] == null) return 0;
                if (stats[(int) t][(int)Statstype.Count][id].Length <= style ) return 0;
                return stats[(int) t][(int)Statstype.Count][id][style] ;
            };
            
                       
            

            if( statsType == StoreStatsInfo.TooMany){
                for (int yi = 0; yi < Main.maxTilesY; yi++)
                for (int xi = 0; xi < Main.maxTilesX; xi++)                
                {                    
                    UpdateStats(Vartype.BiomeSpread,xi,yi,yi);     
                    if(Main.tile[xi,yi].HasTile && Main.tile[xi,yi].TileType == TileID.LivingWood)  livingTrees[xi] = 1;                                 
                }
                int ltdc = Main.maxTilesX+4;
                for (int xi = 0; xi < Main.maxTilesX; xi++){
                    if(livingTrees[xi] == 1) {
                        ltdc = 0;
                    }
                    ltdc++;                    
                    livingTrees[xi] = ltdc;
                }
                for (int xi = Main.maxTilesX-1; xi >= 0; xi--){
                    if(livingTrees[xi] == 1) {
                        ltdc = 0;
                    }
                    ltdc++;                    
                    livingTrees[xi] = Math.Min(ltdc,livingTrees[xi])-1;
                }



                int[] co = Enumerable.Repeat(0, (int)StatstypeBiomeSpread.Length).ToArray();
                int[] su = Enumerable.Repeat(0, (int)StatstypeBiomeSpread.Length).ToArray();
                int[] fy = Enumerable.Repeat(-1, (int)StatstypeBiomeSpread.Length).ToArray();
                int[] ty = Enumerable.Repeat(-1, (int)StatstypeBiomeSpread.Length).ToArray();

                bool found = false;
                for(int si =0; si < (int)StatstypeBiomeSpread.Length; si++){                                
                    for (int yi = 0; yi < Main.maxTilesY; yi++){
                        
                        int v = stats[(int)Vartype.BiomeSpread][si][yi][(int)bsSide.center];                    
                        if( v > 0 && v < Main.maxTilesX) {co[si]++;su[si]+=v; if (fy[si] < 0) fy[si] = yi; found=true; } else if( found ){ ty[si] = yi; found = false;}
                    }
                    BiomeCenter[si] = new Vector2(su[si]/Math.Max(co[si],1), (ty[si]+fy[si] )/2 );                    
                }
                
                
            }

            int uniqueBiomeChest = 0;
            int PyramidExtraChest = 0;
            int BrokenPyramid = 0;

            for (int i = 0; i < Main.maxChests; i++)
            {
                if (Main.chest[i] == null || Main.chest[i].item[0] == null || Main.chest[i].item[0].type == ItemID.None) continue;
                
                //short chestIndex = (short)i;
                short posX = (short)Main.chest[i].x;
                short posY = (short)Main.chest[i].y;
                //bool isChestLocked = Chest.IsLocked(posX, posY);

                for(int t =0; t < 40 && Main.chest[i].item[t] !=null ; t++){
                
                    int iid = Main.chest[i].item[t].type;                    
                    
                    UpdateStats(Vartype.ChestItem, posX, posY, iid);
                    
                    if(Main.chest[i].item[t].prefix > 0 && Main.chest[i].item[t].prefix<PrefixID.Count){ 
                        
                        UpdateStats(Vartype.Modifier, posX, posY, Main.chest[i].item[t].prefix);                        
                        UpdateStats(Vartype.Modifier, posX, posY, 0);                        
                    
                    }
                    if(t==0){
                        bool inPyramid = Main.tile[posX,posY].WallType == WallID.SandstoneBrick;
                        bool PyramidLoot = iid == ItemID.SandstorminaBottle || iid == ItemID.FlyingCarpet || iid == ItemID.PharaohsMask;
                        if(inPyramid && !PyramidLoot) PyramidExtraChest++;
                        if(!inPyramid && PyramidLoot) BrokenPyramid++;

                    }

                    if(iid == ItemID.None) break;
                    if(t==0 && (iid == ItemID.PiranhaGun || iid == ItemID.ScourgeoftheCorruptor || iid == ItemID.VampireKnives || iid == ItemID.RainbowGun || iid == ItemID.StaffoftheFrostHydra || iid == ItemID.StormTigerStaff  ) ){
                        if(Main.tile[posX,posY].TileFrameX>200) //todo exact values
                            break;
                        else
                            uniqueBiomeChest++;
                    }
                        
                    
                }
            }
            if(ModMenuMod.uiSuperSeed != null && ModMenuMod.uiSuperSeed.generationProgress!=null) ModMenuMod.uiSuperSeed.generationProgress.Message = "Compute and store stats";
            
            for(short i=0; i< ItemID.Count; i++){                                
                if( GetCount(Vartype.ChestItem, i, 0)  > 0 ) 
                    EXstats += (i==0?"Any chest item":PropertyCondition.ContainsItemList.GetItemName(i)) +": " + GetStatsText(Vartype.ChestItem,i,0)   +Environment.NewLine;
            }
            EXstats += "Open Biome chests: " + uniqueBiomeChest + Environment.NewLine;
            EXstats += "Broken Pyramid chests: " + BrokenPyramid + Environment.NewLine;
            EXstats += "Pyramid extra chests: " + PyramidExtraChest + Environment.NewLine;            
            
            EXstats += "Pyramid chests: " + ( GetCount(Vartype.ChestItem, ItemID.SandstorminaBottle, 0) + GetCount(Vartype.ChestItem, ItemID.FlyingCarpet, 0)  + GetCount(Vartype.ChestItem, ItemID.PharaohsMask, 0)  ) + Environment.NewLine;
            EXstats += "Pyramid unique items: " + ((GetCount(Vartype.ChestItem, ItemID.SandstorminaBottle, 0) >0?1:0) + (GetCount(Vartype.ChestItem, ItemID.FlyingCarpet, 0)  >0?1:0) + (GetCount(Vartype.ChestItem, ItemID.PharaohsMask, 0)  >0?1:0) ) +Environment.NewLine;
            
            EXstats += Environment.NewLine + "## Modifier ##" + Environment.NewLine + StatsTableNames + Environment.NewLine;

           for(short i=0; i< PrefixID.Count; i++){
                if( GetCount(Vartype.Modifier, i, 0)  > 0 ) 
                    EXstats += (i==0?"Any modifier":GetName4ID(typeof(PrefixID), i)) + ": " + GetStatsText(Vartype.Modifier,i,0) + Environment.NewLine;
            }


            EXstats += Environment.NewLine + "## Chest Items with modifier ##" + Environment.NewLine + StatsTableNames + Environment.NewLine;
            for(short i=0; i< ItemID.Count; i++){                                
                if( GetCount(Vartype.ChestItem, i, 0)  > 0 ) {
                    for(short m=1; m < PrefixID.Count; m++){                                
                        if( GetCount(Vartype.ChestItem, i, m)  > 0 ){
                            EXstats += GetName4ID(typeof(PrefixID), m) + " " +(i==0?"Any item with modifier":PropertyCondition.ContainsItemList.GetItemName(i)) +": " + GetStatsText(Vartype.ChestItem,i,m)   +Environment.NewLine;
                        }
                    }                    
                }
            }

            

            string EXstatsPart2 = Environment.NewLine + "## Tiles & objects ##" + Environment.NewLine + StatsTableNames + Environment.NewLine;


            int DungeonMin = Main.maxTilesY;

            int snowAU = Main.maxTilesX;
            int jungAU = Main.maxTilesX;
            int desertUG = Main.maxTilesX;
            int evilAU = Main.maxTilesX;
            int temple = Main.maxTilesX;
            int JunglTilesWrongSide = 0;
            int SnowTilesWrongSide = 0;
                   
            int jungLastX = -1000;
            for (int xi = 0; xi < Main.maxTilesX; xi++)
            for (int yi = 0; yi < Main.maxTilesY; yi++)
            {
                if(Main.tile[xi,yi] == null ) continue;
                if(Main.tile[xi,yi].LiquidAmount>0 && Main.tile[xi,yi].LiquidType>=0){
                    UpdateStats(Vartype.Liquid,xi,yi, Main.tile[xi,yi].LiquidType );                    
                }
                if(Main.tile[xi,yi].RedWire || Main.tile[xi,yi].GreenWire || Main.tile[xi,yi].BlueWire || Main.tile[xi,yi].YellowWire ){
                                        
                    if(Main.tile[xi,yi].RedWire)UpdateStats(Vartype.Wire,xi,yi, 0 );
                    if(Main.tile[xi,yi].GreenWire)UpdateStats(Vartype.Wire,xi,yi, 0 );
                    if(Main.tile[xi,yi].BlueWire)UpdateStats(Vartype.Wire,xi,yi, 0 );
                    if(Main.tile[xi,yi].YellowWire)UpdateStats(Vartype.Wire,xi,yi, 0 );
                }
                
                

                if(Main.tile[xi,yi].WallType != WallID.None) UpdateStats(Vartype.Wall, xi,yi, Main.tile[xi,yi].WallType);
                if(!Main.tile[xi,yi].HasTile) continue;
                Tile tile = Main.tile[xi,yi];
                
                ushort tild = tile.TileType;

                UpdateStats(Vartype.Tile, xi,yi, tild);

                if( TileID.Sets.DungeonBiome[tild]>0 && yi < Main.worldSurface){
                    DungeonMin = Math.Min(DungeonMin, yi);
                }
                else if(TileID.Sets.SnowBiome[tild]>0 && yi<Main.worldSurface){
                    int val = xi-Main.maxTilesX/2;                    
                    snowAU = GenVars.dungeonSide<0 && snowAU!=Main.maxTilesX ?Math.Max(snowAU,val) :Math.Min(snowAU,val); 
                    if(  (xi>Main.maxTilesX/2 && GenVars.dungeonSide<0) ||  (xi<Main.maxTilesX/2 && GenVars.dungeonSide>0) ) SnowTilesWrongSide++;

                }else if( (TileID.Sets.JungleBiome[tild] > 0 || TileID.CorruptJungleGrass == tild || TileID.CrimsonJungleGrass == tild)  && yi<Main.worldSurface ){
                    int val = xi-Main.maxTilesX/2;                    
                    int jungAUnew = GenVars.dungeonSide>0 && jungAU!=Main.maxTilesX ?Math.Max(jungAU,val) :Math.Min(jungAU,val); 
                    
                    if(  (xi>Main.maxTilesX/2 && GenVars.dungeonSide>0) ||  (xi<Main.maxTilesX/2 && GenVars.dungeonSide<0) ) JunglTilesWrongSide++;

                    bool setNew = jungAUnew!=jungAU;
                    if(setNew ){
                        //some random jungle tiles can appear in snow and crimson/corruption
                        if( Math.Abs(xi-jungLastX) > 25){
                            int counterJ = 0;
                            for(int xii = Math.Max(xi-30,0);xii<Math.Min(xi+31,Main.maxTilesX);xii++)
                            for(int yii = Math.Max(yi-30,0);yii<Math.Min(yi+31,Main.maxTilesY);yii++)
                                if(Main.tile[xii,yii].HasTile && (TileID.Sets.JungleBiome[Main.tile[xii,yii].TileType] > 0 || TileID.CorruptJungleGrass == Main.tile[xii,yii].TileType || TileID.CrimsonJungleGrass == Main.tile[xii,yii].TileType )  ) counterJ++;                        
                            
                            if(counterJ<100) setNew = false;
                        }
                    }
                    if(setNew){
                        jungLastX = xi;
                        jungAU = jungAUnew;
                    }
                    
                }else if(WallID.Sets.AllowsUndergroundDesertEnemiesToSpawn[tile.WallType]  && yi>Main.rockLayer && xi > GenVars.desertHiveLeft && xi < GenVars.desertHiveRight ){ //needed?, just genvars?
                    int val = xi-Main.maxTilesX/2;
                    desertUG = xi<Main.maxTilesX/2 && desertUG!=Main.maxTilesX ?Math.Max(desertUG,val) :Math.Min(desertUG,val);
                }
                else if((TileID.Sets.Corrupt[tild] || TileID.Sets.Crimson[tild])  && yi < Main.worldSurface){
                    int val = xi-Main.maxTilesX/2;
                    evilAU = xi<Main.maxTilesX/2 && evilAU!=Main.maxTilesX ?Math.Max(evilAU,val) :Math.Min(evilAU,val);
                }

                if( tild == TileID.LihzahrdBrick ){
                    int val = xi-Main.maxTilesX/2;
                    val *= GenVars.dungeonSide<0? 1:-1;
                    temple = Math.Min(temple,val);                    
                }
            }

            int counterDbelowground =0 ;
            for(int xi = GenVars.dungeonX-10; xi<GenVars.dungeonX+11; xi++) counterDbelowground += (Main.tile[xi,DungeonMin-1]!=null && Main.tile[xi,DungeonMin-1].HasTile ? 1:0  );


            for(short lid=0; lid < LiquidID.Count ; lid++){
                
                EXstatsPart2 += GetLiquidName(lid) +",-1: " + GetStatsText(Vartype.Liquid,lid,0) +  Environment.NewLine;
                
            }
            

            EXstatsPart2 += "Wire,-1: " + GetStatsText(Vartype.Wire,0,0) +Environment.NewLine;

            for(ushort tild=0; tild < TileID.Count ; tild++){

                if(stats[(int)Vartype.Tile][(int)Statstype.Count][tild] == null || stats[(int)Vartype.Tile][(int)Statstype.Count][tild].Length==0) continue;

                for(int style = 0; style< stats[(int)Vartype.Tile][(int)Statstype.Count][tild].Length; style++){
                    if( GetCount(Vartype.Tile, tild, style ) >0 ){
                            
                            EXstatsPart2 +=  GetName4usID(typeof(TileID), tild)+","+style+ ": " + GetStatsText(Vartype.Tile,tild,style) + Environment.NewLine;

                            if(tild==TileID.LargePiles2 && style==17)
                                EXstatsPart2 +=  "EnchantedSword"+",-1: " + GetStatsText(Vartype.Tile,tild,style)+ Environment.NewLine;
                    }
                }
            }

            EXstatsPart2 += Environment.NewLine + Environment.NewLine +Environment.NewLine  + "## Walls ##" + Environment.NewLine + StatsTableNames + Environment.NewLine;
            for(ushort wild=1; wild < WallID.Count ; wild++){
                if(stats[(int)Vartype.Wall][(int)Statstype.Count][wild] == null || stats[(int)Vartype.Wall][(int)Statstype.Count][wild].Length==0) continue;
                EXstatsPart2 +=  GetName4usID(typeof(WallID), wild) + ": " + GetStatsText(Vartype.Wall,wild,0) + Environment.NewLine;

            }




            EXstatsPart2 += Environment.NewLine + Environment.NewLine +Environment.NewLine + "## Points of interest ##" + Environment.NewLine + Environment.NewLine;
            EXstatsPart2 += "Spawn: " + new Vector2(Main.spawnTileX, Main.spawnTileY) + Environment.NewLine;
            EXstatsPart2 += "Predicted Spawn: " + new Vector2(WorldInfo.PredictedSpawnX, WorldInfo.PredictedSpawnHeight) + Environment.NewLine;
            EXstatsPart2 += "Dungeon entrance: " + new Vector2(Main.dungeonX, Main.dungeonY) + Environment.NewLine;
            EXstatsPart2 += "Shimmer biome: " + new Vector2((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y) + Environment.NewLine;
            EXstatsPart2 += "Jungle main entrance (aprox.): " + new Vector2(WorldInfo.JungleMainEntranceX, WorldInfo.JungleMainEntranceYapprox) + Environment.NewLine;
            EXstatsPart2 += "Snow surface center (aprox.): " + new Vector2(WorldInfo.SnowTopOfSurfaceCenterX, WorldInfo.SnowTopOfSurfaceCenterY) + Environment.NewLine;
            EXstatsPart2 += "Desert surface center (aprox.): " + new Vector2(WorldInfo.DesertTopOfSurfaceCenterX, WorldInfo.DesertTopOfSurfaceCenterYapprox) + Environment.NewLine;
            EXstatsPart2 += "Dungeon beach: " + new Vector2(WorldInfo.DungeonBeachX, WorldInfo.DungeonBeachHeight) + Environment.NewLine;
            EXstatsPart2 += "Jungle beach: " + new Vector2(WorldInfo.JungleBeachX, WorldInfo.JungleBeachHeight) + Environment.NewLine;



            String[] points  = {"Spawn", "Jungle main entrance", "Near predicted Spawn (max dx = y)", "Near Jungle main entrance (max dx = y)", "Near Desert center surface, in/below Desert (max dx = y)" , "Near Snow center surface, in/below Snow (max dx = y+100)" , "Near Jungle beach (max dx = y+200)"  };
            ref Pathlength.Pathlength curPathlength =  ref DataExtractor.pathlength;

            EXstatsPart2 += Environment.NewLine + Environment.NewLine +Environment.NewLine + "## Pathlength ##" + Environment.NewLine + Environment.NewLine;
            
            for(int i = 0; i< (statsType == StoreStatsInfo.TooMany?points.Length:1) ; i++){
                if(i==2) EXstatsPart2 += Environment.NewLine + "# Pathlengths with close range #" + Environment.NewLine + Environment.NewLine;
                int fr = -1;
                int to = -1;
                //todo improve code
                string curName = points[i];
                switch(i){
                    case 0:
                        curPathlength =  ref DataExtractor.pathlength;
                        break;
                    case 1:
                        curPathlength =  ref DataExtractor.pathlengthFromJungleMainEntrance;
                        break;
                    case 2:
                        curPathlength =  ref DataExtractor.quickPathlength;
                        break;
                    case 3:
                        curPathlength =  ref DataExtractor.quickPathlengthJungleMainEntrance;
                        break;
                    case 4:
                        curPathlength =  ref DataExtractor.quickPathlengthDesertSurface;
                        fr = GenVars.desertHiveLeft;
                        to = GenVars.desertHiveRight;
                        break;
                    case 5:
                        curPathlength =  ref DataExtractor.quickPathlengthSnowCSurface;
                        break;
                    case 6:
                        curPathlength =  ref DataExtractor.quickPathlengthJungleBeach;
                        break;
                }
                int tmpv = 0;
                if( i== 5){
                    EXstatsPart2 += "From "  + curName + " to Underground: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) Main.worldSurface))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv)) + Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) Main.rockLayer))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) Main.rockLayer+40))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer +50%: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer)) ))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Desert deep item depth: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int)((GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7)  ))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern +50% until lava line -140 (snow end): "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int)(int)(Main.rockLayer+(GenVars.lavaLine-140))/2   ))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;            
                    EXstatsPart2 += "From "  + curName + " to start of lava line -140 (snow end): "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) GenVars.lavaLine-140))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to start of lava line: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) GenVars.lavaLine))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to start of lava line +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) GenVars.lavaLine+40))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Underworld: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) Main.maxTilesY-200))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Underworld +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepthSnow(ref curPathlength, (int) Main.maxTilesY-200+40))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;                
                }else{                
                    EXstatsPart2 += "From "  + curName + " to Underground: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) Main.worldSurface, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) Main.rockLayer, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) Main.rockLayer+40, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern layer +50%: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int)(Main.rockLayer+0.5*(Main.maxTilesY-200-Main.rockLayer)) , fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Desert deep item depth: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int)((GenVars.desertHiveHigh * 3 + GenVars.desertHiveLow * 4) / 7)  , fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Cavern +50% until lava line -140 (snow end): "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int)(int)(Main.rockLayer+(GenVars.lavaLine-140))/2   , fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;            
                    EXstatsPart2 += "From "  + curName + " to start of lava line -140 (snow end): "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) GenVars.lavaLine-140, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to start of lava line: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) GenVars.lavaLine, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to start of lava line +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) GenVars.lavaLine+40, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Underworld: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) Main.maxTilesY-200, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Underworld +40: "   + ((tmpv = PCPointOfInterestList.FindMinPlToDeepth(ref curPathlength, (int) Main.maxTilesY-200+40, fr, to))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    if(i==6){
                        EXstatsPart2 += "From "  + curName + " to Shimmer center: " + ((tmpv = curPathlength.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y-1))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;                
                        EXstatsPart2 += "From "  + curName + " to Jungle main entrance: "   + ((tmpv = PCPointOfInterestList.findMin(ref curPathlength, WorldInfo.JungleMainEntranceYapprox-10, WorldInfo.JungleMainEntranceYapprox,  WorldInfo.JungleMainEntranceX, -1  )) > curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;                        
                    }
                }
                if(i<2){
                    EXstatsPart2 += "+" + Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Predicted Spawn: "   + ((tmpv = curPathlength.get(WorldInfo.PredictedSpawnX, WorldInfo.PredictedSpawnHeight-1))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Jungle main entrance: "   + ((tmpv = PCPointOfInterestList.findMin(ref curPathlength, WorldInfo.JungleMainEntranceYapprox-10, WorldInfo.JungleMainEntranceYapprox,  WorldInfo.JungleMainEntranceX, -1  )) > curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Dungeon side beach: "   + ((tmpv = PCPointOfInterestList.findMin(ref curPathlength, WorldInfo.DungeonBeachHeight-10, WorldInfo.DungeonBeachHeight,  WorldInfo.DungeonBeachX, -1  )) > curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Jungle side beach: "   + ((tmpv = PCPointOfInterestList.findMin(ref curPathlength, WorldInfo.JungleBeachHeight-10, WorldInfo.JungleBeachHeight,  WorldInfo.JungleBeachX, -1  )) > curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Dungeon entrance: "   + ((tmpv = curPathlength.get(Main.dungeonX, Main.dungeonY-1))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to Shimmer center: " + ((tmpv = curPathlength.get((int)GenVars.shimmerPosition.X, (int)GenVars.shimmerPosition.Y-1))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;                
                    EXstatsPart2 += "From "  + curName + " to center of Snow surface: "   + ((tmpv = curPathlength.get(WorldInfo.SnowTopOfSurfaceCenterX, WorldInfo.SnowTopOfSurfaceCenterY))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                    EXstatsPart2 += "From "  + curName + " to center of Desert surface: "   + ((tmpv = curPathlength.get(WorldInfo.DesertTopOfSurfaceCenterX, WorldInfo.DesertTopOfSurfaceCenterYapprox))> curPathlength.maxValue?(">"+curPathlength.maxValue):(""+tmpv))+ Environment.NewLine;
                }

                EXstatsPart2 += Environment.NewLine;
            }
            



            EXstatsPart2 += Environment.NewLine + Environment.NewLine + "## Derived  stats ##" + Environment.NewLine;
            int dummy = 0;
            EXstatsPart2 += Math.Abs(dummy = Helpers.BasicFunctions.HorizontalDistToPyramid(Main.maxTilesX/2,true))>=Main.maxTilesX?"":("Closest Pyramid to mid difference: " + dummy + Environment.NewLine) ;
            EXstatsPart2 += Math.Abs(dummy = Helpers.BasicFunctions.HorizontalDisToOasis(Main.maxTilesX/2,true))>=Main.maxTilesX?"":("Closest Oasis to mid difference: " + dummy + Environment.NewLine) ;
            EXstatsPart2 += Math.Abs(dummy = (livingTrees[Main.maxTilesX/2-1]<livingTrees[Main.maxTilesX/2]?-1:1)*livingTrees[Main.maxTilesX/2]) >=Main.maxTilesX?"":("Closest Living tree to mid difference: " + dummy + Environment.NewLine) ;
            EXstatsPart2 += Math.Abs(dummy = Helpers.BasicFunctions.HorizontalDistToMountainCaveEntrance(Main.maxTilesX/2,true))>=Main.maxTilesX?"":("Closest mountain cave to mid difference: " + dummy + Environment.NewLine) ;
          
            EXstatsPart2 += "Dungeon below ground indicator: " + counterDbelowground + Environment.NewLine;
            EXstatsPart2 += "Jungle biome tiles wrong side: " + JunglTilesWrongSide + Environment.NewLine;
            EXstatsPart2 += "Snow biome tiles wrong side: " + SnowTilesWrongSide + Environment.NewLine;            
            EXstatsPart2 += "Snow surface to mid difference: " + snowAU + Environment.NewLine;
            EXstatsPart2 += "Jungle surface to mid difference: " + jungAU + Environment.NewLine;
            EXstatsPart2 += "Jungle-Snow surface distance: " + ( GenVars.dungeonSide<0? jungAU -snowAU: -jungAU +snowAU ) + Environment.NewLine;
            EXstatsPart2 += "Dungeon entrance to mid difference: " + (GenVars.dungeonX -Main.maxTilesX/2) + Environment.NewLine;
            EXstatsPart2 += "Dungeon entrance to start gen. mid difference: " + ((GenVars.dungeonX - GenVars.dungeonLocation )*(GenVars.dungeonSide <0?-1:1 ) )+ Environment.NewLine;
            EXstatsPart2 += "Dungeon entrance - Snow (mid) difference: " + ((GenVars.dungeonX -Main.maxTilesX/2)-snowAU)*( GenVars.dungeonSide<0?-1:1) + Environment.NewLine;
            EXstatsPart2 += "Desert biome to mid difference: " + (desertUG) + Environment.NewLine;
            EXstatsPart2 += "Evil biome to mid difference: " + (evilAU) + Environment.NewLine;            
            EXstatsPart2 += "Temple biome to mid difference: " + (temple)*( GenVars.dungeonSide<0?-1:1) + Environment.NewLine;
            EXstatsPart2 += "Dungeon to Desert center tiles in mid direction: " + (((GenVars.desertHiveLeft+GenVars.desertHiveRight)/2-GenVars.dungeonX)*(GenVars.dungeonSide<0? 1:-1 )) + Environment.NewLine;
            EXstatsPart2 += "Desert & Snow same side: " + (desertUG*snowAU >0) + (desertUG*snowAU >0?Math.Abs(desertUG) < Math.Abs(snowAU)?" Desert closert to mid" : " Snow closert to mid":"" )  + Environment.NewLine;
            
            
            if(statsType == StoreStatsInfo.TooMany){
                EXstatsPart2 += "Jungle center to Snow center distance: " + BiomeCenter[(int)StatstypeBiomeSpread.Jungle].Distance(BiomeCenter[(int)StatstypeBiomeSpread.Snow] ) + Environment.NewLine;
                EXstatsPart2 += "Jungle center to Temple center distance: " + BiomeCenter[(int)StatstypeBiomeSpread.Jungle].Distance(BiomeCenter[(int)StatstypeBiomeSpread.Temple] ) + Environment.NewLine;
                EXstatsPart2 += "Temple center to Snow center distance: " + BiomeCenter[(int)StatstypeBiomeSpread.Snow].Distance(BiomeCenter[(int)StatstypeBiomeSpread.Temple] ) + Environment.NewLine;
                EXstatsPart2 += "Temple center to Shimmer center distance: " + Vector2.Distance(BiomeCenter[(int)StatstypeBiomeSpread.Temple], new Vector2((float)GenVars.shimmerPosition.X,(float)GenVars.shimmerPosition.Y) ) + Environment.NewLine;
                EXstatsPart2 += "Temple center to Desert x-distance: " + Math.Min(Math.Abs(GenVars.desertHiveLeft-BiomeCenter[(int)StatstypeBiomeSpread.Temple].X), Math.Abs(GenVars.desertHiveRight-BiomeCenter[(int)StatstypeBiomeSpread.Temple].X))   + Environment.NewLine;                
                EXstatsPart2 += "Shimmer center to Jungle center distance: " + Vector2.Distance(BiomeCenter[(int)StatstypeBiomeSpread.Jungle], new Vector2((float)GenVars.shimmerPosition.X,(float)GenVars.shimmerPosition.Y) ) + Environment.NewLine;
                EXstatsPart2 += "Shimmer center to Desert x-distance: " + Math.Min( Math.Abs(GenVars.shimmerPosition.X-GenVars.desertHiveLeft), Math.Abs(GenVars.shimmerPosition.X-GenVars.desertHiveRight))   + Environment.NewLine;                
                EXstatsPart2 += "Shimmer center above Temple center y-difference: " + (BiomeCenter[(int)StatstypeBiomeSpread.Temple].Y - (float)GenVars.shimmerPosition.Y ) + Environment.NewLine;
                EXstatsPart2 += "Shimmer center - Temple center mid difference: " + ((BiomeCenter[(int)StatstypeBiomeSpread.Temple].X - (float)GenVars.shimmerPosition.X )*(GenVars.dungeonSide<0?-1:1 ))  + Environment.NewLine;

                for(StatstypeBiomeSpread si = 0; si < StatstypeBiomeSpread.Length; si++){
                    EXstatsPart2 += "used " + si + " center (mean of y-mids): " + BiomeCenter[(int) si] + Environment.NewLine;
                }
            }
            
             return EXstats+EXstatsPart2;

        }

        //todo optimize export
        public static string GetName4usID(Type type, ushort ID)
        {
            //var type = typeof(TileID);
            foreach (var field in type.GetFields())
            {
                if (field.FieldType == typeof(ushort) && (ushort)field.GetValue(null) == ID)
                {
                    return field.Name;
                }
            }
            return ID.ToString();
        }

        public static string GetName4ID(Type type, int ID){            
            foreach (var field in type.GetFields())
            {
                if (field.FieldType == typeof(int) && (int)field.GetValue(null) == ID)
                {
                    return field.Name;
                }
            }
            return ID.ToString();
        }

        public static int GetID4Name(Type type, string name){            
            foreach (var field in type.GetFields())
            {
                if (field.FieldType == typeof(int) && field.Name.Equals(name))
                {
                    return (int)field.GetValue(null);
                }
            }
            return -1;
        }

        public static string GetLiquidName(short ID)
        {

          var type = typeof(LiquidID);
            foreach (var field in type.GetFields())
            {
                if (field.FieldType == typeof(short) && (short)field.GetValue(null) == ID)
                {
                    return field.Name;
                }
            }
            return ID.ToString();
        }
       

    }
}