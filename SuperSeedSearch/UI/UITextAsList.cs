using Terraria.GameContent.UI.Elements;

namespace SuperSeedSearch.UI
{
    public class UITextAsList : UIList{
            public UIScrollbar scrollbar = null;            
            
            //List<UIText> textaslist = null;
            string text = "";
    
            public UITextAsList(string text){
                ManualSortMethod = Helpers.BasicFunctions.ManualNotSortMethod;
                SetPadding(0);                
                PaddingTop=0;
                HAlign = 0f;
                VAlign = 0f;
                Width.Set(0, 1f);
                Height.Set(0f, 1f);
                PaddingBottom = 0;
                MarginBottom = 0;
                MarginTop = 0;
                ListPadding = 10; 
                
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

                this.text = text;
                //textaslist = new List<UIText>();
   
            }            
            bool ishowing = false;                      
            
            public void Show(){                               
                ishowing = true;
                Clear();       
                string[] words = text.Split(' ');
                //textaslist.Clear();
                UIText line = new UIText("");
                int lasti = 0;   
                line.MarginTop = 4;
                Add(line); 
                double outDim = this.GetOuterDimensions().Width-4;
                for (int i = lasti; i<words.Length;i++){
                    string textOld = line.Text;
                    line.SetText(line.Text+(line.Text.Length==0?"":" ") + words[i]);
                    line.Recalculate();                    
                    if (line.GetOuterDimensions().Width > outDim || words[i].Equals("\n") ){
                        
                        line.SetText(textOld);
                        line = new UIText( words[i].Equals("\n")?"":words[i]);
                        Add(line );
                        lasti = i;                        
                    }                    
                } 
                ishowing = false;
            }


            

    }
}