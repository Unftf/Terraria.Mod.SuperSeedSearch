using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;


namespace SuperSeedSearch.UI
{
    public class UIStatsText : UIList{
            public UIScrollbar scrollbar = null;
            UIText uiheader = null;
            UIVerticalSeparator sep = null;
            Dictionary<string, Stat> statsList = null;
    
            public UIStatsText(){
                ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
                SetPadding(0);
                HAlign = 0f;
                VAlign = 0f;
                Width.Set(0, 1f);
                Height.Set(0f, 1f);
                PaddingBottom = 0;
                MarginBottom = 0;
                MarginTop = 0;
                ListPadding = 20; 
                
                scrollbar = new UIScrollbar();
                scrollbar.SetView(100f, 1000f);
                scrollbar.Height.Set(-18f, 1f);
                scrollbar.HAlign = 1.0f;
                scrollbar.Top.Pixels = 6;
                scrollbar.MarginRight = 2;
                scrollbar.MarginTop = 4;


                Helpers.Helpers.SetUIBorderToZero(this);
                Width.Set(-scrollbar.MarginRight - scrollbar.Width.Pixels, 1f);
                Height.Set(0f, 1f);


                SetScrollbar(scrollbar);
                
                uiheader = new UIText(ConstantEnum.SearchQueryHeader.Stats, 0.5f, true);
                uiheader.MarginTop = 10;
                uiheader.HAlign = 0.5f;
                uiheader.TextColor = ConstantEnum.UIColor.Header;
                //UpdateListEntries(null);

                sep = new UIVerticalSeparator();
                Helpers.Helpers.SetUIBorderToZero(sep);
                sep.MarginTop = (uiheader.Height.Pixels + 6)*0-ListPadding/2;
                sep.MarginBottom = sep.Height.Pixels + 6;
                sep.Color = ConstantEnum.UIColor.SepMainPanel;
                sep.Width.Set(-scrollbar.MarginRight, 1);       

                statsList = new Dictionary<string, Stat>();     

   
            }
            public class Stat : UIElement{
                internal static Color backgroundColor = new Color(0, 0, 0, 0);
                public UIText UIvalue = null;

                public Stat(string name, string value){
                    SetPadding(0);                       
                    MarginTop = 0;                                      
                    HAlign = 0f;
                    VAlign = 0f;
                    Width.Set(0, 1);
                    Height.Set(0, 1);
                    valueNew = value;
                    
                    UIGreaterOrLess greaterLessEqualUI = new UIGreaterOrLess(ConstantEnum.GreaterOrLessEqual.Equal);
                    UIElement leftparaGoLE = new UIElement();
                    Helpers.Helpers.SetUIBorderToZero(leftparaGoLE);          
                    leftparaGoLE.Height.Set(0, 1);
                    leftparaGoLE.Width.Set(100, 0);
                    
                    greaterLessEqualUI.HAlign = 1f;
                    greaterLessEqualUI.VAlign = 0f;
                    greaterLessEqualUI.MarginTop = 0;
                    UIText textHead = new UIText(name);
                    textHead.HAlign = 0f;
                    Append(textHead);
                    
                    leftparaGoLE.MarginTop = textHead.GetInnerDimensions().Height;
                    leftparaGoLE.Append(greaterLessEqualUI);

            
                    Append(leftparaGoLE);

                    Helpers.Helpers.SetUIBorderToZero(this);

                    Width.Set(0, 1f);
                    Height.Set(greaterLessEqualUI.Height.Pixels + 0 * greaterLessEqualUI.MarginTop, 0);


                    UIElement inputbox = new UIElement();       
                        
                    Helpers.Helpers.SetUIBorderToZero(inputbox);                        
                    inputbox.MarginLeft = leftparaGoLE.Width.Pixels;            
                    inputbox.PaddingLeft = 7;            
                    inputbox.VAlign = 0.0f;
                    inputbox.HAlign = 0f;
                    inputbox.Width.Set(-inputbox.MarginLeft - inputbox.PaddingLeft-16, 1f);
                    inputbox.Height.Set(0, 1);                     
                    //inputbox.BackgroundColor = ConstantEnum.UIColor.BGCnormal;
                    //inputbox.BackgroundColor = ConstantEnum.UIColor.BGCinputPanel;
                    //inputbox.BorderColor = Color.Transparent;                       

                    UIvalue = new UIText(valueNew);
                    //UIvalue.TextColor = ConstantEnum.UIColor.inputPanelText;
                    UIvalue.MarginTop = 7;

                    UIList toPreventOverFlow = new UIList();
                    Helpers.Helpers.SetUIBorderToZero(toPreventOverFlow);
                    toPreventOverFlow.Add(UIvalue);
                    toPreventOverFlow.VAlign = 0.5f;
                    toPreventOverFlow.Width.Set(0, 1f);
                    toPreventOverFlow.Height.Set(0, 1f);

                    inputbox.Append(toPreventOverFlow);
                    inputbox.MarginTop = textHead.GetInnerDimensions().Height;
                    Append(inputbox);
                }
                string valueNew = "";
                public void SetValue(string value){
                    valueNew = value;
                    //UIvalue.SetText(value);
                }
                public override void Draw(SpriteBatch spriteBatch)
                {
                    
                    if(!UIvalue.Text.Equals(valueNew)){
                            UIvalue.SetText(valueNew);
                    }
                    base.Draw(spriteBatch);
                }
            }
            public void SetStat(string name, int value, string key = ""){SetStat(name, ""+value,key);}

            public void SetStat(string name, string value, string key = ""){
                string dkey = key.Length==0?name:key;
                if (!statsList.ContainsKey(dkey)) { statsList.Add(dkey, new Stat(name, value)); }
                else
                {
                    statsList[dkey].SetValue(value);
                }
            }      
            public void ClearAllStats(){
                Clear();
                statsList.Clear();
            }

            bool ishowing = false;                      
            public bool isEmpty(){
                return statsList==null || statsList.Count==0;
            }
            public void Show(){
                //if(ishowing) return;                
                ishowing = true;
                Clear();
                if(statsList==null || statsList.Count==0) return;
                Add(uiheader);
                Add(sep);                
                foreach(var stat in statsList){
                    Add(stat.Value);
                }
                ishowing = false;
            }


            

    }
}