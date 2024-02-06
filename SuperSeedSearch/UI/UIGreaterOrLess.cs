using System;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;


namespace SuperSeedSearch.UI
{
    public class UIGreaterOrLess : UIPanel
    {
        UIText gol = null;
        private ConstantEnum.GreaterOrLessEqual value = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual;
        public event Action<ConstantEnum.GreaterOrLessEqual> OnGolChanged;
        bool changeNotAllowed = false;
        public UIGreaterOrLess(ConstantEnum.GreaterOrLessEqual greaterLessEqual = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual, bool changeNotAllowed = false)
        {
            this.changeNotAllowed = changeNotAllowed;

            SetPadding(0);
            MarginTop = 0;
            MarginLeft = 0;
            MarginRight= 0;
            MarginBottom= 0;
            HAlign = 0.0f;
            VAlign = 0.0f;


            gol = new UIText("#", 0.5f, true);
            gol.TextColor = Color.Gainsboro;
            Width.Set(32, 0);
            Height.Set(32, 0);
            if(!changeNotAllowed)
                OnLeftClick += ChangeValue;
            value = greaterLessEqual;
            SetTextToValue(value);
            SetPadding(0);
            gol.HAlign = 0.5f;
            gol.VAlign = 0.5f;
            //BackgroundColor = Color.Red;
            BackgroundColor = UIConstraint.backgroundColor;
            BorderColor = UIConstraint.backgroundColor;

            Append(gol);
        }

        public void ChangeValue(UIMouseEvent evt, UIElement listeningElement)
        {         
                      
            switch (value)
            {
                case ConstantEnum.GreaterOrLessEqual.GreaterOrEqual:
                    value = ConstantEnum.GreaterOrLessEqual.LessOrEqual;
                    break;
                case ConstantEnum.GreaterOrLessEqual.LessOrEqual:
                    value = ConstantEnum.GreaterOrLessEqual.GreaterOrEqual;
                    break;
                case ConstantEnum.GreaterOrLessEqual.NotEqual:
                    value = ConstantEnum.GreaterOrLessEqual.Equal;
                    break;
                case ConstantEnum.GreaterOrLessEqual.Equal:
                    value = ConstantEnum.GreaterOrLessEqual.NotEqual;
                    break;
            }
            SetTextToValue(value);            
        }

        public void SetTextToValue(ConstantEnum.GreaterOrLessEqual value)
        {
            this.value = value;
            switch (value)
            {
                case ConstantEnum.GreaterOrLessEqual.GreaterOrEqual:
                    gol.SetText("≥");
                    break;
                case ConstantEnum.GreaterOrLessEqual.LessOrEqual:
                    gol.SetText("≤");
                    break;
                case ConstantEnum.GreaterOrLessEqual.NotEqual:
                    gol.SetText("≠");
                    break;
                case ConstantEnum.GreaterOrLessEqual.Equal:
                    gol.SetText("=");
                    break;
            }
            OnGolChanged?.Invoke(value);
        }


    }
}