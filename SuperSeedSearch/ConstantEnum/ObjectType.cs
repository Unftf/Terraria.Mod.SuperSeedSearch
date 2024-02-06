
using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


//todo: only needed fore broken?
namespace SuperSeedSearch.ConstantEnum
{

    //Todo unify with object condition list    
    public enum ObjectTypeEnum : ushort{        
        Other,
        Statue,
        Chest,
        Painting,
        Pile, 
        [Display(Name = "Enchanted Sword")]
        EnchantedSword,
        [Display(Name = "Geyser trap")]
        GeyserTrap,
        [Display(Name = "Beee hive")]
        BeeHive,       
        Door,       
        Banner,       
        Extractinator,
        [Display(Name = "Grandfather clock")]
        GrandfatherClock,
        Chandelier,
        [Display(Name = "Living loom")]
        LivingLoom,
        Lamp,        
        [Display(Name = "Bast statue")]
        CatBast 
    }

    public static class ObjectType{
        public static readonly List<string> ObjectTypeList = Helpers.EnumHelper<ObjectTypeEnum>.GetAllEnumVariablesAsString();
        
        //Todo unify with object condition list   
        static Dictionary<ObjectTypeEnum,List<ushort>> dictObjectType = new Dictionary<ObjectTypeEnum,List<ushort>>{
            {ObjectTypeEnum.Statue, new List<ushort>{TileID.Statues, TileID.MushroomStatue }},
            {ObjectTypeEnum.Chest, new List<ushort>{TileID.Containers, TileID.Containers2 }},
            {ObjectTypeEnum.Painting, new List<ushort>{TileID.Painting2X3, TileID.Painting3X2, TileID.Painting3X3,  TileID.Painting4X3,  TileID.Painting6X4 }},
            {ObjectTypeEnum.Pile, new List<ushort>{TileID.LargePiles, TileID.LargePiles2, TileID.BeachPiles,  TileID.ShellPile,  TileID.SmallPiles }},
            {ObjectTypeEnum.GeyserTrap, new List<ushort>{TileID.GeyserTrap }},
            {ObjectTypeEnum.BeeHive, new List<ushort>{TileID.BeeHive }},
            {ObjectTypeEnum.Door, new List<ushort>{TileID.ClosedDoor }},
            {ObjectTypeEnum.Banner, new List<ushort>{TileID.Banners }},
            {ObjectTypeEnum.Extractinator, new List<ushort>{TileID.Extractinator }},
            {ObjectTypeEnum.GrandfatherClock, new List<ushort>{TileID.GrandfatherClocks }},
            {ObjectTypeEnum.Chandelier, new List<ushort>{TileID.Chandeliers }},
            {ObjectTypeEnum.LivingLoom, new List<ushort>{TileID.LivingLoom }},
            {ObjectTypeEnum.Lamp, new List<ushort>{TileID.Lamps }},            
            {ObjectTypeEnum.CatBast, new List<ushort>{TileID.CatBast }},            
        };

        static Dictionary<ushort,ObjectTypeEnum> dictObjectTypeReverse = new Dictionary<ushort,ObjectTypeEnum>();
        static ObjectType(){
            foreach(var ob in dictObjectType){
                foreach(var us in ob.Value){
                    dictObjectTypeReverse.Add(us,ob.Key);
                }
            }
        }

        public static string TileToObjectType(int posx, int posy){
            ushort tid = Main.tile[posx,posy].TileType;
            if(dictObjectTypeReverse.ContainsKey(tid)  ){
                ObjectTypeEnum ot = dictObjectTypeReverse[tid];
                if(tid==TileID.LargePiles2 && Main.tile[posx,posy].TileFrameX/54 == 17 && Main.tile[posx,posy].TileFrameY/36 == 0){
                    return Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ObjectTypeEnum.EnchantedSword);    
                }else if(tid==TileID.Statues){
                    if(Main.tile[posx,posy].TileFrameX >= 1656 && Main.tile[posx,posy].TileFrameX <= 1764) return Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ObjectTypeEnum.Statue);
                    else Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ObjectTypeEnum.Other); 
                }
                
                return Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ot);
            } 
            else return Helpers.EnumHelper<ObjectTypeEnum>.GetNameOFEnumVariable(ObjectTypeEnum.Other);
        }


        //Todo unify with dictObjectType condition list   
        public static Dictionary<string,short> dictObjectStringToItemType = new Dictionary<string,short>{
            {ConstantEnum.PCObjectCondTypes.Anvil, ItemID.IronAnvil},
            {ConstantEnum.PCObjectCondTypes.ESnotShrine, ItemID.EnchantedSword},            
            {ConstantEnum.PCObjectCondTypes.GemTreeAmber, ItemID.Amber},
            {ConstantEnum.PCObjectCondTypes.GemTreeRuby, ItemID.Ruby},
            {ConstantEnum.PCObjectCondTypes.GemTreeDiamond, ItemID.Diamond},
            {ConstantEnum.PCObjectCondTypes.GemTreeTopaz, ItemID.Topaz},
            {ConstantEnum.PCObjectCondTypes.GemTree, ItemID.StoneBlock},
            {ConstantEnum.PCObjectCondTypes.GlowTulipPet, ItemID.GlowTulip},
            {ConstantEnum.PCObjectCondTypes.CatBast, ItemID.CatBast},
            {ConstantEnum.PCObjectCondTypes.Detonator, ItemID.Detonator},       
            {ConstantEnum.PCObjectCondTypes.BrokenObject, ItemID.PoopBlock},                 
            {ConstantEnum.PCObjectCondTypes.GoldCoinStashLarge, ItemID.GoldCoin}, 
            //{ConstantEnum.PCObjectCondTypes.GoldCoinStashSmall, ItemID.GoldCoin}, 
            //{ConstantEnum.PCObjectCondTypes.GoldCoinStashSmallPyramid, ItemID.GoldCoin}, 
            {ConstantEnum.PCObjectCondTypes.SilverCoinStashLarge, ItemID.SilverCoin}, 
            //{ConstantEnum.PCObjectCondTypes.SilverCoinStashSmall, ItemID.SilverCoin}, 
           // {ConstantEnum.PCObjectCondTypes.SilverCoinStashSmallPyramid, ItemID.SilverCoin}, 
            //{ConstantEnum.PCObjectCondTypes.Painting2x3, ItemID.Paintbrush}, //Todo
            {ConstantEnum.PCTilesCondTypes.DirtiestPet, ItemID.DirtiestBlock}, //Todo extra?
        };


    }





}