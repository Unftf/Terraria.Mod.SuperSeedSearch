using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;



namespace SuperSeedSearch.UI
{
    public class UICloseButton : UIHoverSelectorPanel
    {
        public UICloseButton()
        {
            Width.Set(40, 0);
            Height.Set(40, 0);
            MarginTop = 4;
            MarginRight = 4

            ;
            MarginBottom = 0;
            SetPadding(4);
            BackgroundColor = ConstantEnum.UIColor.BGCstartpause;
            BorderColor = ConstantEnum.UIColor.BCstartpauseNormal;

            UIImage exit = new UIImage(ModContent.Request<Texture2D>(ModMenuMod.modName+"/img/Xclose32"));
            exit.VAlign = 00f;
            exit.HAlign = 00f;
            exit.MarginLeft = 0;
            exit.MarginTop = 0;
            Append(exit);
        }

    }
}