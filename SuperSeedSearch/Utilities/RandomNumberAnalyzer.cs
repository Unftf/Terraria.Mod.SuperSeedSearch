using Terraria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria.ID;
using System.IO;
using Terraria.Utilities;
using SuperSeedSearch.UI;



//TODO refactor, old legacy code
namespace SuperSeedSearch.RandomNumberAnalyzer
{
    public static class RandomNumberAnalyzer{
        public static int BulbPredictionTickCount = 60*3;
        static double[] RNGvalue;
        static int currentSeed = -1;

        static string bulbtxt;

        static List<Tuple<int,int,int>> PlanteraQuickBulbs;

        public static string GetBulbText(){
            if(currentSeed != ModMenuMod.wGPassChanger.currentSeed) return "";
            else return bulbtxt;
        }
        public static List<Tuple<int,int,int>> GetQuickBulbListIfExists(){
            if(currentSeed != ModMenuMod.wGPassChanger.currentSeed) return new List<Tuple<int,int,int>>();
            else return PlanteraQuickBulbs;
        }

        public static bool HasBeenUsedForCurrentSeed(){
            return currentSeed == ModMenuMod.wGPassChanger.currentSeed;
        }


        static public void generateNewRNG(int untilInd){            
            RNGvalue = new double[untilInd+1];
            currentSeed = ModMenuMod.wGPassChanger.currentSeed;
            UnifiedRandom rand = new UnifiedRandom(currentSeed);
            for(int i = 0; i <= untilInd; i++){
                RNGvalue[i] = rand.NextDouble();
            }
            findQBulbs(untilInd);//exclude?
        }
        static void findQBulbs(int untilInd){
            bulbtxt = "#Found potential quick bulb at " + currentSeed +Environment.NewLine;
            PlanteraQuickBulbs = new List<Tuple<int,int,int>>();
            int heightY = (Main.maxTilesY-20-(int)Main.worldSurface+1);
            int border =  (int)((Main.worldSurface+Main.rockLayer)/2);
            int with = Main.maxTilesX-20;
            int noAboveground = (int)((double)(Main.maxTilesX * Main.maxTilesY) * 3E-05f*2.0)+2+4;//308
               
            for(int ind = noAboveground; ind < untilInd; ind++){
                double r1 =  RNGvalue[ind];
                
                //assuming plant grown there
                if(  RNGvalue[ind]*60 < 1 && RNGvalue[ind-1]*25 < 1 ){
                    int y1 = (int)(Main.worldSurface -1 + RNGvalue[ind-2] * heightY);
                    int y2 = (int)(Main.worldSurface -1 + RNGvalue[ind-3] * heightY);
                    if(y2> border && RNGvalue[ind-2]*300 >1 ) { //300 = 0 case missing
                        int x2 = 10+(int)(with * RNGvalue[ind-4]);
                        PlanteraQuickBulbs.Add( new Tuple<int, int, int>(x2,y2,ind) );
                        bulbtxt += "  " + x2 + ", " + y2 + " at ind " + ind + " with tile id " + (Main.tile[x2, y2].HasTile?Main.tile[x2, y2].TileType:"air") + Environment.NewLine;
                        //show= false;
                    }
                    if(y1< border){                       
                        int x1 = 10+(int)(with * RNGvalue[ind-3]);                       
                        PlanteraQuickBulbs.Add( new Tuple<int, int, int>(x1,y1,ind) );
                        bulbtxt += "  " + x1 + ", " + y1 + " at ind " + ind + " with tile id " + (Main.tile[x1, y1].HasTile?Main.tile[x1, y1].TileType:"air") + Environment.NewLine;
                        
                    }
                }
                //assuming no plant grown there
                if(  RNGvalue[ind]*60 < 1 && RNGvalue[ind-1]*25 < 1 && RNGvalue[ind-2]*10 >= 1 ){
                    int y1 = (int)(Main.worldSurface -1 + RNGvalue[ind-3] * heightY);
                    int y2 = (int)(Main.worldSurface -1 + RNGvalue[ind-4] * heightY);
                    if(y2> border && RNGvalue[ind-3]*300 >1 ) { //300 = 0 case missing
                        int x2 = 10+(int)(with * RNGvalue[ind-5]);
                        PlanteraQuickBulbs.Add( new Tuple<int, int, int>(x2,y2,ind) );
                        bulbtxt += "  " + x2 + ", " + y2 + " at ind " + ind + " with tile id " + (Main.tile[x2, y2].HasTile?Main.tile[x2, y2].TileType:"air") + Environment.NewLine;
                        //show= false;
                    }
                    if(y1< border){                       
                        int x1 = 10+(int)(with * RNGvalue[ind-4]);                       
                        PlanteraQuickBulbs.Add( new Tuple<int, int, int>(x1,y1,ind) );
                        bulbtxt += "  " + x1 + ", " + y1 + " at ind " + ind + " with tile id " + (Main.tile[x1, y1].HasTile?Main.tile[x1, y1].TileType:"air") + Environment.NewLine;
                        
                    }
                }




            }
            bulbtxt += ("Found " + PlanteraQuickBulbs.Count + " potential quick bulbs at seed " + currentSeed + " surface "+ Main.worldSurface + " cavern " + Main.rockLayer + " border " + border + Environment.NewLine);
        }

        public static bool IsPosAqBulb(int posx, int posy, int untilInd, bool isNatural = false){
            if(currentSeed != ModMenuMod.wGPassChanger.currentSeed || RNGvalue == null || RNGvalue.Length <= untilInd)
                generateNewRNG(untilInd);

            foreach(var bulb in PlanteraQuickBulbs){
                if( bulb.Item1 == posx && bulb.Item2 == posy && untilInd >= bulb.Item3 ){

                        if(isNatural){
                            if( !Main.tile[posx,posy].HasTile ||  Main.tile[posx,posy].TileType != TileID.JungleGrass || !Main.tile[posx-1,posy].HasTile || Main.tile[posx-1,posy].TileType != TileID.JungleGrass || Main.tile[posx, posy].LiquidAmount != 0)
                                return false;

                            for(int xi = posx-1; xi<posx+1;xi++)
                            for(int yi = posy-2; yi<posy;  yi++){
                                Tile tile = Main.tile[xi,yi];
                                ushort type = tile.TileType;
                                
                                if( (tile!=null && tile.HasTile && type != 61 && type != 62 && type != 655 && type != 69 && type != 74 && type != 233 && (type != 185 || tile.TileFrameY != 0))   )
                                    return false;
                            }
                        }
                    return true;
                }
            }
            return false;
        }

        public static void BulbSpawnHelper(){//Todo, not always working
            if(PlanteraQuickBulbs==null) return;
            foreach(var bulb in PlanteraQuickBulbs){    
                    

                    var tile = Main.tile[bulb.Item1, bulb.Item2]; tile.HasTile = true; 
                    var tile2 = Main.tile[bulb.Item1-1, bulb.Item2]; tile2.HasTile = true;

                    WorldGen.ReplaceTile(bulb.Item1, bulb.Item2, TileID.JungleGrass,0);
                    WorldGen.ReplaceTile(bulb.Item1-1, bulb.Item2, TileID.JungleGrass,0);                    
                    
                    WorldGen.KillTile(bulb.Item1, bulb.Item2-1);
                    WorldGen.KillTile(bulb.Item1, bulb.Item2-2);                    
                    WorldGen.KillTile(bulb.Item1-1, bulb.Item2-1);
                    WorldGen.KillTile(bulb.Item1-1, bulb.Item2-2);
            }

        }

        public static int PredictAntiRandom(int seed){
            UnifiedRandom genRand = new UnifiedRandom(seed);
            double rr =genRand.Next();
            rr+=genRand.Next();
            float next = 0f;
			while (next == 0f)next = (float)genRand.Next(-400, 401) * 0.001f;
            for(int i=0;i<5+3;i++)rr+=genRand.Next();
            if((!WorldGen.dontStarveWorldGen || WorldGen.drunkWorldGen) && genRand.Next() == 0){rr++;};
			if (genRand.Next() == 0){}
            if ((!WorldGen.dontStarveWorldGen || WorldGen.drunkWorldGen) && genRand.Next() == 0){rr++;}

            return rr>0?genRand.Next(2):0;

        }

    }

}