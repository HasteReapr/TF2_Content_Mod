using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using TF2_Content.Items.Medic;

namespace TF2_Content.UI
{
	internal class MiniUberUI : UIState
	{
		private UIText text;
		private UIElement area;
		private UIImage barFrame;
		private Color gradientA;
		private Color gradientB;
		public static bool canShow;

		public override void OnInitialize()
		{
			area = new UIElement();
			Left.Set(10, 0f);
			Top.Set(10, 0f);
			area.Width.Set(124, 0f);
			area.Height.Set(60, 0f);

			barFrame = new UIImage(ModContent.GetTexture("TF2_Content/UI/UberchargeBeamFrame"));
			barFrame.Left.Set(0, 0f);
			barFrame.Top.Set(0, 0f);
			barFrame.Width.Set(124, 0f);
			barFrame.Height.Set(20, 0f);

			text = new UIText("0/0", 0.8f);
			text.Width.Set(124, 0f);
			text.Height.Set(20, 0f);
			text.Top.Set(25, 0f);
			text.Left.Set(0, 0f);

			gradientA = new Color(76, 0, 0);
			gradientB = new Color(119, 0, 0);

			area.Append(text);
			area.Append(barFrame);
			Append(area);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (!(Main.LocalPlayer.HeldItem.modItem is Mediguns) || !canShow)
				return;

			base.Draw(spriteBatch);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);

			var modPlayer = Main.LocalPlayer.GetModPlayer<MedicPlayer>();
			float quotient = modPlayer.CurrentUber / 100;
			quotient = Utils.Clamp(quotient, 0f, 1f);

			Rectangle hitbox = barFrame.GetInnerDimensions().ToRectangle();
			hitbox.X += 12;
			hitbox.Width -= 24;
			hitbox.Y += 2;
			hitbox.Height -= 4;

			int left = hitbox.Left;
			int right = hitbox.Right;
			int steps = (int)((right - left) * quotient);
			for (int i = 0; i < steps; i += 1)
			{
				float percent = (float)i / (right - left);
				spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
			}
		}
		public override void Update(GameTime gameTime)
		{
			if (!(Main.LocalPlayer.HeldItem.modItem is Mediguns) || !canShow)
				return;

			var modPlayer = Main.LocalPlayer.GetModPlayer<MedicPlayer>();
			text.SetText($"Ubercharge: {(int)modPlayer.CurrentUber} / 100");

			area.Left.Set(Main.mouseX - 10, 0f);
			area.Top.Set(Main.mouseY - 10, 0f);
			area.Recalculate();
		}
	}
}