using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Terraria.ID;
using Terraria.Audio;



namespace SuperSeedSearch.UI
{
    public class UIHoverSelectorPanel : UIPanel
    {       

        public UIHoverSelectorPanel()
        {
            Width.Set(0, 1);
            Height.Set(0, 1);
      
            this.OnMouseOver += hoverIn;
            this.OnMouseOut += hoverOut;

        }

        Color borderColorBf = Color.Black;
        void hoverIn(UIMouseEvent mouseEvent, UIElement target)
        {
            borderColorBf = BorderColor;
            BorderColor = ConstantEnum.UIColor.BCstartpauseHover;
            SoundEngine.PlaySound(SoundID.MenuTick );

        }

        void hoverOut(UIMouseEvent mouseEvent, UIElement target)
        {
            //BorderColor = ConstantEnum.UIColor.BCstartpauseNormal;
            BorderColor = borderColorBf;
            //SoundEngine.PlaySound(SoundID.MenuTick );

        }


    }
}