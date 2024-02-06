
using Terraria.ID;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;       

namespace SuperSeedSearch.ConstantEnum
{
    //todo maybe some walls are missing or not needed
    public class BiomeWalls{
        [Display(Name = "Dungeon any wall")]
        public static readonly int[] DungeonAny = {WallID.BlueDungeonUnsafe, WallID.BlueDungeonSlabUnsafe, WallID.BlueDungeonTileUnsafe,WallID.GreenDungeonUnsafe, WallID.GreenDungeonSlabUnsafe, WallID.GreenDungeonTileUnsafe,WallID.PinkDungeonUnsafe, WallID.PinkDungeonSlabUnsafe, WallID.PinkDungeonTileUnsafe};
        [Display(Name = "Dungeon brick wall")]
        public static readonly int[] DungeonBrick = {WallID.BlueDungeonUnsafe, WallID.GreenDungeonUnsafe, WallID.PinkDungeonUnsafe};
        [Display(Name = "Dungeon slab wall")]
        public static readonly int[] DungeonSlab = {WallID.BlueDungeonSlabUnsafe, WallID.GreenDungeonSlabUnsafe, WallID.PinkDungeonSlabUnsafe};
        [Display(Name = "Dungeon tiled wall")]
        public static readonly int[] DungeonTilee = {WallID.BlueDungeonTileUnsafe, WallID.GreenDungeonTileUnsafe, WallID.PinkDungeonTileUnsafe};
        
        [Display(Name = "Evil biome wall")]
        public static readonly int[] evilBiomeTiles = {WallID.EbonstoneUnsafe, WallID.CrimstoneUnsafe };
        [Display(Name = "Jungle wall")]
        public static readonly int[] Jungle = {WallID.JungleUnsafe, WallID.JungleUnsafe1, WallID.JungleUnsafe2,WallID.JungleUnsafe3, WallID.JungleUnsafe4};
        [Display(Name = "Snow biome wall")]
        public static readonly int[] SnowBiome = {WallID.SnowWallUnsafe};
        [Display(Name = "Desert Underground biome wall")]
        public static readonly int[] DesertUndergroundBiome = {WallID.HardenedSand, WallID.Sandstone};
        
        [Display(Name = "Living Wood wall")]
        public static readonly int[] LivingWood = {WallID.LivingWoodUnsafe};
        [Display(Name = "Granite wall")]
        public static readonly int[] Granite = {WallID.GraniteUnsafe};
        [Display(Name = "Marble wall")]
        public static readonly int[] Marble = {WallID.MarbleUnsafe};
        [Display(Name = "Lihzahrd Brick wall")]
        public static readonly int[] LihzahrdBrick = {WallID.LihzahrdBrickUnsafe};
        [Display(Name = "Bee Hive wall")]
        public static readonly int[] BeeHive = {WallID.HiveUnsafe};        
        [Display(Name = "Pyramid wall")]
        public static readonly int[] Pyramid = {WallID.SandstoneBrick};
    }

}






