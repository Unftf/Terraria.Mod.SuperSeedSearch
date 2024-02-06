
namespace SuperSeedSearch.ConstantEnum
{
    public static class ConstraintNames
    { //todo  sort after type
        public const string ParamAddon = "Param";//addOn for all functions with parameter

        public const string DistanceHorizontal = "horizontal distance to";
        public const string MidOfWorld = "mid of world";
        public const string DifferenceHorizontalToMid = "horizontal difference to-mid-distance";
        public const string DistanceToMidWorldHorizontal = DistanceHorizontal +" " + MidOfWorld;
        public const string DistanceToSpawndHorizontal = DistanceHorizontal + " Spawn";
        public const string DistanceToPredictedSpawndHorizontal = DistanceHorizontal + " Predicted Spawn";
        public const string DistanceToJungleMainEntranceHorizontal = DistanceHorizontal + " Jungle main entrance";
        public const string DungeonFirstRoomEarly = "Dungeon (1st gen. UG room)";
        public const string Dungeon = "Dungeon";
        public const string DistanceToDungeonEarlyHorizontal = DistanceHorizontal + " " + DungeonFirstRoomEarly;
        public const string DistanceToDungeonHorizontal = DistanceHorizontal + " "+ Dungeon;
        public const string DepthBelowPredictedSpawn = "Depth below predicted spawn";
        public const string DepthBelowSpawn = "Depth below Spawn";
        public const string DepthBelowJungleMainEntrance = "Depth below jungle main entrance";
        public const string StartOfSnowBiomeGen = "Start of Snow biome gen. (at world top)";
        public const string ClosestMountaincaveEntrance = "closest mountain cave entrance";
        public const string ClosestOasis = "closest Oasis";
        public const string ClosestPyramid = "closest Pyramid";
        public const string ClosestPotentialPyramid = "closest potential Pyramid location";
        public const string StartOfBeach = "Start of beach"; 
        public const string EndofWorld = "end of the world";

        public const string DistanceToSpawndEuclid = "Euclidean distance to Spawn";
        public const string DistanceEuclid = "Euclidean distance to";
        public const string DistanceToPredSpawndEuclid = "Euclidean distance to predicted Spawn";
        public const string DistanceToJungleMainEntranceEuclid = "Euclidean distance to jungle main entrance";
        public const string DistanceToSpawndEuclidParam1 = "scale X";
        public const string DistanceToPredSpawndEuclidParam1 = "scale X";
        public const string PathlengthSpawn = "Pathlength from player Spawn";
        public const string PathlengthJungleMain = "Pathlength from Jungle main entrance";
        public const string PathlengthSpawnReducedDepth = "Pathlength from player Spawn - best depth cost";
        public const string PathlengthJungleMainReducedDepth = "Pathlength from Jungle main entrance - best depth cost";
        public const string PathlengthQuickPredictedSpawn = "Near path from predicted Spawn early world gen.";
        public const string PathlengthQuickJungleMain = "Near path from Jungle main entrance early world gen.";
        public const string PathlengthQuickDesertCSurface = "Near path from Desert center surface early world gen.";
        public const string PathlengthQuickSnowCSurface = "Near path from Snow center surface early world gen.";
        public const string PathlengthQuickShimmer = "Near path from Shimmer center early world gen.";


        public const string TilesOnDungeonSide = "Tiles on Dungeon side";
        public const string TilesOnTempleSide = "Tiles on Temple side";
        public const string HeightAbove = "height above";
        public const string DepthBelow = "depth below";
        public const string HeightAboveUndergroundLayer = "Height above underground layer";
        public const string HeightAboveCavernUndergroundLayer = "Height above cavern layer";
        public const string WestLeftOf = "west/left of";
        public const string EastRigthOf = "east/right of";

        public const string WestLeftOfGreater0 = "west/left (0+) of";
        public const string EastRigthOfGreater0 = "east/right (0+) of";

        public const string WestLeftOfMid = "Tiles " + WestLeftOf + " " + MidOfWorld;
        public const string EastRightOfMid = "Tiles " + EastRigthOf + " " + MidOfWorld;

        public const string XCoordinate = "X-coordinate (in Tiles)";
        public const string YCoordinate = "Y-coordinate (in Tiles)";


        public const string CloserToEndOfWorld = "closer to end of world (ocean) as";

        public const string ContainsItem = "Contains item";        
        public const string ContainsItemParam1 = "stack size";// ≥

        public const string ContainsModifier = "Contains item with modifier"; 

        public const string ContainsItemID = "Contains item ID";        
        public const string ContainsItemIDParam1 = "stack size";// ≥
        public const string IsLocked = "Is locked";
        public const string ChestType = "Chest Type";
                
        public const string RNGNG = "random number";

        public const string RNGN1 = "1st "+RNGNG;
        public const string RNGN2 = "2nd "+RNGNG;
        public const string RNGN3 = "3rd "+RNGNG + " (approx 3e-8)";
        public const string RNGN4 = "4th "+RNGNG;
        public const string RNGN5 = "5th "+RNGNG;
        public const string RNGN6 = "6th "+RNGNG + " (approx 1e-9)";
        public const string RNGN7 = "7th "+RNGNG;        
        public const string RNGN8 = "8th "+RNGNG;     
        public const string RNGN9 = "9th "+RNGNG;     
        public const string RNGNidelta = "ith + delta "+RNGNG;  
        public const string RNGNideltaParam1 = "scale min";  
        public const string RNGNideltaParam2 = "scale max";  
        public const string RNGNideltaParam3 = "delta i";
        public const string RNGNiParameters = "Add search parameters to find";
        public const string RNGNiParametersParam1 = "start index";
        public const string RNGNiParametersParam2 = "step size";
        public const string RNGNiParametersParam3 = "max index";


        public const string MoonType = "Moon type";
        public const string DungeonColor = "Dungeon color";
        public const string DungeonSide = "Dungeon side";
        public const string DesertStyleGuess = "Guess Desert style";
        public const string ExtensionDesertOasis = "Local bonus: Oasis nearby";
        


        public const string ExtensionDesertPotentialPyramid = "Local bonus: Potential Pyramid nearby (early gen.)";
        public const string ExtensionDesertlPyramid = "Local bonus Pyramid nearby";
        
        public const string HardmodeHallowSide = "Hardmode Hallow side";
        public const string HardmodeHallowAtJungleSnow = "Hallow Biome at Dungeon or Jungle side";
        public const string OreCopperOrTin = "T1: Copper or Tin Ore";
        public const string OreIronOrLead = "T2: Iron or Lead Ore";
        public const string OreSilverOrTungsten = "T3: Silver or Tungsten Ore";
        public const string OreGoldOrPlatinum = "T4: Gold or Platinum Ore";
        public const string UndergroundLayerSize = "Underground layer size";
        public const string CavernLayerSize = "Cavern layer size";
        public const string UndergroundLayerTilesFromTopWorld = "Tiles top of world to Underground layer";
        public const string CavernLayerTilesFromTopWorld = "Tiles top of world to Cavern layer";
        public const string UndergroundLayerTilesToSkyNPCSpawn = "Start of sky NPC spawn above Underground layer";//todo replace
        public const string SkyNPCSpawn = "start of sky NPC spawn";
        public const string WorldSurfaceLow = "highest surface altitude (worldSurfaceLow, mountain can be higher)";
        public const string LavaLineDepth = "Tiles bottom to lava line";
        public const string GlowingMossType = "Glowing moss type";
        public const string JungleShrineType = "Jungle shrine type";
        public const string OceanCaveDungeon = "Has Ocean Cave on Dungeon side";

        public const string SalamanderGiantShellyCrawdad_Salamander = "Salamander style";
        public const string SalamanderGiantShellyCrawdad_GiantShelly = "Giant Shelly style";
        public const string SalamanderGiantShellyCrawdad_Crawdad = "Crawdad style";

        public const string SalamanderGiantShellyCrawdad_Extinct = "Is extinct";
        public const string SalamanderGiantShellyCrawdad_UniqueCount = "Unique style count";

        public const string TreeStyle = "Has Forest tree style";
        public const string TreeStyle1 = "1st Forest tree style";
        public const string TreeStyle2 = "2nd Forest tree style";
        public const string TreeStyle3 = "3rd Forest tree style (min medium)";
        public const string TreeStyle4 = "4th Forest tree style (min large)";



        public const string BackgroundStyle = "Has background style";

        public const string BackgroundStyleForest1 = "1st Forest background style";
        public const string BackgroundStyleForest2 = "2nd Forest background style";
        public const string BackgroundStyleForest3 = "3rd Forest background style (min medium)";
        public const string BackgroundStyleForest4 = "4th Forest background style (min large)";


        public const string BackgroundStyleForestChange1 = "1st Forest BG transition at %";
        public const string BackgroundStyleForestChange2 = "2nd Forest BG transition at % (min medium)";
        public const string BackgroundStyleForestChange3 = "3rd Forest BG transition at % (min large)";
        
        public const string BackgroundStyleCavern1 = "1st Cavern background style";
        public const string BackgroundStyleCavern2 = "2nd Cavern background style";
        public const string BackgroundStyleCavern3 = "3rd Cavern background style (min medium)";
        public const string BackgroundStyleCavern4 = "4th Cavern background style (min large)";


        public const string BackgroundStyleCavernChange1 = "1st Cavern BG transition at %";
        public const string BackgroundStyleCavernChange2 = "2nd Cavern BG transition at % (min medium)";
        public const string BackgroundStyleCavernChange3 = "3rd Cavern BG transition at % (min large)";



        public const string IDTile = "Tile ID";
        public const string IDWall = "Wall ID";
        public const string FrameX = "Frame X";
        public const string FrameY = "Frame Y";

        public const string GraniteMarbleDetonator = "Granite/Marble/Detonator variable";
        public const string LifeCrystalAltarStatue = "Life Crystal/Altar/Statue variable";
        public const string SilverCabinHolesSurfCavesEnd = "Silver/Cabins/Holes variable";
        public const string BuriedChestsStart = "Start of buried chests variable";
        public const string WorldSurface2BuriedChestsStart = "Tiles from start Underground to start buried chest variable";
        

        public const string StatueType = "Statue type";
        public const string ObjectType = "Object type";
        public const string HasTilesNearby = "Has tiles nearby";
        public const string HasTilesAbove = "Has tiles above";
        public const string HasTilesBelow = "Has tiles below";
        public const string HasTilesAboveBelow = "Has tiles above or below";
        public const string HasTilesLeft = "Has tiles left";
        public const string HasTilesRight = "Has tiles right";
        public const string HasTilesLeftRight = "Has tiles left or right";
        public const string radius = "Radius";
        public const string HasTilesNearbyParam1 = radius;

        public const string HasBGWallNearby = "Has background wall nearby";
        public const string HasBGWallNearbyParam1 = radius;

        public const string HasBGWallDifferentNearby = "Has background wall nearby different to";
        public const string HasBGWallDifferentNearbyParam1 = radius;

        public const string dist = "dist";

        public const string HasTilesAboveParam1 = dist;
        public const string HasTilesBelowParam1 = dist;
        public const string HasTilesAboveBelowParam1 = dist;
        public const string HasTilesLeftParam1 = dist;
        public const string HasTilesRightParam1 = dist;
        public const string HasTilesLeftRightParam1 = dist;



        public static string GetParameterNameTextfromFunctionNameText(string name, int paramID)
        {
            foreach (var field in typeof(ConstraintNames).GetFields())
            {
                 
                if (field.FieldType == typeof(string))
                {
                    if ((field.GetValue(null) as string).Equals(name))
                    {
                        name = field.Name;
                        break;
                    }
                }
            }


            string nameParam = (name + ParamAddon + paramID).ToLower();

            foreach (var field in typeof(ConstraintNames).GetFields())
            {                
                if (field.Name.ToLower().Equals(nameParam))
                {
                    return (string)field.GetValue(null);
                }
            }
            return "";
        }

    }
}