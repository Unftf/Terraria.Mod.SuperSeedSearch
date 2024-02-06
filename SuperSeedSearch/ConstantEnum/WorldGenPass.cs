using System.Collections.Generic;



namespace SuperSeedSearch.ConstantEnum
{
    public enum WorldGenPass : int
    {
        PreWorldGen = -1,
        Unknown = 0,
        Reset = 1,
        Terrain,
        Dunes,
        OceanSand,
        SandPatches,
        Tunnels,
        MountCaves,
        DirtWallBackgrounds,
        RocksInDirt,
        DirtInRocks,
        Clay,
        SmallHoles,
        DirtLayerCaves,
        RockLayerCaves,
        SurfaceCaves,
        WavyCaves,
        GenerateIceBiome,
        Grass,
        Jungle,
        MudCavesToGrass,
        FullDesert,
        FloatingIslands,
        MushroomPatches,
        Marble,
        Granite,
        DirtToMud,
        Silt,
        Shinies,
        Webs,
        Underworld,
        Corruption,
        Lakes,
        Dungeon,
        Slush,
        MountainCaves,
        Beaches,
        Gems,
        GravitatingSand,
        CreateOceanCaves,
        Shimmer,
        CleanUpDirt,
        Pyramids,
        DirtRockWallRunner,
        LivingTrees,
        WoodTreeWalls,
        Altars,
        WetJungle,
        JungleTemple,
        Hives,
        JungleChests,
        SettleLiquids,
        RemoveWaterFromSand,
        Oasis,
        ShellPiles,
        SmoothWorld,
        Waterfalls,
        Ice,
        WallVariety,
        LifeCrystals,
        Statues,
        BuriedChests,
        SurfaceChests,
        JungleChestsPlacement,
        WaterChests,
        SpiderCaves,
        GemCaves,
        Moss,
        Temple,
        CaveWalls,
        JungleTrees,
        FloatingIslandHouses,
        QuickCleanup,
        Pots,
        Hellforge,
        SpreadingGrass,
        SurfaceOreandStone,
        PlaceFallenLog,
        Traps,
        Piles,
        SpawnPoint,
        GrassWall,
        Guide,
        Sunflowers,
        PlantingTrees,
        Herbs,
        DyePlants,
        WebsAndHoney,
        Weeds,
        GlowingMushroomsandJunglePlants,
        JunglePlants,
        Vines,
        Flowers,
        Mushrooms,
        GemsInIceBiome,
        RandomGems,
        MossGrass,
        MudsWallsInJungle,
        Larva,
        SettleLiquidsAgain,
        CactusPalmTreesCoral,
        TileCleanup,
        LihzahrdAltars,
        MicroBiomes,
        WaterPlants,
        Stalac,
        RemoveBrokenTraps,
        FinalCleanup,

        PostWorldGen,

        

    }

    public static class WorldGenPassDict
    {
        public static Dictionary<string, WorldGenPass> worldGenPassDict = new Dictionary<string, WorldGenPass>{            
            {ConstantEnum.WorldGenPassName.Reset  , WorldGenPass.Reset},
            {ConstantEnum.WorldGenPassName.Terrain   , WorldGenPass.Terrain},
            {ConstantEnum.WorldGenPassName.Dunes , WorldGenPass.Dunes},
            {ConstantEnum.WorldGenPassName.OceanSand , WorldGenPass.OceanSand},
            {ConstantEnum.WorldGenPassName.SandPatches   , WorldGenPass.SandPatches},
            {ConstantEnum.WorldGenPassName.Tunnels   , WorldGenPass.Tunnels},
            {ConstantEnum.WorldGenPassName.MountCaves    , WorldGenPass.MountCaves},
            {ConstantEnum.WorldGenPassName.DirtWallBackgrounds   , WorldGenPass.DirtWallBackgrounds},
            {ConstantEnum.WorldGenPassName.RocksInDirt   , WorldGenPass.RocksInDirt},
            {ConstantEnum.WorldGenPassName.DirtInRocks   , WorldGenPass.DirtInRocks},
            {ConstantEnum.WorldGenPassName.Clay  , WorldGenPass.Clay},
            {ConstantEnum.WorldGenPassName.SmallHoles    , WorldGenPass.SmallHoles},
            {ConstantEnum.WorldGenPassName.DirtLayerCaves    , WorldGenPass.DirtLayerCaves},
            {ConstantEnum.WorldGenPassName.RockLayerCaves    , WorldGenPass.RockLayerCaves},
            {ConstantEnum.WorldGenPassName.SurfaceCaves  , WorldGenPass.SurfaceCaves},
            {ConstantEnum.WorldGenPassName.WavyCaves  , WorldGenPass.WavyCaves},
            {ConstantEnum.WorldGenPassName.GenerateIceBiome  , WorldGenPass.GenerateIceBiome},
            {ConstantEnum.WorldGenPassName.Grass , WorldGenPass.Grass},
            {ConstantEnum.WorldGenPassName.Jungle    , WorldGenPass.Jungle},
            {ConstantEnum.WorldGenPassName.MudCavesToGrass   , WorldGenPass.MudCavesToGrass},
            {ConstantEnum.WorldGenPassName.FullDesert    , WorldGenPass.FullDesert},
            {ConstantEnum.WorldGenPassName.FloatingIslands   , WorldGenPass.FloatingIslands},
            {ConstantEnum.WorldGenPassName.MushroomPatches   , WorldGenPass.MushroomPatches},
            {ConstantEnum.WorldGenPassName.Marble    , WorldGenPass.Marble},
            {ConstantEnum.WorldGenPassName.Granite   , WorldGenPass.Granite},
            {ConstantEnum.WorldGenPassName.DirtToMud , WorldGenPass.DirtToMud},
            {ConstantEnum.WorldGenPassName.Silt  , WorldGenPass.Silt},
            {ConstantEnum.WorldGenPassName.Shinies   , WorldGenPass.Shinies},
            {ConstantEnum.WorldGenPassName.Webs  , WorldGenPass.Webs},
            {ConstantEnum.WorldGenPassName.Underworld    , WorldGenPass.Underworld},
            {ConstantEnum.WorldGenPassName.Corruption    , WorldGenPass.Corruption},
            {ConstantEnum.WorldGenPassName.Lakes , WorldGenPass.Lakes},
            {ConstantEnum.WorldGenPassName.Dungeon   , WorldGenPass.Dungeon},
            {ConstantEnum.WorldGenPassName.Slush , WorldGenPass.Slush},
            {ConstantEnum.WorldGenPassName.MountainCaves , WorldGenPass.MountainCaves},
            {ConstantEnum.WorldGenPassName.Beaches   , WorldGenPass.Beaches},
            {ConstantEnum.WorldGenPassName.Gems  , WorldGenPass.Gems},
            {ConstantEnum.WorldGenPassName.GravitatingSand   , WorldGenPass.GravitatingSand},
            {ConstantEnum.WorldGenPassName.CreateOceanCaves  , WorldGenPass.CreateOceanCaves},
            {ConstantEnum.WorldGenPassName.Shimmer   , WorldGenPass.Shimmer},            
            {ConstantEnum.WorldGenPassName.CleanUpDirt   , WorldGenPass.CleanUpDirt},            
            {ConstantEnum.WorldGenPassName.Pyramids  , WorldGenPass.Pyramids},
            {ConstantEnum.WorldGenPassName.DirtRockWallRunner    , WorldGenPass.DirtRockWallRunner},
            {ConstantEnum.WorldGenPassName.LivingTrees   , WorldGenPass.LivingTrees},
            {ConstantEnum.WorldGenPassName.WoodTreeWalls , WorldGenPass.WoodTreeWalls},
            {ConstantEnum.WorldGenPassName.Altars    , WorldGenPass.Altars},
            {ConstantEnum.WorldGenPassName.WetJungle , WorldGenPass.WetJungle},
            {ConstantEnum.WorldGenPassName.JungleTemple  , WorldGenPass.JungleTemple},
            {ConstantEnum.WorldGenPassName.Hives , WorldGenPass.Hives},
            {ConstantEnum.WorldGenPassName.JungleChests  , WorldGenPass.JungleChests},
            {ConstantEnum.WorldGenPassName.SettleLiquids , WorldGenPass.SettleLiquids},
            {ConstantEnum.WorldGenPassName.RemoveWaterFromSand   , WorldGenPass.RemoveWaterFromSand},
            {ConstantEnum.WorldGenPassName.Oasis , WorldGenPass.Oasis},
            {ConstantEnum.WorldGenPassName.ShellPiles    , WorldGenPass.ShellPiles},
            {ConstantEnum.WorldGenPassName.SmoothWorld   , WorldGenPass.SmoothWorld},
            {ConstantEnum.WorldGenPassName.Waterfalls    , WorldGenPass.Waterfalls},
            {ConstantEnum.WorldGenPassName.Ice   , WorldGenPass.Ice},
            {ConstantEnum.WorldGenPassName.WallVariety   , WorldGenPass.WallVariety},
            {ConstantEnum.WorldGenPassName.LifeCrystals  , WorldGenPass.LifeCrystals},
            {ConstantEnum.WorldGenPassName.Statues   , WorldGenPass.Statues},
            {ConstantEnum.WorldGenPassName.BuriedChests  , WorldGenPass.BuriedChests},
            {ConstantEnum.WorldGenPassName.SurfaceChests , WorldGenPass.SurfaceChests},
            {ConstantEnum.WorldGenPassName.JungleChestsPlacement , WorldGenPass.JungleChestsPlacement},
            {ConstantEnum.WorldGenPassName.WaterChests   , WorldGenPass.WaterChests},
            {ConstantEnum.WorldGenPassName.SpiderCaves   , WorldGenPass.SpiderCaves},
            {ConstantEnum.WorldGenPassName.GemCaves  , WorldGenPass.GemCaves},
            {ConstantEnum.WorldGenPassName.Moss  , WorldGenPass.Moss},
            {ConstantEnum.WorldGenPassName.Temple    , WorldGenPass.Temple},
            {ConstantEnum.WorldGenPassName.CaveWalls , WorldGenPass.CaveWalls},
            {ConstantEnum.WorldGenPassName.JungleTrees   , WorldGenPass.JungleTrees},
            {ConstantEnum.WorldGenPassName.FloatingIslandHouses  , WorldGenPass.FloatingIslandHouses},
            {ConstantEnum.WorldGenPassName.QuickCleanup  , WorldGenPass.QuickCleanup},
            {ConstantEnum.WorldGenPassName.Pots  , WorldGenPass.Pots},
            {ConstantEnum.WorldGenPassName.Hellforge , WorldGenPass.Hellforge},
            {ConstantEnum.WorldGenPassName.SpreadingGrass    , WorldGenPass.SpreadingGrass},
            {ConstantEnum.WorldGenPassName.SurfaceOreandStone    , WorldGenPass.SurfaceOreandStone},
            {ConstantEnum.WorldGenPassName.PlaceFallenLog    , WorldGenPass.PlaceFallenLog},
            {ConstantEnum.WorldGenPassName.Traps , WorldGenPass.Traps},
            {ConstantEnum.WorldGenPassName.Piles , WorldGenPass.Piles},
            {ConstantEnum.WorldGenPassName.SpawnPoint    , WorldGenPass.SpawnPoint},
            {ConstantEnum.WorldGenPassName.GrassWall , WorldGenPass.GrassWall},
            {ConstantEnum.WorldGenPassName.Guide , WorldGenPass.Guide},
            {ConstantEnum.WorldGenPassName.Sunflowers    , WorldGenPass.Sunflowers},
            {ConstantEnum.WorldGenPassName.PlantingTrees , WorldGenPass.PlantingTrees},
            {ConstantEnum.WorldGenPassName.Herbs , WorldGenPass.Herbs},
            {ConstantEnum.WorldGenPassName.DyePlants , WorldGenPass.DyePlants},
            {ConstantEnum.WorldGenPassName.WebsAndHoney  , WorldGenPass.WebsAndHoney},
            {ConstantEnum.WorldGenPassName.Weeds , WorldGenPass.Weeds},
            {ConstantEnum.WorldGenPassName.GlowingMushroomsandJunglePlants   , WorldGenPass.GlowingMushroomsandJunglePlants},
            {ConstantEnum.WorldGenPassName.JunglePlants  , WorldGenPass.JunglePlants},
            {ConstantEnum.WorldGenPassName.Vines , WorldGenPass.Vines},
            {ConstantEnum.WorldGenPassName.Flowers   , WorldGenPass.Flowers},
            {ConstantEnum.WorldGenPassName.Mushrooms , WorldGenPass.Mushrooms},
            {ConstantEnum.WorldGenPassName.GemsInIceBiome    , WorldGenPass.GemsInIceBiome},
            {ConstantEnum.WorldGenPassName.RandomGems    , WorldGenPass.RandomGems},
            {ConstantEnum.WorldGenPassName.MossGrass , WorldGenPass.MossGrass},
            {ConstantEnum.WorldGenPassName.MudsWallsInJungle , WorldGenPass.MudsWallsInJungle},
            {ConstantEnum.WorldGenPassName.Larva , WorldGenPass.Larva},
            {ConstantEnum.WorldGenPassName.SettleLiquidsAgain    , WorldGenPass.SettleLiquidsAgain},
            {ConstantEnum.WorldGenPassName.CactusPalmTreesCoral   , WorldGenPass.CactusPalmTreesCoral},
            {ConstantEnum.WorldGenPassName.TileCleanup   , WorldGenPass.TileCleanup},
            {ConstantEnum.WorldGenPassName.LihzahrdAltars    , WorldGenPass.LihzahrdAltars},
            {ConstantEnum.WorldGenPassName.MicroBiomes   , WorldGenPass.MicroBiomes},
            {ConstantEnum.WorldGenPassName.WaterPlants   , WorldGenPass.WaterPlants},
            {ConstantEnum.WorldGenPassName.Stalac    , WorldGenPass.Stalac},
            {ConstantEnum.WorldGenPassName.RemoveBrokenTraps , WorldGenPass.RemoveBrokenTraps},
            {ConstantEnum.WorldGenPassName.FinalCleanup  , WorldGenPass.FinalCleanup}
            };

            public const string   WGPassCounterIdentifier = "#";
            public const string   WGPassPosWorldGenIdentifier = "END";
            public const string   WGPassPreWorldGenIdentifier = "-01";
            public const int   WGPassIdentifierLength = 4;

            public static string AsPreConstraintText(WorldGenPass val){
                int ival = (int)val ;                
                string asString = val switch {
                      WorldGenPass.PreWorldGen => WGPassPreWorldGenIdentifier     ,
                      WorldGenPass.PostWorldGen => WGPassPosWorldGenIdentifier     ,
                    _ => (ival.ToString()).PadLeft( (ival<0?2:3), '0')
                };
                return WGPassCounterIdentifier + asString ;
            }

            public static bool IsWGNameTargetWorldGenPassID(string name, WorldGenPass targetWGP){
                return ConstantEnum.WorldGenPassDict.worldGenPassDict.ContainsKey(name) &&  ConstantEnum.WorldGenPassDict.worldGenPassDict[name] == targetWGP; 
            }

    }






}