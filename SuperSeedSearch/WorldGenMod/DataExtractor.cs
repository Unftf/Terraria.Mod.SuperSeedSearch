using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.WorldBuilding;
using System.Reflection;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;
using SuperSeedSearch.UI;
using Terraria.ID;
using SuperSeedSearch.Helpers;


namespace SuperSeedSearch.WorldGenMod
{
    public class DataExtractor
    {
        //todo1:better import gatherDataAt
        public class VanillaVar{
            public string name = "";
            public Type type = null;
            public bool isWG = false;

            public VanillaVar(string name, Type type, bool isWGClass = false){
                this.isWG = isWGClass;
                this.type = type;
                this.name = name;
            }
            //no need anymore? all variables are public now, todo
            //public static VanillaVar OasisWidth = new VanillaVar("oasisWidth", typeof(int[]), true);
            //public static VanillaVar NumOasis = new VanillaVar("numOasis", typeof(int), true);
            //public static VanillaVar MountainCavesX = new VanillaVar("mCaveX", typeof(int[]), true);//0.25..0.75

            //public static VanillaVar DungeonLocationXEarly = new VanillaVar("dungeonLocation", typeof(int));//only works good for left side dungeon, right side entraced is shifted to inner world, <0.2, >0.8


        }
        public static class DetermineDataNames{
            public readonly static string PredictedSpawnHeight = "Predicted spawn height (in early WG)";
            public readonly static string PredictedSpawnX = "Predicted spawn X (in early WG)";

            public readonly static string JungleMainEntranceX = "Jungle main entrance X";
            public readonly static string JungleMainEntranceYapprximated = "Jungle main entrance approximated Y";
            public readonly static string DesertTopOfSurfaceCenterX = "Desert center X";
            public readonly  static string DesertTopOfSurfaceCenterYapprox = "Desert center approximated surface Y";
            public readonly  static string SnowTopOfSurfaceCenterXY = "Snow center approximate surface X/Y";

            public readonly  static string JungleBeachTopOfSurfaceY = "Jungle beach compute surface Y";
            


            public readonly  static string ComputePathLength = "Compute pathlength from Spawn";            
            public readonly static string ComputePathLengthJungle = "Compute pathlength from Jungle main entrance";     

            public readonly static string ComputeQuickPathLengthSpawn = "Compute near path from (predicted) Spawn";            
            public readonly static string ComputeQuickPathLengthJungleMain = "Compute near path from Jungle main entrance";            
            public readonly static string ComputeQuickPathLengthDesertCSurf = "Compute near path from Desert center surface";            
            public readonly static string ComputeQuickPathLengthSnowCSurf = "Compute near path from Snow center surface";            

            public readonly static string ComputeQuickPathLengthJungleBeach = "Compute near path from Jungle beach";    

            
        }
     
        private Dictionary<WorldGenPass,List<VanillaVar>> gatherDataAt = new Dictionary<WorldGenPass,List<VanillaVar>>{
            //{WorldGenPass.Reset, new List<VanillaVar>{ VanillaVar.DungeonLocationXEarly}},                                                   
        };

        public class Determiner{
            public string name = "";
            private Action<string> evalFun = null;
            public Determiner(string name, Action<string> determineFun){
                this.name=name;
                this.evalFun = determineFun;
            }
            public void eval(){
                if(ModMenuMod.uiSuperSeed!=null && ModMenuMod.uiSuperSeed.generationProgress!=null) ModMenuMod.uiSuperSeed.generationProgress.Message = name;
                evalFun(name);
            }
        }

        //todo array +enum
        public static Pathlength.Pathlength pathlength = null;
        public static Pathlength.Pathlength pathlengthFromJungleMainEntrance = null;

        public static Pathlength.Pathlength quickPathlength = null;
        public static Pathlength.Pathlength quickPathlengthJungleMainEntrance = null;

        public static Pathlength.Pathlength quickPathlengthDesertSurface = null;        
        public static Pathlength.Pathlength quickPathlengthSnowCSurface = null;      
        public static Pathlength.Pathlength quickPathlengthJungleBeach = null;      


        public static void Reset(){
            pathlength = null;
            pathlengthFromJungleMainEntrance = null;
            quickPathlength = null;
            quickPathlengthJungleMainEntrance = null;
            quickPathlengthDesertSurface = null;
            quickPathlengthSnowCSurface = null;             
            quickPathlengthJungleBeach = null;             
        }

        private Dictionary<WorldGenPass,List<Determiner>> determineDataAt = new Dictionary<WorldGenPass,List<Determiner>>{            
            {WorldGenPass.Terrain, new List<Determiner>{new Determiner(DetermineDataNames.PredictedSpawnHeight, computePredSpawnHeightAndBeachHeight)}},
            {WorldGenPass.SmallHoles, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthSpawn, SetupQuickPathlengthSpawn)}},
            {WorldGenPass.GenerateIceBiome, new List<Determiner>{new Determiner(DetermineDataNames.SnowTopOfSurfaceCenterXY, ApproxSnowSurfCenterXY)}},
            {WorldGenPass.Jungle, new List<Determiner>{new Determiner(DetermineDataNames.JungleMainEntranceYapprximated, ApproximatedJungleMainEntranceHeight), new Determiner(DetermineDataNames.ComputeQuickPathLengthJungleMain, SetupQuickPathlengthJungleMainEntrance)   }},
            {WorldGenPass.FullDesert, new List<Determiner>{new Determiner(DetermineDataNames.DesertTopOfSurfaceCenterYapprox, ApproxDesertCenterSurfaceY), new Determiner(DetermineDataNames.ComputeQuickPathLengthDesertCSurf, SetupQuickPathlengthDesertSurface)  }},
            {WorldGenPass.MountainCaves, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthSpawn, SetupQuickPathlengthSpawn), new Determiner(DetermineDataNames.ComputeQuickPathLengthSnowCSurf, SetupQuickPathlengthSnowCSurface) }},
            {WorldGenPass.Shimmer, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthJungleBeach, SetupQuickPathlengthJungleBeach) }},
            {WorldGenPass.LivingTrees, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthSpawn, SetupQuickPathlengthSpawn), new Determiner(DetermineDataNames.ComputeQuickPathLengthDesertCSurf, SetupQuickPathlengthDesertSurface)  }},                        
            {WorldGenPass.Hives, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthJungleMain, SetupQuickPathlengthJungleMainEntrance), new Determiner(DetermineDataNames.ComputeQuickPathLengthJungleBeach, SetupQuickPathlengthJungleBeach) }},
            {WorldGenPass.Oasis, new List<Determiner>{new Determiner(DetermineDataNames.ComputeQuickPathLengthDesertCSurf, SetupQuickPathlengthDesertSurface) }},
            {WorldGenPass.FinalCleanup, new List<Determiner>{new Determiner(DetermineDataNames.ComputePathLength, SetupPathlength), new Determiner(DetermineDataNames.ComputePathLengthJungle, SetupPathlengthJungle)  }},                        
        };

        public static void computePredSpawnHeightAndBeachHeight(string name){
            int sx = Main.maxTilesX/2;
            int sy = (int)Main.worldSurface;

            WorldInfo.PredictedSpawnX = sx;

            if(Main.tenthAnniversaryWorld){
                sx = 275+15;
                sx = (( (int)(WorldInfo.RNGNumbers.RNGN1*2) != 0) ? (Main.maxTilesX - sx) : sx);
            }    
            if (WorldGen.remixWorldGen)
			{
				sy = Main.maxTilesY - 10;
				while ( WorldGen.SolidTile(sx, sy) || WorldGen.SolidTile(sx+1, sy) || WorldGen.SolidTile(sx-1, sy) ) sy--;
				WorldInfo.PredictedSpawnHeight = sy + 1;
			}
            else{
                //WorldInfo.SetValue(DetermineDataNames.PredictedSpawnX, sx);     //todo also clear them
                        
                //while(  (!Main.tile[sx,sy].HasTile || !Main.tileSolid[Main.tile[sx,sy].TileType] ) && sy<Main.maxTilesY-1){sy++;};
                //WorldInfo.SetValue(name, sy);     
                //WorldInfo.PredictedSpawnHeight = sy;       

                WorldInfo.PredictedSpawnHeight = BasicFunctions.AllignToNearbySurfaceLevel(sx,sy);                    
            }    
            
            int jbx = GenVars.dungeonSide<0? GenVars.rightBeachStart+50: GenVars.leftBeachEnd-50;
            int jby = (int)Main.worldSurface;     
            while ( WorldGen.SolidTile(jbx, jby))jby--;            
            WorldInfo.JungleBeachHeight = jby-1;
            WorldInfo.JungleBeachX = jbx;

            int dbx = GenVars.dungeonSide<0? GenVars.leftBeachEnd-50: GenVars.rightBeachStart+50;
            int dby = (int)Main.worldSurface;     
            while ( WorldGen.SolidTile(dbx, dby))dby--;            
            WorldInfo.DungeonBeachHeight = dby-1;
            WorldInfo.DungeonBeachX = dbx;

        }
        public static void ApproxSnowSurfCenterXY(string name){
            int scx = (GenVars.snowMinX[(int)Main.worldSurface]+GenVars.snowMaxX[(int)Main.worldSurface])/2;
            int scy = (int)Main.worldSurface;   
            int scxs = scx+1;
            bool hasTile = true; 
            while(scx != scxs){
                scxs = scx;
                hasTile = true; 
                while(hasTile && scy-->100){
                    hasTile = false;                
                    for(int i=scx-25;i<=scx+25;i++){
                        if ((Main.tile[i,scy].HasTile && (Main.tileSolid[Main.tile[i,scy].TileType] || TileID.Sets.SnowBiome[Main.tile[i,scy].TileType]>0 )) || Main.tile[i,scy].WallType != WallID.None ){
                            hasTile = true;                        
                            break;
                        }
                    }                
                } 
                scx = (GenVars.snowMinX[scy]+GenVars.snowMaxX[scy])/2;
            }
            hasTile = false;
            while(!hasTile && scy++<Main.rockLayer){                
                for(int i=scx-3;i<=scx+3;i++){
                    if ((Main.tile[i,scy].HasTile && (Main.tileSolid[Main.tile[i,scy].TileType] || TileID.Sets.SnowBiome[Main.tile[i,scy].TileType]>0 )) || Main.tile[i,scy].WallType != WallID.None || Main.tile[i,scy].LiquidAmount >0){
                        hasTile = true;                        
                        break;
                    }
                }
            }  
            scy--;
            //WorldInfo.SetValue(DetermineDataNames.SnowTopOfSurfaceCenterX, scx);     
            WorldInfo.SnowTopOfSurfaceCenterX = scx;
            //WorldInfo.SetValue(DetermineDataNames.SnowTopOfSurfaceCenterY, scy);     
            WorldInfo.SnowTopOfSurfaceCenterY = scy;

        }

        public static void ApproxDesertCenterSurfaceY(string name){
            int dcx = (GenVars.desertHiveLeft+GenVars.desertHiveRight)/2;
            
            int dcy = GenVars.desertHiveHigh+1;            
            bool hasTile = true;
            
            while(hasTile && dcy-->100){
                hasTile = false;                
                for(int i=dcx-25;i<=dcx+25;i++){
                    if ((Main.tile[i,dcy].HasTile && (Main.tileSolid[Main.tile[i,dcy].TileType] || TileID.Sets.isDesertBiomeSand[Main.tile[i,dcy].TileType] )) || Main.tile[i,dcy].WallType != WallID.None || Main.tile[i,dcy].LiquidAmount >0){
                        hasTile = true;                        
                        break;
                    }
                }                
            }             
            hasTile = false;
            while(!hasTile && dcy++<Main.rockLayer){                
                for(int i=dcx-25;i<=dcx+25;i++){
                    if ((Main.tile[i,dcy].HasTile && (Main.tileSolid[Main.tile[i,dcy].TileType] || TileID.Sets.isDesertBiomeSand[Main.tile[i,dcy].TileType] )) || Main.tile[i,dcy].WallType != WallID.None || Main.tile[i,dcy].LiquidAmount >0 ){
                        hasTile = true;                        
                        break;
                    }
                }
            }            
            dcy--;
            //WorldInfo.SetValue(DetermineDataNames.DesertTopOfSurfaceCenterX, dcx);     
            WorldInfo.DesertTopOfSurfaceCenterX = dcx;
            //WorldInfo.SetValue(DetermineDataNames.DesertTopOfSurfaceCenterYapprox, dcy);     
            WorldInfo.DesertTopOfSurfaceCenterYapprox = dcy;

        }
        public static void ApproximatedJungleMainEntranceHeight(string name){
            int x = GenVars.JungleX;  
            int y = 100;
            //WorldInfo.SetValue(DetermineDataNames.JungleMainEntranceX, x);     
            WorldInfo.JungleMainEntranceX = x;

            while(  (!Main.tile[x,y].HasTile || !Main.tileSolid[Main.tile[x,y].TileType] || (Main.tile[x,y].TileType != TileID.JungleGrass && Main.tile[x,y].TileType != TileID.Mud))
                && (!Main.tile[x-1,y].HasTile || !Main.tileSolid[Main.tile[x-1,y].TileType] || (Main.tile[x-1,y].TileType != TileID.JungleGrass && Main.tile[x-1,y].TileType != TileID.Mud))
                && (!Main.tile[x+1,y].HasTile || !Main.tileSolid[Main.tile[x+1,y].TileType] || (Main.tile[x+1,y].TileType != TileID.JungleGrass && Main.tile[x+1,y].TileType != TileID.Mud))
                && y<Main.maxTilesY-1) {y++;};

            bool foundAny = true;
            while(  y-->100 && foundAny ){
                foundAny = false;
                for(int xi=x-15;xi<x+16;xi++)if((Main.tile[x,y].HasTile && Main.tileSolid[Main.tile[x,y].TileType]) || (Main.tile[x,y-2].HasTile && Main.tileSolid[Main.tile[x,y-2].TileType]) || Main.tile[x,y].WallType != WallID.None ) {foundAny = true;break; }
            }                

            //WorldInfo.SetValue(name, y); 
            WorldInfo.JungleMainEntranceYapprox = y;    
                     
        }

        //todo unify

        public static void SetupQuickPathlengthSpawn(string name){
            quickPathlength = new Pathlength.Pathlength(WorldInfo.PredictedSpawnX, WorldInfo.PredictedSpawnHeight-1, ModMenuMod.wGPassChanger.currentSeed, name:name, xrestrictedbyyPlus:0); ;            
        }
        public static void SetupQuickPathlengthJungleMainEntrance(string name){            
            quickPathlengthJungleMainEntrance = new Pathlength.Pathlength(WorldInfo.JungleMainEntranceX, WorldInfo.JungleMainEntranceYapprox, ModMenuMod.wGPassChanger.currentSeed, name:name, xrestrictedbyyPlus:0);
        }
        public static void SetupQuickPathlengthDesertSurface(string name){
            quickPathlengthDesertSurface = new Pathlength.Pathlength(WorldInfo.DesertTopOfSurfaceCenterX, WorldInfo.DesertTopOfSurfaceCenterYapprox, ModMenuMod.wGPassChanger.currentSeed, name:name, xrestrictedbyyPlus:0); ;            
        }        
        public static void SetupQuickPathlengthSnowCSurface(string name){
            quickPathlengthSnowCSurface = new Pathlength.Pathlength(WorldInfo.SnowTopOfSurfaceCenterX, WorldInfo.SnowTopOfSurfaceCenterY, ModMenuMod.wGPassChanger.currentSeed, false, name:name, xrestrictedbyyPlus:100); ;            
        }   
        public static void SetupQuickPathlengthJungleBeach(string name){
            quickPathlengthJungleBeach = new Pathlength.Pathlength(WorldInfo.JungleBeachX, WorldInfo.JungleBeachHeight, ModMenuMod.wGPassChanger.currentSeed, false, name:name, xrestrictedbyyPlus:200); ;            
            //quickPathlengthJungleBeach = new Pathlength.Pathlength((int)GenVars.shimmerPosition.X, (int) GenVars.shimmerPosition.Y-1, ModMenuMod.wGPassChanger.currentSeed, false, name:name, xrestrictedbyyPlus:200); ;            
        } 


        public static void SetupPathlength(string name){
            pathlength = new Pathlength.Pathlength(Main.spawnTileX, Main.spawnTileY-1, ModMenuMod.wGPassChanger.currentSeed, false, name:name);            
        }
        public static void SetupPathlengthJungle(string name){        
            pathlengthFromJungleMainEntrance = new Pathlength.Pathlength(WorldInfo.JungleMainEntranceX, WorldInfo.JungleMainEntranceYapprox, ModMenuMod.wGPassChanger.currentSeed,false,2, name:name);            
        }



        public void ExtactDataFromCurWGPass(GenPass targetPass,string lastPassName, ConstantEnum.WorldGenPass passNameID){        
                
            if( gatherDataAt.ContainsKey(passNameID)    ) 
            foreach(VanillaVar vvar in gatherDataAt[passNameID] ){                
                if(vvar.isWG)
                    ExtractVarFromWorldGen(vvar.name);
                else
                    ExtractVarFromPass(targetPass, vvar.name);                
            }
            if( determineDataAt.ContainsKey(passNameID)    ) 
            foreach(var dvar in determineDataAt[passNameID] ){
                dvar.eval();                
            }

        } 


        private void ExtractVarFromPass(GenPass targetPass, string varname)
        {   //does not work for jungle/temple/terrain
            FieldInfo generationMethod = typeof(PassLegacy).GetField("_method", BindingFlags.Instance | BindingFlags.NonPublic);
            WorldGenLegacyMethod method = (WorldGenLegacyMethod)generationMethod.GetValue(targetPass);                       
            var passField = method.Method.DeclaringType?.GetFields
            (
                BindingFlags.NonPublic |
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.Static
            )
            .Single(x => x.Name == varname);
            WorldInfo.SetValue(varname, passField.GetValue(method.Target));             
        }
        private void ExtractVarFromWorldGen(string varname)
        {   //does not work for jungle/temple/terrain
            FieldInfo generationMethod = typeof(WorldGen).GetField(varname, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);            
      
            WorldInfo.SetValue(varname, generationMethod.GetValue(null)  );            
        }


    }
}