using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;




namespace SuperSeedSearch.UI
{
    internal class UIInputEntry : UIElement
    {
        UISuperSeed superSeedUI; UIPanel inputbox; public UIText UIvalue;
        List<string> contentList = null;
        public string valueValue = "";

        bool disableInput;
        public event Action OnDeletePressed;
        UIDeleteButton deleteButton;

        public PropertyCondition.Constraint condiElm;
        int isParamert;
        public event Action<string, int> onTextChange;
        public event Action<ConstantEnum.GreaterOrLessEqual> onGolChanged;
        UIGreaterOrLess greaterLessEqualUI;



        public UIInputEntry(PropertyCondition.Constraint condiElm, int isParamert = -1, bool disableInput = false, bool noDelete = false, bool noGoleChange = false)
        {
            this.superSeedUI = ModMenuMod.uiSuperSeed;
            this.isParamert = isParamert;
            this.condiElm = condiElm;



            this.disableInput = disableInput;
            UIElement leftparaGoLE = new UIElement();
            Helpers.Helpers.SetUIBorderToZero(leftparaGoLE);          
            leftparaGoLE.Height.Set(0, 1);
            leftparaGoLE.Width.Set(100, 0);

            greaterLessEqualUI = new UIGreaterOrLess(condiElm.gole, noGoleChange);
            greaterLessEqualUI.OnGolChanged += (gol) => onGolChanged?.Invoke(gol);

            greaterLessEqualUI.HAlign = 1f;
            greaterLessEqualUI.VAlign = 0f;
            greaterLessEqualUI.MarginTop = 0;

            string defaultValue = "";
            defaultValue = condiElm.targetValue.GetValueAsString();


            this.contentList = condiElm.valueList;

            if (isParamert > 0)
            {
                greaterLessEqualUI.SetTextToValue(ConstantEnum.GreaterOrLessEqual.Equal);

                string paramName = ConstantEnum.ConstraintNames.ParamAddon + isParamert;

                if (isParamert == 1)
                {
                    paramName = condiElm.ParameterValue1.name;
                    defaultValue = condiElm.ParameterValue1.GetValueAsString();
                    this.contentList = ConstantEnum.ConditionElementValueList.FindValueValueList4ValueName(paramName);
                }
                else if (isParamert == 2)
                {
                    paramName = condiElm.ParameterValue2.name;
                    defaultValue = condiElm.ParameterValue2.GetValueAsString();
                    this.contentList = ConstantEnum.ConditionElementValueList.FindValueValueList4ValueName(paramName);
                }
                else if (isParamert == 3)
                {
                    paramName = condiElm.ParameterValue3.name;
                    defaultValue = condiElm.ParameterValue3.GetValueAsString();
                    this.contentList = ConstantEnum.ConditionElementValueList.FindValueValueList4ValueName(paramName);
                }
                UIText param = new UIText(paramName);
                param.HAlign = 1f;
                param.VAlign = 0;
                param.MarginRight = greaterLessEqualUI.Width.Pixels / 4;
                param.MarginTop = 10;
                leftparaGoLE.Append(param);
            }
            else
                leftparaGoLE.Append(greaterLessEqualUI);



            if (defaultValue.Length == 0)
            {
                defaultValue = ConstantEnum.ConditionElementValueList.FindDefaultValue4ValueName(defaultValue);

                if (defaultValue.Length == 0 && contentList != null && contentList.Count > 0)
                    defaultValue = contentList[0];
            }

            if (isParamert < 0)
            {
                if (!noDelete)
                {
                    deleteButton = new UIDeleteButton();
                    deleteButton.OnLeftClick += deleteEntry;
                    deleteButton.VAlign = 0.75f;

                    leftparaGoLE.Append(deleteButton);
                }
            }

            Append(leftparaGoLE);

            Helpers.Helpers.SetUIBorderToZero(this);

            Width.Set(0, 1f);
            Height.Set(greaterLessEqualUI.Height.Pixels + 0 * greaterLessEqualUI.MarginTop, 0);


            inputbox = new UIPanel();       
                 
            Helpers.Helpers.SetUIBorderToZero(inputbox);

            
            inputbox.MarginLeft = leftparaGoLE.Width.Pixels;            
            inputbox.PaddingLeft = 7;            
            inputbox.VAlign = 1.0f;
            inputbox.Width.Set(-inputbox.MarginLeft - inputbox.PaddingLeft, 1f);
            inputbox.Height.Set(0, 1);
            //inputbox.BackgroundColor = Color.Red;
            //inputbox.BackgroundColor = ConstantEnum.UIColor.BGinputPanel;
            inputbox.BorderColor = Color.Transparent;
            inputbox.OnLeftClick += OpenContextMenuLeft;
            inputbox.OnRightClick += OpenContextMenuRight;
            inputbox.OnLeftMouseDown += ClickInProcess;
            inputbox.OnLeftMouseUp += ReleaseInProcess;


            valueValue = defaultValue;

            UIvalue = new UIText(valueValue);
            UIvalue.TextColor = ConstantEnum.UIColor.inputPanelText;
            UIvalue.MarginTop = 7;

            UIList toPreventOverFlow = new UIList();
            Helpers.Helpers.SetUIBorderToZero(toPreventOverFlow);
            toPreventOverFlow.Add(UIvalue);
            toPreventOverFlow.VAlign = 0.5f;
            toPreventOverFlow.Width.Set(0, 1f);
            toPreventOverFlow.Height.Set(0, 1f);

            inputbox.Append(toPreventOverFlow);
            Append(inputbox);
        }


        public void deleteEntry(UIMouseEvent evt, UIElement listeningElement)
        {
            OnDeletePressed?.Invoke();
        }


        public void OpenContextMenuLeft(UIMouseEvent evt, UIElement listeningElement)
        {
            genContextPanel(evt, false);            
            base.LeftClick(evt);
        }
        public void OpenContextMenuRight(UIMouseEvent evt, UIElement listeningElement)
        {
            genContextPanel(evt, true);            
            base.LeftClick(evt);
        }
        private void genContextPanel(UIMouseEvent evt, bool startWithTargetText){
            if (clickInProcess != 1)
            {
                clickInProcess = 0;
                superSeedUI?.GenerateContextPanel(contentList, disableInput)?.OpenContextMenu(evt, this, UIvalue, UpdateText,startWithTargetText);
            }            
        }

        int clickInProcess = 0;
        private void ClickInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement.Equals(inputbox) && (superSeedUI?.contextMenu?.intputWithContext != null && superSeedUI.contextMenu.intputWithContext.focused == true))
            {
                clickInProcess = 1;
            }
        }
        private void ReleaseInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickInProcess == 1)
                clickInProcess = 2;
        }


        private void UpdateText(string newText)
        {
            if (newText.Length != 0)
            {

                if (isParamert < 0)
                    condiElm.targetValue.SetValue(newText);
                else if (isParamert == 1)
                    condiElm.ParameterValue1.SetValue(newText);
                else if (isParamert == 2)
                    condiElm.ParameterValue2.SetValue(newText);
                else if (isParamert == 3)
                    condiElm.ParameterValue3.SetValue(newText);
                UIvalue.SetText(newText);
                onTextChange?.Invoke(newText, isParamert);
            }

        }


    }

}
