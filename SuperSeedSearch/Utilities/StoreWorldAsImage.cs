using Terraria.ModLoader;
using Terraria;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Map;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ReLogic.Content;
using Terraria.Utilities;
using SuperSeedSearch.UI;
using Terraria.Cinematics;
using Terraria.WorldBuilding;
using SuperSeedSearch.PropertyCondition.PCObject;




//TODO refactor, old legacy code
namespace SuperSeedSearch.StoreWorldAsImage
{

    public static class StoreWorldAsImage
    {

        public static void StoreCurrent(ConstantEnum.StoreAsPicture StoreWorldAsPicture, bool debug=false)
        {    
            HashSet<Tuple<int,int,int>> chestItemList = new HashSet<Tuple<int,int,int>>(); //Todo own chestitem version
            
            if(StoreWorldAsPicture == ConstantEnum.StoreAsPicture.PictureDefault || StoreWorldAsPicture == ConstantEnum.StoreAsPicture.PictureQueriedOnly || StoreWorldAsPicture == ConstantEnum.StoreAsPicture.PictureRare)
                InsertMatchingChestItems(ref chestItemList );


            if(StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureNoIcons && StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureQueriedOnly ){
                CheckChests(ref chestItemList, StoreWorldAsPicture == ConstantEnum.StoreAsPicture.PictureRare);
                CheckTiles(ref chestItemList, StoreWorldAsPicture == ConstantEnum.StoreAsPicture.PictureRare);
            }

            
            StoreMapAsPNG(ref chestItemList, StoreWorldAsPicture, debug);       
        }

        private static void StoreMapAsPNG(ref HashSet<Tuple<int,int,int>> chestItemList, ConstantEnum.StoreAsPicture StoreWorldAsPicture, bool debug=false)
        {

            int dimX = Main.maxTilesX;
            int dimY = Main.maxTilesY;
            int scale = 1;
            /*while (dimX > 6200)
            {
                dimX /= 2;
                dimY /= 2;
                scale *= 2;
            }*/

            int bytes = dimX * dimY * 4;

            byte[] rgbValues = new byte[bytes];


            //todo in main loop ? (not needed there)
            List<Tuple<int, int>> hearts = new List<Tuple<int, int>>();
            List<Tuple<int, int>> demonAltars = new List<Tuple<int, int>>();
            List<Tuple<int, int>> rubies = new List<Tuple<int, int>>();
            List<Tuple<int, int>> diamonds = new List<Tuple<int, int>>();
            List<Tuple<int, int>> explosive = new List<Tuple<int, int>>();
            List<Tuple<int, int>> evilOre = new List<Tuple<int, int>>();
            List<Tuple<int, int>> plantBulbs = new List<Tuple<int, int>>();
            List<Tuple<int, int>> beds = new List<Tuple<int, int>>();
            List<Tuple<int, int>> waterBolt = new List<Tuple<int, int>>();
         

            int spawnbl = ((Main.maxTilesX / 5) - 100) / scale;
            int spawnbr = spawnbl + 200;
            int hbt = (int)Main.worldSurface - ((90 * Main.maxTilesY) / 1200) / scale;
            int hbb = (int)Main.worldSurface - ((40 * Main.maxTilesY) / 1200) / scale;
            int indx = 0;
            int surflc = 8;
            int spwnHarpLc = 8;
            for (int y = 0; y < Main.maxTilesY; y += scale)
                for (int x = 0; x < Main.maxTilesX; x += scale)
                {
                    MapTile cur = MapHelper.CreateMapTile(x, y, 255);
                    Color cc = MapHelper.GetMapTileXnaColor(ref cur);

                    if (x > Main.offLimitBorderTiles && x < Main.maxTilesX - Main.offLimitBorderTiles && y > Main.offLimitBorderTiles && y < Main.maxTilesY - Main.offLimitBorderTiles)
                    {
                        if (Main.tile[x, y].TileType == TileID.Heart && Main.tile[x, y].TileFrameX == 0 && Main.tile[x, y].TileFrameY == 0)
                            hearts.Add(new Tuple<int, int>(x, y));
                        else if (Main.tile[x, y].TileType == TileID.DemonAltar && Main.tile[x, y].TileFrameY == 0 && (
                            (Main.tile[x, y].TileFrameX == 18 && Main.tile[x, y].WallType != WallID.EbonstoneUnsafe && Main.tile[x, y].WallType != WallID.CorruptGrassUnsafe && Main.tile[x, y + 2].TileType != TileID.Ebonstone && Main.tile[x - 1, y + 2].TileType != TileID.Ebonstone && Main.tile[x + 1, y + 2].TileType != TileID.Ebonstone) ||
                            (Main.tile[x, y].TileFrameX == 72 && Main.tile[x, y].WallType != WallID.CrimstoneUnsafe && Main.tile[x, y].WallType != WallID.CrimsonGrassUnsafe && Main.tile[x, y + 2].TileType != TileID.Crimstone && Main.tile[x - 1, y + 2].TileType != TileID.Crimstone && Main.tile[x + 1, y + 2].TileType != TileID.Crimstone)
                            ))
                            demonAltars.Add(new Tuple<int, int>(x, y));
                        else if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == TileID.Ruby || (Main.tile[x, y].TileType == TileID.ExposedGems && Main.tile[x, y].TileFrameX == 72) || (Main.tile[x, y].TileType == TileID.SmallPiles && Main.tile[x, y].TileFrameX == 828 && Main.tile[x, y].TileFrameY == 18))
                            rubies.Add(new Tuple<int, int>(x, y));
                        else if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == TileID.Diamond || (Main.tile[x, y].TileType == TileID.ExposedGems && Main.tile[x, y].TileFrameX == 90) || (Main.tile[x, y].TileType == TileID.SmallPiles && Main.tile[x, y].TileFrameX == 864 && Main.tile[x, y].TileFrameY == 18))
                            diamonds.Add(new Tuple<int, int>(x, y));
                        else if (Main.tile[x, y].HasTile && Main.tile[x, y].TileType == TileID.Explosives)
                            explosive.Add(new Tuple<int, int>(x, y));
                        else if (Main.tile[x, y].HasTile && y < Main.worldSurface && (Main.tile[x, y].TileType == TileID.Demonite || Main.tile[x, y].TileType == TileID.Crimtane))
                            evilOre.Add(new Tuple<int, int>(x, y));   
                        else if(Main.tile[x, y].TileType == TileID.PlanteraBulb && Main.tile[x, y].TileFrameX == 0 && Main.tile[x, y].TileFrameY == 0 )
                            plantBulbs.Add(new Tuple<int, int>(x, y));   
                        else if(Main.tile[x, y].TileType == TileID.Beds && Main.tile[x, y-1].TileType != TileID.Beds && Main.tile[x+3, y].TileType != TileID.Beds && Main.tile[x-2, y].TileType != TileID.Beds )
                            beds.Add(new Tuple<int, int>(x, y));  
                        else if (Main.tile[x, y].TileType == TileID.Books && Main.tile[x, y].TileFrameX == 90 )  
                            waterBolt.Add(new Tuple<int, int>(x, y));  
                    }

                    rgbValues[indx++] = cc.B;
                    rgbValues[indx++] = cc.G;
                    rgbValues[indx++] = cc.R;
                    rgbValues[indx++] = cc.A;

                    if ((x == spawnbl || x == spawnbr) && y < hbb && y > hbt)
                    {
                        rgbValues[indx - 4] = (byte)(128 + (rgbValues[indx - 4] >> 1));
                        rgbValues[indx - 3] = (byte)(128 + (rgbValues[indx - 3] >> 1));
                        rgbValues[indx - 2] = (byte)(128 + (rgbValues[indx - 2] >> 1));
                    }
                    if (y == (int)Main.worldSurface && (surflc--) < 3)
                    {
                        rgbValues[indx - 4] = (byte)(128 + (rgbValues[indx - 4] >> 1));
                        rgbValues[indx - 3] = (byte)(128 + (rgbValues[indx - 3] >> 1));
                        rgbValues[indx - 2] = (byte)(128 + (rgbValues[indx - 2] >> 1));
                        if (surflc == 0) surflc = 8;
                    }
                    if (y == (int)(Main.worldSurface * 0.45 + 47) && (spwnHarpLc--) < 3)
                    {
                        rgbValues[indx - 4] = (byte)(128 + (rgbValues[indx - 4] >> 1));
                        rgbValues[indx - 3] = (byte)(128 + (rgbValues[indx - 3] >> 1));
                        rgbValues[indx - 2] = (byte)(128 + (rgbValues[indx - 2] >> 1));
                        if (spwnHarpLc == 0) spwnHarpLc = 8;
                    }
                    if(Main.tile[x, y].RedWire || Main.tile[x, y].GreenWire || Main.tile[x, y].BlueWire || Main.tile[x, y].YellowWire){
                        rgbValues[indx - 4] = (byte)(0 + (rgbValues[indx - 4] ));
                        rgbValues[indx - 3] = (byte)(0 + (rgbValues[indx - 3] ));
                        rgbValues[indx - 2] = (byte)(64 + ((rgbValues[indx - 2] >> 1) + (rgbValues[indx - 2] >> 2)));
                    }
                }

            //draw Spawm
            int aw = 0;

            if (Main.spawnTileX > Main.offLimitBorderTiles && Main.spawnTileY > Main.offLimitBorderTiles && Main.spawnTileX < Main.maxTilesX - Main.offLimitBorderTiles && Main.spawnTileY < Main.maxTilesY - Main.offLimitBorderTiles)
                for (int y = Main.spawnTileY - 1; y > Main.spawnTileY - 36; y--)
                {
                    int x = Main.spawnTileX;
                    int off = y * 4 * Main.maxTilesX + x * 4;

                    rgbValues[off + 0] = 0;
                    rgbValues[off + 1] = 80;
                    rgbValues[off + 2] = 50;
                    rgbValues[off + 3] = 255;

                    for (int awi = 0; awi < (aw < 18 ? aw : 4); awi++)
                    {
                        rgbValues[off + 0 + 4 * (awi / 3)] = 0;
                        rgbValues[off + 1 + 4 * (awi / 3)] = 80;
                        rgbValues[off + 2 + 4 * (awi / 3)] = 50;
                        rgbValues[off + 3 + 4 * (awi / 3)] = 255;

                        rgbValues[off + 0 - 4 * (awi / 3)] = 0;
                        rgbValues[off + 1 - 4 * (awi / 3)] = 80;
                        rgbValues[off + 2 - 4 * (awi / 3)] = 50;
                        rgbValues[off + 3 - 4 * (awi / 3)] = 255;
                    }
                    aw++;
                }


            if (true)
            {
                foreach (var altars in demonAltars)
                {
                    DrawCircle(ref rgbValues, altars, scale, new Color(50, 150, 255));
                }
                foreach (var heart in hearts)
                {
                    DrawCircle(ref rgbValues, heart, scale, new Color(255, 0, 255));
                }

                {
                    bool rareOnly = StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureRare;
                for (int ci = 0; ci < Main.maxChests; ci++)
                {
                    if (Main.chest[ci] != null && Main.chest[ci].x > Main.offLimitBorderTiles && Main.chest[ci].y > Main.offLimitBorderTiles && Main.chest[ci].x < Main.maxTilesX - Main.offLimitBorderTiles && Main.chest[ci].item.Length > 0 && Main.chest[ci].item[0] != null && Main.chest[ci].item[0].active && Main.chest[ci].item[0].stack > 0)
                    {
                        if (Main.chest[ci].y < Main.worldSurface + (rareOnly?15:150) && Main.tile[Main.chest[ci].x, Main.chest[ci].y].TileType == TileID.Containers2 && Main.tile[Main.chest[ci].x, Main.chest[ci].y].TileFrameX / 36 == 4)
                        {
                            ///	DrawCircle(ref rgbValues, new Tuple<int,int>(Main.chest[ci].x, Main.chest[ci].y), scale, new Color(45, 228, 24));      
                            if(StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureNoIcons && StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureQueriedOnly)
                            if( StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureRare ||  ( Main.chest[ci].y < Main.worldSurface+50 && (Math.Abs(Main.chest[ci].x-Main.maxTilesX/2)<250 ||   Math.Abs(Main.chest[ci].x-GenVars.leftBeachEnd)<250  ||   Math.Abs(Main.chest[ci].x-GenVars.rightBeachStart)<250 )) )
                                DrawItemImage(ref rgbValues, ItemID.DeadMansChest, Main.chest[ci].x, Main.chest[ci].y, scale);
                        }
                        DrawCircle(ref rgbValues, new Tuple<int, int>(Main.chest[ci].x, Main.chest[ci].y), scale, new Color(245, 228, 24));
                    }
                }}

                foreach (var diamond in diamonds)
                {
                    DrawCircle(ref rgbValues, diamond, scale, new Color(200, 200, 200), 5);
                }
                foreach (var diamond in diamonds)
                {
                    DrawCircle(ref rgbValues, diamond, scale, new Color(230, 230, 230), 3);
                }
                foreach (var ruby in rubies)
                {
                    DrawCircle(ref rgbValues, ruby, scale, new Color(255, 0, 180), 5);
                }
                foreach (var ruby in rubies)
                {
                    DrawCircle(ref rgbValues, ruby, scale, new Color(255, 0, 0), 3);
                }

                foreach (var expl in explosive)
                {
                    DrawCircle(ref rgbValues, expl, scale, new Color(255, 240, 0), 3);
                    DrawCircle(ref rgbValues, expl, scale, new Color(255, 255, 160), 5);
                }
                foreach (var evilo in evilOre)
                {
                    DrawCircle(ref rgbValues, evilo, scale, new Color(255, 80, 30), 3);
                    DrawCircle(ref rgbValues, evilo, scale, new Color(255, 127, 39), 5);
                }

                //if(RandomNumberAnalyzer.RandomNumberAnalyzer.GetQuickBulbListIfExists()!=null)
                foreach (var bulb in RandomNumberAnalyzer.RandomNumberAnalyzer.GetQuickBulbListIfExists()){
                    
                    DrawCircle(ref rgbValues, new Tuple<int,int>(bulb.Item1,bulb.Item2) , scale, new Color(225, 128, 206),9);
                    if(bulb.Item3 < 20000)
                        DrawCircle(ref rgbValues, new Tuple<int,int>(bulb.Item1,bulb.Item2) , scale, new Color(225, 128, 206),13);
                    if(RandomNumberAnalyzer.RandomNumberAnalyzer.IsPosAqBulb(bulb.Item1, bulb.Item2, (int)(500*RandomNumberAnalyzer.RandomNumberAnalyzer.BulbPredictionTickCount*((double)Main.maxTilesX/4200)), true)){
                        if(StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureNoIcons && StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureQueriedOnly ) 
                            DrawItemImage(ref rgbValues, ItemID.PlanteraTrophy, bulb.Item1, bulb.Item2, scale);
                    }
                }
                foreach (var wb in waterBolt)
                {
                    DrawCircle(ref rgbValues, wb, scale, new Color(164, 164, 250));                                     
                }

                if(StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureNoIcons && StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureQueriedOnly ) 
                foreach (var bulb in plantBulbs)
                {
                    
                    //DrawCircle(ref rgbValues, bulb, scale, new Color(225, 128, 206));     
                    DrawItemImage(ref rgbValues, ItemID.PlanteraPetItem, bulb.Item1, bulb.Item2, scale);  
                    //Helpers.Helpers.writeDebugFile(" bulb grown at "  +bulb.Item1 + " " + bulb.Item2 + " on " + ModMenuMod.wGPassChanger.currentSeed);
                }
                foreach (var bed in beds)
                {
                    DrawCircle(ref rgbValues, bed, scale, new Color(40, 130, 230));                                     
                }

            }

            if(StoreWorldAsPicture != ConstantEnum.StoreAsPicture.PictureNoIcons) 
            foreach (Tuple<int,int,int> ci in chestItemList)
            {
                DrawItemImage(ref rgbValues, ci.Item1, ci.Item2, ci.Item3, scale);
            }


            DrawSeed(ref rgbValues);

            string filename = "";
            if(!debug)
                filename= Main.worldPathName.Substring(0, Main.worldPathName.Length - 4)+".png";
            else if(debug)
                filename = Main.WorldPath + Path.DirectorySeparatorChar + ((int)ModMenuMod.wGPassChanger.lastWorldGenPassID )+"."+ ModMenuMod.wGPassChanger.lastWorldGenPassID  + ".png";
            
            
            //using FileStream fileStream = File.Create(filename);
            //SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Bgra32> img = SixLabors.ImageSharp.Image.LoadPixelData<SixLabors.ImageSharp.PixelFormats.Bgra32>(rgbValues, Main.maxTilesX, Main.maxTilesY);
            //SixLabors.ImageSharp.ImageExtensions.SaveAsPng((SixLabors.ImageSharp.Image)(object)img, (Stream)fileStream);
            //fileStream.Close();

            for(int bi=0;bi<bytes;bi+=4)(rgbValues[bi], rgbValues[bi+2]) = (rgbValues[bi+2], rgbValues[bi]); //other format now, todo            
            using FileStream stream = File.Create(filename);
		    PlatformUtilities.SavePng(stream, Main.maxTilesX, Main.maxTilesY, Main.maxTilesX, Main.maxTilesY, rgbValues);
            
            rgbValues = null;
            hearts.Clear();
            hearts = null;
            demonAltars.Clear();
            demonAltars = null;
        }
        public static void InsertMatchingChestItems(ref HashSet<Tuple<int,int,int>> chestItemList){
            if (!Storage.WorldInfo.HasKey(ConstantEnum.Statistics.FoundMatchingChestItems)) return;

            List<  Tuple<string, int, int, int>  > matchChests = (List<  Tuple<string, int, int, int>  > ) Storage.WorldInfo.Info[ConstantEnum.Statistics.FoundMatchingChestItems] ;

            if(matchChests==null) return;
            foreach(var ci in matchChests){
                if (ci==null) continue;
                chestItemList.Add(new Tuple<int, int, int>(ci.Item2,ci.Item3,ci.Item4) );
            }

        }

        public static void CheckChests(ref HashSet<Tuple<int,int,int>> chestItemList, bool rareOnly)
        {

            List<ChestItem> items2check = new List<ChestItem>();

            items2check.Add(new ChestItem(ItemID.CreativeWings));
            items2check.Add(new ChestItem(ItemID.PharaohsMask));
            items2check.Add(new ChestItem(ItemID.FlyingCarpet));
            items2check.Add(new ChestItem(ItemID.SandstorminaBottle));                        
            items2check.Add(new ChestItem(ItemID.LuckyHorseshoe));
            items2check.Add(new ChestItem(ItemID.CelestialMagnet));   
            items2check.Add(new ChestItem(ItemID.ShinyRedBalloon));
            //items2check.Add(new ChestItem(ItemID.SkyMill));
            items2check.Add(new ChestItem(ItemID.Starfury));

            items2check.Add(new ChestItem(ItemID.ShadowKey, maxDepth: (int)(Main.worldSurface + 16)));//+6
            items2check.Add(new ChestItem(ItemID.GoldenKey, onlyUnlocked:true, maxDepth: (int)(Main.worldSurface + 8)));//8

            items2check.Add(new ChestItem(ItemID.PiranhaGun, onlyUnlocked:true));
            items2check.Add(new ChestItem(ItemID.RainbowGun, onlyUnlocked:true));
            items2check.Add(new ChestItem(ItemID.VampireKnives, onlyUnlocked:true));
            items2check.Add(new ChestItem(ItemID.StormTigerStaff, onlyUnlocked:true));
            items2check.Add(new ChestItem(ItemID.StaffoftheFrostHydra, onlyUnlocked:true));
            items2check.Add(new ChestItem(ItemID.ScourgeoftheCorruptor, onlyUnlocked:true));


            if(rareOnly==false){
                items2check.Add(new ChestItem(ItemID.ShadowKey, maxDepth: (int)(Main.worldSurface + 150), minDepth: (int)(Main.worldSurface + 15)  ));                
                items2check.Add(new ChestItem(ItemID.GoldenKey, onlyUnlocked:true, maxDepth: (int)(Main.worldSurface + 150), minDepth: (int)(Main.worldSurface + 7)  ));                

                items2check.Add(new ChestItem(ItemID.Aglet));
                items2check.Add(new ChestItem(ItemID.AnkletoftheWind));
                items2check.Add(new ChestItem(ItemID.Bomb, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.HermesBoots, 250, 0.4f));
                items2check.Add(new ChestItem(ItemID.FlurryBoots, 250, 0.4f));            
                items2check.Add(new ChestItem(ItemID.IronBar, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.IronskinPotion, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.LeadBar, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.ThrowingKnife, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.MagicConch, 500, 0.5f));//135
                items2check.Add(new ChestItem(ItemID.MagicMirror, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.IceMirror, 120, 0.4f));
                items2check.Add(new ChestItem(ItemID.CatBast));
                    
                items2check.Add(new ChestItem(ItemID.Muramasa));
                items2check.Add(new ChestItem(ItemID.CobaltShield));
                
                
            
                items2check.Add(new ChestItem(ItemID.FeatherfallPotion, 200, 0.4f));
                items2check.Add(new ChestItem(ItemID.GravitationPotion, 200, 0.4f));
                items2check.Add(new ChestItem(ItemID.SpelunkerPotion, 200, 0.4f));
                items2check.Add(new ChestItem(ItemID.SuspiciousLookingEye, 200, 0.4f));
                items2check.Add(new ChestItem(ItemID.FlowerBoots));
                items2check.Add(new ChestItem(ItemID.Dynamite, 200, 0.4f));
                items2check.Add(new ChestItem(ItemID.LavaCharm));

                items2check.Add(new ChestItem(ItemID.IceSkates));
                items2check.Add(new ChestItem(ItemID.Boomstick));
                items2check.Add(new ChestItem(ItemID.WaterWalkingBoots));
                items2check.Add(new ChestItem(ItemID.FeralClaws));
                items2check.Add(new ChestItem(ItemID.FiberglassFishingPole));
                //items2check.Add(new ChestItem(ItemID.SharkBait));
                items2check.Add(new ChestItem(ItemID.BabyBirdStaff));      
                items2check.Add(new ChestItem(ItemID.SandBoots, 500, 0.5f, maxDepth: (int)(Main.worldSurface + 250)));      
                items2check.Add(new ChestItem(ItemID.MeteoriteBar, Main.maxTilesX, 0.5f, onlyUnlocked: true));
                items2check.Add(new ChestItem(ItemID.TeleportationPotion, 400, 0.5f, onlyUnlocked: true));

                items2check.Add(new ChestItem(ItemID.Seaweed));
                items2check.Add(new ChestItem(ItemID.Fish));
            }






            //check chests
            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];
                if (chest != null && chest.item[0] != null)
                {

                    //check if doubl chest
                    int cx = chest.x;
                    int cy = chest.y;

                    if (chest.x < Main.offLimitBorderTiles || chest.x > Main.maxTilesX - Main.offLimitBorderTiles || chest.y < Main.offLimitBorderTiles || chest.y > Main.maxTilesX - Main.offLimitBorderTiles)
                        continue;

                    for (int l = 0; l < 40; l++)
                    {
                        Item item = chest.item[l];
                        if (item == null) { break; }
                        if(Chest.IsLocked(cx, cy)){
                            //if ( PCObjectConditionList.CheckIfTileIsPartOfBrokenObject()  ) Todo

                        }

                        foreach (ChestItem cit in items2check)
                        {
                            if (item.type == cit.itemID && cit.IsPosCloseToSpawn(cx, cy) && cit.DepthCheck(cy) && (!cit.onlyUnlocked || !Chest.IsLocked(cx, cy)) && (cit.itemID != ItemID.ShadowKey || Main.tile[cx,cy].TileFrameX < 200)  ) // todo exact frame numbers
                            {

                                chestItemList.Add(new Tuple<int,int,int>(item.type, cx, cy));
                            }

                        }


                    }


                }
            }


        }

        public static bool hasLavaSurf = false;
        public static bool hasSnowTree = false;
        public static void CheckTiles(ref HashSet<Tuple<int,int,int>> chestItemList, bool rareOnly)
        {
            List<TileItem> tiles2check = new List<TileItem>();

            tiles2check.Add(new TileItem(TileID.LargePiles2, 936, 18, ItemID.EnchantedSword));            
            tiles2check.Add(new TileItem(TileID.Heart, 0, 0, ItemID.LifeCrystal, rareOnly?40:80));
            tiles2check.Add(new TileItem(TileID.Books, 90, 0, ItemID.WaterBolt, maxDepth: (int)(Main.worldSurface + 8)));          
            tiles2check.Add(new TileItem(TileID.Ruby, -1, -1, ItemID.Ruby, rareOnly?30:80));
            tiles2check.Add(new TileItem(TileID.ExposedGems, 72, -1, ItemID.Ruby, rareOnly?30:80));
            tiles2check.Add(new TileItem(TileID.SmallPiles, 828, 18, ItemID.Ruby, rareOnly?30:80));              
            tiles2check.Add(new TileItem(TileID.SapphireGemspark, -1, -1, ItemID.SapphireGemsparkBlock));   
            

            if(rareOnly==false){
                tiles2check.Add(new TileItem(TileID.DirtiestBlock, -1, -1, ItemID.DirtiestBlock));
                tiles2check.Add(new TileItem(TileID.GlowTulip, 0, 0, ItemID.GlowTulip));
                tiles2check.Add(new TileItem(TileID.CatBast, 0, 0, ItemID.CatBast));
            }

            bool sandF = false;
            bool rubyF = false;
            hasLavaSurf = false;
            hasSnowTree = false;
            for (int x = 10; x < Main.maxTilesX - 10; x++)
            {
                for (int y = 10; y < Main.maxTilesY - 10; y++)
                {
                    var tile = Main.tile[x, y];
                    if (tile == null) continue;
                    if (y < Main.worldSurface && !hasLavaSurf && Main.tile[x, y].LiquidAmount > 0 && (Main.tile[x, y - 1] == null || Main.tile[x, y - 1].LiquidAmount < 255)
                       && Main.tile[x, y].LiquidType == 1)
                    {
                        bool isfree = true;
                        for (int yi = y; yi > y - 14 && isfree && yi > 10; yi--)
                        {
                            if (Main.tile[x, yi] != null && Main.tile[x, yi].HasTile)
                                isfree = false;
                        }
                        hasLavaSurf = isfree || hasLavaSurf;
                    }
                    if ((tile.TileType == TileID.LivingMahogany || tile.TileType == TileID.LivingMahoganyLeaves) && !hasSnowTree)
                    {
                        bool inIce = false;
                        int range = 3;
                        for (int xi = x - range; xi <= x + range && !inIce; xi++)
                            inIce |= Main.tile[xi, y].TileType == TileID.SnowBlock || Main.tile[xi, y].TileType == TileID.IceBlock;
                        for (int yi = y - range; yi <= y + range && !inIce; yi++)
                            inIce |= Main.tile[x, yi].TileType == TileID.SnowBlock || Main.tile[x, yi].TileType == TileID.IceBlock;

                        hasSnowTree = inIce;
                    }


                    if (!tile.HasTile) continue;
                    foreach (TileItem tit in tiles2check)
                    {
                        if (tit.IsSame(tile) && tit.IsPosCloseToSpawn(x, y) && tit.DepthCheck(y))
                        {
                            if (tit.tileID != TileID.Sand || !sandF) sandF = true; else continue;
                            if (tit.tileID != TileID.Ruby || !rubyF) rubyF = true; else continue;

                            chestItemList.Add(new Tuple<int,int,int>(tit.itemID, x, y));


                        }

                    }


                }
            }

        }
        public static void DrawSeed(ref byte[] rgbValues){
            int seed = ModMenuMod.wGPassChanger.currentSeed;
            string ws = ModMenuMod.wGPassChanger.worldSize switch { ConstantEnum.WorldSize.small => "1.", ConstantEnum.WorldSize.medium => "2", _ or ConstantEnum.WorldSize.large => "3." };
            string gm = (Main.GameMode+1)+".";
            string et = (WorldGen.crimson?2:1)+".";
            int x0 = 10;
            int y0 = 0;
            int ssize = 20;
            int crop = 10;
            int x0ff = 0;

            string seeds = ws+gm+et+seed;
            for(int i=0; i< seeds.Length; i++){

                switch(seeds[i])
                {
                    case '0':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue0, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '1':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue1, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '2':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue2, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '3':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue3, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '4':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue4, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '5':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue5, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '6':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue6, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '7':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue7, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '8':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue8, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    case '9':
                        DrawItemImage(ref rgbValues, ItemID.AlphabetStatue9, x0+ssize*i+x0ff, y0, 1, false, crop);
                        break;
                    default:
                        x0ff-= 8;
                        break;
                }               
            }

            int scale = 1; //todo
            int iconbgs = 48;
            for(int yi=y0+1;yi<y0+3;yi++)
            for (int xi = x0+16; xi < 14+x0 + ssize*(seeds.Length)+x0ff ; xi++)
            {
                int offt = (yi+iconbgs-crop-4 ) * Main.maxTilesX * 4 / scale + xi * 2 + xi * 2 - iconbgs / 2 * 4;
                rgbValues[offt + 0] = 103;
                rgbValues[offt + 1] = 56;
                rgbValues[offt + 2] = 56;
                rgbValues[offt + 3] = 255;

            }


        }

        public static void DrawCircle(ref byte[] rgbValues, Tuple<int, int> where, int scale, Color cc, int iconbgs = 17)
        {
            //const int iconbgs = 17;


            int x = where.Item1 / scale;
            int y = where.Item2 / scale - iconbgs / 2;//+1 removed


            int maxoff = rgbValues.Length - 1;


            for (int h = 0; h < iconbgs; h++)
                for (int w = 0; w < iconbgs; w++)
                {
                    int offt = (y + h) * Main.maxTilesX * 4 / scale + x * 4 + w * 4 - iconbgs / 2 * 4;
                    //int imgof = h * iconbgs + w;
                    float val = ((h - iconbgs / 2) * (h - iconbgs / 2) + (w - iconbgs / 2) * (w - iconbgs / 2));
                    bool isWhite = (val <= (iconbgs * iconbgs / 3) && val >= (iconbgs * iconbgs / 6));

                    //rgbValues[offt + 0] = isWhite ? (byte)(((cc.B >> 2) + (cc.B >> 1)) + (rgbValues[offt + 0] >> 2)) : rgbValues[offt + 0];
                    //rgbValues[offt + 1] = isWhite ? (byte)(((cc.G >> 2) + (cc.G >> 1)) + (rgbValues[offt + 0] >> 2)) : rgbValues[offt + 1];
                    //rgbValues[offt + 2] = isWhite ? (byte)(((cc.R >> 2) + (cc.R >> 1)) + (rgbValues[offt + 0] >> 2)) : rgbValues[offt + 2];
                    //rgbValues[offt + 3] = isWhite ? (byte)(((cc.A >> 2) + (cc.A >> 1)) + (rgbValues[offt + 0] >> 2)) : rgbValues[offt + 3];

                    if (offt + 3 > maxoff)
                        continue;
                    
                    rgbValues[offt + 0] = isWhite ? (byte)cc.B : rgbValues[offt + 0];
                    rgbValues[offt + 1] = isWhite ? (byte)cc.G : rgbValues[offt + 1];
                    rgbValues[offt + 2] = isWhite ? (byte)cc.R : rgbValues[offt + 2];
                    rgbValues[offt + 3] = isWhite ? (byte)cc.A : rgbValues[offt + 3];
                }

        }

        
        public static volatile Texture2D cit = null;
        
        public static volatile Color[] tc = null;

        public static void DrawItemImage(ref byte[] rgbValues, int itemID, int px, int py, int scale, bool withBG = true, int crop=0)
        {

            //Texture2D cit = Main.instance.Content.Load<Texture2D>("Images" + Path.DirectorySeparatorChar + "Item_" + itemID);
            //if(itemID==989) return;

            cit = null;
            tc = null;
            bool loaded = false;
            Main.QueueMainThreadAction(() =>
                {                   
                    cit = ModContent.Request<Texture2D>("Terraria/Images/" + "Item_" + itemID, AssetRequestMode.ImmediateLoad).Value;
                    tc = new Color[cit.Width * cit.Height];
                    cit.GetData<Color>(tc);                    
                    loaded = true;
                });
            while (tc == null || !loaded) { };


            int x = px / scale;
            int y = py / scale + 4;


            const int iconbgs = 48;

            int maxoff = rgbValues.Length - 1;

            if(withBG)
            {
                while (y < Main.maxTilesY / scale - 4 * iconbgs)
                {
                    int offt = (y + 4) * Main.maxTilesX * 4 / scale + x * 4;
                    //if (rgbValues[offt + 0] != (byte)255 || rgbValues[offt + 1] != (byte)255 || rgbValues[offt + 2] != (byte)255)

                    if (offt + 3 > maxoff)
                        return;// changed from continue ??? bug maybe here possible

                    if (rgbValues[offt + 3] != (byte)254)
                        break;
                    else
                        y += 1;//iconbgs-1;

                }

                if (y >= Main.maxTilesY / scale - 4 * iconbgs)
                {
                    while (x < Main.maxTilesX / scale - 4 * iconbgs)
                    {
                        int offt = (y + 4) * Main.maxTilesX * 4 / scale + x * 4;
                        int offt2 = (y + 4 + iconbgs / 2) * Main.maxTilesX * 4 / scale + x * 4;
                        int offt3 = (y + 4 + iconbgs / 2) * Main.maxTilesX * 4 / scale + (x - iconbgs / 2 + 4) * 4;
                        //if (rgbValues[offt + 0] != (byte)255 || rgbValues[offt + 1] != (byte)255 || rgbValues[offt + 2] != (byte)255)

                        if (offt + 3 > maxoff || offt2 + 3 > maxoff || offt3 + 3 > maxoff)
                            return;// changed from continue ??? bug maybe here possible

                        if (rgbValues[offt + 3] != (byte)254 && rgbValues[offt2 + 3] != (byte)254 && rgbValues[offt3 + 3] != (byte)254)
                            break;
                        else
                            x += 1;//iconbgs - 1;

                    }
                }



                for (int h = 0; h < iconbgs; h++)
                    for (int w = 0; w < iconbgs; w++)
                    {
                        int offt = (y + h) * Main.maxTilesX * 4 / scale + x * 4 + w * 4 - iconbgs / 2 * 4;
                        //int imgof = h * iconbgs + w;

                        bool isWhite = ((h - iconbgs / 2) * (h - iconbgs / 2) + (w - iconbgs / 2) * (w - iconbgs / 2)) > (iconbgs * iconbgs / 4);

                        if (offt + 3 > maxoff)
                            continue;

                        rgbValues[offt + 0] = isWhite ? rgbValues[offt + 0] : (byte)((rgbValues[offt + 0] >> 2) + (rgbValues[offt + 0] >> 4) + 176);
                        rgbValues[offt + 1] = isWhite ? rgbValues[offt + 1] : (byte)((rgbValues[offt + 1] >> 2) + (rgbValues[offt + 1] >> 4) + 176);
                        rgbValues[offt + 2] = isWhite ? rgbValues[offt + 2] : (byte)((rgbValues[offt + 2] >> 2) + (rgbValues[offt + 2] >> 4) + 176);
                        rgbValues[offt + 3] = isWhite ? rgbValues[offt + 3] : (byte)(254);
                    }
            }


            int yoff = iconbgs / 2 - cit.Height / 2;
            for (int h = 0; h < cit.Height-crop; h++)
                for (int w = 0; w < cit.Width; w++)
                {
                    int offt = (y + yoff + h) * Main.maxTilesX * 4 / scale + x * 4 + w * 4 - cit.Width / 2 * 4;
                    int imgof = h * cit.Width + w;

                    bool isWhite = (tc[imgof].A == 0); //;&& ( (h- cit.Height/2)* (h - cit.Height / 2) + (w-cit.Width/2)* (w - cit.Width / 2) ) > (cit.Width* cit.Height / 4 );

                    if (offt + 3 > maxoff)
                        continue;

                    rgbValues[offt + 0] = isWhite ? rgbValues[offt + 0] : (byte)((rgbValues[offt + 0] >> 2) + ((tc[imgof].B >> 1) + (tc[imgof].B >> 2)));
                    rgbValues[offt + 1] = isWhite ? rgbValues[offt + 1] : (byte)((rgbValues[offt + 1] >> 2) + ((tc[imgof].G >> 1) + (tc[imgof].G >> 2)));
                    rgbValues[offt + 2] = isWhite ? rgbValues[offt + 2] : (byte)((rgbValues[offt + 2] >> 2) + ((tc[imgof].R >> 1) + (tc[imgof].R >> 2)));
                    rgbValues[offt + 3] = isWhite ? rgbValues[offt + 3] : (byte)254;// tc[imgof].A;
                }

            tc = null;
        }
        public class ChestItem
        {
            public int x, y;
            public int itemID;
            public int maxDistSpawn;
            public float xscale;
            public float yscale;
            public int maxDepth;
            public int minDepth;
            public bool onlyUnlocked;

            public int stackSize;

            public ChestItem()
            {
                x = -1; itemID = -1;
                y = -1; maxDistSpawn = -1;
                xscale = 0;
                yscale = 0;
                onlyUnlocked = false;
                stackSize = 0;
            }

            public ChestItem(int itemID, int px, int py)
            {
                this.x = px;
                this.y = py;
                this.itemID = itemID;
                this.maxDistSpawn = Int32.MaxValue;
                xscale = 1.0f;
                yscale = 1.0f;
                this.maxDepth = Int32.MaxValue;
                this.minDepth = 0;
                this.onlyUnlocked = false;
                this.stackSize = 1;
            }
            public ChestItem(int itemID, int maxDistSpawn = Int32.MaxValue, float xscale = 1.0f, int maxDepth = Int32.MaxValue, bool onlyUnlocked = false, int minStackSize = 1, int minDepth = 0)
            {
                this.x = -1;
                this.y = -1;
                this.itemID = itemID;
                this.maxDistSpawn = maxDistSpawn;
                this.xscale = xscale;
                this.yscale = 1f;
                normScale();
                this.maxDepth = maxDepth;
                this.minDepth = minDepth;
                this.onlyUnlocked = onlyUnlocked;
                this.stackSize = minStackSize;

            }
            protected void normScale()
            {
                if (xscale == 0 && yscale == 0) { xscale = 1f; yscale = 1f; }
                float norm = (float)Math.Sqrt(xscale * xscale + yscale * yscale);
                this.xscale = xscale / norm;
                yscale = yscale / norm;
            }
            protected float computeDistance(int fx, int fy, int tx, int ty)
            {
                normScale();
                return (float)Math.Sqrt(xscale * xscale * Math.Abs(fx - tx) * Math.Abs(fx - tx) + yscale * yscale * Math.Abs(fy - ty) * Math.Abs(fy - ty));
            }

            public float DistanceTo(int px, int py)
            {
                return computeDistance(x, y, px, py);
            }
            public bool IsCloseToSpawn()
            {
                return DistanceTo(Main.spawnTileX, Main.spawnTileY) <= maxDistSpawn;
            }
            public bool IsPosCloseToSpawn(int px, int py)
            {
                return computeDistance(Main.spawnTileX, Main.spawnTileY, px, py) <= maxDistSpawn;
            }
            public bool DepthCheck(int y)
            {
                return y < maxDepth && y > minDepth;
            }
        }

        public class TileItem : ChestItem
        {
            public int tileID, frameX, frameY;//

            public TileItem(int tileID, int frameX, int frameY, int itemID, int px, int py)
            {
                this.x = px;
                this.y = py;
                this.itemID = itemID;
                this.maxDistSpawn = Int32.MaxValue;
                xscale = 1.0f;
                yscale = 1.0f;
                this.tileID = tileID;
                this.frameX = frameX;
                this.frameY = frameY;
                this.maxDepth = Int32.MaxValue;
            }
            public TileItem(int tileID, int frameX, int frameY, int itemID, int maxDistSpawn = Int32.MaxValue, float xscale = 1.0f, int maxDepth = Int32.MaxValue)
            {
                this.x = -1;
                this.y = -1;
                this.itemID = itemID;
                this.maxDistSpawn = maxDistSpawn;
                this.xscale = xscale;
                this.yscale = 1f;
                normScale();
                this.tileID = tileID;
                this.frameX = frameX;
                this.frameY = frameY;
                this.maxDepth = maxDepth;
            }
            public bool IsSame(Tile tile)
            {
                return tile.TileType == tileID && (tile.TileFrameX == frameX || frameX == -1) && (tile.TileFrameY == frameY || frameY == -1);
            }


        }
    }

}