//based on
//https://github.com/JavidPack/RecipeBrowser/blob/master/UIElements/NewUITextBox.cs
//jopojelly allowed me to use


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ReLogic.Graphics;
using System;
using Terraria;
//using Terraria.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

//using Terraria.ModLoader;

namespace SuperSeedSearch.UI
{
    internal class UITextInput : UIPanel//UITextPanel<string>
    {
        internal bool focused = false;

        //private int _cursor;
        //private int _frameCount;
        private int _maxLength = 100;

        public string hintText { get; private set; }
        internal string currentString = "";
        private int textBlinkerCount;
        private int textBlinkerState;

        public event Action OnFocus;

        public event Action OnUnfocus;

        public event Action OnTextChanged;

        public event Action OnTabPressed;

        public event Action OnEnterPressed;

        public event Action OnMouseLeftPressed; //###

        //public event Action OnUpPressed;
        internal bool unfocusOnEnter = true;

        internal bool unfocusOnTab = true;

        internal bool unfocusOnClick = true;

        internal bool unfocusInUpdate = true;
        
        public UITextInput(string hintText, string text = "")
        {
            this.hintText = hintText;
            currentString = text;
            SetPadding(0);
            PaddingTop = 4;

            HAlign = 0f;
            VAlign = 0.5f;
            Width.Set(0, 1f);
            Height.Set(30, 0);

            BackgroundColor = Color.Transparent;
            BorderColor = Color.Transparent;

        }

        public override void LeftClick(UIMouseEvent evt)
        {
            Focus();
            base.LeftClick(evt);
        }

        public override void RightClick(UIMouseEvent evt)
        {
            base.RightClick(evt);
            SetText("");
        }

        public void SetUnfocusKeys(bool unfocusOnEnter, bool unfocusOnTab)
        {
            this.unfocusOnEnter = unfocusOnEnter;
            this.unfocusOnTab = unfocusOnTab;
        }


        public void Unfocus()
        {
            if (focused)
            {
                focused = false;
                Main.blockInput = false;

                OnUnfocus?.Invoke();

            }
        }

        public void Focus()
        {
            if (!focused)
            {
                Main.clrInput();
                focused = true;
                Main.blockInput = true;

                OnFocus?.Invoke();
            }
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (!ContainsPoint(MousePosition) && Main.mouseLeft && unfocusInUpdate)
            {
                // TODO, figure out how to refocus without triggering unfocus while clicking enable button. Edit: huh?
                Unfocus();
            }
            base.Update(gameTime);
        }

        public void SetText(string text)
        {
            if (text.ToString().Length > this._maxLength)
            {
                text = text.ToString().Substring(0, this._maxLength);
            }
            if (currentString != text)
            {
                currentString = text;
                OnTextChanged?.Invoke();
            }
        }
        public void SetHintText(string text)
        {
            if (text.ToString().Length > this._maxLength)
            {
                text = text.ToString().Substring(0, this._maxLength);
            }
            if (hintText != text)
            {
                hintText = text;
                OnTextChanged?.Invoke();
            }
        }

        public void SetTextMaxLength(int maxLength)
        {
            this._maxLength = maxLength;
        }


        private static bool JustPressed(Keys key)
        {
            return Main.inputText.IsKeyDown(key) && !Main.oldInputText.IsKeyDown(key);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Rectangle hitbox = GetInnerDimensions().ToRectangle();

            // Draw panel
            base.DrawSelf(spriteBatch);
            //	Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.Yellow);

            if (focused)
            {
                Terraria.GameInput.PlayerInput.WritingText = true;
                Main.instance.HandleIME();
                string newString = Main.GetInputText(currentString);

                if (!newString.Equals(currentString))
                {
                    currentString = newString;
                    OnTextChanged?.Invoke();
                }
                else
                {
                    currentString = newString.Substring(0, Math.Min(newString.Length, _maxLength)); ;
                }

                if (JustPressed(Keys.Tab))
                {
                    if (unfocusOnTab) Unfocus();
                    OnTabPressed?.Invoke();
                }
                if (Main.mouseLeft)
                {
                    Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
                    if (unfocusOnClick && ContainsPoint(MousePosition)) Unfocus();
                    OnMouseLeftPressed?.Invoke();
                }
                if (JustPressed(Keys.Enter))
                {
                    Main.drawingPlayerChat = false;
                    if (unfocusOnEnter) Unfocus();
                    OnEnterPressed?.Invoke();
                }

                if (++textBlinkerCount >= 20)
                {
                    textBlinkerState = (textBlinkerState + 1) % 2;
                    textBlinkerCount = 0;
                }
                Main.instance.DrawWindowsIMEPanel(new Vector2(98f, (float)(Main.screenHeight - 36)), 0f);
            }
            string displayString = currentString;
            if (this.textBlinkerState == 1 && focused)
            {
                displayString = displayString + "|";
            }
            CalculatedStyle space = base.GetDimensions();
            space.Y += base.PaddingTop;
            Color color = Color.Gainsboro;
            if (currentString.Length == 0)
            {
            }
            Vector2 drawPos = space.Position() + new Vector2(0, 0);
            if (currentString.Length == 0)
            {
                color *= 0.65f;
                //Utils.DrawBorderString(spriteBatch, hintText, new Vector2(space.X, space.Y), Color.Gray, 1f);
                //spriteBatch.DrawString(Main.fontMouseText, hintText, drawPos, color);
                //Utils.DrawBorderString(spriteBatch, hintText, new Vector2(space.X, space.Y), color, 1f);
                Utils.DrawBorderString(spriteBatch, " " + hintText, drawPos, color, 1f);
            }
            if (focused || currentString.Length != 0)
            {
                //Utils.DrawBorderString(spriteBatch, displayString, drawPos, Color.White, 1f);                
                //spriteBatch.DrawString(Main.fontMouseText, displayString, drawPos, color);
                Utils.DrawBorderString(spriteBatch, displayString, drawPos, color, 1f);
            }

        }





    }
}