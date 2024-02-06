using System;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using SuperSeedSearch.ConstantEnum;
using System.ComponentModel.DataAnnotations;    


namespace SuperSeedSearch.UI
{
    public enum TabIsActive{
        active,
        disabled,
        [Display(Name = "global, always need to be true as well")]
        global,
        [Display(Name = "passive, only check if world gen pass reached")]
        passive
    }

    public class UIConditionInterface
    {
        public MainProps mainPropsPanel {get; private set;}
        public UIAllTab currentTab;
        public List<UIAllTab> allTabs{get; private set;}
        UISuperSeed uiSuperSeed;
        public UIConditionInterface(UISuperSeed uiSuperSeed)
        {
            allTabs = null;
            this.uiSuperSeed = uiSuperSeed;
            mainPropsPanel = new MainProps(this.uiSuperSeed);
            this.uiSuperSeed.Append(mainPropsPanel);            
            Reset();                      
        }

        public void Reset(){
            allTabs = new List<UIAllTab>();

            currentTab = mainPropsPanel;
           

            allTabs.Add(currentTab);
            MoveInTab();  
        }

        private static void ManualSortMethodBase(List<UIElement> list)
        {
        }

        abstract public class UIAllTab : UIPanel
        {
            abstract public void DeSelect();
            abstract public void Select();

            abstract public bool IsMainTab();

            abstract public event Action<UIAllTab> OnSelected;
            abstract public event Action<UIAllTab> OnDeSelected;
           
            public TabIsActive IsActiveTab {get;private set;}= TabIsActive.active;

            public void SetActiveState(string state){
                IsActiveTab = Helpers.EnumHelper<TabIsActive>.GetValueFromName(state);                     
            }
              
        }

        public class MainProps : UIAllTab
        {
            public override event Action<UIAllTab> OnSelected;
            public override event Action<UIAllTab> OnDeSelected;

            public MainProps(UISuperSeed superSeedUI)
            {
                
                SetPadding(3);
                PaddingLeft = 5;
                PaddingTop = 2;
                BackgroundColor = ConstantEnum.UIColor.BGCnormal;

                MainOptionList mol = new MainOptionList(this);
                Append(mol);
                Append(mol.scrollbar);               
            }
            public override bool IsMainTab(){return true;}

            public void ChangeValue(Enum from, string to, bool insertNewAsContext = false){                
                if(dictMenuItems.ContainsKey(from)){                    
                    dictMenuItems[from].UpdateText(to, !insertNewAsContext, insertNewAsContext );                    
                }
            }
            public string GetValue(Enum from){                
                if(dictMenuItems.ContainsKey(from)){                    
                    return dictMenuItems[from].GetText;                    
                }
                return "";
            }



            Dictionary<Enum,UIMainTabInputEntry> dictMenuItems = new Dictionary<Enum,UIMainTabInputEntry>();

            private class MainOptionList : UIList
            {
                public UIScrollbar scrollbar;
                public MainOptionList(MainProps mainProps)
                {
                    this.ManualSortMethod = ManualSortMethodBase;

                    UIElement head = new UIElement();                    
                    head.MarginTop = 0;
                    head.Width.Precent = 1;
                    head.Height.Pixels = 32;
                    //head.BackgroundColor = ConstantEnum.UIColor.BCstartpauseHover;
                    head.SetPadding(0);
                    head.MarginBottom = -12;

                    UIText uiheader = new UIText(ConstantEnum.SearchQueryHeader.MainSettings, 0.5f, true);                    
                    uiheader.HAlign = 0.5f;
                    uiheader.VAlign = 0.5f;
                    uiheader.TextColor = ConstantEnum.UIColor.Header;

                    head.Append(uiheader);
                    
                    
                    Add(head);
                    

                    scrollbar = new UIScrollbar();
                    scrollbar.SetView(100f, 1000f);
                    scrollbar.Height.Set(-18f, 1f);
                    scrollbar.HAlign = 1.0f;
                    scrollbar.Top.Pixels = 6;
                    scrollbar.MarginRight = 2;
                    scrollbar.MarginTop = 4;


                    UIVerticalSeparator sep = new UIVerticalSeparator();
                    sep = new UIVerticalSeparator();
                    sep.MarginTop = uiheader.Height.Pixels;
                    sep.MarginBottom = sep.Height.Pixels + 6;
                    sep.Color = ConstantEnum.UIColor.SepMainPanel;
                    sep.SetPadding(0);
                    sep.MarginLeft = 0;
                    sep.MarginRight = 0;
                    sep.Width.Set(-scrollbar.MarginRight, 1);
                    Add(sep);

                    Width.Set(-scrollbar.MarginRight - scrollbar.Width.Pixels, 1f);
                    SetPadding(0);
                    HAlign = 0f;
                    VAlign = 0f;
                    Height.Set(0f, 1f);
                    PaddingBottom = 0;
                    MarginBottom = 0;
                    MarginTop = 0;
                    ListPadding = 12;
                    SetScrollbar(scrollbar);

                    foreach (var item in ConstantEnum.MainSettingDict.mainSetting )
                    {
                        UIMainTabInputEntry entry; 
                        if(item.Key == ConstantEnum.MainSettingEnum.SearchConfigName){
                            entry = new UIMainTabInputEntryLoadSave(item.Value.name, item.Value.options, item.Value.startingVal,item.Value.disableCustomInput, item.Value.doNotSortContentList,item.Value.doAfterInitAndTextChange);
                        }
                        else{
                            entry = new UIMainTabInputEntry(item.Value.name, item.Value.options, item.Value.startingVal,item.Value.disableCustomInput, item.Value.doNotSortContentList,item.Value.doAfterInitAndTextChange);
                        }


                        mainProps.dictMenuItems.Add(item.Key, entry);
                        Add(entry);
                    }
                  
                }
            }

            public override void DeSelect()
            {
                OnDeSelected?.Invoke(this as UIAllTab);
            }
            public override void Select()
            {
                OnSelected?.Invoke(this as UIAllTab);
            }
        }

        public class WGHacksTab :UIAllTab{
            public override event Action<UIAllTab> OnSelected;
            public override event Action<UIAllTab> OnDeSelected;            

            public override bool IsMainTab(){return false;}

            public void ChangeValue(Enum from, string to){                
                if(dictMenuItems.ContainsKey(from)){                    
                    dictMenuItems[from].UpdateText(to, true);                    
                }
            }
            public string GetValue(Enum from){                
                if(dictMenuItems.ContainsKey(from)){                    
                    return dictMenuItems[from].GetText;                    
                }
                return "";
            }
            //todo generalization
            Dictionary<Enum,UIMainTabInputEntry> dictMenuItems = new Dictionary<Enum,UIMainTabInputEntry>();

            public WGHackOptionList hol {get;private set;}
            public WGHacksTab(UISuperSeed superSeedUI)
            {
                SetPadding(3);
                PaddingLeft = 5;
                PaddingTop = 2;
                BackgroundColor = ConstantEnum.UIColor.BGCnormal;

                hol = new WGHackOptionList(this);
                Append(hol);
                Append(hol.scrollbar);                
            }

   
            public class WGHackOptionList : UIList
            {
                public UIScrollbar scrollbar;
                public UIMainTabInputEntry inputActiveTab;
                public WGHackOptionList(WGHacksTab hackTab)
                {
                    this.ManualSortMethod = ManualSortMethodBase;
                    
                    /*UIText uiheader = new UIText(SearchQueryHeader.WGHacks, 0.5f, true);
                    uiheader.MarginTop = 10;
                    uiheader.HAlign = 0.5f;
                    uiheader.TextColor = ConstantEnum.UIColor.Header;
                    Add(uiheader);*/

                    UIElement head = new UIElement();                    
                    head.MarginTop = 0;
                    head.Width.Precent = 1;
                    head.Height.Pixels = 32;
                    //head.BackgroundColor = ConstantEnum.UIColor.BCstartpauseHover;
                    head.SetPadding(0);
                    head.MarginBottom = -12;
                    
                    
                    UIText uiheader = new UIText(ConstantEnum.SearchQueryHeader.WGHacks, 0.5f, true);                    
                    uiheader.HAlign = 0.5f;
                    uiheader.VAlign = 0.5f;
                    uiheader.TextColor = ConstantEnum.UIColor.Header;
                    head.Append(uiheader);

                    inputActiveTab = new UIMainTabInputEntry("", new List<string>{Helpers.EnumHelper<TabIsActive>.GetNameOFEnumVariable(TabIsActive.active),Helpers.EnumHelper<TabIsActive>.GetNameOFEnumVariable(TabIsActive.disabled) }, null, true, true, (s) => hackTab.SetActiveState(s) );            
                    inputActiveTab.Width.Set(256+94,0f);
                    inputActiveTab.VAlign = 0.5f;
                    inputActiveTab.HAlign = 1f;                 
                    head.Append(inputActiveTab);

                    Add(head);


                    scrollbar = new UIScrollbar();
                    scrollbar.SetView(100f, 1000f);
                    scrollbar.Height.Set(-18f, 1f);
                    scrollbar.HAlign = 1.0f;
                    scrollbar.Top.Pixels = 6;
                    scrollbar.MarginRight = 2;
                    scrollbar.MarginTop = 4;


                    UIVerticalSeparator sep = new UIVerticalSeparator();
                    sep = new UIVerticalSeparator();
                    sep.MarginTop = uiheader.Height.Pixels;
                    sep.MarginBottom = sep.Height.Pixels + 6;
                    sep.Color = ConstantEnum.UIColor.SepMainPanel;
                    sep.SetPadding(0);
                    sep.MarginLeft = 0;
                    sep.MarginRight = 0;
                    sep.Width.Set(-scrollbar.MarginRight, 1);
                    Add(sep);

                    foreach (var item in ConstantEnum.WGHackDict.wgHackSetting )
                    {
                        var entry = new UI.UIMainTabInputEntry(item.Value.name, item.Value.options, item.Value.startingVal,item.Value.disableCustomInput, item.Value.doNotSortContentList,item.Value.doAfterInitAndTextChange);
                        hackTab.dictMenuItems.Add(item.Key,entry);                        
                        Add(entry);
                    }

                    Width.Set(-scrollbar.MarginRight - scrollbar.Width.Pixels, 1f);
                    SetPadding(0);
                    HAlign = 0f;
                    VAlign = 0f;
                    Height.Set(0f, 1f);
                    PaddingBottom = 0;
                    MarginBottom = 0;
                    MarginTop = 0;
                    ListPadding = 12;
                    SetScrollbar(scrollbar);

                    
                    
                }


                public new int SortMethod(UIElement e1, UIElement e2){
                    return 1;
                }


            }
            public override void DeSelect()
            {
                OnDeSelected?.Invoke(this as UIAllTab);
            }
            public override void Select()
            {
                OnSelected?.Invoke(this as UIAllTab);
            }
        }



        public class SearchQueryTab : UIAllTab
        {            
            public override event Action<UIAllTab> OnSelected;
            public override event Action<UIAllTab> OnDeSelected;

            public UIMainTabInputEntry inputActiveTab;

            public UISearchQueryTab tabListContent {get;private set;}
            public string header {get;private set;}= "";
            public SearchQueryTab(UISuperSeed superSeedUI, int id)
            {
                SetPadding(3);
                PaddingLeft = 5;
                PaddingTop = 2;
                BackgroundColor = ConstantEnum.UIColor.BGCnormal;
                header = ConstantEnum.SearchQueryHeader.SearchQuery + " " + id;
                tabListContent = new UISearchQueryTab(this, header,  PropertyCondition.AllTypeList.pcAllcondTypeList);

                Append(tabListContent.scrollbar);
                Append(tabListContent);

            }
            public override void DeSelect()
            {
               
                tabListContent.SelectEntry(null);
                ModMenuMod.uiSuperSeed?.ConditionConstraintList?.ClearConstraintList();//replace to out of class
                OnDeSelected?.Invoke(this as UIAllTab);
            }
            public override void Select()
            {
                OnSelected?.Invoke(this as UIAllTab);
            }

            public override bool IsMainTab(){return false;}
        }


        private void MoveInTab(int tabNum = -1)
        {
            const float val = 0.52f;
            const float vsiz = 0.57f;

            UIAllTab tabToMove = null;
            if (tabNum < 0) tabToMove = currentTab;
            else if (allTabs != null && allTabs.Count > tabNum) tabToMove = allTabs[tabNum];
            if (tabToMove == null) return;

            tabToMove.HAlign = 0f;
            tabToMove.VAlign = val;
            tabToMove.MarginLeft = 19;

            //tabToMove.Width.Set(-tabToMove.MarginLeft * 2, 0.58f);
            tabToMove.Width.Set(-tabToMove.MarginLeft * 1.5f, 0.54f);
            tabToMove.Height.Set(0f, vsiz);
            tabToMove.Select();
            tabToMove.Recalculate();
        }

        private void MoveOutTab(int tabNum = -1)
        {
            UIAllTab tabToMove = null;
            if (tabNum < 0) tabToMove = currentTab;
            else if (allTabs != null && allTabs.Count > tabNum) tabToMove = allTabs[tabNum];
            if (tabToMove == null) return;
            tabToMove.DeSelect();
            tabToMove.VAlign = 2.55f;        
            tabToMove.Recalculate();    
        }


        public void ChangeTab(int tabNum)
        {            
            MoveOutTab();                                    
            while (allTabs.Count <= tabNum)
            {
                UIAllTab newct = null;                
                UITabBar.UITabbarTab.TabType  tabType = uiSuperSeed.tabBar.tabList[((allTabs.Count != UITabBar.MaxTabs)?allTabs.Count+1:1)].type;                
                if(tabType == UITabBar.UITabbarTab.TabType.TAB_CONDITION){
                    newct = new SearchQueryTab(uiSuperSeed, allTabs.Count);
                }                
                else if(tabType == UITabBar.UITabbarTab.TabType.TAB_WGHAX){
                    newct = new WGHacksTab(uiSuperSeed);
                }else{
                                  
                }
                
                allTabs.Add(newct);
                uiSuperSeed.Append(newct);
                MoveOutTab(allTabs.Count - 1);                           
            }            
            MoveInTab(tabNum);            
            currentTab = allTabs[tabNum];            

        }

    }

}