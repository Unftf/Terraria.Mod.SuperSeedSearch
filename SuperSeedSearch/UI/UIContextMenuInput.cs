using Terraria;
using Microsoft.Xna.Framework;


namespace SuperSeedSearch.UI
{
    internal class UIContextMenuInput : UITextInput
    {
        UIContextMenu contextMenu;

        internal UIContextMenuInput(UIContextMenu contextMenu, string hintText, string text = "") : base(hintText)
        {
            this.contextMenu = contextMenu;


            this.SetHintText(hintText);
            currentString = text;
            SetPadding(0);
            PaddingTop = 4;

            HAlign = 0f;
            VAlign = 0.0f;
            Width.Set(0, 1f);
            Height.Set(30, 0);

            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;
            base.unfocusInUpdate = false;
            if (contextMenu != null){
                OnUnfocus += contextMenu.UnfocusIt;                
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);            
            if (focused && !ContainsPoint(MousePosition) && (contextMenu == null || !contextMenu.ContainsPoint(MousePosition)) && Main.mouseLeft )
            {
                Unfocus();
                contextMenu?.UnfocusIt();
            }
            base.Update(gameTime);
        }
    }
}