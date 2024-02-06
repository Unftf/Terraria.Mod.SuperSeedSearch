using Terraria.ModLoader;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Biomes;
using Terraria.GameContent.Creative;
using Terraria.GameContent.Events;
using Terraria.GameContent.Generation;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI.States;
using Terraria.Graphics.Capture;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;
using Terraria.Utilities;
using Terraria.WorldBuilding;
using System.Reflection;
using ReLogic.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using System.Drawing.Imaging;
using Terraria.Graphics;
using System.Collections;
using System.IO;
using ReLogic.Utilities;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI.Gamepad;
using Terraria.Social;
using ReLogic.Content;
using ReLogic.OS;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content.Sources;
using ReLogic.Localization.IME;
using ReLogic.Peripherals.RGB;
using Steamworks;
using Terraria.Achievements;
using Terraria.Cinematics;
using Terraria.GameContent.Ambience;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Golf;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Liquid;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Skies;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.BigProgressBar;
using Terraria.GameContent.UI.Chat;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Light;
using Terraria.Graphics.Renderers;
using Terraria.Graphics.Shaders;
using Terraria.Initializers;
using Terraria.ModLoader.Core;
using System.Globalization;
using SuperSeedSearch.Storage;
using SuperSeedSearch.WorldGenMod;


namespace SuperSeedSearch.Helpers
{
    public static class BasicFunctions
    {
        public static int DistanceToMidWorldHorizontal(int posX)
        {
            return Math.Abs(Main.maxTilesX/2 - posX);
        }
        public static int DistanceToSpawnHorizontal(int posX)
        {
            return Math.Abs(Main.spawnTileX - posX);
        }

        public static int DistanceToDungeonXHorizontal(int posX)
        {
            //return Math.Abs(WorldInfo.GetValueAsInt(DataExtractor.VanillaVar.DungeonLocationXEarly.name) - posX);
            return Math.Abs(GenVars.dungeonLocation - posX);
        }

        public static int DistanceToJungleMainEntranceHorizontal(int posX)
        {
            return Math.Abs(WorldInfo.JungleMainEntranceX - posX);
        }

        
        public static double DistanceEuclid(int posX1, int posY1, int posX2, int posY2, double weightFactorX = 1f)
        {
            double f2 = weightFactorX * weightFactorX;
            double horz = Math.Abs(posX1 - posX2);
            double vert = Math.Abs(posY1 - posY2);            
            return Math.Sqrt(2.0 * f2 / (f2 + 1) * horz * horz + 2.0 / (f2 + 1) * vert * vert);
        }

        public static int EndOfWorldThan2nd(int posX1, int posX2){
            int od1 = posX1<Main.maxTilesX/2?posX1:Main.maxTilesX-posX1;
            int od2 = posX2<Main.maxTilesX/2?posX2:Main.maxTilesX-posX2;
            return od2-od1;
        }

        public static int SkyNPCSpawn => (int)(Main.worldSurface * 0.45 + 47);

        public static double HeightAbove2nd(int posY, int point2nd ) => point2nd - posY;
        
        public static int DepthBelowPredictedSpawn(int posY) => posY-WorldInfo.PredictedSpawnHeight; 
      
        public static int HorizontalDistToStartOfIceBiomeGen(int posX){
            int sl = GenVars.snowOriginLeft;
            int sr = GenVars.snowOriginRight;
            if(posX>sl && posX<sr){
                return 0; //or better a diff inside the snow biome?
            }            
            return Math.Min(Math.Abs(posX-sl),Math.Abs(posX-sr));
        }
  
        public static int HorizontalDistToMountainCaveEntrance(int posX, bool returnDiff2middistanceInstead = false){
            int numMCaves = GenVars.numMCaves;
            int[] CavesX = GenVars.mCaveX;
            //int[] CavesY = WorldInfo.GetValueAsIntArray(DataExtractor.VanillaVar.MountainCavesY.name);
            int minDist = Int32.MaxValue;
            int minDiff = Int32.MaxValue;
            for(int i=0;i<numMCaves;i++){
                int distNew = Math.Abs(posX-CavesX[i]);
                if(distNew<minDist){
                    minDist = Math.Min(minDist,distNew);
                    minDiff = (CavesX[i]-posX) * (posX > Main.maxTilesX/2 ? -1: 1);  
                }                
            }  
            return returnDiff2middistanceInstead?minDiff:minDist;
        }

         public static int HorizontalDisToOasis(int posX, bool returnDiff2middistanceInstead = false, int returnNumWithDiff = -1){
            
            int numOasis = GenVars.numOasis;
            Point[] Oasis = GenVars.oasisPosition;
            
            int minDist = Int32.MaxValue;
            int minDiff = Int32.MaxValue;
            int counter = 0;
            for(int i=0;i<numOasis;i++){
                int distNew = Math.Abs(posX-Oasis[i].X);
                if(distNew<minDist){
                    minDist = Math.Min(minDist,distNew);
                    minDiff = (Oasis[i].X-posX) * (posX > Main.maxTilesX/2 ? -1: 1);  
                }       
                if(distNew<=returnNumWithDiff)  counter++;       
            }  
            
            return returnNumWithDiff<0?(returnDiff2middistanceInstead?minDiff:minDist) : counter;
        }

        public static int HorizontalDistToPyramid(int posX, bool returnDiff2middistanceInstead = false, bool justMight = false,  int returnNumWithDiff = -1){
            int numPy = GenVars.numPyr;
            int[] Pyx = GenVars.PyrX;            
            int minDist = Int32.MaxValue;
            int minDiff = Int32.MaxValue;
            int counter = 0;
            for(int i=0;i<numPy;i++){
                int distNew = Math.Abs(posX-Pyx[i]);
                if(distNew<minDist ||  distNew < returnNumWithDiff ){
                    int Pyy = GenVars.PyrY[i];
                    bool actuallyAPyramid = false;
                    if(justMight) actuallyAPyramid = true;
                    for(int j=0; j< Pyy && !actuallyAPyramid;j+=5){
                        for(int s=-1; s < 3; s+=2){
                            if( Main.tile[Pyx[i],Pyy+j*s].HasTile && Main.tile[Pyx[i],Pyy+j*s].TileType == TileID.SandstoneBrick ){
                                actuallyAPyramid = true;
                            }
                        }
                    }

                    if(!actuallyAPyramid) continue;
                    if(distNew<=returnNumWithDiff) counter++;

                    if( distNew<minDist ){
                        minDist = Math.Min(minDist,distNew);
                        minDiff = (Pyx[i]-posX) * (posX > Main.maxTilesX/2 ? -1: 1); 
                    }
                }                
            }  
            return returnNumWithDiff<0? (returnDiff2middistanceInstead?minDiff:minDist) : counter;
        }
     
        

        public static int HorizontalDistToBeach(int posX){            
            int minDist = Math.Min(Math.Abs(GenVars.leftBeachEnd-posX), Math.Abs(GenVars.rightBeachStart-posX));                     
            return minDist;
        }

        public static int HorizontalDistToEndOfWorld(int posX){            
            int minDist = Math.Min(Math.Abs(posX), Math.Abs(Main.maxTilesX-posX));                     
            return minDist;
        }




        public static void ManualNotSortMethod(object list)
        {
        }


        public static bool TryToParseAnyDoubleNumber(string textToparse){
            double dummy = 0;
            return TryToParseAnyDoubleNumber(textToparse, out dummy);
        }
        public static bool TryToParseAnyDoubleNumber(string textToparse, out double value)
        {
            if(textToparse == null){
                value = double.NaN;
                return false;
            }

            double val1 = 0;
            double val2 = 0;

            bool gg1 = double.TryParse(textToparse.Replace('.', ','), NumberStyles.Any, CultureInfo.InvariantCulture, out val1);
            bool gg2 = double.TryParse(textToparse.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out val2);
            if (gg2)
            {
                if (gg1)
                {
                    value = Math.Abs(val1) < Math.Abs(val2) ? val1 : val2;
                    return true;
                }
                value = val1;
                return true;
            }
            value = double.NaN;
            if( textToparse.Equals( double.PositiveInfinity.ToString() ) ) value = double.PositiveInfinity;
            else if( textToparse.Equals( double.NegativeInfinity.ToString() ) ) value = double.NegativeInfinity;
            else return false;

            return true;
        }
        public static double ParseAnyDoubleNumber(string textToparse)
        {
            if(textToparse==null) return 0;
            double val = 0;
            TryToParseAnyDoubleNumber(textToparse,  out val);
            return val;
        }

        public static string MinMaxIntString(string ins, int min, int max, string backup){
            int intout = 0;
            bool isInt = Int32.TryParse(ins, out intout);
            if(!isInt){
                double dout = 0;
                bool isD = Double.TryParse(ins, out dout);
                if(!isInt) return backup;
                intout = ((int)dout);
            }
            intout = Math.Min(Math.Max(intout,min),max);
            return intout.ToString();

        }

        public static int AllignToNearbySurfaceLevel(int px, int py, int lrcheckrang = 25){
            while(py-->0){
                bool free = false;
                for(int s=-1;s<2;s+=2){
                    int c = 0;
                    for(int lr=1;lr<25;lr++){
                        Tile t = Main.tile[px+lr*s,py];
                        if(t.WallType != WallID.None ||  (t.HasTile && Main.tileSolid[t.TileType] && t.TileType != TileID.WoodBlock ) ) c++;
                    }
                    if(c<6){ free = true; break;}
                }
                if(free){ py++; break;}
            }
            while(py++<Main.maxTilesY){
                bool nfree = false;
                for(int s=-1;s<2;s+=2){
                    int c = 0;
                    for(int lr=1;lr<25;lr++){
                        Tile t = Main.tile[px+lr*s,py];
                        if(t.WallType != WallID.None ||  (t.HasTile && Main.tileSolid[t.TileType] && t.TileType != TileID.WoodBlock) ) c++;
                    }
                    if(c>6){ nfree = true; break;}
                }
                if(nfree){ py--; break;}
            }

            while ( WorldGen.SolidTile(px, py) || WorldGen.SolidTile(px+1, py) || WorldGen.SolidTile(px-1, py) ) py--;
            return py;

        }


    }
}