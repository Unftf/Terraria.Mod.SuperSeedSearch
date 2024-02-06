using System;
using System.Collections.Generic;
using Terraria.ID;



namespace SuperSeedSearch.PropertyCondition
{
    public static class ContainsItemList
    {

        public static Dictionary<string, short> LookUpItem = new Dictionary<string, short>();
        public static Dictionary<short, short> MappingIsAlso = new Dictionary<short, short>{
                {ItemID.TinBar, ItemID.CopperBar},
                {ItemID.LeadBar, ItemID.IronBar},
                {ItemID.TungstenBar, ItemID.SilverBar},
                {ItemID.PlatinumBar, ItemID.GoldBar},
        };

        public static List<String> pcContainsItemList = new List<string>
        {
            Add( ItemID.AncientChisel),
            Add( ItemID.AngelStatue),            
            Add( ItemID.Aglet),
            Add( ItemID.AnkletoftheWind),
            Add( ItemID.ArcheryPotion),
            Add( ItemID.AquaScepter),
            Add( ItemID.BabyBirdStaff, "Finch Staff"),
            Add( ItemID.BattlePotion),
            Add( ItemID.BandofRegeneration),
            Add( ItemID.BeachBall),
            Add( ItemID.BeeMinecart),
            Add( ItemID.BlessingfromTheHeavens),
            Add( ItemID.BlizzardinaBottle),
            Add( ItemID.Blowpipe),
            Add( ItemID.BlueMoon),            
            Add( ItemID.Bomb),
            Add( ItemID.Boomstick),
            Add( ItemID.Bottle),
            Add( ItemID.BuilderPotion),
            Add( ItemID.CanOfWorms),
            Add( ItemID.CordageGuide),  
            Add( ItemID.CatBast),
            Add( ItemID.CelestialMagnet),
            Add( ItemID.ClimbingClaws),
            Add( ItemID.CloudinaBottle),
            Add( ItemID.CobaltShield),       
            Add( ItemID.CopperBar, "Copper or Tin Bar"),
            Add( ItemID.Constellation),
            Add( ItemID.CreativeWings),                   
            Add( ItemID.DarkLance),                   
            Add( ItemID.DeadMansSweater),                   
            Add( ItemID.DesertMinecart),                                 
            Add( ItemID.Dynamite),  
            Add( ItemID.EnchantedBoomerang),  
            Add( ItemID.EncumberingStone),  
            Add( ItemID.Extractinator),                             
            Add( ItemID.FeatherfallPotion),                 
            Add( ItemID.FeralClaws),                            
            Add( ItemID.FiberglassFishingPole),            
            Add( ItemID.Fish),            
            Add( ItemID.FlamingArrow),
            Add( ItemID.Flamelash),
            Add( ItemID.Flare),
            Add( ItemID.FlareGun),
            Add( ItemID.Flipper),
            Add( ItemID.FloatingTube),            
            Add( ItemID.FlowerBoots),
            Add( ItemID.FlowerofFire),
            Add( ItemID.FlurryBoots),
            Add( ItemID.FlyingCarpet),
            Add( ItemID.GillsPotion),
            Add( ItemID.Glowstick),
            Add( ItemID.GoldBar, "Gold or Platinum Bar"),            
            Add( ItemID.GoldCoin),
            Add( ItemID.GoldenKey),                        
            Add( ItemID.GravitationPotion),   
            Add( ItemID.Grenade),                            
            Add( ItemID.Handgun),                
            Add( ItemID.HealingPotion),    
            Add( ItemID.HeartreachPotion),    
            Add( ItemID.HellfireArrow),                        
            Add( ItemID.HellwingBow),                        
            Add( ItemID.HellMinecart),                        
            Add( ItemID.HerbBag),   
            Add( ItemID.HermesBoots),  
            Add( ItemID.HighPitch),  
            Add( ItemID.HoneyDispenser),             
            Add( ItemID.HunterPotion), 
            Add( ItemID.IceBlade), 
            Add( ItemID.IceBoomerang), 
            Add( ItemID.IceMachine), 
            Add( ItemID.IceMirror), 
            Add( ItemID.IceSkates), 
            Add( ItemID.IronBar, "Iron or Lead Bar"),
            Add( ItemID.InfernoPotion),                        
            Add( ItemID.IronskinPotion),                        
            Add( ItemID.InvisibilityPotion),            
            Add( ItemID.JestersArrow),            
            Add( ItemID.LadybugMinecart),       
            Add( ItemID.LavaCharm),        
            Add( ItemID.LeafWand),
            Add( ItemID.LesserHealingPotion),            
            Add( ItemID.LihzahrdPowerCell),                            
            Add( ItemID.LifeforcePotion),                            
            Add( ItemID.LihzahrdFurnace),                            
            Add( ItemID.LivingWoodWand),      
            Add( ItemID.LivingLoom),
            Add( ItemID.LoveisintheTrashSlot),
            Add( ItemID.LuckyHorseshoe),                                                                                       
            Add( ItemID.Mace),              
            Add( ItemID.MagicConch),  
            Add( ItemID.MagicMirror),
            Add( ItemID.MagicPowerPotion),
            Add( ItemID.ManaRegenerationPotion),  
            Add( ItemID.MeteoriteBar),
            Add( ItemID.MoonLordLegs),
            Add( ItemID.MiningPotion),
            Add( ItemID.Muramasa),
            Add( ItemID.MysticCoilSnake),
            Add( ItemID.NightOwlPotion),
            Add( ItemID.ObsidianSkinPotion),
            Add( ItemID.PortableStool),
            Add( ItemID.PotionOfReturn),
            Add( ItemID.PharaohsMask),                    
            Add( ItemID.Radar),       
            Add( ItemID.RedPotion), 
            Add( ItemID.RegenerationPotion),       
            Add( ItemID.RecallPotion),       
            Add( ItemID.RestorationPotion),       
            Add( ItemID.Rope),     
            Add( ItemID.SandBoots),                                                
            Add( ItemID.SandstorminaBottle),
            Add( ItemID.ScarabBomb),
            Add( ItemID.Seaweed),
            Add( ItemID.SeeTheWorldForWhatItIs),                                                
            Add( ItemID.ShadowKey),
            Add( ItemID.SharkBait), 
            Add( ItemID.ShinePotion),
            Add( ItemID.ShinyRedBalloon),      
            Add( ItemID.ShoeSpikes),
            Add( ItemID.Shuriken),         
            Add( ItemID.SilverBar, "Silver or Tungsten Bar"),            
            Add( ItemID.SilverBullet),
            Add( ItemID.SilverCoin),
            Add( ItemID.SkyMill),
            Add( ItemID.SnowballCannon),
            Add( ItemID.SolarTablet),
            Add( ItemID.StaffofRegrowth),                        
            Add( ItemID.Starfury),
            Add( ItemID.SunflowerMinecart),   
            Add( ItemID.Sunfury),   
            Add( ItemID.SunOrnament),                                      
            Add( ItemID.SuspiciousLookingEye),
            Add( ItemID.SpelunkerPotion),
            Add( ItemID.Spear),                        
            Add( ItemID.SwiftnessPotion),               
            Add( ItemID.TeleportationPotion),         
            Add( ItemID.ThornsPotion),      
            Add( ItemID.ThunderSpear),                  
            Add( ItemID.ThunderStaff),                              
            Add( ItemID.Torch),       
            Add( ItemID.ThrowingKnife),       
            Add( ItemID.TrapsightPotion),
            Add( ItemID.Trident),
            Add( ItemID.TungstenBullet),
            Add( ItemID.Umbrella),    
            Add( ItemID.UnholyTrident),    
            Add( ItemID.Valor),    
            Add( ItemID.WandofSparking),    
            Add( ItemID.WebSlinger),    
            Add( ItemID.WaterWalkingBoots),    
            Add( ItemID.WaterWalkingPotion),    
            Add( ItemID.WoodenBoomerang),    
            Add( ItemID.WoodenArrow),    
            Add( ItemID.Wood),    

        };

        public static List<String> pcContainsModifierList = AllModifierList();
        private static List<String>  AllModifierList(){
            List<String> mods = new List<string>{ ConstantEnum.ConstantsStrings.None};
            for(int mi=1; mi<PrefixID.Count; mi++){
                mods.Add( Storage.Stats.GetName4ID(typeof(PrefixID), mi) );
            }
            return mods;
        }
     
        private static string Add(short id, string specialName = "")
        {
            string name = specialName.Length==0? GetItemName(id): specialName;

            LookUpItem.Add(name, id);            
            return name;
        }


        public static string GetItemName(short ID)
        {
            var type = typeof(ItemID);
            foreach (var field in type.GetFields())
            {
                if (field.FieldType == typeof(short) && (short)field.GetValue(null) == ID)
                {
                    return field.Name;
                }
            }
            return ID.ToString();

        }

  


    }
}