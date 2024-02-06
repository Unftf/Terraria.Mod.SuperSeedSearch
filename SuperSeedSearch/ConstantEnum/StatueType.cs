
using Terraria;
using Terraria.ID;
using System;
using System.Collections.Generic;




namespace SuperSeedSearch.ConstantEnum
{
    public enum StatueType : short{        
        StarStatue = 0,
        SlimeStatue,
        BatStatue,
        GoldFishStatue,
        BunnyStatue,
        SkeletonStatue,
        BombStatue,
        CrabStatue,
        JellyfishStatue,
        ChestStatue,
        HeartStatue,
        KingStatue,
        QueenStatue,
        PiranhaStatue,
        SharkStatue,
        WallCreeperSpiderStatue,
        UnicornStatue,
        DripplerStatue,
        WraithStatue,
        BoneSkeletonStatue,
        UndeadVikingStatue,
        MedusaStatue,
        HarpyStatue,
        PigronStatue,
        HopliteStatue,
        GraniteGolemStatue,
        ArmedZombieStatue,
        BloodZombieStatue,        
        MushroomStatue,  
        Other
    }

    public static class StatueTypes{
        static readonly Dictionary<Tuple<ushort,short,short>,StatueType> dictStatueTileFrameToType = new Dictionary<Tuple<ushort,short,short>,StatueType>{
            {AsTuple(105,19,1), StatueType.ArmedZombieStatue},  
            {AsTuple(105,7,0), StatueType.BatStatue},  
            {AsTuple(105,20,1), StatueType.BloodZombieStatue},  
            {AsTuple(105,17,0), StatueType.BombStatue},  
            {AsTuple(105,12,1), StatueType.BoneSkeletonStatue},
            {AsTuple(105,9,0), StatueType.BunnyStatue},  
            {AsTuple(105,27,0), StatueType.ChestStatue},  
            {AsTuple(105,18,0), StatueType.CrabStatue},  
            {AsTuple(105,10,1), StatueType.DripplerStatue},  
            {AsTuple(105,8,0), StatueType.GoldFishStatue},  
            {AsTuple(105,18,1), StatueType.GraniteGolemStatue},  
            {AsTuple(105,15,1), StatueType.HarpyStatue},  
            {AsTuple(105,37,0), StatueType.HeartStatue},  
            {AsTuple(105,17,1), StatueType.HopliteStatue},  
            {AsTuple(105,23,0), StatueType.JellyfishStatue},  
            {AsTuple(105,40,0), StatueType.KingStatue},  
            {AsTuple(105,14,1), StatueType.MedusaStatue},               
            {AsTuple(105,16,1), StatueType.PigronStatue},  
            {AsTuple(105,42,0), StatueType.PiranhaStatue},  
            {AsTuple(105,41,0), StatueType.QueenStatue},  
            {AsTuple(105,50,0), StatueType.SharkStatue},  
            {AsTuple(105,10,0), StatueType.SkeletonStatue},  
            {AsTuple(105,4,0), StatueType.SlimeStatue},  
            {AsTuple(105,2,0), StatueType.StarStatue},  
            {AsTuple(105,13,1), StatueType.UndeadVikingStatue},  
            {AsTuple(105,9,1), StatueType.UnicornStatue},  
            {AsTuple(105,8,1), StatueType.WallCreeperSpiderStatue},  
            {AsTuple(105,11,1), StatueType.WraithStatue},
            {AsTuple(349,0,0), StatueType.MushroomStatue},
        };
        public static readonly Dictionary<StatueType, short> dictStatueTypeToItemId = new Dictionary<StatueType, short>{
            {StatueType.ArmedZombieStatue, ItemID.ZombieArmStatue},  
            {StatueType.BatStatue, ItemID.BatStatue},  
            {StatueType.BloodZombieStatue, ItemID.BloodZombieStatue},  
            {StatueType.BombStatue, ItemID.BombStatue},  
            {StatueType.BoneSkeletonStatue, ItemID.BoneSkeletonStatue},
            {StatueType.BunnyStatue, ItemID.BunnyStatue},  
            {StatueType.ChestStatue, ItemID.ChestStatue},  
            {StatueType.CrabStatue, ItemID.CrabStatue},  
            {StatueType.DripplerStatue, ItemID.DripplerStatue},  
            {StatueType.GoldFishStatue, ItemID.FishStatue},  
            {StatueType.GraniteGolemStatue, ItemID.GraniteGolemStatue},  
            {StatueType.HarpyStatue, ItemID.HarpyStatue},  
            {StatueType.HeartStatue, ItemID.HeartStatue},  
            {StatueType.HopliteStatue, ItemID.HopliteStatue},  
            {StatueType.JellyfishStatue, ItemID.JellyfishStatue},  
            {StatueType.KingStatue, ItemID.KingStatue},  
            {StatueType.MedusaStatue, ItemID.MedusaStatue},               
            {StatueType.PigronStatue, ItemID.PigronStatue},  
            {StatueType.PiranhaStatue, ItemID.PiranhaStatue},  
            {StatueType.QueenStatue, ItemID.QueenStatue},  
            {StatueType.SharkStatue, ItemID.SharkStatue},  
            {StatueType.SkeletonStatue, ItemID.SkeletonStatue},  
            {StatueType.SlimeStatue, ItemID.SlimeStatue},  
            {StatueType.StarStatue, ItemID.StarStatue},  
            {StatueType.UndeadVikingStatue, ItemID.UndeadVikingStatue},  
            {StatueType.UnicornStatue, ItemID.UnicornStatue},  
            {StatueType.WallCreeperSpiderStatue, ItemID.WallCreeperStatue},  
            {StatueType.WraithStatue, ItemID.WraithStatue},
            {StatueType.MushroomStatue, ItemID.MushroomStatue},
        };

        public static readonly List<string> StatueTypeDict = Helpers.EnumHelper<StatueType>.GetAllEnumVariablesAsString();

        static Tuple<ushort,short,short> AsTuple(ushort tileID, short frameXdivby36, short frameYdivby54){
            return new Tuple<ushort,short,short>(tileID, frameXdivby36, frameYdivby54);
        }
        public static StatueType tile2Type(int tileX, int tileY ){
            ushort type = Main.tile[tileX,tileY].TileType;
            short fx = (short)(Main.tile[tileX,tileY].TileFrameX/36);
            short fy = (short)(Main.tile[tileX,tileY].TileFrameY/54);
            var key = new Tuple<ushort,short,short>(type, fx,fy);
            return dictStatueTileFrameToType.ContainsKey(key)? dictStatueTileFrameToType[key]: StatueType.Other;
        }
    }
    public class StatueMemId{
        public const int MAXSTATUETYPES = 120;
        public static int tile2IDWhichMightBreakAtUpdate(int tileX, int tileY ){
            ushort type = Main.tile[tileX,tileY].TileType;
            short fx = (short)(Main.tile[tileX,tileY].TileFrameX/36);
            short fy = (short)(Main.tile[tileX,tileY].TileFrameY/54);
            int id = fx+fy*55;
            return type==TileID.Statues?id:(MAXSTATUETYPES-type/100);
        }
    }


}