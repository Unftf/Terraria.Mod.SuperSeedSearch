using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SuperSeedSearch.PropertyCondition;



namespace SuperSeedSearch.UI
{
    public class UIConstraintList : UIList
    {
        List<PropertyCondition.Condition> typeSuitableConditionList;
        string header;
        public UIScrollbar scrollbar { get; }
        UI.UIConstraint uitypeTypeSelector;
        PropertyCondition.Constraint typeTypeSelector;
        UI.UIConstraint uiCountEntry;
        PropertyCondition.Constraint CountEntry;
        UIText uiheader;

        UIVerticalSeparator sep;
        UIVerticalSeparator sep2, sep3;

        PropertyCondition.ConditionTypeList proConTypeList = null;

        UIText AddNewText;
        UIPanel AddNewPanel; UISuperSeed uiSuperSeed;

        List<string> contentListText;
        List<PropertyCondition.Constraint> contentListCondElm;
        List<UI.UIConstraint> currentList;

        List<PropertyCondition.Constraint> currentListAsCondElm;
        public event Action<string> onNameChanged;

        public UIConstraintList(UISuperSeed uiSuperSeed)
        {
            currentList = new List<UI.UIConstraint>();
            this.uiSuperSeed = uiSuperSeed;
            this.ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
            scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(-18f, 1f);
            scrollbar.HAlign = 1.0f;
            scrollbar.Top.Pixels = 6;
            scrollbar.MarginRight = 2;
            scrollbar.MarginTop = 4;

            currentListAsCondElm = new List<PropertyCondition.Constraint>();


            Helpers.Helpers.SetUIBorderToZero(this);
            Width.Set(-scrollbar.MarginRight - scrollbar.Width.Pixels, 1f);
            Height.Set(0f, 1f);


            SetScrollbar(scrollbar);

            uiheader = new UIText("", 0.5f, true);
            uiheader.MarginTop = 10;
            uiheader.HAlign = 0.5f;
            uiheader.TextColor = ConstantEnum.UIColor.Header;
            UpdateListEntries(null);

            sep = new UIVerticalSeparator();
            Helpers.Helpers.SetUIBorderToZero(sep);
            sep.MarginTop = uiheader.Height.Pixels + 6;
            sep.MarginBottom = sep.Height.Pixels + 6;
            sep.Color = ConstantEnum.UIColor.SepMainPanel;
            sep.Width.Set(-scrollbar.MarginRight, 1);

            sep2 = new UIVerticalSeparator();
            sep2.Color = ConstantEnum.UIColor.SepMainPanelBelowType;
            sep2.CopyStyle(sep);

            sep2.MarginBottom = sep2.Height.Pixels + 6;
            sep2.MarginTop = uitypeTypeSelector.GetInnerDimensions().Height / 2;


            sep3 = new UIVerticalSeparator();
            sep3.CopyStyle(sep2);
            sep3.Color = ConstantEnum.UIColor.SepMainPanelBelowType;
            sep3.MarginTop = 8;

            AddNewPanel = new UIPanel();
            AddNewPanel.MarginTop = 14;
            AddNewPanel.Width.Set(0, 1);
            AddNewPanel.Height.Set(40, 0);
            AddNewPanel.BorderColor = Color.Transparent;
            AddNewPanel.BackgroundColor = ConstantEnum.UIColor.BGCContextMenu;
            AddNewPanel.OnLeftClick += OpenContextMenu;

            AddNewText = new UIText(ConstantEnum.ConstantsStrings.AddConditionElementButtonText);
            Helpers.Helpers.SetUIBorderToZero(AddNewText);

            AddNewText.OnLeftMouseDown += ClickInProcess;
            AddNewText.OnLeftMouseUp += ReleaseInProcess;
            AddNewText.Width.Set(0, 1);
            AddNewText.Height.Set(0, 1);
            AddNewPanel.Append(AddNewText);



            // OnClick += UpdateName;//TODO some better way


            Updatelist();
        }
        public void UpdateListEntries(PropertyCondition.ConditionTypeList proConTypeList)
        {
            currentList.Clear();
            if (proConTypeList != null)
            {
                typeSuitableConditionList = new List<PropertyCondition.Condition>(proConTypeList.suitableTypeConditions);
                header = proConTypeList.header;
                this.proConTypeList = proConTypeList;

            }
            else
            {
                this.proConTypeList = new PropertyCondition.ConditionTypeList(ConstantEnum.ConditionType.Unknown);

                typeSuitableConditionList = new List<PropertyCondition.Condition>();
                header = ConstantEnum.ConditionTypeNames.Unknown;
            }

            uiheader.SetText(header);

            List<string> typeTypeList = new List<string>();

            foreach (var item in typeSuitableConditionList)
            {
                typeTypeList.Add(item.name);
            }
                        
            string lastTarget = (currenttarget==null || currenttarget.ConditionStateTypeSelctor == null?"":currenttarget.ConditionStateTypeSelctor.targetValue);
            typeTypeSelector = new Constraint(ConstantEnum.ConstantsStrings.ConditionTypeName, null, typeTypeList, targetValue:lastTarget );

            UpdateUItypeTypeSelector(typeTypeSelector);

            


            UpdateContentList();            
        }
        private void UpdateUItypeTypeSelector(Constraint typeTypeSelector)
        {
            
            uitypeTypeSelector = new UIConstraint(typeTypeSelector, false, false, true, true);
            typeTypeSelector = uitypeTypeSelector.condiElm;
            uitypeTypeSelector.onTextChange += ChangeType;
            //uitypeTypeSelector.onTextChange += (a, b) => UpdateNameAndStoreState(null, null);
            uitypeTypeSelector.OnDeletePressed += () => { currentList = new List<UI.UIConstraint>(); currentListAsCondElm = new List<PropertyCondition.Constraint>(); Updatelist(); };
            uitypeTypeSelector.onGolChanged += (gol) => ChangeGol(gol, uitypeTypeSelector.condiElm);

            CountEntry = new Constraint(ConstantEnum.ConstantsStrings.ConditionCountName, null,
                                             ConstantEnum.ConditionElementValueList.lookUpValueList[ConstantEnum.ConditionElementValueList.KeyWords.defaultValue], ConstantEnum.WorldGenPass.PostWorldGen, 1);
            uiCountEntry = new UIConstraint(CountEntry, false, true, true);
            CountEntry = uiCountEntry.condiElm;
            uiCountEntry.onGolChanged += (gol) => ChangeGol(gol, uiCountEntry.condiElm);
            uiCountEntry.onTextChange += (a, b) => UpdateNameAndStoreState(null, null);
        }


        private void UpdateContentList()
        {

            contentListText = new List<string>();
            contentListCondElm = new List<Constraint>();
            List<Constraint> newCurrentList = new List<Constraint>();

            if (typeSuitableConditionList != null)
                foreach (var item in typeSuitableConditionList)
                {
                    
                    if (item != null && item.possibleConstrElemts != null && uitypeTypeSelector.valueValue.Equals(item.name)){
                        
                        foreach (var item2 in item.possibleConstrElemts)
                        {
                            if (item2 != null)
                            {
                                contentListText.Add(item2.name + " " + ConstantEnum.WorldGenPassDict.AsPreConstraintText(item2.delayToworldGenPass));
                                contentListCondElm.Add(item2);
                            }

                        }
                        break;
                    }
                }            
            currentList = new List<UIConstraint>();
            currentListAsCondElm = new List<Constraint>();

            Updatelist();

        }

        public void UpdateNameAndStoreState(UIMouseEvent mouseEvent, UIElement target)
        {            
            if (CountEntry != null && currenttarget != null && typeTypeSelector != null)
            {
                string txt = "";
                if (proConTypeList != null && proConTypeList.canBeCounted)
                {
                    txt += $"{ConstantEnum.GreaterOrLessEqualClass.ToDString(CountEntry.gole)}";
                    txt += $" {CountEntry.targetValue.GetValueAsString()} ";
                }

                int hasItems = 0;
                foreach (var item in currentListAsCondElm)
                {
                    string t = item.SpecialDisplay(hasItems==0);
                    txt += t;
                    if (t.Length > 0) hasItems++;
                }
                if (hasItems == 0)
                {
                    txt += typeTypeSelector.targetValue.GetValueAsString() + "";
                }

                if (currentListAsCondElm.Count - hasItems > 0)
                {
                    txt += $" with {currentListAsCondElm.Count - hasItems} constr." + (currentListAsCondElm.Count - hasItems > 1 ? "s" : "");
                }

                ConstantEnum.WorldGenPass firstwgMin = ConstantEnum.WorldGenPass.PostWorldGen;
                ConstantEnum.WorldGenPass firstwg = ConstantEnum.WorldGenPass.PostWorldGen;
                //TODO: improve code
                if (typeSuitableConditionList != null)
                    foreach (var item in contentListCondElm)
                    {
                        firstwgMin = (ConstantEnum.WorldGenPass)Math.Min((int)firstwgMin, (int)item.delayToworldGenPass);
                    }
                if (currentListAsCondElm != null)
                    foreach (var item in currentListAsCondElm)
                    {
                        firstwg = (ConstantEnum.WorldGenPass)Math.Min((int)firstwg, (int)item.delayToworldGenPass);
                    }
                if (currentListAsCondElm == null || currentListAsCondElm.Count == 0 || firstwg < firstwgMin)
                    firstwg = firstwgMin;

                txt = txt + " starting " + ConstantEnum.WorldGenPassDict.AsPreConstraintText(firstwg);

                currenttarget.SetName(txt);
                currenttarget?.StoreCurrentState(proConTypeList, this.typeTypeSelector, this.CountEntry, currentListAsCondElm);                
            }


        }

        private void ChangeType(string newst, int param)
        {            
            if (uitypeTypeSelector.valueValue.Equals(newst) ) return;            
            typeTypeSelector.SetValue(newst, param);
            UpdateUItypeTypeSelector(typeTypeSelector);
            UpdateContentList();//if some condition has some special constraint or different first entry point
            UpdateNameAndStoreState(null, null);            

        }
        private void ChangeGol(ConstantEnum.GreaterOrLessEqual gol, Constraint target)
        {
            target.gole = gol;
            UpdateNameAndStoreState(null, null);
        }

        private int clickInProcess = 0;
        public void OpenContextMenu(UIMouseEvent evt, UIElement listeningElement)
        { 

            if (clickInProcess != 1)
            {
                clickInProcess = 0;
                uiSuperSeed?.GenerateContextPanel(contentListText, false)?.OpenContextMenu(evt, this, AddNewText, AddToCurrentList);
            }
            //base.LeftClick(evt);
        }
        private void ClickInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement.Equals(AddNewText) && (uiSuperSeed?.contextMenu?.intputWithContext != null && uiSuperSeed.contextMenu.intputWithContext.focused == true))
            {
                clickInProcess = 1;
            }
        }
        private void ReleaseInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickInProcess == 1)
                clickInProcess = 2;
        }

        private void AddToCurrentList(string which)
        {
            int where = which.IndexOf(ConstantEnum.WorldGenPassDict.WGPassCounterIdentifier);
            string whichSmall = which;
            if (where > 0)
            {
                whichSmall = ((which.Substring(0, where) + which.Substring(where, which.Length - where - ConstantEnum.WorldGenPassDict.WGPassIdentifierLength))).Trim();
            }
            if (which.Length > 0 && contentListText.Contains(which))
            {
                foreach (var item2 in contentListCondElm)
                {
                    if (whichSmall.Equals(item2.name))
                    {
                        UI.UIConstraint propp2 = new UI.UIConstraint(item2);
                        propp2.OnDeletePressed += () => RemoveElem(propp2);
                        propp2.onGolChanged += (gol) => ChangeGol(gol, propp2.condiElm);
                        propp2.onTextChange += (a, b) => UpdateNameAndStoreState(null, null);
                        currentList.Add(propp2);
                        currentListAsCondElm.Add(propp2.condiElm);

                    }
                }
                Updatelist();
                UpdateNameAndStoreState(null, null);
            }
        }



        public void Updatelist()
        {
            this.Clear();
            if (header == ConstantEnum.ConditionTypeNames.Unknown)
                return;
            Add(uiheader);
            Add(sep);
            Add(uitypeTypeSelector);
            if (proConTypeList != null && proConTypeList.canBeCounted) Add(uiCountEntry);
            Add(sep2);

            foreach (var item2 in currentList)
            {

                Add(item2);

            }


            Add(sep3);
            Add(AddNewPanel);
            UpdateNameAndStoreState(null, null);
        }

        private void RemoveElem(UI.UIConstraint target)
        {
            int index = currentList.IndexOf(target);
            if (index >= 0)
            {
                currentList.RemoveAt(index);
                currentListAsCondElm.RemoveAt(index);
            }
            Updatelist();
            UpdateNameAndStoreState(null, null);
        }

        public void SetListEntries(ConditionTypeList conditionTypeList, Constraint typeTypeSelector, Constraint CountEntry, List<PropertyCondition.Constraint> condElmList)
        {
            currentList = new List<UIConstraint>();
            currentListAsCondElm = new List<Constraint>();

            UpdateListEntries(conditionTypeList);

            if (typeTypeSelector != null)
            {

                typeTypeSelector = new Constraint(typeTypeSelector);
                uitypeTypeSelector = new UI.UIConstraint(typeTypeSelector, false, false, true);
                this.typeTypeSelector = uitypeTypeSelector.condiElm;
                uitypeTypeSelector.OnDeletePressed += () => { currentList = new List<UI.UIConstraint>(); currentListAsCondElm = new List<PropertyCondition.Constraint>(); Updatelist(); };
                uitypeTypeSelector.onTextChange += ChangeType;
            }
            else{
                
            }
            if (CountEntry != null)
            {

                uiCountEntry = new UI.UIConstraint(CountEntry, false, true, true);
                this.CountEntry = uiCountEntry.condiElm;
                uiCountEntry.onGolChanged += (gol) => ChangeGol(gol, uiCountEntry.condiElm);
                uiCountEntry.onTextChange += (a, b) => UpdateNameAndStoreState(null, null);
            }

            if (condElmList != null)
                foreach (var item in condElmList)
                {

                    UI.UIConstraint propp2 = new UI.UIConstraint(item);
                    propp2.OnDeletePressed += () => RemoveElem(propp2);
                    propp2.onGolChanged += (gol) => ChangeGol(gol, propp2.condiElm);
                    propp2.onTextChange += (a, b) => UpdateNameAndStoreState(null, null);
                    currentList.Add(propp2);
                    currentListAsCondElm.Add(propp2.condiElm);
                }


        }



        public void ClearConstraintList()
        {
            currenttarget?.StoreCurrentState(proConTypeList, this.typeTypeSelector, this.CountEntry, currentListAsCondElm);
            Clear();
            currenttarget = null;
        }
        UIConditionEntry currenttarget;
        
        public void UpdateConstraintList(PropertyCondition.ConditionTypeList conditionTypeList, PropertyCondition.Constraint typeTypeSelector, PropertyCondition.Constraint CountEntry, List<PropertyCondition.Constraint> condElmList, UIConditionEntry target)
        {
            if (currenttarget != null)
            {

                currenttarget.StoreCurrentState(proConTypeList, this.typeTypeSelector, this.CountEntry, currentListAsCondElm);//state need to be converted to some list first
            }
            currenttarget = target;

            Clear();
            SetListEntries(conditionTypeList, typeTypeSelector, CountEntry, condElmList);
            Updatelist();            
        }




    }
}