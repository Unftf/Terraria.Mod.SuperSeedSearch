using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;


namespace SuperSeedSearch.UI
{
    public class UIDeleteButton: UIImageButton
    {			

		public UIDeleteButton() : base(ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete")) {			
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch);
		}
	}
}