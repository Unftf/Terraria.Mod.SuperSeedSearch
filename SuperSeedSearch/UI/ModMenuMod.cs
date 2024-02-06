using Terraria.ModLoader;
using Terraria;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperSeedSearch.WorldGenMod;

using Terraria.ID;
using Terraria.Audio;


namespace SuperSeedSearch.UI
{
    public class ModMenuMod : ModMenu
    {
        public static UISuperSeed uiSuperSeed = null;
        public static WGPassChanger wGPassChanger = null;        
        public static ModMenuMod instance = null;

        public const string modName = "SuperSeedSearch";


        private bool oldValPlayOldTile;
        public override void OnSelected()
        {   
            Main.menuMode = 888;
            oldValPlayOldTile = Main.instance.playOldTile;
            Main.instance.playOldTile = true;

                        
            instance = this;
            uiSuperSeed = new UISuperSeed();
            uiSuperSeed.SetUpUIElements();
            uiSuperSeed.onExit += Exit;

            
            Main.MenuUI.SetState(Main.AchievementsMenu);
            Main.MenuUI.SetState(uiSuperSeed);
            
            wGPassChanger = new WGPassChanger(uiSuperSeed);
            wGPassChanger.onWorldGenStart += uiSuperSeed.UnHideProgressBar;
            wGPassChanger.onWorldGenEnd += uiSuperSeed.HideProgressBar;
            wGPassChanger.onWorldGenEnd += uiSuperSeed.startPause.ChangeToStartDisplay;

            uiSuperSeed.onStartWorldGen += wGPassChanger.StartWorldGen;
            uiSuperSeed.onStopWorldGen += wGPassChanger.StopWorldGen;     

           
                 
        }


        public override void OnDeselected()
        {
            //Exit();
        }

        private void Exit()
        {
            Main.instance.playOldTile = oldValPlayOldTile;
            //Main.menuMode = 0;
            wGPassChanger?.StopWorldGen();
            wGPassChanger?.ResetSecrets();
            if (uiSuperSeed != null) uiSuperSeed.onExit -= Exit;                        
            Main.MenuUI.GoBack();
            Main.MenuUI.SetState(null);
            
            //ModMenuMod.instance.UserInterface.SetState(null);
            Main.menuMode = 0;
            

            instance = null;
            uiSuperSeed = null;
        }


        private double sunMoonTargetY = 100;

        public override bool PreDrawLogo(SpriteBatch spriteBatch, ref Vector2 logoDrawCenter, ref float logoRotation, ref float logoScale, ref Color drawColor)
        {
            //sunMoonTargetY = -Main.screenHeight*0.125-(Main.screenHeight*0.01)*(6+4*Math.Cos(Math.PI*Main.time/27000));
            sunMoonTargetY = -Main.screenHeight * 0.225 - (Main.screenHeight * 0.01) * (6 + 4 * Math.Cos(Math.PI * Main.time / 27000));

            if (!Main.alreadyGrabbingSunOrMoon)
            {
                Main.sunModY = (short)(Main.sunModY + ((int)(0.1 * (sunMoonTargetY - Main.sunModY) * 1000)) / 1000);
                Main.moonModY = (short)(Main.moonModY + ((int)(0.1 * (sunMoonTargetY - Main.moonModY) * 1000)) / 1000);
            }
            return false;
        }

    }
}