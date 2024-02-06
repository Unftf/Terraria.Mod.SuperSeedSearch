using Terraria;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.WorldBuilding;
using SuperSeedSearch.Storage;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.WorldGenMod;



namespace SuperSeedSearch.PropertyCondition.PCBiome
{
    public static class PCBiomeConditionList
    {

        public static List<Condition> pcBiomeConditionList = null;



        static PCBiomeConditionList()
        {
            AddToList(ConstantEnum.PCBiomeTypes.NumPyramid, new List<ushort> { TileID.SandstoneBrick }, ConstantEnum.WorldGenPass.Pyramids, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CountBool(c) );
            AddToList(ConstantEnum.PCBiomeTypes.MaxPosPyramid, null, ConstantEnum.WorldGenPass.Dunes, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CheckPossiblePyramid(c, false) );            
            AddToList(ConstantEnum.PCBiomeTypes.PyramidEarlyCountSandDungeon, new List<ushort> { TileID.Sand,TileID.SandstoneBrick  }, ConstantEnum.WorldGenPass.Dunes, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CheckPossiblePyramid(c, true) );            
            AddToList(ConstantEnum.PCBiomeTypes.NumFloatingIslands, new List<ushort> { TileID.Cloud }, ConstantEnum.WorldGenPass.FloatingIslands, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CountBool(c) );
            AddToList(ConstantEnum.PCBiomeTypes.NumEvilBiome, new List<ushort> { TileID.Ebonstone, TileID.Ebonsand, TileID.CorruptGrass, TileID.CorruptHardenedSand, TileID.CorruptIce ,TileID.Crimstone,TileID.Crimsand,TileID.CrimsonGrass, TileID.CrimsonHardenedSand, TileID.FleshIce }, ConstantEnum.WorldGenPass.Corruption, ()=> Main.maxTilesX, (c,p) => ( p.posY>GenVars.worldSurfaceLow?IncreaserBool(c,p):false ) , (c) => CountBool(c) );
            AddToList(ConstantEnum.PCBiomeTypes.NumLivingTree, new List<ushort> { TileID.LivingWood }, ConstantEnum.WorldGenPass.LivingTrees, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CountBool(c,6),  (p) => IsTree(p) );
            AddToList(ConstantEnum.PCBiomeTypes.NumESS, new List<ushort> { TileID.LargePiles2 }, ConstantEnum.WorldGenPass.MicroBiomes, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CountBool(c) , (p) => IsShrine(p) );
            AddToList(ConstantEnum.PCBiomeTypes.NumOasis, null, ConstantEnum.WorldGenPass.Oasis, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CheckOasis(c)  );
            AddToList(ConstantEnum.PCBiomeTypes.NumMountainCanves, null, ConstantEnum.WorldGenPass.MountCaves, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CheckMountainCave(c)  );
            AddToList(ConstantEnum.PCBiomeTypes.NumSurfaceTunnels, null, ConstantEnum.WorldGenPass.Tunnels, ()=> Main.maxTilesX, (c,p) => IncreaserBool(c,p), (c) => CheckSurfaceTunnels(c)  );
             
        }

        public static bool IncreaserBool(Condition c, PropertyElement p){            
            c.memory.Set(p.posX, true);            
            return true;
        }

        public static bool CountBool(Condition c, int holeAllowed =10 ){
            int target = (int)c.targetEvalValue.valueDouble;
            int appears = 0;
            int lf = -1337;
            if(c.memory.data!=null)
            for(int i=0; i < c.memory.data.Length; i++){
                if(c.memory.Get(i)){  if (i>lf+holeAllowed ){ appears++;    } lf=i;}
            } 
            return c.gole.CompareAgoleB(appears, target);
        }
        public static bool IsShrine(PropertyElement p){
            //if(p.frameX/54==17 && p.frameY/36==0 && p.IDWall==WallID.Flower) {
            if(p.frameX/54==17 && p.frameY/36==0) {
                for(int i=3;i<12;i+=2)
                    if( !Main.tile[p.posX,p.posY+i].HasTile || Main.tile[p.posX,p.posY+i].TileType != TileID.Dirt) return false;

                if((Main.tile[p.posX,p.posY+1].HasTile && Main.tile[p.posX,p.posY+1].TileType == TileID.Grass) || (Main.tile[p.posX,p.posY+2].HasTile && Main.tile[p.posX,p.posY+2].TileType == TileID.Grass)){                    
                    return true;
                }
            }
            return false;
        }
        public static bool IsTree(PropertyElement p){
            //if(p.frameX/54==17 && p.frameY/36==0 && p.IDWall==WallID.Flower) {
            if(p.posY < Main.worldSurface &&
               Main.tile[p.posX,p.posY+1].HasTile && Main.tile[p.posX,p.posY+1].TileType == TileID.LivingWood && 
               Main.tile[p.posX,p.posY-1].HasTile && Main.tile[p.posX,p.posY-1].TileType == TileID.LivingWood ) {
                for(int i=-8;i<8;i+=2)
                    if( !Main.tile[p.posX,p.posY+i].HasTile || Main.tile[p.posX,p.posY+i].TileType != TileID.LivingWood) return false;

                if((Main.tile[p.posX-5,p.posY-8].HasTile && Main.tile[p.posX-5,p.posY-8].TileType == TileID.LeafBlock) || (Main.tile[p.posX+5,p.posY-8].HasTile && Main.tile[p.posX+5,p.posY-8].TileType == TileID.LeafBlock)){
                    
                    return true;                
                }
            }
            return false;
        }
        public static bool CheckOasis(Condition c){
            if(c.memory.data==null) return false;

            int NumOasis = GenVars.numOasis;
            if(NumOasis<1) return c.gole.CompareAgoleB(0, (int)c.targetEvalValue.valueDouble);

            Point[] OasisPosition = GenVars.oasisPosition;
            int[] OasisWidth = GenVars.oasisWidth;
            int ocount = 0;
            for(int i=0; i<NumOasis;i++){
                int posX = (int)OasisPosition[i].X;                
                for(int j = Math.Max(posX-OasisWidth[i],0);j<Math.Min(posX+OasisWidth[i],Main.maxTilesX);j++){
                    if (c.memory.Get(j)){
                        ocount++;                        
                        break;
                    }
                }
            }            
            return c.gole.CompareAgoleB(ocount, (int)c.targetEvalValue.valueDouble);
        }
        public static bool CheckMountainCave(Condition c){
            if(c.memory.data==null) return false;

            int NumMountainCanves =  GenVars.numMCaves;
            if(NumMountainCanves<1) return c.gole.CompareAgoleB(0, (int)c.targetEvalValue.valueDouble);

            int[] CavesX = GenVars.mCaveX;            
            int ccount = 0;
            for(int i=0; i<NumMountainCanves;i++){
                int posX = (int)CavesX[i];                                
                if (c.memory.Get(posX)){
                    ccount++;  
                }                
            }            
            return c.gole.CompareAgoleB(ccount, (int)c.targetEvalValue.valueDouble);
        }

        public static bool CheckPossiblePyramid(Condition c, bool doCheckYSandAndDungeon){
            if(c.memory.data==null) return false;

            int NumMaxPossPyramids =  GenVars.numPyr;
            if(NumMaxPossPyramids<1) return c.gole.CompareAgoleB(0, (int)c.targetEvalValue.valueDouble);

            int[] possPyrSpotsX = GenVars.PyrX;    
            List<int> posSpots = new List<int>();        
            List<int> posSpotsInd = new List<int>();
            int ccount = 0;
            for(int i=0; i<NumMaxPossPyramids;i++){
                int posX = (int)possPyrSpotsX[i]; 
                if(doCheckYSandAndDungeon)   
                if((GenVars.dungeonSide<0 && posX<GenVars.dungeonLocation+Main.maxTilesX*0.15) ||
                    (GenVars.dungeonSide>0 && posX>GenVars.dungeonLocation-Main.maxTilesX*0.15)){
                        continue;//dungeonLocation!=dungeonX
                }                            
                if (c.memory.Get(posX)){
                    ccount++;  
                    posSpots.Add(posX);
                    posSpotsInd.Add(i);
                }                
            }
            int minDist = WorldGen.drunkWorldGen?110:220;
            for(int j=0; j<posSpots.Count; j++){
            for(int i=0; i<posSpotsInd[j];i++){       
                if(posSpotsInd[j] == i) continue;
                if(Math.Abs(posSpots[j] -possPyrSpotsX[i])<minDist )
                {
                    ccount--;break;
                }
                if(doCheckYSandAndDungeon){
                    int y;
                    for(y=GenVars.PyrY[i]; y<Main.worldSurface;y++)if(Main.tile[ GenVars.PyrX[i] , GenVars.PyrY[i]+y ].HasTile)break;                    
                    if(!(Main.tile[ GenVars.PyrX[i] , y ].TileType == TileID.Sand || Main.tile[ GenVars.PyrX[i] , y ].TileType == TileID.SandstoneBrick) || y==Main.worldSurface)
                    {ccount--;break;}
                }
                
            }                
            }            
            return c.gole.CompareAgoleB(ccount, (int)c.targetEvalValue.valueDouble);
        }

        public static bool CheckSurfaceTunnels(Condition c){
            if(c.memory.data==null) return false;

            int NumMaxTunnels =  GenVars.numTunnels;
            if(NumMaxTunnels<1) return c.gole.CompareAgoleB(0, (int)c.targetEvalValue.valueDouble);

            int[] possTunnelsXCenter = GenVars.tunnelX;            
            int ccount = 0;
            for(int i=0; i<NumMaxTunnels;i++){
                int posX = (int)possTunnelsXCenter[i];                                
                if (c.memory.Get(posX)){
                    ccount++;  
                }                
            }            
            return c.gole.CompareAgoleB(ccount, (int)c.targetEvalValue.valueDouble);
        }


        static void AddToList(string name, List<ushort> tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<int> GetConditionMemorySize, 
                         Func<Condition, PropertyElement, bool> postConstraintCheckFun, Func<Condition, bool> CondEvalFun, Func<PropertyElement,bool> matchFun = null
                         )
        {
            if (pcBiomeConditionList == null) pcBiomeConditionList = new List<Condition>();
            List<PropertyElement> propel = new List<PropertyElement>();

            if(tileId!=null){
            foreach (ushort id in tileId)
                {
                    propel.Add(new PropertyElement { CondType = ConstantEnum.PropertyType.Tile, IDTile = (short)id, isActive=true, matchFun=matchFun });
                }
            }else{
                propel.Add(new PropertyElement{CondType = ConstantEnum.PropertyType.Tile, posY=200} );
            }

            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcBasicDistanceConstraintList)
            {
                if(!condel.name.Contains(ConstantEnum.ConstraintNames.DistanceHorizontal) && !condel.name.Contains(ConstantEnum.ConstraintNames.CloserToEndOfWorld) 
                    && !condel.name.Contains(ConstantEnum.ConstraintNames.WestLeftOf, StringComparison.InvariantCultureIgnoreCase) 
                    && !condel.name.Contains(ConstantEnum.ConstraintNames.EastRigthOf, StringComparison.InvariantCultureIgnoreCase)
                    && !condel.name.Contains(ConstantEnum.ConstraintNames.XCoordinate, StringComparison.InvariantCultureIgnoreCase)
                    && !condel.name.Contains(ConstantEnum.ConstraintNames.DifferenceHorizontalToMid)
                     ) continue;

                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;

              
                condelli.Add(condelNew);
            }       
         

            if(postConstraintCheckFun!=null)
                pcBiomeConditionList.Add(new Condition(ConditionType.Biome, name, propel, condelli,new Condition.TargetEvalValue(0, CondEvalFun), GetConditionMemorySize ){postConstraintCheckFun=postConstraintCheckFun} );
            else
                pcBiomeConditionList.Add(new Condition(ConditionType.Biome, name, propel, condelli,new Condition.TargetEvalValue(0, CondEvalFun), GetConditionMemorySize ) );
        }

        
    }
}
