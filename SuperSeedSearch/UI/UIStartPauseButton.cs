using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;




namespace SuperSeedSearch.UI
{
    public class UIStartPauseButton : UIHoverSelectorPanel
    {
        UIText startPauseText;
        public bool isInSearchMode = false;
        public event Action OnStartClick;
        public event Action OnPauseClick;
        public UIStartPauseButton()
        {
            Width.Set(350, 0);
            Height.Set(64, 0);
         
            MarginTop = 0;
            MarginBottom = 0;
            SetPadding(0);
            BackgroundColor = ConstantEnum.UIColor.BGCstartpause;
            BorderColor = ConstantEnum.UIColor.BCstartpauseNormal;

            UIImage startPausB = new UIImage(ModContent.Request<Texture2D>("SuperSeedSearch/img/magnifier"));
            startPausB.VAlign = 0.0f;
            startPausB.HAlign = 0.0f;
            startPausB.MarginLeft = 5;
            startPausB.MarginTop = -2;
            Append(startPausB);

            startPauseText = new UIText("Start search", 1, true);
            startPauseText.VAlign = 0.5f;
            startPauseText.HAlign = 0f;

            startPauseText.MarginLeft = 70;
            Append(startPauseText);

            this.OnLeftMouseDown += ChangeState;
            isInSearchMode = false;

        }
        private void ChangeState(UIMouseEvent mouseEvent, UIElement target)
        {
            if (isInSearchMode)
            {
                //ChangeToStartDisplay();    
                startPauseText.SetText("..ending soon");            
                OnPauseClick?.Invoke();
            }
            else
            {
                ChangeToPauseDisplay();
                OnStartClick?.Invoke();
            }
            

        }

        public void ChangeToPauseDisplay()
        {
            startPauseText.SetText("Stop search");
            isInSearchMode = true;
        }
        public void ChangeToStartDisplay()
        {
            startPauseText.SetText("Start search");
            isInSearchMode = false;
        }  

    }
}