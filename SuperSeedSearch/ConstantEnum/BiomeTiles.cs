
using Terraria.ID;
using Terraria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;       

namespace SuperSeedSearch.ConstantEnum
{
    public class BiomeTiles{
        [Display(Name = "Evil biome tiles")]
        public static readonly int[] evilBiomeTiles = {23, 25, 32, 112, 163, 400, 398, 199, 200, 201, 203, 205, 234, 352, 401, 399};//todo replace by names and remove not HM
        [Display(Name = "Jungle Grass")]
        public static readonly int[] JungleGrass = {TileID.JungleGrass};
        [Display(Name = "Snow biome tiles")]
        public static readonly int[] SnowBiome = {TileID.SnowBlock, TileID.IceBlock};
        [Display(Name = "Desert Underground biome tiles")]
        public static readonly int[] DesertUndergroundBiome = {TileID.HardenedSand, TileID.Sandstone};
        [Display(Name = "Sand Block")]
        public static readonly int[] SandBlock = {TileID.Sand};
        public static readonly int[] Dungeon = {TileID.BlueDungeonBrick, TileID.PinkDungeonBrick, TileID.BlueDungeonBrick};
        [Display(Name = "Living Wood tiles")]
        public static readonly int[] LivingWood = {TileID.LivingWood, TileID.LeafBlock};
        public static readonly int[] Granite = {TileID.Granite};
        public static readonly int[] Marble = {TileID.Marble};
        [Display(Name = "Lihzahrd Brick")]
        public static readonly int[] LihzahrdBrick = {TileID.LihzahrdBrick};
        [Display(Name = "Bee Hive")]
        public static readonly int[] BeeHive = {TileID.Hive};
        [Display(Name = "Glowing Mushroom")]
        public static readonly int[] GlowingMushroom = {TileID.MushroomGrass};
        [Display(Name = "Pyramid tiles")]
        public static readonly int[] Pyramid = {TileID.SandstoneBrick}; 
        [Display(Name = "Dirt Block")]
        public static readonly int[] DirtBlock = {TileID.Dirt};
        [Display(Name = "Grass blocks")]
        public static readonly int[] GrassBlock = {TileID.Grass, TileID.AshGrass, TileID.JungleGrass, TileID.CorruptGrass, TileID.CrimsonGrass, TileID.HallowedGrass, TileID.MushroomGrass, TileID.CorruptJungleGrass, TileID.CrimsonJungleGrass};
        [Display(Name = "Stone Block")]
        public static readonly int[] StoneBlock = {TileID.Stone};
        
    }

}






