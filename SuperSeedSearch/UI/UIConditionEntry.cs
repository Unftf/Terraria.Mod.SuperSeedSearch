using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SuperSeedSearch.PropertyCondition;


namespace SuperSeedSearch.UI
{
    public class UIConditionEntry : UIPanel
    {

        internal static Color backgroundColor = new Color(0, 0, 0, 0);


        UISuperSeed superSeedUI;
        public string valueName = "";


        public event Action OnDeletePressed;
        public event Action<UIConditionEntry> OnCondEntrySelected;

        bool disableInput = false;
        UIPanel inputbox;



        public List<PropertyCondition.Constraint> ConditionState;
        public PropertyCondition.Constraint ConditionStateTypeSelctor, CounterState;


        public PropertyCondition.ConditionTypeList condition;


        UIText DisplayText;
        public UIConditionEntry(PropertyCondition.ConditionTypeList condition, bool disableInput = false)
        {
            this.condition = condition;
            ConditionState = new List<PropertyCondition.Constraint>();
            ConditionStateTypeSelctor = null;
            CounterState = null;

            //condition.suitableTypeConditions;
            this.disableInput = disableInput;
            this.valueName = condition.header;
            this.superSeedUI = ModMenuMod.uiSuperSeed;


            SetPadding(0);
            BackgroundColor = backgroundColor;
            BorderColor = backgroundColor;
      
            DisplayText = new UIText(valueName); 
            DisplayText.MarginTop = 7;       
            DisplayText.TextColor = ConstantEnum.UIColor.inputPanelText;

            SetPadding(0);
            HAlign = 0f;
            VAlign = 0f;
            Width.Set(0, 1);
            float heightLine = (DisplayText.GetInnerDimensions().Height + (DisplayText.MarginTop + DisplayText.MarginBottom + DisplayText.PaddingTop + DisplayText.PaddingBottom));
            Height.Set(32, 0);

            UIDeleteButton delb = new UIDeleteButton();
            delb.OnLeftClick += deleteEntry;
            delb.VAlign = 0.5f;
            Append(delb);

            inputbox = new UIPanel();

            inputbox.SetPadding(0);
            inputbox.MarginTop = 0;
            inputbox.MarginLeft = delb.Width.Pixels + 8;
            inputbox.PaddingTop = 0;
            inputbox.PaddingLeft = 7;
            inputbox.HAlign = 0f;
            inputbox.VAlign = 1.0f;
            inputbox.Width.Set(-inputbox.MarginLeft - inputbox.PaddingLeft, 1f);

            inputbox.Height.Set(0, 1);
            //inputbox.BackgroundColor = Color.Red;
            inputbox.BackgroundColor = ConstantEnum.UIColor.BGCinputPanel;
            inputbox.BorderColor = Color.Transparent;
            //inputbox.OnClick += OpenContextMenu;


            UIList toPreventOverFlow = new UIList();

            toPreventOverFlow.Add(DisplayText);
            toPreventOverFlow.SetPadding(0);
            toPreventOverFlow.MarginTop = 0;
            toPreventOverFlow.MarginBottom = 0;
            toPreventOverFlow.HAlign = 0f;
            toPreventOverFlow.VAlign = 0.5f;
            toPreventOverFlow.Width.Set(0, 1f);
            toPreventOverFlow.Height.Set(0, 1f);

            inputbox.Append(toPreventOverFlow);




            Append(inputbox);
            inputbox.OnLeftClick += SetConstraintList;
        }

        public void deleteEntry(UIMouseEvent evt, UIElement listeningElement)
        {
            OnDeletePressed?.Invoke();
        }

        public void SetConstraintList(UIMouseEvent evt, UIElement listeningElement)
        {
            if (ConditionState != null)
            {
                ModMenuMod.uiSuperSeed?.ConditionConstraintList?.UpdateConstraintList(condition, ConditionStateTypeSelctor, CounterState, ConditionState, this);
                OnCondEntrySelected?.Invoke(this);                
            }
        }

        public void StoreCurrentState(PropertyCondition.ConditionTypeList conditionType, PropertyCondition.Constraint ConditionStateTypeSelctor, PropertyCondition.Constraint CounterState, List<PropertyCondition.Constraint> ConditionState)
        {
            this.condition = conditionType;
            this.ConditionState = ConditionState;
            this.ConditionStateTypeSelctor = ConditionStateTypeSelctor;
            this.CounterState = CounterState;

        }

        public void DeSelect()
        {
            inputbox.BorderColor = ConstantEnum.UIColor.BCinputPanel;
        }
        public void Select()
        {
            inputbox.BorderColor = ConstantEnum.UIColor.BCinputPanelSelected;            
        }
        public void SetName(string newName)
        {
            DisplayText.SetText(newName);
        }

        public Condition ExportAsCondition()
        {
            if (ConditionStateTypeSelctor == null || ConditionStateTypeSelctor.targetValue == null || CounterState == null || CounterState.targetValue == null) return null;

            string name = valueName; 
            
            Func<Condition, bool> evalfun = null;
            List<PropertyElement> prop = new List<PropertyElement>();
            List<Constraint> condPos = new List<Constraint>();

            bool isPersistent = false;
            double countDivisor = 1;
            double countSubtractor = 1;
            Func<Condition, PropertyElement, bool> postConstraintCheckFun = null;
            Func<int> getCondMemoryFun = null;
            foreach (var item in condition.suitableTypeConditions)
            {
                if (ConditionStateTypeSelctor.targetValue.GetValueAsString().Equals(item.name))
                {
                    prop = item.appliesToPropertyElements;

                    evalfun = item.targetEvalValue.evalFun;
                    condPos = item.possibleConstrElemts;
                    isPersistent = item.isPersistent;
                    countDivisor = item.countDivisor;
                    countSubtractor = item.countSubtractor;
                    postConstraintCheckFun = item.postConstraintCheckFun;
                    getCondMemoryFun = item.GetConditionMemorySize;
                    break;
                }
            }


            //need to make copy from all staff else it can change if used edits during the search
            //name + " " + Conditi
            Condition cond = new Condition(condition.conditionType, ConditionStateTypeSelctor.targetValue.GetValueAsString(), prop, condPos,
                             new Condition.TargetEvalValue(CounterState.targetValue.GetValueAsString(), evalfun), getCondMemoryFun )
                                        {isPersistent=isPersistent,countDivisor=countDivisor,countSubtractor=countSubtractor, postConstraintCheckFun=postConstraintCheckFun };//TODO1: do something better


            cond.SetCondElmChosenList(ConditionState);

            cond.gole = CounterState.gole;


            cond.ResetPreStartOfSearch();


            return cond;
        }


    }
}