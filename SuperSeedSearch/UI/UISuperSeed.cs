
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using SuperSeedSearch.Storage;


namespace SuperSeedSearch.UI
{


    public class UISuperSeed : UIState
    {

        public GenerationProgress generationProgress;
        private UIGenProgressBar progressBar = new UIGenProgressBar();
        private UIHeader progressMessage = new UIHeader();
        public UIStartPauseButton startPause ;

        public UI.UIContextMenu contextMenu = null;

        public UIStatsText statsText = null;
    
        public UIConstraintList ConditionConstraintList;   
        public UITabBar tabBar = null;

        public event Action onExit;
        public event Action onStartWorldGen, onStopWorldGen;
        public UIConditionInterface condInterface = null;
        //public UITabBar tabBar;

        UIPanel constraintPanel;
        UIPanel statsPanel;
        UIPanel textPanel;

        UITextAsList textaslist;

        public UISuperSeed()
        {
            //SetUpUIElements();           
        }
        

        public void SetUpUIElements()
        {
            
            const float val = 0.52f;
            const float vsiz = 0.57f;

            progressBar = new UIGenProgressBar();
            generationProgress = new GenerationProgress();

            progressBar.MarginBottom = 40f;
            progressBar.HAlign = 0.5f;
            progressBar.VAlign = 1f;
            progressMessage.CopyStyle(progressBar);
            progressMessage.MarginBottom = 80f;

            HideProgressBar();
            Append(progressBar);
            Append(progressMessage);

            startPause = new UIStartPauseButton();
            startPause.HAlign = 0.15f;
            startPause.VAlign = 0.075f;
            startPause.OnStartClick += StartWorldGen;
            startPause.OnPauseClick += PauseWorldGen;
            Append(startPause);

            UICloseButton CloseButton = new UICloseButton();
            CloseButton.VAlign = 0f;
            CloseButton.HAlign = 1f;
            CloseButton.OnLeftClick += stopClick;
            Append(CloseButton);
               

            statsPanel = new UIPanel();
            statsText = new UIStatsText();              
            statsPanel.Append(statsText.scrollbar);
            statsPanel.Append(statsText);
            statsPanel.SetPadding(3);
            statsPanel.PaddingLeft = 5;
            statsPanel.PaddingTop = 2;
            statsPanel.HAlign = 1f;
            statsPanel.VAlign = val;
            statsPanel.MarginRight = 19;
            statsPanel.Width.Set(-statsPanel.MarginRight * 1.5f, 0.46f);
            statsPanel.Height.Set(0f, vsiz);
            statsPanel.BackgroundColor = new Color(73, 94, 171);
            Append(statsPanel);

            textPanel = new UIPanel();    
            textPanel.SetPadding(3);
            textPanel.PaddingLeft = 5;
            textPanel.PaddingTop = 2;
            textPanel.HAlign = 1f;
            textPanel.VAlign = val;
            textPanel.MarginRight = 19;
            textPanel.Width.Set(-textPanel.MarginRight * 1.5f, 0.46f);
            textPanel.Height.Set(0f, vsiz);
            textPanel.BackgroundColor = new Color(73, 94, 171);
            textaslist = new UITextAsList(ConstantEnum.MultiLineText.Welcome);
            textPanel.Append(textaslist.scrollbar);
            textPanel.Append(textaslist);
            Append(textPanel);
            

            constraintPanel = new UIPanel();
            ConditionConstraintList = new UIConstraintList(this);
            constraintPanel.Append(ConditionConstraintList.scrollbar);
            constraintPanel.Append(ConditionConstraintList);
            constraintPanel.SetPadding(3);
            constraintPanel.PaddingLeft = 5;
            constraintPanel.PaddingTop = 2;
            constraintPanel.HAlign = 1f;
            constraintPanel.VAlign = val;
            constraintPanel.MarginRight = 19;
            constraintPanel.Width.Set(-constraintPanel.MarginRight * 1.5f, 0.46f);
            constraintPanel.Height.Set(0f, vsiz);
            constraintPanel.BackgroundColor = new Color(73, 94, 171);
            Append(constraintPanel);




            condInterface = new UIConditionInterface(this);

            tabBar = new UITabBar();
            tabBar.OnTabSelected += condInterface.ChangeTab;
            Append(tabBar); 

            textaslist.Show();
            ShowTextPanel();        

        }

        private bool isGen = false;
        public void StartWorldGen()
        {            
            onStartWorldGen?.Invoke();
            isGen = true;
        }
        public void PauseWorldGen()
        {
            onStopWorldGen?.Invoke();
            isGen = false;
        }



        public UIContextMenu GenerateContextPanel(List<string> contentList, bool disableCustomInput = false, bool doNotSortList = false, bool forceEnableCustomInput = false)
        {
            if (contextMenu != null)
            {
                contextMenu.RemoveAllChildren();
                this.RemoveChild(contextMenu);
                contextMenu?.Remove();
            }
            contextMenu = new UIContextMenu(contentList, doNotSortList);       
            if(disableCustomInput)     
                contextMenu.disableCustomInput = disableCustomInput;
            if(forceEnableCustomInput && !disableCustomInput) contextMenu.disableCustomInput = disableCustomInput;

            //contextMenu.HideIt();            
            Append(contextMenu);
            return contextMenu;
        }


        static double lastTotalProgress = -1;
        static string lastStringProgress = "";
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);    
        }
        public override void Update(GameTime gameTime){
            base.Update(gameTime);
            if(isGen && ModMenuMod.wGPassChanger != null && ModMenuMod.wGPassChanger.lastWorldGenPassID == ConstantEnum.WorldGenPass.PreWorldGen)
                generationProgress.Message = "Seed "+ModMenuMod.wGPassChanger.currentSeed;
            if (generationProgress == null || !isGen || (lastTotalProgress == generationProgress.TotalProgress && lastStringProgress.Equals(generationProgress.Message)) || generationProgress.Message.Length == 0) return;
            progressBar.SetProgress((float)generationProgress.TotalProgress, (float)generationProgress.Value);//###### tmodError? should be double
            progressMessage.Text = generationProgress.Message;
            lastTotalProgress = generationProgress.TotalProgress;
        }


        public void ShowStatsPanel(){
            statsPanel.HAlign = Math.Abs(statsPanel.HAlign);
            constraintPanel.HAlign = -Math.Abs(constraintPanel.HAlign);
            textPanel.HAlign = -Math.Abs(textPanel.HAlign);    

            
            constraintPanel.Recalculate();
            textPanel.Recalculate();
            statsPanel.Recalculate();
        }
        public void ShowConstraintPanel(){
            constraintPanel.HAlign = Math.Abs(constraintPanel.HAlign);
            statsPanel.HAlign = -Math.Abs(statsPanel.HAlign);
            textPanel.HAlign = -Math.Abs(textPanel.HAlign); 

            statsPanel.Recalculate();            
            textPanel.Recalculate();                      
            constraintPanel.Recalculate();
        }
        public void ShowTextPanel(){
            textPanel.HAlign = Math.Abs(textPanel.HAlign);    
            constraintPanel.HAlign = -Math.Abs(constraintPanel.HAlign);
            statsPanel.HAlign = -Math.Abs(statsPanel.HAlign);

            statsPanel.Recalculate();
            constraintPanel.Recalculate();
            textPanel.Recalculate();                                         
        }

        public void HideProgressBar()
        {
            if(progressBar==null) return;
            progressBar.VAlign = -progressBar.VAlign;
            progressMessage.VAlign = -progressMessage.VAlign;
            progressBar.Recalculate();
            progressMessage.Recalculate();
        }
        public void UnHideProgressBar()
        {
            if(progressBar==null) return;
            progressBar.VAlign = -progressBar.VAlign;
            progressMessage.VAlign = -progressMessage.VAlign;
            progressBar.Recalculate();
            progressMessage.Recalculate();
        }

        private void stopClick(UIMouseEvent evt, UIElement listeningElement)
        {
            condInterface = null;
            progressBar= null;
            startPause = null;
            contextMenu = null;
            statsText = null;
            tabBar = null;
            WorldInfo.Info.Clear();
            PassMemory.Clear();

            onExit?.Invoke();
        }



    }
}