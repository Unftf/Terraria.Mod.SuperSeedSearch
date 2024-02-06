using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;



namespace SuperSeedSearch.ConstantEnum
{
    public static class UIColor
    {
        public static Color BGCnormal = new Color(73, 94, 171, 255);
        public static Color BGCContextMenu = new Color(73 + 23, 94 + 23, 171 + 23, 255);

        public static Color BGCstartpause = new Color(53, 65, 120, 255);
        public static Color BCstartpauseNormal =  (new UIPanel()).BorderColor;
        public static Color BCstartpauseHover =  new Color(153, 165, 120, 255);
        public static Color BGCTab = new Color(33, 43, 79);        
        public static Color BCTab = BGCnormal ;
        public static Color BGCTabTextPanel = BGCTab;
        public static Color BGCTabTextPanelActive = BGCnormal;
        public static Color BCTabTextPanel = BGCTab;

   

        public static Color SepMainPanel = new Color(BGCnormal.R / 4, BGCnormal.G / 4, BGCnormal.B / 4, 100);
        public static Color SepMainPanelBelowType = new Color(BGCnormal.R / 4, BGCnormal.G / 4, BGCnormal.B / 4, 50);
        public static Color SepContext = new Color(BGCContextMenu.R / 4, BGCContextMenu.G / 4, BGCContextMenu.B / 4, 100);

        public static Color Header = Color.FloralWhite;

        public static Color BGCinputPanel = (new UIPanel()).BackgroundColor;
        public static Color BCinputPanel = (new UIPanel()).BackgroundColor;
        public static Color BCinputPanelSelected = Color.RoyalBlue;
        public static Color inputPanelText = Color.Gainsboro;
        public static Color DeleteX = Color.IndianRed;
        public static Color DeleteXTrans = new Color(DeleteX.R, DeleteX.G, DeleteX.B, 100);
    }
}