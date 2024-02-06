using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSeedSearch.PropertyCondition
{
    public static class WorldStyles
    {
        public static List<String> pcMoonType = new List<string>
        {
            "Normal","Yellow","Ringed","Mythril","Bright Blue","Green","Pink","Orange","Purple" 
        };
        public static List<String> pcDungeonColor = new List<string>
        {
            "Blue","Green","Pink" 
        };
        public static List<String> pcLeftRightSide = new List<string>
        {
            "Left","Right",
        };
        public static List<String> pcDesertStyles = new List<string>
        {
            "Normal","Chambers entrance", "Ant hill entrance", "Larva hole entrance", "Pit entrance"
        };
        public static List<String> pcJungelDungeonSide = new List<string>
        {
            "At Jungle Biome side","At Dungeon side",
        };
        public static List<String> pcOreCopperOrTin = new List<string>
        {
            "Copper Ore","Tin Ore",
        };
        public static List<String> pcOreIronOrLead = new List<string>
        {
            "Iron Ore","Lead Ore",
        };
        public static List<String> pcOreSilverOrTungsten = new List<string>
        {
            "Silver Ore","Tungsten Ore",
        };
        public static List<String> pcOreGoldOrPlatinum = new List<string>
        {
            "Gold Ore","Platinum Ore",
        };
        public static List<String> pcGlowingMossType = new List<string>
        {
            "Argon (pink glowing)","Xenon (light blue/cyan glowing)","Krypton (green glowing)","Neon (purple glowing)" 
        };
        public static List<String> pcJungleShrineType = new List<string>
        {
            "Iridescent brick","Mudstone","Mahogany","Tin","Gold" 
        };
        public static List<String> pcOceanCaveDungeon = new List<string>
        {
            "Has an Ocean Cave on Dungeon side","Has no Ocean Cave on Dungeon side" 
        };


        public static List<String> pcSalamanderGiantShellyCrawdad_Salamander = new List<string>
        {
            "Salamander 1 (blue)", "Salamander 2 (green)",  "Salamander 3 (dark gray)",  "Salamander 4 (orange)",  "Salamander 5 (pink)",  "Salamander 6 (purple)",  "Salamander 7 (light gray)",  "Salamander 8 (white)",  "Salamander 9 (gold)", 
        };


        public static List<String> pcSalamanderGiantShellyCrawdad_GiantShelly = new List<string>
        {
            "Giant Shelly 1 (purple)", "Giant Shelly 2 (orange)", 
        };

        public static List<String> pcSalamanderGiantShellyCrawdad_Crawdad = new List<string>
        {
            "Crawdad 1 (red)", "Crawdad 2 (blue)", 
        };

        public static List<String> pcSalamanderGiantShellyCrawdad_Extinct = new List<string>
        {
            "Crawdad", "Giant Shelly", "Salamander"
        };

        public static List<String> pcExtensionDesertBiome = new List<string>
        {
            "Potential Pyramid", "Pyramid", "Oasis"
        };


        public const char bgStyleDivider = ':';        
        const String bgStyle = "backround style";
        public const String ForestBackground = "Forest "+bgStyle;
        public const String CavernBackground = "Cavern "+bgStyle;
        public const String CavernIceBackground = "Cavern Ice "+bgStyle;
        public const String CavernHellBackground = "Cavern Hell "+bgStyle;
        public const String CavernJungleBackground = "Cavern Jungle "+bgStyle;
        public const String CorruptionBackground = "Corruption "+bgStyle;
        public const String JungleBackground = "Jungle "+bgStyle;
        public const String SnowBackground = "Snow "+bgStyle;
        public const String HallowBackground = "Hallow "+bgStyle;
        public const String CrimsonBackground = "Crimson "+bgStyle;
        public const String DesertBackground = "Desert "+bgStyle;
        public const String OceanBackground = "Ocean "+bgStyle;
        public const String MushroomBackground = "Mushroom "+bgStyle;
        public const String UnderworldBackground = "Underworld "+bgStyle;

        static List<string> Combine(String baseName, int maxNum, List<string> extra = null){
            List<string> styles = new List<string>();
            for(int i=0;i<=maxNum;i++)
                styles.Add(baseName+ bgStyleDivider+" "+i);
            if(extra !=null && extra.Count>0){
                for(int i=0;i<extra.Count;i++)
                styles.Add(baseName+ bgStyleDivider+" "+extra[i]);
            }    
            return styles;
        }

        public static List<String> pcBackgroundStyleForest = Combine(ForestBackground,10, new List<string>{"31","51","71","72","73"});
        public static List<String> pcBackgroundStyleCavern = Combine(CavernBackground,7);

        public static List<String> pcBackgroundStyle = 
            pcBackgroundStyleForest.Concat(
            pcBackgroundStyleCavern).Concat(
            Combine(CavernIceBackground,3)).Concat(
            Combine(CavernHellBackground,2)).Concat(
            Combine(CavernJungleBackground,1)).Concat(
            Combine(CorruptionBackground,4)).Concat(
            Combine(JungleBackground,5)).Concat(
            Combine(SnowBackground,7, new List<string>{"21","22","31","32","41","42"} )).Concat(
            Combine(HallowBackground,4)).Concat(
            Combine(CrimsonBackground,5)).Concat(
            Combine(DesertBackground,4)).Concat(
            Combine(OceanBackground,5)).Concat(
            Combine(MushroomBackground,3)).Concat(
            Combine(UnderworldBackground,2))
                .ToList();


        public const String TreeStyle = "Tree style";
        public static List<String> pcTreeStyle = Combine(TreeStyle,5);


    }
}


 