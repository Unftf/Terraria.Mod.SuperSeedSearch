//based on
//https://github.com/JavidPack/RecipeBrowser/blob/master/UIElements/NewUITextBox.cs
//jopojelly allowed me to use


using Microsoft.Xna.Framework;
using System;
using Terraria;
//using Terraria.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System.Collections.Generic;


namespace SuperSeedSearch.UI
{

    public class UIContextMenu : UIPanel
    {
        const float MaxWidthContextRatio = 0.42f;
        const float MinWidthContextPixel = 240;

        internal UIContextMenuInput intputWithContext;

        List<string> contentList;


        UIList uilist;
        UIScrollbar optionListScrollbar;
        UIList inputAsList;

        UIVerticalSeparator sep;

        bool canNotBeWritten = false;
        internal bool disableCustomInput = false;
        public UIContextMenu(List<string> contentList, bool doNotSortList = false)
        {
            this.contentList = new List<string>(contentList);

            double dummy;
            bool isnumberlist = contentList.Count==0?false:Helpers.BasicFunctions.TryToParseAnyDoubleNumber(contentList[0], out dummy);
            if (contentList.Count > 0 && !isnumberlist && !doNotSortList)
                this.contentList.Sort();
            if (!isnumberlist && contentList.Count!=0)
                disableCustomInput = true;

            SetPadding(5);
            HAlign = 0f; //##
            VAlign = 0f;//##
            Width.Set(100, 0f);//##
            Height.Set(0, 0.3f);
            BackgroundColor = ConstantEnum.UIColor.BGCContextMenu;
            intputWithContext = new UIContextMenuInput(this, "", "");

            intputWithContext.OnTextChanged += UpdateContentList;

            sep = new UIVerticalSeparator();
            sep.MarginTop = intputWithContext.Height.Pixels + 6;
            sep.Color = ConstantEnum.UIColor.SepContext;
            //sep.Color = BorderColor;
            sep.SetPadding(0);
            sep.MarginLeft = 0;
            sep.MarginRight = 0;
            sep.Width.Set(0, 1);

            inputAsList = new UIList();
            inputAsList.SetPadding(0);
            inputAsList.HAlign = 0f;
            inputAsList.VAlign = 0f;
            inputAsList.Width.Set(0, 1f);
            inputAsList.Height.Set(0f, 1f);
            inputAsList.Add(intputWithContext);


            this.Append(inputAsList);



            optionListScrollbar = new UIScrollbar();
            optionListScrollbar.SetView(100f, 1000f);
            optionListScrollbar.Height.Set(-intputWithContext.Height.Pixels - 12 - 6 - sep.Height.Pixels - 4, 1f);
            optionListScrollbar.HAlign = 1.0f;
            optionListScrollbar.VAlign = 0.0f;
            optionListScrollbar.Top.Pixels = 6;
            optionListScrollbar.MarginRight = 2;
            optionListScrollbar.MarginTop = intputWithContext.Height.Pixels + 6 + sep.Height.Pixels + 3;


            Append(optionListScrollbar);
            uilist = new UIList();
            uilist.ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
            uilist.SetPadding(0);
            uilist.HAlign = 0f;
            uilist.VAlign = 0f;
            uilist.MarginLeft = -3;
            uilist.Width.Set(-optionListScrollbar.MarginRight - optionListScrollbar.Width.Pixels - uilist.MarginLeft, 1f);

            uilist.MarginTop = intputWithContext.Height.Pixels + 5 + sep.Height.Pixels;
            uilist.PaddingBottom = 4;

            uilist.Height.Set(-intputWithContext.Height.Pixels, 1f);
            uilist.ListPadding = 0;


            //UpdateContentList();
            uilist.SetScrollbar(optionListScrollbar);
            Append(uilist);
            this.Append(sep);

        }
        public static int maxEntries = 10;
        private List<string> currentUIlistMatches = null;
        private void UpdateContentList()
        {

            string curs = intputWithContext.currentString.ToLower();

            uilist.Clear();


            float size = 0;
            currentUIlistMatches = new List<string>();

            foreach (string val in contentList)
            {
                if (curs.Length != 0 && !(val.ToLower()).Contains(curs)) continue;


                currentUIlistMatches.Add(val);

                UITextPanel<string> entry = new UITextPanel<string>(val);
                entry.BackgroundColor = Color.Transparent;
                entry.BorderColor = Color.Transparent;
                entry.SetPadding(0);
                entry.PaddingLeft = 6;
                entry.TextHAlign = 0;
                entry.PaddingTop = 5;

                entry.HAlign = 0;
                entry.Width.Set(0, 1);
                entry.Height.Set(intputWithContext.Height.Pixels - 2, 0);
                entry.MarginTop = 0;
                entry.MarginBottom = 0;
                entry.MarginLeft = 0;
                entry.OnLeftClick += ValueSelected;
                entry.TextColor = Color.Silver;
                entry.OnMouseOver += ChangeColorToHoverIn;
                entry.OnMouseOut += ChangeColorToHoverOut;


                uilist.Add(entry);
                //if(entries++<maxEntries)size += entry.GetOuterDimensions().Height + uilist.ListPadding;
                size += entry.GetOuterDimensions().Height + uilist.ListPadding;
                //size += 21;
            }

            if (size == 0) { size -= 6; HideScrollbar(); }
            else
            {
                RestoreScrollbar();
                //uilist._items[uilist._items.Count-1].MarginBottom = 0;
                uilist._items[0].MarginTop = 6;
                size += uilist._items[0].MarginTop + uilist._items[uilist._items.Count - 1].MarginBottom;
            }
            float offset = uilist.MarginTop + uilist.PaddingTop + 0 * uilist.PaddingBottom + PaddingTop + PaddingBottom;
            float newHeight = Math.Min(size, Math.Min(Main.ScreenSize.Y - Main.mouseY - offset, Main.ScreenSize.Y * 0.4f));
            Height.Set(newHeight + offset, 0f);
            float newWidth = Main.ScreenSize.X * MaxWidthContextRatio;
            //Width.Set(newWidth, 0f);
        }

        private void HideScrollbar()
        {
            //there neeed to be a better way
            optionListScrollbar.MarginRight -= Main.screenWidth * 2;
            sep.MarginLeft -= Main.screenWidth * 2;
        }
        private void RestoreScrollbar()
        {
            //there neeed to be a better way
            optionListScrollbar.MarginRight = 2;
            sep.MarginLeft = 0;
        }

        private void ChangeColorToHoverIn(UIMouseEvent evt, UIElement listeningElement)
        {
            (listeningElement as UITextPanel<string>).TextColor = Color.FloralWhite;
        }
        private void ChangeColorToHoverOut(UIMouseEvent evt, UIElement listeningElement)
        {
            (listeningElement as UITextPanel<string>).TextColor = Color.Silver;
        }


        private void ValueSelected(UIMouseEvent evt, UIElement listeningElement)
        {
            intputWithContext.SetText((listeningElement as UITextPanel<string>).Text);
            intputWithContext.Unfocus();
        }


        private void OpenAt(float posX, float posY)
        {
            this.MarginLeft = posX;
            this.MarginTop = posY;            
        }

        public UIText currentTarget { get; private set; }
        private Action<string> triggerFunAfterSelect = null;

        private void ComputeAndSetNewWidth(float posX)
        {
            float newWidth = Math.Min((Main.ScreenSize.X - 3 * 19) * MaxWidthContextRatio, Main.ScreenSize.X - posX);
            float maxw = intputWithContext.hintText.Length * 12;
            if (contentList != null)
            {
                foreach (var item in contentList)
                {
                    maxw = Math.Max(maxw, item.Length * 12);
                }
            }
            newWidth = Math.Min(newWidth, maxw+12);
            newWidth = Math.Max(newWidth, MinWidthContextPixel);



            Width.Set(newWidth, 0f);
        }

        public void OpenContextMenu(UIMouseEvent evt, UIElement listeningElement, UIText target, Action<string> triggerFunAfterSelect = null, bool startWithTargetText = false)
        {
            this.triggerFunAfterSelect = triggerFunAfterSelect;
            currentTarget = target;
            UpdateContentList();
            var dims = target.GetInnerDimensions();
            OpenAt(dims.X - 5, dims.Y - 8 + 0 * dims.Height);

            intputWithContext.SetText(startWithTargetText?target.Text:"");
            intputWithContext.SetHintText(target.Text);
            ComputeAndSetNewWidth(dims.X);
            intputWithContext.Focus();
            //OpenAt(evt.MousePosition.X - 5, evt.MousePosition.Y - 3, 42);
            this.Recalculate();
        }


        public void HindeIt()
        {
            this.MarginLeft = -Width.Pixels * 2 - intputWithContext.Width.Pixels * 2;
            //this.MarginLeft = Main.ScreenSize.X/2;
            currentTarget = null;

        }

        public void UnfocusIt()
        {
            if (currentTarget != null)
            {

                string replaceText = intputWithContext.currentString.Length == 0 ? currentTarget.Text : intputWithContext.currentString;
                if (disableCustomInput && intputWithContext.currentString.Length != 0)
                {

                    //int ind = currentUIlistMatches.FindIndex(s => s.Contains(intputWithContext.currentString, StringComparison.OrdinalIgnoreCase));//ignores 100% match
                    int ind = -1;
                    int minSize = 1337;
                    for(int i=0; i<currentUIlistMatches.Count;i++){
                            if(currentUIlistMatches[i].Equals(intputWithContext.currentString, StringComparison.OrdinalIgnoreCase) ) { ind = i; break;}
                            else if (currentUIlistMatches[i].Contains(intputWithContext.currentString, StringComparison.OrdinalIgnoreCase) && currentUIlistMatches[i].Length < minSize) { 
                                ind = i;        
                                minSize = currentUIlistMatches[i].Length;
                            }
                    }

                    if (ind >= 0)
                    {
                        replaceText = currentUIlistMatches[ind];
                    }
                    else
                    {
                        replaceText = currentTarget.Text;
                    }
                }
                //bool wasNumber = Helpers.BasicFunctions.TryToParseAnyDoubleNumber(currentTarget.Text);
                bool shouldBeNumber = Helpers.BasicFunctions.TryToParseAnyDoubleNumber(contentList.Count==0?currentTarget.Text:contentList[contentList.Count-1]);
                
                double value;
                bool willBeNumber = Helpers.BasicFunctions.TryToParseAnyDoubleNumber(replaceText, out value);
                bool specialCase = contentList.Contains(replaceText);

                replaceText = shouldBeNumber && (!specialCase || willBeNumber) ? value.ToString() : replaceText;
                

                if (shouldBeNumber == willBeNumber || (shouldBeNumber != willBeNumber && specialCase) ){
                    if (triggerFunAfterSelect != null)
                    {
                        triggerFunAfterSelect(replaceText);
                    }
                    else
                    {
                        currentTarget?.SetText(replaceText);
                    }
                }

                intputWithContext.Unfocus();
                HindeIt();
                this.Recalculate();

            }


        }




    }




}