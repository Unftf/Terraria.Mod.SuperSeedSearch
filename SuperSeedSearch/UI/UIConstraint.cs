using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.UI.Elements;


namespace SuperSeedSearch.UI
{

    public class UIConstraint : UIPanel //swap to element
    {
        internal static Color backgroundColor = new Color(0, 0, 0, 0);

       
        UISuperSeed superSeedUI;
        public string valueName = "";
        
        public string valueValue = "";

        public UIGreaterOrLess greaterLessEqualUI;

        UIList paramList = null;

        public event Action OnDeletePressed;
        public event Action<ConstantEnum.GreaterOrLessEqual> onGolChanged;

        public PropertyCondition.Constraint condiElm;



        public Action<string, int> onTextChange;
        public ConstantEnum.WorldGenPass delayToworldGenPass { get; private set; }

        public UIConstraint(PropertyCondition.Constraint condiElmBase, bool disableCustomInput = false, bool noDelete = false, bool noWGPassInfo = false, bool noGoleChange = false)
        {
            condiElm = new PropertyCondition.Constraint(condiElmBase);
            this.valueName = condiElm.name;
            
            this.superSeedUI = ModMenuMod.uiSuperSeed;
            this.delayToworldGenPass = condiElm.delayToworldGenPass;
            

            SetPadding(0);
            BackgroundColor = backgroundColor;
            BorderColor = backgroundColor;
            MarginTop = 0 * 20;
            UIText textHead = new UIText(this.valueName + (noWGPassInfo ? "" : " " + ConstantEnum.WorldGenPassDict.AsPreConstraintText(delayToworldGenPass)));
            
            textHead.HAlign = 0f;
            Append(textHead);


            SetPadding(0);
            HAlign = 0f;
            VAlign = 0f;
            Width.Set(0, 1);
            float heightLine = (textHead.GetOuterDimensions().Height + (textHead.MarginTop + textHead.MarginBottom + textHead.PaddingTop + textHead.PaddingBottom)) * 2;
            Height.Set(heightLine * 1.5f, 0);

            paramList = new UIList();
            paramList.ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
            paramList.SetPadding(0);
            paramList.HAlign = 0f;
            paramList.VAlign = 0f;
            paramList.Width.Set(0, 1f);
            paramList.Height.Set(0f, 1f);
            paramList.PaddingBottom = 0;
            paramList.MarginBottom = 0;
            paramList.MarginTop = textHead.GetInnerDimensions().Height;
            paramList.ListPadding = 0;

            UIInputEntry mainInp = new UIInputEntry(condiElm, -1, disableCustomInput, noDelete, noGoleChange);
            mainInp.OnDeletePressed += () => OnDeletePressed?.Invoke();
            mainInp.onTextChange += (text, isparam) => onTextChange?.Invoke(text, isparam);
            mainInp.onGolChanged += (gol) => onGolChanged?.Invoke(gol);

            valueValue = mainInp.valueValue;
            paramList.Add(mainInp);
            //Append((AddInputPanel(condiElm)));
            if (condiElm.ParameterValue1.IsParameterSet)
            {
                UIInputEntry param1 = new UIInputEntry(condiElm, 1, disableCustomInput);
                param1.onTextChange += (text, isparam) => onTextChange?.Invoke(text, isparam);
                paramList.Add(param1);
                Height.Set(Height.Pixels + heightLine, 0);
            }
            if (condiElm.ParameterValue2.IsParameterSet)
            {
                UIInputEntry param2 = new UIInputEntry(condiElm, 2, disableCustomInput);
                param2.onTextChange += (text, isparam) => onTextChange?.Invoke(text, isparam);
                paramList.Add(param2);
                Height.Set(Height.Pixels + heightLine, 0);
            }
            if (condiElm.ParameterValue3.IsParameterSet)
            {
                UIInputEntry param3 = new UIInputEntry(condiElm, 3, disableCustomInput);
                param3.onTextChange += (text, isparam) => onTextChange?.Invoke(text, isparam);
                paramList.Add(param3);
                Height.Set(Height.Pixels + heightLine, 0);
            }
            Append(paramList);

        }

    }

}