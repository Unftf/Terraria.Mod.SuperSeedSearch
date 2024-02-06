using Terraria;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Terraria.ID;
using System.IO;
using Terraria.Utilities;
using Terraria.ModLoader;
using System.Linq;
using Terraria.WorldBuilding;


//TODO refactor, old legacy code
namespace SuperSeedSearch.Pathlength
{   
    public class Pathlength{
        public static bool nodig = false;
        public static bool nodigDrawPath = false;

        int[,] pathLength = null;
        int[] pathLengthToDepth;
        int[] pathLengthToDepthMin;
        bool debugMode = false;

        int StartX,StartY;

        int maxsize; 
        public int maxValue { get; private set; } 


        public int at(int posX, int posY ){
            if(pathLength == null) ComputePathlength();
            return pathLength[posX,posY];
        }
        public int get(int posX, int posY ){
            if(pathLength == null) ComputePathlength();

            int val = pathLength[posX,posY];

            if(posX>0 && posY>0 && posX < Main.maxTilesX-2 && posY< Main.maxTilesY-2){
                int[] nn = {                                                  
                            pathLength[posX-1, posY],
                            pathLength[posX+1, posY],
                            pathLength[posX, posY-1],
                            pathLength[posX, posY+1],
                            pathLength[posX-1, posY+1],
                            pathLength[posX-1, posY-1],
                            pathLength[posX+1, posY-1],
                            pathLength[posX+1, posY+1],
                            };                    
                for(int i=0;i<nn.Length;i++){
                    if(val > nn[i] ) val = nn[i];
                }
            }


            //if(debugMode)Console.WriteLine($" pathlength to {posX},{posY} is {pathLength[posX,posY]}, around {val}");
            return val;
        }
        public int getReducedByBestDepthPl(int posX, int posY ){
            if(pathLength == null) ComputePathlength();
            int pl = pathLength[posX,posY];
            //todo check: just subtracting depth cost y-1 is not enough )
            if(posX>0 && posY>0 && posX < Main.maxTilesX-2 && posY< Main.maxTilesY-2){
                int[] nn = {                                                  
                            pathLength[posX-1, posY]-pathLengthToDepth[posY],
                            pathLength[posX+1, posY]-pathLengthToDepth[posY],
                            pathLength[posX, posY-1]-pathLengthToDepth[posY-1],
                            pathLength[posX, posY+1]-pathLengthToDepth[posY+1],
                            pathLength[posX-1, posY+1]-pathLengthToDepth[posY+1],
                            pathLength[posX-1, posY-1]-pathLengthToDepth[posY-1],
                            pathLength[posX+1, posY-1]-pathLengthToDepth[posY-1],
                            pathLength[posX+1, posY+1]-pathLengthToDepth[posY+1],
                            };                    
                for(int i=0;i<nn.Length;i++){
                    if(pl > nn[i] ) pl = nn[i];
                }
            }
            
            if(pl== Int32.MaxValue || pathLengthToDepth[posY] == Int32.MaxValue || pathLengthToDepth[posY-1] == Int32.MaxValue || pathLengthToDepth[posY+1] == Int32.MaxValue  ) return pl;
            //float dc = DepthCost(posY);            
            
            return pl;
        }

        readonly bool storeAsImage;
        readonly int seed;
        readonly int xrestrictedbyyPlus;
        readonly string name;
        public Pathlength(int StartX, int StartY, int seed = -1, bool storeAsImage = false, float maxPlScalingFac = 4, int xrestrictedbyyPlus = -1, string name =""){
            this.StartX = StartX;
            this.StartY = StartY;
            maxsize = (int)(maxPlScalingFac/2.0*3.0 *Main.maxTilesX + maxPlScalingFac * Main.maxTilesY ); // values can be greater!
            maxValue = maxsize/pathNormFac;
            this.storeAsImage = storeAsImage;
            this.seed = seed;
            this.xrestrictedbyyPlus = xrestrictedbyyPlus;
            this.name = name;
        }
  
        void ComputePathlength()
        {   
            string oldMess = "";
            
            if(UI.ModMenuMod.uiSuperSeed!=null && UI.ModMenuMod.uiSuperSeed.generationProgress!=null){
                oldMess = UI.ModMenuMod.uiSuperSeed.generationProgress.Message;
                UI.ModMenuMod.uiSuperSeed.generationProgress.Message = name.Length==0?$"Compute pathlength from ({StartX},{StartY})":name;
            } 

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime;

            
            List<Tuple<int, int>>[] waypoints = new List<Tuple<int, int>>[maxsize];
            short[,] travelCost = new short[Main.maxTilesX, Main.maxTilesY];

            pathLength = new int[Main.maxTilesX, Main.maxTilesY];
            pathLengthToDepth = new int[Main.maxTilesY];
            pathLengthToDepthMin = new int[Main.maxTilesY];
            
            Array.Fill(pathLengthToDepth, Int32.MaxValue);            
            
            //todo find something better
            const int minHeight = 30;            
            int[] depth = new int[Main.maxTilesX];
            int[] depth1 = new int[Main.maxTilesX];            
            int[] depth2 = new int[Main.maxTilesX];
            for (int x = 0; x < Main.maxTilesX; x++)
                for (int y = 0; y < Main.maxTilesY; y++){
                    pathLength[x, y] = Int32.MaxValue;     
                    if( depth[x] == 0 && Math.Min(x,Main.maxTilesX-x)<150 )   depth[x] = (int)GenVars.worldSurfaceLow;               
                    if( depth[x] == 0 && y+minHeight < Main.maxTilesY && ((Main.tile[x,y+minHeight].HasTile && Main.tileSolid[Main.tile[x,y+minHeight].TileType]) ||  Main.tile[x,y+minHeight].LiquidAmount >0) ){                        
                        //depth[x] = Math.Min(y, (int)GenVars.worldSurfaceLow-30);
                        depth[x] = (int)(y>GenVars.worldSurfaceLow? (y+GenVars.worldSurfaceLow)/2:y)  ;
                    }
                }    

            for(int i=5;i>0;i-=2){
                depth.CopyTo(depth1,0);
                depth.CopyTo(depth2,0);

                for (int x = 140; x < Main.maxTilesX-140; x++)
                    depth1[x] = Math.Min(depth1[x], depth1[x-i]+1);
                for (int x = Main.maxTilesX-139; x > 139; x--)
                    depth2[x] = Math.Min(depth2[x], depth2[x+i]+1);
                for (int x = 140; x < Main.maxTilesX-140; x++) depth[x] = Math.Min(depth1[x],depth2[x]);
            }
            depth1 = null;
            depth2 = null;

            for (int x = 0; x < Main.maxTilesX; x++)
                for (int y = 0; y < Main.maxTilesY; y++){
                    if( (xrestrictedbyyPlus>=0 && Math.Abs(StartX-x)-xrestrictedbyyPlus >y) || y < depth[x] )  // limit? worldsurfacelow - 50 ~25% faster, but pathlength differ
                        travelCost[x, y] = (short)NO_PASS;
                    else
                        travelCost[x, y] = (short)TravelCost(x, y);
                }

            
            float pathNormFacInv = 1.0f/pathNormFac;

            pathLengthToDepthMin[StartY] = 0;
            for (int y = StartY-1; y >= 0; y--)
                pathLengthToDepthMin[y] = pathLengthToDepthMin[y+1] + costU + DepthCost(y);
            for (int y = StartY+1; y < Main.maxTilesY; y++)
                pathLengthToDepthMin[y] = pathLengthToDepthMin[y-1] + costD + DepthCost(y);
            for (int y = 0; y < Main.maxTilesY; y++)
                pathLengthToDepthMin[y] = (int)(pathNormFacInv *pathLengthToDepthMin[y]);


            pathLength[StartX, StartY] = 0;
            waypoints[0] = new List<Tuple<int, int>> { new Tuple<int, int>(StartX, StartY) };
            for(int yi=1; yi<maxsize;yi++)waypoints[yi] = new List<Tuple<int, int>> ();

            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);            
            if(debugMode)Console.WriteLine(" analyze time after init pathfinding " + elapsedTime);
            
            
            int countTotal = 0;
            for (int l = 0; l < maxsize; l++)
            {
                if (waypoints[l] != null)
                {
                    foreach (Tuple<int, int> p in waypoints[l])
                        AddWayPoints(ref pathLength, ref waypoints, ref travelCost, p.Item1, p.Item2, l);
                    countTotal += waypoints[l].Count;
                    waypoints[l].Clear();
                }
            }
            if(debugMode) Console.WriteLine($"node count total: " + countTotal + " ~" + Math.Round((float)countTotal/1000/1000)+"M" );
                        

            //norm to ~tiles num --> pathlengthNormFac            
            for (int x = 1; x < Main.maxTilesX-1; x++)
                for (int y = 1; y < Main.maxTilesY-1; y++){ 
                    if (pathLength[x, y] != Int32.MaxValue){
                        pathLength[x, y] = (int)Math.Ceiling(pathNormFacInv * pathLength[x, y]);
                        pathLengthToDepth[y] = Math.Min(pathLengthToDepth[y], pathLength[x, y] );
                    }
                    
                }
            
            travelCost = null;
            waypoints = null;

            ts = stopWatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);            
            if(debugMode)Console.WriteLine(" analyze time after pathfinding " + elapsedTime);

            //##################### only for debug
            if (storeAsImage || nodigDrawPath  )
            {
                int dimX = Main.maxTilesX;
                int dimY = Main.maxTilesY;
                int scale = 1;
                /*while (dimX > 6200)
                {
                    dimX /= 2;
                    dimY /= 2;
                    scale *= 2;
                }*/

                int bytes = dimX * dimY * 4;
                byte[] rgbValues = new byte[bytes];

                int indx = 0;
                float normf = 1.0f/ (pathNormFacInv*maxsize);
                for (int y = 0; y < Main.maxTilesY; y += scale)
                    for (int x = 0; x < Main.maxTilesX; x += scale)
                    {

                        int cv = (int)(((float)pathLength[x, y]) * normf * 255.0f);
                        //int cv = (int)(((float)pathLength[x, y]) / (pathNormFacInv));
                        //int cv = (int)(((float)(pathLength[x, y]-pathLengthToDepth[y] )) / (pathLengthToDepthMin[Main.maxTilesY-1]) * 255.0f);
                        //int cv = (int)(((float)(pathLength[x, y]-pathLengthToDepth[y] )) / ( pathLengthToDepth[y]-pathLengthToDepthMin[y]+1 ) * 255.0f);
                        if(pathLength[x, y]==Int32.MaxValue) cv = 255;

                        cv = cv > 255 ? 255 : cv;
                        rgbValues[indx++] = (byte)(255-cv);
                        rgbValues[indx++] = (byte)(255- cv);
                        rgbValues[indx++] = (byte)(255- cv);
                        rgbValues[indx++] = 255;

                    }

                //draw Spawm
                int aw = 0;

                for (int y = StartY - 1; y > StartY - 36; y--)
                {
                    int x = StartX;
                    int off = y * 4 * Main.maxTilesX + x * 4;

                    rgbValues[off + 0] = 0;
                    rgbValues[off + 1] = 80;
                    rgbValues[off + 2] = 50;
                    rgbValues[off + 3] = 255;

                    for (int awi = 0; awi < (aw < 18 ? aw : 4); awi++)
                    {
                        rgbValues[off + 0 + 4 * (awi / 3)] = 0;
                        rgbValues[off + 1 + 4 * (awi / 3)] = 80;
                        rgbValues[off + 2 + 4 * (awi / 3)] = 50;
                        rgbValues[off + 3 + 4 * (awi / 3)] = 255;

                        rgbValues[off + 0 - 4 * (awi / 3)] = 0;
                        rgbValues[off + 1 - 4 * (awi / 3)] = 80;
                        rgbValues[off + 2 - 4 * (awi / 3)] = 50;
                        rgbValues[off + 3 - 4 * (awi / 3)] = 255;
                    }
                    aw++;
                    
                }               
                ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);                
                if(debugMode)Console.WriteLine(" image preparation time after pathfinding " + elapsedTime);

                using FileStream fileStream = File.Create(Main.WorldPath + @"/" + seed + "_(" + StartX + ")(" + StartY +")_paths.png");
                //SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Bgra32> img = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Bgra32>(rgbValues, Main.maxTilesX, Main.maxTilesY);
                //SixLabors.ImageSharp.ImageExtensions.SaveAsPng((SixLabors.ImageSharp.Image)(object)img, (Stream)fileStream);
                //fileStream.Close();
                
                for(int bi=0;bi<bytes;bi+=4)(rgbValues[bi], rgbValues[bi+2]) = (rgbValues[bi+2], rgbValues[bi]);//other format now, todo
                PlatformUtilities.SavePng(fileStream, Main.maxTilesX, Main.maxTilesY, Main.maxTilesX, Main.maxTilesY, rgbValues);
                
                
                ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);                
                if(debugMode)Console.WriteLine(" time after storing pathfinding image" + elapsedTime);


                if(UI.ModMenuMod.uiSuperSeed!=null && UI.ModMenuMod.uiSuperSeed.generationProgress!=null){                    
                    UI.ModMenuMod.uiSuperSeed.generationProgress.Message = oldMess;
                } 

            }
          
       


        }

        //Todo remove?
        Tuple<List<Tuple<int, int>>, int, int> FindCaveEntrance(ref int[,] pathlength, int x, int y, bool excludeLastMiningTiles = true, int borderLeftX = -1, int borderRightX = 100000)
        {
            List<Tuple<int, int>> path = new List<Tuple<int, int>>();
            path.Add(new Tuple<int, int>(x, y));

            int ox = -1;
            int oy = -1;

            int ominv = pathlength[x + 0, y + 0];
            int tilesToMine = 0;
            int ctilesToMine = 0;
            int tilesOutside = 0;

            while ((y > Main.worldSurface && y > Main.spawnTileY + 10) || Main.tile[x, y].WallType != WallID.None || Main.tile[x, y].HasTile == true)
            {
                ox = x;
                oy = y;

                //writeDebugFile("at " + x +"," + y + " :" +pathlength[x + 0, y + 0] + " " + pathlength[x + 1, y + 0] + " " + pathlength[x + 1, y + 1] + " " + pathlength[x - 1, y + 1] +" " + pathlength[x + 0, y + 1]  + " " + pathlength[x - 1, y + 0] + " " + pathlength[x - 1, y - 1] + " " + pathlength[x + 0, y - 1] + " " + pathlength[x + 1, y - 1]);


                int minv = pathlength[x + 0, y + 0];

                if (x < borderLeftX || x > borderRightX)
                    tilesOutside++;

                ominv = minv;



                if (minv > pathlength[x + 1, y + 0]) minv = pathlength[x + 1, y + 0];
                if (minv > pathlength[x + 1, y + 1]) minv = pathlength[x + 1, y + 1];
                if (minv > pathlength[x + 0, y + 1]) minv = pathlength[x + 0, y + 1];
                if (minv > pathlength[x - 1, y + 1]) minv = pathlength[x - 1, y + 1];
                if (minv > pathlength[x - 1, y + 0]) minv = pathlength[x - 1, y + 0];
                if (minv > pathlength[x - 1, y - 1]) minv = pathlength[x - 1, y - 1];
                if (minv > pathlength[x + 0, y - 1]) minv = pathlength[x + 0, y - 1];
                if (minv > pathlength[x + 1, y - 1]) minv = pathlength[x + 1, y - 1];

                if (minv == pathlength[x + 1, y - 1]) { x++; y--; }
                else if (minv == pathlength[x, y - 1]) { y--; }
                else if (minv == pathlength[x - 1, y - 1]) { x--; y--; }
                else if (minv == pathlength[x - 1, y]) { x--; }
                else if (minv == pathlength[x - 1, y + 1]) { x--; y++; }
                else if (minv == pathlength[x, y + 1]) { y++; }
                else if (minv == pathlength[x + 1, y + 1]) { x++; y++; }
                else if (minv == pathlength[x + 1, y]) { x++; y++; }

                if (minv < ominv - 60 && !(Main.tile[x, y].LiquidAmount > 0))
                    ctilesToMine++;
                else
                {
                    //exclude last tiles 
                    tilesToMine += ctilesToMine;
                    ctilesToMine = 0;
                }

                if (ox == x && oy == y)
                {
                    //if (x != Main.spawnTileX && y != Main.spawnTileY)
                    //    writeDebugFile("entrance finding failed for seed " + seed);
    
                    //should not happen  , only if start is in spawn like in seed 170170245
                    break;
                }
                path.Add(new Tuple<int, int>(x, y));
            }

            if (!excludeLastMiningTiles) tilesToMine += ctilesToMine;
            return new Tuple<List<Tuple<int, int>>, int, int>(path, tilesToMine, tilesOutside);
        }


        const int pathNormFac = 5;
        const int costLR = pathNormFac;
        const int costU = 5;
        const int costD = 3;
        const int costDLR = 6;
        const int costULR = 7;

        private void AddWayPoints(ref int[,] pathLength, ref List<Tuple<int, int>>[] waypoints, ref short[,] travelCost, int x, int y, int l)
        {
            

            if (pathLength[x, y] < l || x < 42 || y < 10 || x > Main.maxTilesX - 43 || y > Main.maxTilesY - 11) return;

            //careful: tracelCost in short with value max 10,000, 3 times close to max short value
            int tcl = travelCost[x - 1, y] + travelCost[x - 1, y + 1] + travelCost[x - 1, y - 1];
            int tcr = travelCost[x + 1, y] + travelCost[x + 1, y + 1] + travelCost[x + 1, y - 1];
            int tcu = travelCost[x, y - 1] + travelCost[x + 1, y - 1];
            int tcd = travelCost[x, y + 1] + travelCost[x + 1, y + 1];

            int mpl = l + MIN_PATH_LENGTH;
            if (pathLength[x - 1, y] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x - 1, y, l, costLR, tcl);
            if (pathLength[x + 1, y] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x + 1, y, l, costLR, tcr);
            if (pathLength[x, y + 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x, y + 1, l, costD, tcd);
            if (pathLength[x, y - 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x, y - 1, l, costU, tcu, true);
            if (pathLength[x - 1, y - 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x - 1, y - 1, l, costULR, tcu + tcl, true);
            if (pathLength[x + 1, y - 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x + 1, y - 1, l, costULR, tcu + tcr, true);
            if (pathLength[x - 1, y + 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x - 1, y + 1, l, costDLR, tcd + tcl);
            if (pathLength[x + 1, y + 1] > mpl)
                AddPoint(ref pathLength, ref waypoints, ref travelCost, x + 1, y + 1, l, costDLR, tcd + tcr);

        }


        const short costWater = 20;
        const short costHoneyBon = 15;
        const short costLavaBon = 800;
        const short costShimmerBon = 100;
        const short costFullLiqBoni = 15;

        const short costPlatform = 0;

        const short costUnderWBoni = 15;
        const short costCavernLavaBoni = 3;
        const short costCavernBoni = 2;
        const short costUnderGBoni = 1;
        const short costDungeonBoni = 1;
        const short costDungeonBelowSurfBoni = 200;

        const short costWebThorn = 15; //maybe too high 
        const short costMineCart = -17; //maybe too good
        const short costDirt = 90; //90 maybe too high, changed from 60 to 50
        const short costStone = 160; //160 maybe too high, changed from 120 to 100

        const short costEvilWall = 2;

        private int DepthCost(int y){
            if (y > Main.maxTilesY - 190)
                return costUnderWBoni;
            else if (y > Terraria.WorldBuilding.GenVars.lavaLine )
                return costCavernLavaBoni;
            else if (y > Main.rockLayer)
                return costCavernBoni;
            else if (y > Main.worldSurface)
                return costUnderGBoni;
            return 0;
        }


        private int TravelCost(int x, int y)
        {
            //ignores stuff not generated in vanilla world gen

            int bon = DepthCost(y);

            ushort type = Main.tile[x, y].TileType;
            ushort wall = Main.tile[x, y].WallType;

            if (wall == WallID.EbonstoneUnsafe || wall == WallID.CorruptGrassUnsafe || wall == WallID.CrimstoneUnsafe || wall == WallID.CrimsonGrassUnsafe )
                bon += costEvilWall;

            bool inDungeon = isInDungeon(x, y);
            if (inDungeon)
            {
                if (y < Main.worldSurface + 3)
                {
                    bon += costDungeonBoni;
                }
                else
                {
                    bon += costDungeonBelowSurfBoni;
                }

            }

            if (Main.tile[x, y].LiquidAmount > 0)
            {
                if (x == 0 || x == Main.maxTilesX - 1 || y == 0 || y == Main.maxTilesY - 1)
                    bon += costFullLiqBoni;
                else if (Main.tile[x + 1, y].LiquidAmount > 0 && Main.tile[x - 1, y].LiquidAmount > 0 && Main.tile[x + 1, y - 1].LiquidAmount > 0 && Main.tile[x - 1, y - 1].LiquidAmount > 0 && Main.tile[x, y - 1].LiquidAmount > 0 && Main.tile[x + 1, y + 1].LiquidAmount > 0 && Main.tile[x - 1, y + 1].LiquidAmount > 0 && Main.tile[x, y + 1].LiquidAmount > 0)
                    bon += costFullLiqBoni;

                return costWater + bon + (Main.tile[x, y].LiquidType == LiquidID.Honey ? costHoneyBon : 0) + (Main.tile[x, y].LiquidType == LiquidID.Lava ? costLavaBon : 0) + (Main.tile[x, y].LiquidType == LiquidID.Shimmer ? costShimmerBon : 0);
            }



            bool val = ((!Main.tile[x, y].HasTile
                || type == TileID.Plants
                || type == TileID.Plants2
                || type == TileID.Trees
                || type == TileID.PalmTree
                || type == TileID.CorruptPlants
                || type == TileID.WaterCandle
                || type == TileID.Bottles
                || type == TileID.Books
                || type == TileID.Books
                || type == TileID.JunglePlants
                || type == TileID.JunglePlants2
                || type == TileID.JungleVines
                || type == TileID.MushroomPlants
                || type == TileID.MushroomTrees
                || type == TileID.SmallPiles
                || type == TileID.LargePiles
                || type == TileID.LargePiles2
                || type == TileID.Coral
                || type == TileID.BloomingHerbs
                || type == TileID.ImmatureHerbs
                || type == TileID.MatureHerbs
                || type == TileID.Vines
                || type == TileID.VineFlowers
                || type == TileID.Cactus
                || type == TileID.Banners
                || type == TileID.Lamps
                || type == TileID.PressurePlates
                || type == TileID.Stalactite
                || type == TileID.Explosives
                || type == TileID.Detonator
                || type == TileID.CrimsonPlants 
                || type == TileID.CrimsonVines
                || type == TileID.DyePlants
                || type == TileID.DyePlants
                || type == TileID.LongMoss
                || type == TileID.Pots
                || type == TileID.WorkBenches
                || type == TileID.Tables
                || type == TileID.Tables2
                || type == TileID.Bookcases
                || type == TileID.Anvils
                || type == TileID.Statues
                || type == TileID.Chairs                
                || type == TileID.Containers
                || type == TileID.Containers2
                || type == TileID.Sunflower
                || type == TileID.ShadowOrbs
                || type == TileID.DemonAltar
                || type == TileID.LihzahrdAltar
                || type == TileID.SharpeningStation
                || type == TileID.AlchemyTable
                || type == TileID.Larva
                || type == TileID.WoodenBeam
                || (type == TileID.ClosedDoor && Main.tile[x, y].TileFrameY != 594 && Main.tile[x, y].TileFrameY != 612 && Main.tile[x, y].TileFrameY != 620)  //temple door not allowed
                ));
            if (val)
                return 0 + bon;

            if (type == TileID.Cobweb || type == TileID.CorruptThorns || type == TileID.CrimsonThorns || type == TileID.JungleThorns)
                return costWebThorn + bon;

            if (type == TileID.Platforms)
                return costPlatform + bon;

            if (type == TileID.MinecartTrack)
                return costMineCart + bon;

            if (type == TileID.Dirt || type == TileID.Grass || type == TileID.CrimsonGrass || type == TileID.CorruptGrass || type == TileID.JungleGrass || type == TileID.MushroomGrass || type == TileID.Sand || type == TileID.ClayBlock || type == TileID.Mud || type == TileID.Silt || type == TileID.Ash || type == TileID.SnowBlock || type == TileID.Slush || type == TileID.HardenedSand || type == TileID.CrimsonHardenedSand || type == TileID.CorruptHardenedSand || type == TileID.Hive)
                return nodig?NO_PASS: costDirt + bon;

            if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick || type == TileID.Obsidian || type == TileID.Ebonstone || type == TileID.Crimstone || type == TileID.Hellstone || type == TileID.LihzahrdBrick || (type == TileID.Traps && Main.tile[x, y].TileFrameY>0 ) || ((type == TileID.Demonite || type == TileID.Crimtane) && y>Main.worldSurface) || (type == TileID.ClosedDoor && (Main.tile[x, y].TileFrameY == 594 || Main.tile[x, y].TileFrameY == 612 || Main.tile[x, y].TileFrameY == 620)))
                return NO_PASS;
            //tod high crimtan

            //stone, ores, mossstone, gemstone
            return nodig?NO_PASS: costStone + bon;
        }

        const int inAirCost = 25;
        const int noWallCost = 30;
        private int checkInAir(ref short[,] travelCost, int x, int y)
        {
            int air = 0;
            

            air = (travelCost[x, y + 2] > 50 && Main.tile[x, y].LiquidAmount == 0) || (travelCost[x + 1, y + 2] > 50 && Main.tile[x, y].LiquidAmount == 0) ? 0 : inAirCost;
            air = Main.tile[x, y + 2].TileType  == TileID.Platforms || Main.tile[x + 1, y + 2].TileType == TileID.Platforms || Main.tile[x, y + 2].TileType  == TileID.MinecartTrack || Main.tile[x + 1, y + 2].TileType == TileID.MinecartTrack ? 0 : air;

            if (air > 0 && Main.tile[x, y + 2].WallType == 0 && Main.tile[x + 1, y + 2].WallType == 0)
                air += noWallCost;

            return air;
        }


        const int MIN_PATH_LENGTH = 3;
        const int NO_PASS = 10000;
        private void AddPoint(ref int[,] pathLength, ref List<Tuple<int, int>>[] waypoints, ref short[,] travelCost, int x, int y, int l, int newLengthAdd, int tileCost, bool countInAir = false)
        {
            const int CAN_NOT_PASS = 1000;

            //if (tileCost > CAN_NOT_PASS)
            //   return;

            int air = 0;
            if (countInAir)
            {
                air = checkInAir(ref travelCost, x, y);
            }



            int add = Math.Max(MIN_PATH_LENGTH, newLengthAdd + tileCost + air);


            if (add > CAN_NOT_PASS)
                return;

            int npl = l + add;

            if (pathLength[x, y] > npl)
            {
                //if(pathLength[x, y] < maxsize){                                        
                //    waypoints[pathLength[x, y]].Remove(new Tuple<int, int>(x, y));//removing takes much longer, large world test 18M 11sec vs 13M 53sec, other way?
                //}

                pathLength[x, y] = npl;
                if (npl < waypoints.Length)
                {
                    //if (waypoints[npl] == null)
                    //    waypoints[npl] = new List<Tuple<int, int>>();
                    waypoints[npl].Add(new Tuple<int, int>(x, y));               
                }
            }

        }

        //e.g. you can also access stuff through walls
        public int FindShortestPathInRange(int x, int y, int mx = 2, int px = 3, int my = 2, int py = 3)
        {
            int minVal = Int32.MaxValue;

            for (int xi = x - mx; xi <= x + px; xi++)
                for (int yi = y - my; yi <= y + py; yi++)
                    minVal = pathLength[xi, yi] < minVal ? pathLength[xi, yi] : minVal;

            //return minVal / pathNormFac;
            return minVal;
        }


        private static bool isInDungeon(int x, int y)
        {            
            ushort chestWall = Main.tile[x, y].WallType;
            if ((chestWall == 7 || chestWall == 8 || chestWall == 9)
                          ||
                        (chestWall == 94 || chestWall == 98 || chestWall == 96)
                          ||
                        (chestWall == 95 || chestWall == 99 || chestWall == 97))
            {
                
                return true;
            }            
            return false;
        }





    }
}