
using Terraria;
using System;
using System.Collections.Generic;


namespace SuperSeedSearch.ConstantEnum
{
    public enum ChestType : short{        
        WoodChest = 0,
        GoldChest,
        ShadowChest,
        RichMahoganyChest,
        IvyChest,
        FrozenChest,
        LivingWoodChest,
        SkywareChest,
        WebCoveredChest,
        LihzahrdChest,
        WaterChest,
        BiomeJungleChest,
        BiomeCorruptionChest,
        BiomeCrimsonChest,
        BiomeHallowedChest,
        BiomeIceChest,
        BiomeDesertChest,
        MushroomChest,
        GraniteChest,
        MarbleChest,
        SandstoneChest,   
        DeadMansChest,
        Other
    }

    public static class ChestID2ChestType{
        static Dictionary<Tuple<ushort,short>,ChestType> dictChestType = new Dictionary<Tuple<ushort,short>,ChestType>{            
            {AsTuple(21,1), ChestType.GoldChest},
            {AsTuple(21,0), ChestType.WoodChest},
            {AsTuple(21,2), ChestType.GoldChest},
            {AsTuple(21,3), ChestType.ShadowChest},
            {AsTuple(21,4), ChestType.ShadowChest},            
            {AsTuple(21,8), ChestType.RichMahoganyChest},
            {AsTuple(21,10), ChestType.IvyChest},
            {AsTuple(21,11), ChestType.FrozenChest},
            {AsTuple(21,12), ChestType.LivingWoodChest},
            {AsTuple(21,13), ChestType.SkywareChest},
            {AsTuple(21,15), ChestType.WebCoveredChest},
            {AsTuple(21,16), ChestType.LihzahrdChest},
            {AsTuple(21,17), ChestType.WaterChest},
            {AsTuple(21,18), ChestType.BiomeJungleChest},
            {AsTuple(21,19), ChestType.BiomeCorruptionChest},
            {AsTuple(21,20), ChestType.BiomeCrimsonChest},
            {AsTuple(21,21), ChestType.BiomeHallowedChest},
            {AsTuple(21,22), ChestType.BiomeIceChest},
            {AsTuple(467,12), ChestType.BiomeDesertChest},
            {AsTuple(21,23), ChestType.BiomeJungleChest},
            {AsTuple(21,24), ChestType.BiomeCorruptionChest},
            {AsTuple(21,25), ChestType.BiomeCrimsonChest},
            {AsTuple(21,26), ChestType.BiomeHallowedChest},
            {AsTuple(21,27), ChestType.BiomeIceChest},
            {AsTuple(467,13), ChestType.BiomeDesertChest},
            {AsTuple(21,32), ChestType.MushroomChest},
            {AsTuple(21,50), ChestType.GraniteChest},
            {AsTuple(21,51), ChestType.MarbleChest},
            {AsTuple(467,10), ChestType.SandstoneChest},
            {AsTuple(467,4), ChestType.DeadMansChest},
            
        };
        public static readonly List<string> ChestTypeList = Helpers.EnumHelper<ChestType>.GetAllEnumVariablesAsString();

        static Tuple<ushort,short> AsTuple(ushort tileID, short frameXdivby36){
            return new Tuple<ushort,short>(tileID, frameXdivby36);
        }



        public static ChestType id2Type(int vanillaChestID ){
            int x = Main.chest[vanillaChestID].x;
            int y = Main.chest[vanillaChestID].y;
            var key = new Tuple<ushort,short>(Main.tile[x,y].TileType, (short)(Main.tile[x,y].TileFrameX/36));
            return dictChestType.ContainsKey(key)? dictChestType[key]: ChestType.Other;
        }

    }
    


}