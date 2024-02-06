using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ID;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.UI;


namespace SuperSeedSearch.PropertyCondition.PCTiles
{
    public static class PCTilesConditionList
    {

        public static List<Condition> pcTilesConditionList = null;



        static PCTilesConditionList()
        {
            AddToList(ConstantEnum.PCTilesCondTypes.PyramidTip, new List<ushort> { TileID.SandstoneBrick }, ConstantEnum.WorldGenPass.Pyramids, IsPyramidTip);
            AddToList(ConstantEnum.PCTilesCondTypes.Pyramid, new List<ushort> { TileID.SandstoneBrick }, ConstantEnum.WorldGenPass.Pyramids);
            AddToList(ConstantEnum.PCTilesCondTypes.SandBlockEarlyWorldGen, new List<ushort> { TileID.Sand }, ConstantEnum.WorldGenPass.SandPatches );
            AddToList(ConstantEnum.PCTilesCondTypes.SandInDunes, new List<ushort> { TileID.Sand }, ConstantEnum.WorldGenPass.Dunes );
            AddToList(ConstantEnum.PCTilesCondTypes.TreeWood,  new List<ushort> {TileID.Trees, TileID.PalmTree, TileID.Bamboo, TileID.VanityTreeSakura, TileID.VanityTreeYellowWillow, TileID.TreeAsh}, ConstantEnum.WorldGenPass.FinalCleanup );//some earlier

            AddToList(ConstantEnum.PCTilesCondTypes.SnowBiomeTiles, new List<ushort> { TileID.SnowBlock, TileID.IceBlock, TileID.FleshIce, TileID.CorruptIce, TileID.BreakableIce, TileID.HallowedIce },
                ConstantEnum.WorldGenPass.GenerateIceBiome);            
            AddToList(ConstantEnum.PCTilesCondTypes.MushroomGrass, new List<ushort> { TileID.MushroomGrass }, ConstantEnum.WorldGenPass.MushroomPatches);
            AddToList(ConstantEnum.PCTilesCondTypes.LihzahrdBrick, new List<ushort> { TileID.LihzahrdBrick }, ConstantEnum.WorldGenPass.JungleTemple);
            AddToList( TileID.Marble, ConstantEnum.WorldGenPass.Marble);            
            AddToList( TileID.Granite, ConstantEnum.WorldGenPass.Granite);
            AddToList( TileID.Cobweb , ConstantEnum.WorldGenPass.WebsAndHoney);//doesnt need to be in spider biome
            AddToList( ConstantEnum.PCTilesCondTypes.BeeHive, new List<ushort> { TileID.Hive }, ConstantEnum.WorldGenPass.Hives);
            AddToList( ConstantEnum.PCTilesCondTypes.LivingTree, new List<ushort> { TileID.LivingWood, TileID.LeafBlock }, ConstantEnum.WorldGenPass.Hives);
            AddToList(ConstantEnum.PCTilesCondTypes.JungleInDanger, new List<ushort> { TileID.JungleGrass }, ConstantEnum.WorldGenPass.Hives, IsEvilBiomeArround);
            AddToList(ConstantEnum.PCTilesCondTypes.IceInDanger, new List<ushort> { TileID.IceBlock }, ConstantEnum.WorldGenPass.Hives, IsEvilBiomeArround);
            AddToList(ConstantEnum.PCTilesCondTypes.HSandInDanger, new List<ushort> { TileID.HardenedSand, TileID.Sandstone }, ConstantEnum.WorldGenPass.Hives, IsEvilBiomeArround);
            AddToList(ConstantEnum.PCTilesCondTypes.EvilTiles, BiomeTiles.evilBiomeTiles.Select( i => (ushort)i ).ToList() , ConstantEnum.WorldGenPass.Hives);
            AddToList(ConstantEnum.PCTilesCondTypes.JungleEarlyBiomeTiles, new List<ushort> { TileID.Mud, TileID.JungleGrass }, ConstantEnum.WorldGenPass.Jungle);//might be overridden later on, TODO test if worth it
            AddToList(ConstantEnum.PCTilesCondTypes.JungleGrass, new List<ushort> {TileID.JungleGrass }, ConstantEnum.WorldGenPass.MudCavesToGrass);
            AddToList(ConstantEnum.PCTilesCondTypes.Water, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=false, matchFun=(p)=>Main.tile[p.posY,p.posY].LiquidType == LiquidID.Water && Main.tile[p.posY,p.posY].LiquidAmount > 32 } , ConstantEnum.WorldGenPass.Oasis);//maybe some extra check for early wg water
            AddToList(ConstantEnum.PCTilesCondTypes.Lava, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=false, matchFun=(p)=>Main.tile[p.posY,p.posY].LiquidType == LiquidID.Lava && Main.tile[p.posY,p.posY].LiquidAmount > 0 } , ConstantEnum.WorldGenPass.Oasis);
            AddToList(ConstantEnum.PCTilesCondTypes.Honey, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=false, matchFun=(p)=>Main.tile[p.posY,p.posY].LiquidType == LiquidID.Honey && Main.tile[p.posY,p.posY].LiquidAmount > 0 } , ConstantEnum.WorldGenPass.Oasis);
            AddToList(ConstantEnum.PCTilesCondTypes.Shimmer, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=false, matchFun=(p)=>Main.tile[p.posY,p.posY].LiquidType == LiquidID.Shimmer && Main.tile[p.posY,p.posY].LiquidAmount > 0 } , ConstantEnum.WorldGenPass.Oasis);

            AddToList( ConstantEnum.PCTilesCondTypes.GemsStone, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.Diamond }, ConstantEnum.WorldGenPass.Gems, IsRealGemStash); 
            AddToList( ConstantEnum.PCTilesCondTypes.Gems, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles, TileID.Amethyst, TileID.Topaz, TileID.Sapphire, TileID.Emerald, TileID.Ruby, TileID.Diamond }, ConstantEnum.WorldGenPass.RandomGems, IsRealGemStash); 
            AddToList( ConstantEnum.PCTilesCondTypes.Ruby, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles, TileID.Ruby }, ConstantEnum.WorldGenPass.RandomGems, (p) => IsGemFrameChecker(p,4,46) ); // gemss also generated in other passes
            AddToList( ConstantEnum.PCTilesCondTypes.Diamond, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles, TileID.Diamond }, ConstantEnum.WorldGenPass.RandomGems, (p) => IsGemFrameChecker(p,5,48) ); 
            AddToList( ConstantEnum.PCTilesCondTypes.Amber, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles }, ConstantEnum.WorldGenPass.RandomGems, (p) => IsGemFrameChecker(p,6,-1) ); 
            AddToList( ConstantEnum.PCTilesCondTypes.Topaz, new List<ushort> { TileID.ExposedGems, TileID.SmallPiles, TileID.Topaz}, ConstantEnum.WorldGenPass.RandomGems, (p) => IsGemFrameChecker(p,2,40) );

            AddToList(ConstantEnum.PCTilesCondTypes.SolidTile, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=true, matchFun=(p)=> Main.tileSolid[Main.tile[p.posY,p.posY].TileType]  } , ConstantEnum.WorldGenPass.MountCaves); 
            AddToList(ConstantEnum.PCTilesCondTypes.UndergroundDesert, new List<ushort> {TileID.HardenedSand, TileID.Sandstone }, ConstantEnum.WorldGenPass.FullDesert);
            AddToList(ConstantEnum.PCTilesCondTypes.DirtiestPet, new List<ushort> {TileID.DirtiestBlock }, ConstantEnum.WorldGenPass.FinalCleanup);            
            AddToList(ConstantEnum.PCTilesCondTypes.Dirt, new List<ushort> {TileID.Dirt,  TileID.Grass}, ConstantEnum.WorldGenPass.Terrain);

            AddToList(ConstantEnum.PCTilesCondTypes.Daybloom, new List<ushort> {TileID.ImmatureHerbs,  0, TileID.MatureHerbs,  0, TileID.BloomingHerbs,  0}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Moonglow, new List<ushort> {TileID.ImmatureHerbs,  18, TileID.MatureHerbs,  18, TileID.BloomingHerbs,  18}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Blinkroot, new List<ushort> {TileID.ImmatureHerbs,  36, TileID.MatureHerbs,  36, TileID.BloomingHerbs,  36}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Deathweed, new List<ushort>  {TileID.ImmatureHerbs,  54, TileID.MatureHerbs,  54, TileID.BloomingHerbs,  54}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Waterleaf, new List<ushort>  {TileID.ImmatureHerbs,  72, TileID.MatureHerbs,  72, TileID.BloomingHerbs,  72}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Fireblossom, new List<ushort>  {TileID.ImmatureHerbs,  90, TileID.MatureHerbs,  90, TileID.BloomingHerbs,  90}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Shiverthorn, new List<ushort>  {TileID.ImmatureHerbs,  108, TileID.MatureHerbs,  108, TileID.BloomingHerbs,  108}, ConstantEnum.WorldGenPass.Herbs,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.Mushroom, new List<ushort>  {TileID.Plants,  8*18}, ConstantEnum.WorldGenPass.Weeds,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.EvilMushroom, new List<ushort>  {TileID.CorruptPlants,  8*18, TileID.CrimsonPlants,  15*18}, ConstantEnum.WorldGenPass.Weeds,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.GlowingMushroom, new List<ushort>  {TileID.MushroomPlants}, ConstantEnum.WorldGenPass.GlowingMushroomsandJunglePlants);
            AddToList(ConstantEnum.PCTilesCondTypes.JungleSpore, new List<ushort>  {TileID.JunglePlants,  8*18}, ConstantEnum.WorldGenPass.GlowingMushroomsandJunglePlants,null,2);
            AddToList(ConstantEnum.PCTilesCondTypes.AshBlock, new List<ushort>  {TileID.Ash}, ConstantEnum.WorldGenPass.Underworld,null);
            AddToList(ConstantEnum.PCTilesCondTypes.DartTrap, new List<ushort>  {TileID.Traps, 0 }, ConstantEnum.WorldGenPass.Traps,null,-2);//might be some dart traps later on
            AddToList(ConstantEnum.PCTilesCondTypes.Clouds, new List<ushort>  {TileID.Cloud, TileID.RainCloud, TileID.SnowCloud }, ConstantEnum.WorldGenPass.FloatingIslands);
            AddToList(ConstantEnum.PCTilesCondTypes.FloatingIslandHouse, new List<ushort>  {TileID.Sunplate }, ConstantEnum.WorldGenPass.FloatingIslandHouses);
            AddToList(ConstantEnum.PCTilesCondTypes.AnyPostWorldGen, new PropertyElement {CondType = ConstantEnum.PropertyType.Tile, isActive=true, matchFun=(p)=> true  } , ConstantEnum.WorldGenPass.FinalCleanup);
            AddToList(ConstantEnum.PCTilesCondTypes.DungeonBrick, new List<ushort>  {TileID.BlueDungeonBrick,TileID.PinkDungeonBrick,TileID.GreenDungeonBrick }, ConstantEnum.WorldGenPass.Dungeon);
        }



        static Func<PropertyElement,int,int, bool> IsGemFrameChecker = (p,fex,fpi) => {
            if(p.IDTile ==  TileID.ExposedGems) return p.frameX/18==fex;
            if(p.IDTile ==  TileID.SmallPiles) return p.frameY/18==1 && (p.frameX/18 == fpi || p.frameX/18 == fpi+1) ;
            return true;
        };
        static Func<PropertyElement, bool> IsRealGemStash = (p) => {
            if(p.IDTile !=  TileID.SmallPiles) return true;
            return p.frameY/18==1 && (p.frameX/18 >= 38 && p.frameX/18 <= 49);
        };

        static Func<PropertyElement, bool>  IsPyramidTip =  (p) => {
                if(p.posX< 10 || p.posY<10 || p.posX>Main.maxTilesX-10 || p.posY>Main.maxTilesY-10 || p.posY>Main.worldSurface+30) return false;


             return ((Main.tile[p.posX-1,p.posY].HasTile&&Main.tile[p.posX-1,p.posY].TileType!= TileID.SandstoneBrick) || !Main.tile[p.posX-1,p.posY].HasTile  )&&
                    ((Main.tile[p.posX+1,p.posY].HasTile&&Main.tile[p.posX+1,p.posY].TileType!= TileID.SandstoneBrick)|| !Main.tile[p.posX+1,p.posY].HasTile  ) &&
                    ((Main.tile[p.posX,p.posY-1].HasTile&&Main.tile[p.posX,p.posY-1].TileType!= TileID.SandstoneBrick)|| !Main.tile[p.posX,p.posY-1].HasTile  ) ;
        };
        
        
        static Func<PropertyElement, bool>  IsEvilBiomeArround = (p) => {
            //if(p.IDTile != TileID.JungleGrass) return false;
            
            return WorldGen.CountNearBlocksTypes(p.posX,p.posY,4,1,BiomeTiles.evilBiomeTiles)>0;
        };



     
        static void AddToList(ushort tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null) => 
            AddToList("",  new List<ushort>{tileId}, FirstEntryPoint, matchFun, 1);
        static void AddToList(List<ushort> tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null, short listIncludesFrameXYstepsize = 1) => 
            AddToList("",  tileId, FirstEntryPoint, matchFun, listIncludesFrameXYstepsize);
        static void AddToList(string name, ushort tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null) => 
            AddToList(name, new List<ushort>{tileId}, FirstEntryPoint, matchFun, 1);

        static void AddToList(string name, List<ushort> tileId, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null, short listIncludesFrameXYstepsize = 1)
        {
            if(name.Length==0)name = TileID.Search.GetName(tileId[0]);

            if (pcTilesConditionList == null) pcTilesConditionList = new List<Condition>();
            List<PropertyElement> propel = new List<PropertyElement>();
            int len = tileId.Count;
            ushort stepsi = (ushort)Math.Abs(listIncludesFrameXYstepsize);
            bool yfirst = listIncludesFrameXYstepsize<0;
            for(ushort id = 0; id < len; id+=stepsi)
            {
                var npe = new PropertyElement { CondType = ConstantEnum.PropertyType.Tile, IDTile = (short) tileId[id], isActive=true, matchFun=matchFun };
                if( (listIncludesFrameXYstepsize>1 && !yfirst) ||  (listIncludesFrameXYstepsize>2 && yfirst) )
                    npe.frameX = (short)tileId[id+(yfirst?2:1)];
                if( (listIncludesFrameXYstepsize>2 && !yfirst) ||  (listIncludesFrameXYstepsize>1 && yfirst) )
                    npe.frameY = (short)tileId[id+(yfirst?1:2)];
                propel.Add(npe);
            }

            AddToList(name, propel, FirstEntryPoint, matchFun);
        }


        static void AddToList(string name, PropertyElement prop, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null) =>
            AddToList(name, new List<PropertyElement>{prop}, FirstEntryPoint, matchFun);

        static void AddToList(string name, List<PropertyElement> propel, ConstantEnum.WorldGenPass FirstEntryPoint, Func<PropertyElement,bool> matchFun = null){
            List<Constraint> condelli = new List<Constraint>();

            List<Constraint> list2Cpy = BasicConstraintList.pcBasicDistanceConstraintList;
            for(int i=0; i<2;i++){

                if(i==1) list2Cpy = BasicConstraintList.pcTileConstraintList;

                foreach (Constraint condel in list2Cpy)
                {
                    Constraint condelNew = new Constraint(condel);

                    if (FirstEntryPoint > condel.delayToworldGenPass)
                        condelNew.delayToworldGenPass = FirstEntryPoint;
                    else
                        condelNew.delayToworldGenPass = condel.delayToworldGenPass;
                    
                    condelli.Add(condelNew);
                }

            }

      

            pcTilesConditionList.Add(new Condition(ConditionType.Tile, name, propel, condelli));

        }


    }

}