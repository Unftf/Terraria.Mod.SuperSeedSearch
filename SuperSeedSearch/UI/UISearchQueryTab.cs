using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SuperSeedSearch.ConstantEnum;

using SuperSeedSearch.PropertyCondition;
using SuperSeedSearch.UI;

namespace SuperSeedSearch.UI
{
    public class UISearchQueryTab : UIList
    {
        UISuperSeed superSeedUI;
        public UIScrollbar scrollbar;

        List<string> contextListText;

        UIElement uihead;

        UIVerticalSeparator sep, sep3;

        UIText AddNewText;
        UIPanel AddNewPanel;

        List<UIConditionEntry> currentList;
        List<ConditionTypeList> typeConditionList;


        UIConditionEntry currentlySelected;
        public UIMainTabInputEntry inputActiveTab;

        public UISearchQueryTab(UIConditionInterface.SearchQueryTab searchTab, String header, AllConditionTypeList allcondTypeList)
        {
            this.superSeedUI = ModMenuMod.uiSuperSeed;
            this.ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
            this.typeConditionList = new List<ConditionTypeList>(allcondTypeList.typeConditionList);


            contextListText = new List<string>();
            foreach (var item in allcondTypeList.conditionTypesIncluded)
            {
                contextListText.Add(ConstantEnum.ConditionTypeHeader.conditionTypeHeader[item]);
            }
            currentList = new List<UIConditionEntry>();

            scrollbar = new UIScrollbar();
            scrollbar.SetView(100f, 1000f);
            scrollbar.Height.Set(-18f, 1f);
            scrollbar.HAlign = 1.0f;
            scrollbar.Top.Pixels = 6;
            scrollbar.MarginRight = 2;
            scrollbar.MarginTop = 4;
            SetScrollbar(scrollbar);
            
            Helpers.Helpers.SetUIBorderToZero(this);            
            Width.Set(-scrollbar.MarginRight - scrollbar.Width.Pixels, 1f);
            Height.Set(0f, 1f);            
            

            uihead = new UIElement();                    
            uihead.MarginTop = 0;
            uihead.Width.Precent = 1;
            uihead.Height.Pixels = 32;
            //head.BackgroundColor = ConstantEnum.UIColor.BCstartpauseHover;
            uihead.SetPadding(0);
            uihead.MarginBottom = -12;

            UIText uiheader = new UIText(header, 0.5f, true);                    
            uiheader.HAlign = 0.5f;
            uiheader.VAlign = 0.5f;
            uiheader.TextColor = ConstantEnum.UIColor.Header;
            uihead.Append(uiheader);

            inputActiveTab = new UIMainTabInputEntry("", Helpers.EnumHelper<TabIsActive>.GetAllEnumVariablesAsString(), null, true, true, (s) => searchTab.SetActiveState(s) );
            inputActiveTab.Width.Set(256+94,0f);
            inputActiveTab.VAlign = 0.5f;
            inputActiveTab.HAlign = 1f;                 
            uihead.Append(inputActiveTab);
                    

            sep = new UIVerticalSeparator();
            Helpers.Helpers.SetUIBorderToZero(sep);   
            sep.MarginTop = uiheader.Height.Pixels;
            sep.MarginBottom = sep.Height.Pixels + 6;
            sep.Color = ConstantEnum.UIColor.SepMainPanel;            
            sep.Width.Set(-scrollbar.MarginRight, 1);

            sep3 = new UIVerticalSeparator();
            sep3.CopyStyle(sep);
            sep3.Color = ConstantEnum.UIColor.SepMainPanelBelowType;
            sep3.MarginTop = 8;


            AddNewPanel = new UIPanel();
            AddNewPanel.MarginTop = 14;
            AddNewPanel.Width.Set(0, 1);
            AddNewPanel.Height.Set(40, 0);
            AddNewPanel.BorderColor = Color.Transparent;
            AddNewPanel.BackgroundColor = ConstantEnum.UIColor.BGCContextMenu;
            AddNewPanel.OnLeftClick += OpenContextMenu;

            AddNewText = new UIText(ConstantEnum.ConstantsStrings.AddConditionButtonText);
            Helpers.Helpers.SetUIBorderToZero(AddNewText);      

            AddNewText.OnLeftMouseDown += ClickInProcess;
            AddNewText.OnLeftMouseUp += ReleaseInProcess;
            AddNewText.Width.Set(0, 1);
            AddNewText.Height.Set(0, 1);
            AddNewPanel.Append(AddNewText);
            ListPadding = 12;

            Updatelist();


        }

        private int clickInProcess = 0;
        public void OpenContextMenu(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickInProcess != 1)
            {
                clickInProcess = 0;
                superSeedUI?.GenerateContextPanel(contextListText, false)?.OpenContextMenu(evt, this, AddNewText, AddToCurrentList);
            }
            base.LeftClick(evt);
        }
        private void ClickInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement.Equals(AddNewText) && (superSeedUI?.contextMenu?.intputWithContext != null && superSeedUI.contextMenu.intputWithContext.focused == true))
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
            if (which.Length > 0 && contextListText.Contains(which))
            {
                foreach (var item2 in typeConditionList)
                {
                    if (which.Equals(item2.header))
                    {
                        UIConditionEntry centry = new UIConditionEntry(item2);
                        centry.OnDeletePressed += () => RemoveElem(centry);
                        centry.OnCondEntrySelected += SelectEntry;
                        currentList.Add(centry);
                        if (currentlySelected != null)
                            currentlySelected.DeSelect();
                        currentlySelected = centry;                        
                    }
                }
                Updatelist();
                SelectEntry(currentlySelected);
                currentlySelected.SetConstraintList(null, null);
            }
        }
        public void InsertCondition(Condition cond){
            ConditionType conditionType = cond.conditionType;
            string condName = cond.name;
            List<Constraint> constraints = cond.constrElemChosenList; 

            foreach (var item2 in typeConditionList)
            {
                if (item2.conditionType == conditionType){                     
                    UIConditionEntry centry = new UIConditionEntry(item2);
                    centry.OnDeletePressed += () => RemoveElem(centry);
                    centry.OnCondEntrySelected += SelectEntry;                    
                    currentList.Add(centry);                                        
                    centry.ConditionState = constraints;                    
                    Updatelist();                    
                    centry.SetConstraintList(null, null);                    
                    centry.ConditionStateTypeSelctor.targetValue.SetValue(condName);                    
                    centry.CounterState.targetValue.SetValue(cond.targetEvalValue.GetValueAsString());                    
                    centry.CounterState.gole = cond.gole;
                    centry.SetConstraintList(null, null);

                    return;
                }
            }            
        }


        public void SelectEntry(UIConditionEntry entry)
        {            
            foreach (var item in currentList)
            {
                item.DeSelect();
            }

            if (entry != null)
            {
                int ind = currentList.IndexOf(entry);
                if (ind >= 0)
                {
                    currentList[ind].Select();

                }
            }
            currentlySelected = entry;            

        }

        public void Updatelist()
        {
            this.Clear();
            Add(uihead);
            Add(sep);

            for (int i = 0; i < currentList.Count; i++)
            {        
                Add(currentList[i]);
            }

            Add(sep3);
            Add(AddNewPanel);
        }

        private void RemoveElem(UIConditionEntry entry)
        {
            int i = currentList.IndexOf(entry);
            if (i >= 0)
            {
                currentList.RemoveAt(i);
                Updatelist();
                if (currentlySelected!=null && currentlySelected.Equals(entry))
                {
                    currentlySelected = null;
                    ModMenuMod.uiSuperSeed?.ConditionConstraintList?.Clear();

                }

            }
        }

        public List<Condition> ExportAsConditonList(){
            List<Condition> condList = new List<Condition>();
            if(currentList!=null)
             
            foreach(var item in currentList){
                Condition newCond = item.ExportAsCondition();
                if(newCond != null)
                    condList.Add( newCond );              
            }
            

            return condList;
        }

    }


}