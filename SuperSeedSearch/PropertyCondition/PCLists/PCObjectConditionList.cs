using Terraria;
using System;
using System.Collections.Generic;
using Terraria.ID;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.RandomNumberAnalyzer;
using Terraria.ObjectData;
using SuperSeedSearch.PropertyCondition.PCChests;


namespace SuperSeedSearch.PropertyCondition.PCObject
{
    public static class PCObjectConditionList
    {
        //todo: won't detect all broken objects
        public static List<Condition> pcObjectConditionList = null;

        public class TileFrame{
            public readonly ushort TileID;
            public readonly short FrameX, FrameY;
            public TileFrame(ushort tileID, short frameX= ConstantsStrings.IsUnsetPropertyValue, short frameY = ConstantsStrings.IsUnsetPropertyValue){
                    this.TileID = tileID;
                    this.FrameX = frameX;
                    this.FrameY = frameY;
            }
        }
        
        static PCObjectConditionList()
        {   
            AddToList(ConstantEnum.PCObjectCondTypes.LifeCrystalHeart, new TileFrame(TileID.Heart,0,0), ConstantEnum.WorldGenPass.LifeCrystals );
            AddToList(ConstantEnum.PCObjectCondTypes.Statue, new List<TileFrame>{ new TileFrame(TileID.Statues), 
                                                                                  new TileFrame(TileID.MushroomStatue) }, 
                                                                                  ConstantEnum.WorldGenPass.Temple, IsRealStatue, 6,
                                                                                  new List<Constraint>{ BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.StatueType, HasStatueType , ConstantEnum.WorldGenPass.Temple)  } );//some generated earlier
            AddToList(ConstantEnum.PCObjectCondTypes.StatueTypes, new List<TileFrame>{ new TileFrame(TileID.Statues), 
                                                                                  new TileFrame(TileID.MushroomStatue) }, 
                                                                                  ConstantEnum.WorldGenPass.Temple, IsRealStatue, postConstraintCheckFun: StatueTypeCouner);
            AddToList(ConstantEnum.PCObjectCondTypes.Anvil, new TileFrame(TileID.Anvils), ConstantEnum.WorldGenPass.BuriedChests, null, 2 );
            AddToList(ConstantEnum.PCObjectCondTypes.ESnotShrine, new TileFrame(TileID.LargePiles2,54*17+18,18), ConstantEnum.WorldGenPass.Piles );
            AddToList(ConstantEnum.PCObjectCondTypes.GemTreeAmber, new TileFrame(TileID.TreeAmber), ConstantEnum.WorldGenPass.Stalac, IsGemTreeBase );
            AddToList(ConstantEnum.PCObjectCondTypes.GemTreeRuby, new TileFrame(TileID.TreeRuby), ConstantEnum.WorldGenPass.Stalac, IsGemTreeBase );
            AddToList(ConstantEnum.PCObjectCondTypes.GemTreeDiamond, new TileFrame(TileID.TreeDiamond), ConstantEnum.WorldGenPass.Stalac, IsGemTreeBase );
            AddToList(ConstantEnum.PCObjectCondTypes.GemTreeTopaz, new TileFrame(TileID.TreeTopaz), ConstantEnum.WorldGenPass.Stalac, IsGemTreeBase );
            AddToList(ConstantEnum.PCObjectCondTypes.GemTree, new List<TileFrame>{new TileFrame(TileID.TreeAmber),new TileFrame(TileID.TreeAmethyst),new TileFrame(TileID.TreeEmerald),new TileFrame(TileID.TreeTopaz),new TileFrame(TileID.TreeSapphire), new TileFrame(TileID.TreeRuby),new TileFrame(TileID.TreeDiamond) }, ConstantEnum.WorldGenPass.Stalac, IsGemTreeBase );
            AddToList(ConstantEnum.PCObjectCondTypes.Sunflower, new TileFrame(TileID.Sunflower), ConstantEnum.WorldGenPass.Sunflowers,null,8 );
            AddToList(ConstantEnum.PCObjectCondTypes.ShadowOrbCrimsonHeart, new TileFrame(TileID.ShadowOrbs), ConstantEnum.WorldGenPass.Corruption,null,4 );

            AddToList(ConstantEnum.PCObjectCondTypes.LihzahrdAltar, new TileFrame(TileID.LihzahrdAltar), ConstantEnum.WorldGenPass.LihzahrdAltars,null,6 );
            AddToList(ConstantEnum.PCObjectCondTypes.LihzahrdDoor, new List<TileFrame>{ new TileFrame(TileID.ClosedDoor,0,594), new TileFrame(TileID.ClosedDoor,18,594),new TileFrame(TileID.ClosedDoor,36,594),
                                                                                        new TileFrame(TileID.ClosedDoor,0,594+18), new TileFrame(TileID.ClosedDoor,18,594+18),new TileFrame(TileID.ClosedDoor,36,594+18),
                                                                                        new TileFrame(TileID.ClosedDoor,0,594+36), new TileFrame(TileID.ClosedDoor,18,594+36),new TileFrame(TileID.ClosedDoor,36,594+36), } , ConstantEnum.WorldGenPass.JungleTemple,null,3 );
            
            AddToList(ConstantEnum.PCObjectCondTypes.DemonAltar, new TileFrame(TileID.DemonAltar), ConstantEnum.WorldGenPass.Altars,null,6 );//might be later at TileCleanup
            AddToList(ConstantEnum.PCObjectCondTypes.GlowTulipPet, new TileFrame(TileID.GlowTulip), ConstantEnum.WorldGenPass.DyePlants,null,4 );
            AddToList(ConstantEnum.PCObjectCondTypes.Bed, new TileFrame(TileID.Beds), ConstantEnum.WorldGenPass.Underworld,null,8 );
            AddToList(ConstantEnum.PCObjectCondTypes.CatBast, new TileFrame(TileID.CatBast), ConstantEnum.WorldGenPass.BuriedChests,null,6 );
            AddToList(ConstantEnum.PCObjectCondTypes.Detonator, new TileFrame(TileID.Detonator), ConstantEnum.WorldGenPass.MicroBiomes,null,4 );
            AddToList(ConstantEnum.PCObjectCondTypes.BrokenObject, null , ConstantEnum.WorldGenPass.FinalCleanup,CheckIfTileIsPartOfBrokenObject,1,new List<Constraint>{ BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.ObjectType, HasobjectType , ConstantEnum.WorldGenPass.FinalCleanup),  BasicConstraintList.ConditionElmWithValues(ConstantEnum.ConstraintNames.IsLocked, PCChestsConditionList.IsLocked , ConstantEnum.WorldGenPass.FinalCleanup)  });

            AddToList(ConstantEnum.PCObjectCondTypes.PlanteraQuickBulb,null, ConstantEnum.WorldGenPass.Terrain, (p) => RandomNumberAnalyzer.RandomNumberAnalyzer.IsPosAqBulb(p.posX, p.posY, (int)(500*RandomNumberAnalyzer.RandomNumberAnalyzer.BulbPredictionTickCount*((double)Main.maxTilesX/4200)) ) ,1, null, null, true);
            AddToList(ConstantEnum.PCObjectCondTypes.PlanteraQuickBulbNatural,null, ConstantEnum.WorldGenPass.MudCavesToGrass, (p) => RandomNumberAnalyzer.RandomNumberAnalyzer.IsPosAqBulb(p.posX, p.posY, (int)(500*RandomNumberAnalyzer.RandomNumberAnalyzer.BulbPredictionTickCount*((double)Main.maxTilesX/4200)), true ) ,1, null, null);
            
            AddToList(ConstantEnum.PCObjectCondTypes.GoldCoinStashSmallPyramid, new List<TileFrame>{ new TileFrame(TileID.SmallPiles,648,18),new TileFrame(TileID.SmallPiles,666,18)} , ConstantEnum.WorldGenPass.Pyramids,null,2 );
            AddToList(ConstantEnum.PCObjectCondTypes.GoldCoinStashSmall, new List<TileFrame>{ new TileFrame(TileID.SmallPiles,648,18),new TileFrame(TileID.SmallPiles,666,18)} , ConstantEnum.WorldGenPass.Piles,null,2 );

            AddToList(ConstantEnum.PCObjectCondTypes.SilverCoinStashSmallPyramid, new List<TileFrame>{ new TileFrame(TileID.SmallPiles,612,18),new TileFrame(TileID.SmallPiles,630,18)} , ConstantEnum.WorldGenPass.Pyramids,null,2 );
            AddToList(ConstantEnum.PCObjectCondTypes.SilverCoinStashSmall, new List<TileFrame>{ new TileFrame(TileID.SmallPiles,612,18),new TileFrame(TileID.SmallPiles,630,18)} , ConstantEnum.WorldGenPass.Piles,null,2 );

            AddToList(ConstantEnum.PCObjectCondTypes.GoldCoinStashLarge, new List<TileFrame>{ new TileFrame(TileID.LargePiles,1098,18),new TileFrame(TileID.LargePiles,1152,18)} , ConstantEnum.WorldGenPass.Piles );
            AddToList(ConstantEnum.PCObjectCondTypes.SilverCoinStashLarge, new List<TileFrame>{ new TileFrame(TileID.LargePiles,1044,18),new TileFrame(TileID.LargePiles,990,18)} , ConstantEnum.WorldGenPass.Piles );


            AddToList(ConstantEnum.PCObjectCondTypes.Painting2x3, new TileFrame(TileID.Painting2X3), ConstantEnum.WorldGenPass.FinalCleanup,null,6 );//todo find wg pass
            AddToList(ConstantEnum.PCObjectCondTypes.Painting3x2, new TileFrame(TileID.Painting3X2), ConstantEnum.WorldGenPass.FinalCleanup,null,6 );
            AddToList(ConstantEnum.PCObjectCondTypes.Painting3x3, new TileFrame(TileID.Painting3X3), ConstantEnum.WorldGenPass.FinalCleanup,null,9 );
            AddToList(ConstantEnum.PCObjectCondTypes.Painting4x3, new TileFrame(TileID.Painting4X3), ConstantEnum.WorldGenPass.FinalCleanup,null,12 );
            AddToList(ConstantEnum.PCObjectCondTypes.Painting6x4, new TileFrame(TileID.Painting6X4), ConstantEnum.WorldGenPass.FinalCleanup,null,24 );
        }  

        

        public static Func<PropertyElement, bool>  CheckIfTileIsPartOfBrokenObject =  (p) => {            
            TileObjectData tdata = TileObjectData.GetTileData(p.IDTile,0);
            if(tdata==null) return false;
            int w = tdata.Width;
            int h = tdata.Height;            
            if(h==1 && w==1) return false;

            //int fsx = tdata.CoordinateFullWidth;//fullObjectSizeX
            //int fsy = tdata.CoordinateFullHeight;


            bool isBrokenObject = IsBrokenObjectTileTopMostLeftMost(p,w,h);
            if(!isBrokenObject) return false;
            
            
            return true;
        };

        public static Func<PropertyElement, int , int, bool> IsBrokenObjectTileTopMostLeftMost = (p, tileSizeX, tileSizeY) => {
            bool ignoreDifferentFrameData = false; 
            if(p.IDTile == TileID.ClosedDoor || p.IDTile == TileID.Chairs || p.IDTile == TileID.Sunflower) //big amount of objects has wrong frame data
                ignoreDifferentFrameData = true;
            if(p.IDTile == TileID.Chairs) return false; // they even have frame numbers not multiple of 18


            Tuple<int,int,int,int> orign = Helpers.Helpers.GetObjectOrigin(p.posX, p.posY);
            int xs = orign.Item1;
            int ys = orign.Item2;
            int frameStartX = orign.Item3;
            int frameStartY = orign.Item4;
            int count = 0;
            Tile holder;
            bool isTopMostLeftMost = false;
            for(int x=0;x<tileSizeX;x++)
            for(int y=0;y<tileSizeY;y++)
            if(!(holder = Main.tile[xs+x,ys+y]).HasTile || holder.TileType != p.IDTile || (!ignoreDifferentFrameData && (holder.TileFrameX != frameStartX+18*x || holder.TileFrameY != frameStartY+18*y)) ) {
                count++;
            }else if(count==x*tileSizeY+y){
                if(holder.TileFrameX == p.frameX && holder.TileFrameY == p.frameY ) isTopMostLeftMost = true;
            }
           
            return count>0 && isTopMostLeftMost;
        };


        public static Func<PropertyElement, bool>  IsGemTreeBase =  (p) => {
            if(!Main.tile[p.posX,p.posY+1].HasTile) return false;
            ushort baser = Main.tile[p.posX,p.posY+1].TileType;
            bool istree= baser==TileID.Stone || baser==TileID.Ebonstone || baser==TileID.Crimstone;            
            return istree;
        };
        public static Func<PropertyElement, bool>  IsRealStatue =  (p) => {
                if(  p.frameX >= 1656 && p.frameX <= 1764) return false;                
             return true;
        };
    
        public static Func<Condition,PropertyElement, bool>  StatueTypeCouner =  (c, p) => {                
                int ii = StatueMemId.tile2IDWhichMightBreakAtUpdate(p.posX,p.posY);
                if(c.memory==null) c.memory = new Storage.PassMemory.SeedMemoryBase<bool>(()=> ConstantEnum.StatueMemId.MAXSTATUETYPES );
                if(c.memory.Get(ii)==true) return false;                    
                c.memory.Set( ii, true);
                return true;
        };
        
        static void AddToList(string name, TileFrame tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement, bool> matchFun = null, int countDivisor = 1,  bool AlsoCheckVoidTiles = false) =>
                    AddToList(name,(tileId==null?null:new List<TileFrame>{ tileId}), FirstEntryPoint, matchFun, countDivisor, null, null  ,AlsoCheckVoidTiles );


        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasStatueType = (p, c) =>        
        {               
            return StatueTypes.tile2Type(p.posX,p.posY).ToString().Equals(c.targetValue.GetValueAsString())?c.targetValue.GetValueAsString():"not " + c.targetValue.GetValueAsString();
        };

        public static Func<PropertyElement, Constraint, ValueStringAndOrDouble> HasobjectType = (p, c) =>        
        {               
            return ObjectType.TileToObjectType(p.posX,p.posY).Equals(c.targetValue.GetValueAsString())?c.targetValue.GetValueAsString():"not " + c.targetValue.GetValueAsString();
        };


        static List<PropertyElement> GetListFromTileFrames(List<TileFrame> tileId, Func<PropertyElement, bool> matchFun = null){
            List<PropertyElement> propel = new List<PropertyElement>();
            if(tileId==null){
                propel.Add(new PropertyElement { CondType = ConstantEnum.PropertyType.Tile, isActive=true, matchFun=matchFun });
                return propel;
            }

            foreach (TileFrame idframe in tileId)
            {
                propel.Add(new PropertyElement { CondType = ConstantEnum.PropertyType.Tile, IDTile = (short)idframe.TileID, frameX = (short)idframe.FrameX, frameY = (short)idframe.FrameY, isActive=true, matchFun=matchFun });
            }
            return propel;
        }

        static void AddToList(string name, List<TileFrame> tileIdframe, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement, bool> matchFun = null,
                             int countDivisor = 1, List<Constraint> bonusConstrains = null, Func<Condition, PropertyElement, bool> postConstraintCheckFun = null, bool AlsoCheckVoidTiles = false )
        {
            if (pcObjectConditionList == null) pcObjectConditionList = new List<Condition>();
            List<PropertyElement> propel = GetListFromTileFrames(tileIdframe, matchFun);
            if(AlsoCheckVoidTiles) for(int i=0; i < propel.Count; i++) propel[i].isActive = null;

            List<Constraint> condelli = new List<Constraint>();

            foreach (Constraint condel in BasicConstraintList.pcBasicDistanceConstraintList)
            {
                Constraint condelNew = new Constraint(condel);

                if (FirstEntryPoint > condel.delayToworldGenPass)
                    condelNew.delayToworldGenPass = FirstEntryPoint;
                else
                    condelNew.delayToworldGenPass = condel.delayToworldGenPass;

                
                condelli.Add(condelNew);
            }      
            if(bonusConstrains!=null){
                foreach (Constraint condel in bonusConstrains)
                {
                    Constraint condelNew = new Constraint(condel);

                    if (FirstEntryPoint > condel.delayToworldGenPass)
                        condelNew.delayToworldGenPass = FirstEntryPoint;
                    else
                        condelNew.delayToworldGenPass = condel.delayToworldGenPass;

                    
                    condelli.Add(condelNew);
                } 
            }

            if(postConstraintCheckFun!=null)
                pcObjectConditionList.Add(new Condition(ConditionType.Object, name, propel, condelli){countDivisor = countDivisor, postConstraintCheckFun=postConstraintCheckFun} );
            else
                pcObjectConditionList.Add(new Condition(ConditionType.Object, name, propel, condelli){countDivisor = countDivisor} );
        }


    }

}