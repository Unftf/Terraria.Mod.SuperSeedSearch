using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;

using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;



namespace SuperSeedSearch.UI
{
    public class UIMainTabInputEntry : UIElement
    {


        internal static Color backgroundColor = new Color(0, 0, 0, 0);


        UISuperSeed SuperSeedUI;
        public string valueName = "";
        public string valueValue = "";

        List<string> contentList = null;

        public UIPanel inputbox;

        public event Action<string> onTextChange;
        Action<string> doAfterInitAndTextChange;
        internal UIText UIvalue;

        bool disableInput;
        bool doNotSortList;
        
        public UIMainTabInputEntry(string valueName, List<string> contentList, PropertyCondition.ValueStringAndOrDouble valval = null, bool disableInput = false, bool doNotSortList = false, Action<string> doAfterInitAndTextChange = null)
        {
            if (contentList == null) contentList = new List<string>();
            this.contentList = contentList;
            this.doAfterInitAndTextChange = doAfterInitAndTextChange;

            this.valueName = valueName;
            this.SuperSeedUI = ModMenuMod.uiSuperSeed;
            this.disableInput = disableInput;
            this.doNotSortList = doNotSortList;

            SetPadding(0);
            HAlign = 0f;
            VAlign = 0f;
            Width.Set(0, 1);
            Height.Set(32, 0);


            UIElement leftValueNameSide = new UIElement();
            Helpers.Helpers.SetUIBorderToZero(leftValueNameSide);
            leftValueNameSide.MarginLeft = 5;            
            leftValueNameSide.Height.Set(0, 1);
            leftValueNameSide.Width.Set(240, 0);



            UIText uiValueName = new UIText(valueName);
            uiValueName.HAlign = 0f;
            uiValueName.VAlign = 0.5f;
            uiValueName.MarginTop = 10;

            uiValueName.SetPadding(4);
            uiValueName.HAlign = 0f;
            uiValueName.MarginTop = 7;
            uiValueName.MarginBottom = 0;
            uiValueName.Height.Set(0, 1);
            leftValueNameSide.Append(uiValueName);

            Append(leftValueNameSide);


            inputbox = new UIPanel();
            Helpers.Helpers.SetUIBorderToZero(inputbox);            
            inputbox.MarginLeft = leftValueNameSide.Width.Pixels + 2 * leftValueNameSide.MarginLeft;            
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

            valueValue = valval == null ? (contentList == null || contentList.Count == 0 ? "" : contentList[0]) : valval.GetValueAsString();

            UIvalue = new UIText(valueValue);
            UIvalue.TextColor = ConstantEnum.UIColor.inputPanelText;
            UIvalue.MarginTop = 7;


            UIList toPreventOverFlow = new UIList();

            toPreventOverFlow.Add(UIvalue);
            Helpers.Helpers.SetUIBorderToZero(toPreventOverFlow);               
            toPreventOverFlow.VAlign = 0.5f;
            toPreventOverFlow.Width.Set(0, 1f);
            toPreventOverFlow.Height.Set(0, 1f);

            inputbox.Append(toPreventOverFlow);


            Append(inputbox);
            doAfterInitAndTextChange?.Invoke(valueValue);
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
                SuperSeedUI?.GenerateContextPanel(contentList, disableInput, doNotSortList, !disableInput)?.OpenContextMenu(evt, this, UIvalue, (t) => UpdateText(t), startWithTargetText);    
            }            
        }

        int clickInProcess = 0;
        private void ClickInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (listeningElement.Equals(inputbox) && (SuperSeedUI?.contextMenu?.intputWithContext != null && SuperSeedUI.contextMenu.intputWithContext.focused == true))
            {
                clickInProcess = 1;
            }
        }
        private void ReleaseInProcess(UIMouseEvent evt, UIElement listeningElement)
        {
            if (clickInProcess == 1)
                clickInProcess = 2;
        }

        public void UpdateText(string newText, bool insertOldAsFirstToContext = false, bool insertNewTextAs2ndContext = false)
        {
            if (newText.Length != 0)
            {                
                if((insertOldAsFirstToContext || insertNewTextAs2ndContext ) && !contentList.Contains(!insertNewTextAs2ndContext?UIvalue.Text: newText)){
                    if(insertNewTextAs2ndContext){ contentList[2] = newText;
                        return;
                    }
                    else{
                        int where = 0;double dummy=0;
                        bool oldWasNumber = Double.TryParse(UIvalue.Text,out dummy);
                        for(int i=0;i<contentList.Count;i++){
                            if( Double.TryParse(contentList[i],out dummy) == oldWasNumber){
                                where = i; break;
                            }
                        }                    
                        contentList[where] = UIvalue.Text;
                    }
                }
                UIvalue.SetText(newText);
                onTextChange?.Invoke(newText);
                doAfterInitAndTextChange?.Invoke(newText);
            }
        }

        public string GetText => UIvalue.Text;      

    }

    public class UIMainTabInputEntryLoadSave : UIMainTabInputEntry
    {
        public UIMainTabInputEntryLoadSave(string valueName, List<string> contentList, PropertyCondition.ValueStringAndOrDouble valval = null, bool disableInput = false, bool doNotSortList = false, Action<string> doAfterInitAndTextChange = null)
                       :base(valueName,contentList,valval,disableInput,doNotSortList,doAfterInitAndTextChange)
        {
                //Todo better code
                UIImage loadimg = new UIImage(ModContent.Request<Texture2D>(ModMenuMod.modName+"/img/configLoad"));
                UIElement load = new UIElement();                            
                //UICloseButton load = new UICloseButton();
                load.SetPadding(0f);
                load.MarginRight = 40;
                load.MarginLeft = 0;
                load.Width.Set(32,0f);
                load.Height.Set(42,0f);
                load.VAlign = 0.5f;
                load.HAlign = 1f;                                                   
                load.OnLeftClick += Load;
                load.Append(loadimg);

                UIImage saveimg = new UIImage(ModContent.Request<Texture2D>(ModMenuMod.modName+"/img/configSave"));     
                UIElement save = new UIElement();                            
                save.SetPadding(0f);
                save.MarginRight = 2;
                save.MarginLeft = 0;
                save.Width.Set(32,0f);
                save.Height.Set(42,0f);
                save.VAlign = 0.5f;
                save.HAlign = 1f;//todod read imag size
                save.OnLeftClick += Save;                            
                save.Append(saveimg);

                inputbox.Width.Set(-inputbox.MarginLeft - inputbox.PaddingLeft -72, 1f);                                            
                                                                        
                Append(load);
                Append(save);
        }

        public void Load(UIMouseEvent mev, UIElement target ){
            Storage.LoadStoreConfigSeedsTxt.LoadConfig(UIvalue.Text);
        }
        public void Save(UIMouseEvent mev, UIElement target ){
            Storage.LoadStoreConfigSeedsTxt.SaveConfig(UIvalue.Text);
        }

    }

}