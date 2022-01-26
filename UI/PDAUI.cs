using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using TF2_Content.Items.Engineer.Summons;
using TF2_Content;

namespace TF2_Content.UI
{
    internal class PDAUI : UIState
    {
        public DragableUIPanel FriendlySelectUI;
        public UIHoverImageButton CloseButton;
        public UIHoverImageButton SelectDisp;
        public UIHoverImageButton SelectTele;
        public UIHoverImageButton SelectTeleExit;
        public UIImage Dispenser;
        public UIImage Teleporter;
        public UIImage TeleporterExit;
        public UIText DispText;
        public UIText TeleText;
        public static bool Visible;
        public string DispenserImage = "TF2_Content/UI/Dispenser";
        public string TeleporterImage = "TF2_Content/UI/Teleporter";

        public override void OnInitialize()
        {
            FriendlySelectUI = new DragableUIPanel();
            FriendlySelectUI.SetPadding(0);
            FriendlySelectUI.Left.Set(400f, 0f);
            FriendlySelectUI.Top.Set(100f, 0f);
            FriendlySelectUI.Width.Set(200, 0f);
            FriendlySelectUI.Height.Set(120, 0f);
            FriendlySelectUI.BackgroundColor = new Color(73, 94, 171);

            Texture2D buttonDeleteTexture = ModContent.GetTexture("Terraria/UI/ButtonDelete");
            UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52"));
            closeButton.Left.Set(170, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            FriendlySelectUI.Append(closeButton);

            Texture2D buttonDispTexture = ModContent.GetTexture("TF2_Content/UI/DispenserButton");
            UIHoverImageButton SelectDisp = new UIHoverImageButton(buttonDispTexture, Language.GetTextValue("Dispenser"));
            SelectDisp.Left.Set(10, 0f);
            SelectDisp.Top.Set(10, 0f);
            SelectDisp.Width.Set(48, 0f);
            SelectDisp.Height.Set(52, 0f);
            SelectDisp.OnClick += new MouseEvent(DispenserButtonClicked);
            FriendlySelectUI.Append(SelectDisp);

            Texture2D buttonTeleTexture = ModContent.GetTexture("TF2_Content/UI/TeleporterButton");
            UIHoverImageButton SelectTele = new UIHoverImageButton(buttonTeleTexture, Language.GetTextValue("Teleporter Entrance"));
            SelectTele.Left.Set(75, 0f);
            SelectTele.Top.Set(50, 0f);
            SelectTele.Width.Set(36, 0f);
            SelectTele.Height.Set(16, 0f);
            SelectTele.OnClick += new MouseEvent(TeleButtonClicked);
            FriendlySelectUI.Append(SelectTele);

            UIHoverImageButton SelectTeleExit = new UIHoverImageButton(buttonTeleTexture, Language.GetTextValue("Teleporter Exit"));
            SelectTeleExit.Left.Set(130, 0f);
            SelectTeleExit.Top.Set(50, 0f);
            SelectTeleExit.Width.Set(36, 0f);
            SelectTeleExit.Height.Set(16, 0f);
            SelectTeleExit.OnClick += new MouseEvent(TeleExitButtonClicked);
            FriendlySelectUI.Append(SelectTeleExit);

            Dispenser = new UIImage(ModContent.GetTexture(DispenserImage));
            Dispenser.Left.Set(10, 0f);
            Dispenser.Top.Set(10, 0f);
            Dispenser.Width.Set(45, 0f);
            Dispenser.Height.Set(52, 0f);
            FriendlySelectUI.Append(Dispenser);

            Teleporter = new UIImage(ModContent.GetTexture(TeleporterImage));
            Teleporter.Left.Set(75, 0f);
            Teleporter.Top.Set(50, 0f);
            Teleporter.Width.Set(126, 0f);
            Teleporter.Height.Set(18, 0f);
            FriendlySelectUI.Append(Teleporter);

            TeleporterExit = new UIImage(ModContent.GetTexture(TeleporterImage));
            TeleporterExit.Left.Set(130, 0f);
            TeleporterExit.Top.Set(50, 0f);
            TeleporterExit.Width.Set(126, 0f);
            TeleporterExit.Height.Set(18, 0f);
            FriendlySelectUI.Append(TeleporterExit);

            DispText = new UIText("Dispenser");
            DispText.Left.Set(5, 0f);
            DispText.Top.Set(85, 0f);
            DispText.Width.Set(138, 0f);
            DispText.Height.Set(34, 0f);
            FriendlySelectUI.Append(DispText);

            TeleText = new UIText("Teleporter 0/1");
            TeleText.Left.Set(5, 0f);
            TeleText.Top.Set(100, 0f);
            TeleText.Width.Set(138, 0f);
            TeleText.Height.Set(34, 0f);
            FriendlySelectUI.Append(TeleText);

            Append(FriendlySelectUI);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuClose);
            Visible = false;
        }

        private void DispenserButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            var modplayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
            modplayer.DispTeleExit = 0;
        }

        private void TeleButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            var modplayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
            modplayer.DispTeleExit = 1;
        }

        private void TeleExitButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuTick);
            var modplayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
            modplayer.DispTeleExit = 2;
        }

        /*protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
            float quotient = (float)modPlayer.SentryHealth / modPlayer.SentryHealthMax;
            quotient = Utils.Clamp(quotient, 0f, 1f);

            Rectangle hitbox = Healthbar.GetInnerDimensions().ToRectangle();
            hitbox.X += 2 - (int)Main.screenPosition.X;
            hitbox.Width -= 4;
            hitbox.Y += 2 - (int)Main.screenPosition.Y;
            hitbox.Height -= 4;

            int left = hitbox.Left;
            int right = hitbox.Right;
            int steps = (int)((right - left) * quotient);
            for (int i = 0; i < steps; i += 1)
            {
                float percent = (float)i / (right - left);
                spriteBatch.Draw(Main.magicPixel, new Rectangle(left + i, hitbox.Y, 1, hitbox.Height), Color.Lerp(gradientA, gradientB, percent));
            }
        }*/

        public override void Update(GameTime gameTime)
        {
            DispText.SetText($"Dispenser {Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<Dispenser_Summon>()]}/1");
            TeleText.SetText($"Teleporter {Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierOne>()]}/2");
        }
    }
}