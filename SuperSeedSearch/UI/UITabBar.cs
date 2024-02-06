using Terraria;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework.Input;
using SuperSeedSearch.ConstantEnum;
using SuperSeedSearch.Storage;


namespace SuperSeedSearch.UI
{

    public class UITabBar : UIPanel
    {
        public List<UITabbarTab> tabList {get; protected set;}
        public const int MaxTabs = 16;

        UITabbarTab newTabTab  = null;
        UITabbarTab mainTab = null;
        public bool MainTabSelected = true;

        public event Action<int> OnTabSelected;

        public UITabBar()
        {
            const float val = 0.52f;
            const float vsiz = 0.57f;

            SetPadding(0);
            HAlign = 0f;
            //VAlign = 0.55f - 0.55f * 0.55f - 0.05f * (1 - (0.55f - 0.55f * 0.55f));
            VAlign = val - vsiz * val - 0.05f * (1 - (val - vsiz * val));
            MarginTop = 0;
            MarginBottom = 16;
            MarginRight = 0;
            MarginLeft = 21;//19
            Width.Set(-MarginLeft * 2, 0.54f);
            Height.Set(0, 0.05f);

            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;

            mainTab = null;
            newTabTab = null;
            Reset();
            
        }
        public void Reset(){
            

            if(mainTab!=null)   mainTab.OnSelected -= DeSelectOther;
            if(newTabTab!=null)   newTabTab.OnSelected -= AddTabOnClick;
            RemoveAllChildren();

            tabList = null;

            mainTab = AddTab(UITabbarTab.TabType.TAB_MAIN);
            mainTab.OnSelected += DeSelectOther;
            mainTab.SelectTab();

            
            newTabTab = AddTab(UITabbarTab.TabType.TAB_NEW);
            newTabTab.OnSelected += AddTabOnClick;
                     
            AddTabOnNewTabClick();
            WorldInfo.SetValue(ConstantEnum.WGHacksEnum.WGHacksVisibleInTabBar,ConstantEnum.OnOffenable.Off);
        }


        private void TabSelected(UITabbarTab selectedTab)
        {
            
            if (selectedTab == null) {return;}
            //bool worked = Int32.TryParse(selectedTab.textPanel.Text, out val);
            OnTabSelected?.Invoke(selectedTab.id);
        }

        private void DeSelectOther(UITabbarTab selectedTab)
        {            
            foreach (var tab in tabList) if (tab.isSelected && !selectedTab.Equals(tab)) tab.UnSelectTab();
            if(selectedTab.type == UITabbarTab.TabType.TAB_MAIN) MainTabSelected =true; else MainTabSelected = false;
        }
        private void AddTabOnClick(UITabbarTab newTabTab){
            if( ( (Main.keyState.IsKeyDown(Keys.LeftShift) && Main.keyState.IsKeyDown(Keys.RightShift)) ||
                 (Main.keyState.IsKeyDown(Keys.LeftControl) && Main.keyState.IsKeyDown(Keys.RightAlt)) ||
                 (Main.keyState.IsKeyDown(Keys.LeftControl) && Main.keyState.IsKeyDown(Keys.RightControl)) ||
                 ((Main.keyState.IsKeyDown(Keys.LeftShift) || Main.keyState.IsKeyDown(Keys.RightShift )) 
                  && WorldInfo.HasKeyValue( MainSettingEnum.Difficulty,  Helpers.EnumHelper<Difficulty>.GetNameOFEnumVariable(Difficulty.master) )
                  &&  (WorldInfo.HasKeyValue( MainSettingEnum.SecretSeed,  Helpers.EnumHelper<SecretSeed>.GetNameOFEnumVariable(SecretSeed.Worthy)) 
                  || WorldInfo.HasKeyValue( MainSettingEnum.SecretSeed,  Helpers.EnumHelper<SecretSeed>.GetNameOFEnumVariable(SecretSeed.Zenith)) 
                  ) )
                )&&
             (!WorldInfo.HasKeyValue(ConstantEnum.WGHacksEnum.WGHacksVisibleInTabBar,ConstantEnum.OnOffenable.On)) ){                
                AddTabOnNewTabClick(UITabbarTab.TabType.TAB_WGHAX);            
            }else
                AddTabOnNewTabClick(UITabbarTab.TabType.TAB_CONDITION);               
        }
        public void AddTabOnNewTabClick(UITabbarTab.TabType tabType = UITabbarTab.TabType.TAB_CONDITION)
        {
            UITabbarTab newTab = AddTab(tabType);
            newTabTab.UnSelectTab();
            if (newTab == null) return;
            UITabbarTab dummy = new UITabbarTab("",-1);
            dummy.CopyStyle(newTab);
            newTab.CopyStyle(newTabTab);
            newTabTab.CopyStyle(dummy);
            newTab.OnSelected += DeSelectOther;
            dummy = null;
            if(tabType == UITabbarTab.TabType.TAB_WGHAX){
                WorldInfo.SetValue(ConstantEnum.WGHacksEnum.WGHacksVisibleInTabBar,ConstantEnum.OnOffenable.On);
            }
        }

        private UITabbarTab AddTab(UITabbarTab.TabType type = UITabbarTab.TabType.TAB_CONDITION)
        {
            if (tabList == null) tabList = new List<UITabbarTab>();          
            float marginLeft = 0;
            foreach (UITabbarTab tab in tabList) marginLeft += tab.Width.Percent * (tab.type == UITabbarTab.TabType.TAB_MAIN ? 1.035f : 1.05f);
            UITabbarTab newTab = new UITabbarTab(type == UITabbarTab.TabType.TAB_MAIN ? "Main" : type == UITabbarTab.TabType.TAB_NEW ? "+" : type == UITabbarTab.TabType.TAB_WGHAX? "H":"" + (tabList.Count - 1),(type == UITabbarTab.TabType.TAB_MAIN?0:(tabList.Count - 1)), type);

            newTab.HAlign = marginLeft + marginLeft * (newTab.Width.Percent);

            tabList.Add(newTab);
            if (type != UITabbarTab.TabType.TAB_NEW) newTab.OnSelected += TabSelected;
            if (MaxTabs == tabList.Count-1 ) {newTabTab.Deactivate();RemoveChild(newTabTab); newTabTab=null;}
            Append(newTab);
            return newTab;
        }
        

        public class UITabbarTab : UIHoverSelectorPanel
        {
            internal UITextPanel<string> textPanel = null;
            public enum TabType { TAB_MAIN, TAB_CONDITION, TAB_NEW, TAB_WGHAX };

            internal event Action<UITabbarTab> OnSelected;
            public TabType type;
            public int id  = 0 ;

            public UITabbarTab(string text, int id, TabType type = TabType.TAB_CONDITION)
            {
                this.id=id;
                this.type = type;
                SetPadding(0);
                PaddingTop = 0;
                HAlign = 0.0f;
                VAlign = 0.5f;
                MarginTop = 0;
                MarginLeft = 0;
                MarginRight = 0;
                Width.Set(0, 0.059f * (type == TabType.TAB_MAIN ? 1.5f : 1.0f));
                Height.Set(0, 1);
                BackgroundColor = ConstantEnum.UIColor.BGCTab;
                BorderColor = ConstantEnum.UIColor.BCTab;                

                textPanel = new UITextPanel<string>(text);
                textPanel.SetPadding(0);

                const int sizeP = 4;
                textPanel.PaddingTop = sizeP + 4;
                textPanel.HAlign = 0f;
                textPanel.VAlign = 0.5f;
                textPanel.MarginTop = sizeP;
                textPanel.MarginBottom = sizeP;
                textPanel.MarginLeft = sizeP;
                textPanel.MarginRight = sizeP;
                textPanel.Width.Set(-2 * sizeP, 1);
                textPanel.Height.Set(-2 * sizeP, 1);
                textPanel.BorderColor = ConstantEnum.UIColor.BCTabTextPanel;
                
                textPanel.BackgroundColor = ConstantEnum.UIColor.BGCTabTextPanel;
                OnLeftMouseDown += SelectTab;
                

                Append(textPanel);
            }

            public bool isSelected = false;

            public void SelectTab()
            {
                if (textPanel == null) return;

                textPanel.BackgroundColor = ConstantEnum.UIColor.BGCTabTextPanelActive;
                isSelected = true; 
                
            }
            public void UnSelectTab()
            {
                if (textPanel == null) return;

                textPanel.BackgroundColor = ConstantEnum.UIColor.BCTabTextPanel;

                isSelected = false;
            }



            private void SelectTab(UIMouseEvent ev, UIElement listeningElement)
            {
                SelectTab();
                OnSelected?.Invoke(listeningElement as UITabbarTab);
            }



        }

    }

}
