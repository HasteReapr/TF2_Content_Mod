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

namespace TF2_Content.UI
{
    internal class SentryUI : UIState
    {
        public VanillaItemSlotWrapper _vanillaItemSlot1;
        public VanillaItemSlotWrapper _vanillaItemSlot2;
        public VanillaItemSlotWrapper _vanillaItemSlot3;
        public VanillaItemSlotWrapper _vanillaItemSlot4;
        public DragableUIPanel SentryGunPanel;
        public UIHoverImageButton CloseButton;
        public UIImage SentryGun;
        public UIImage SentryLegs;
        public UIImage Healthbar;
        public UIText SentryHealth;
        public UIText SentryAmmo;
        public static bool Visible;
        public string SentryGunImage = "TF2_Content/UI/Sentry_Tier_1_Head";

        private Color gradientA;
        private Color gradientB;

        public override void OnInitialize()
        {
            SentryGunPanel = new DragableUIPanel();
            SentryGunPanel.SetPadding(0);
            SentryGunPanel.Left.Set(400f, 0f);
            SentryGunPanel.Top.Set(100f, 0f);
            SentryGunPanel.Width.Set(250, 0f);
            SentryGunPanel.Height.Set(120, 0f);
            SentryGunPanel.BackgroundColor = new Color(73, 94, 171);

            Texture2D buttonDeleteTexture = ModContent.GetTexture("Terraria/UI/ButtonDelete");
            UIHoverImageButton closeButton = new UIHoverImageButton(buttonDeleteTexture, Language.GetTextValue("LegacyInterface.52"));
            closeButton.Left.Set(220, 0f);
            closeButton.Top.Set(10, 0f);
            closeButton.Width.Set(22, 0f);
            closeButton.Height.Set(22, 0f);
            closeButton.OnClick += new MouseEvent(CloseButtonClicked);
            SentryGunPanel.Append(closeButton);

            SentryLegs = new UIImage(ModContent.GetTexture("TF2_Content/Items/Engineer/Summons/SentryGun_lvl_1_Legs"));
            SentryLegs.Left.Set(10, 0f);
            SentryLegs.Top.Set(80, 0f);
            SentryLegs.Width.Set(126, 0f);
            SentryLegs.Height.Set(18, 0f);
            SentryGunPanel.Append(SentryLegs);

            SentryGun = new UIImage(ModContent.GetTexture(SentryGunImage));
            SentryGun.Left.Set(10, 0f);
            SentryGun.Top.Set(60, 0f);
            SentryGun.Width.Set(126, 0f);
            SentryGun.Height.Set(18, 0f);
            SentryGunPanel.Append(SentryGun);

            SentryHealth = new UIText("Sentry Health: x/maxX");
            SentryHealth.Left.Set(60, 0f);
            SentryHealth.Top.Set(60, 0f);
            SentryHealth.Width.Set(138, 0f);
            SentryHealth.Height.Set(34, 0f);
            SentryGunPanel.Append(SentryHealth);

            Healthbar = new UIImage(ModContent.GetTexture("TF2_Content/UI/Sentry_HealthBar"));
            Healthbar.Left.Set(60, 0f);
            Healthbar.Top.Set(80, 0f);
            Healthbar.Width.Set(176, 0f);
            Healthbar.Height.Set(11, 0f);
            SentryGunPanel.Append(Healthbar);

            SentryAmmo = new UIText("Sentry Reserve: x/100");
            SentryAmmo.Left.Set(60, 0f);
            SentryAmmo.Top.Set(100, 0f);
            SentryAmmo.Width.Set(138, 0f);
            SentryAmmo.Height.Set(34, 0f);
            SentryGunPanel.Append(SentryAmmo);

            _vanillaItemSlot1 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 25 },
                Top = { Pixels = 10 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.ammo == AmmoID.Bullet
            };
            SentryGunPanel.Append(_vanillaItemSlot1);

            _vanillaItemSlot2 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 75 },
                Top = { Pixels = 10 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.ammo == AmmoID.Bullet
            };
            SentryGunPanel.Append(_vanillaItemSlot2);

            _vanillaItemSlot3 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 125 },
                Top = { Pixels = 10 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.ammo == AmmoID.Bullet
            };
            SentryGunPanel.Append(_vanillaItemSlot3);

            _vanillaItemSlot4 = new VanillaItemSlotWrapper(ItemSlot.Context.BankItem, 0.85f)
            {
                Left = { Pixels = 175 },
                Top = { Pixels = 10 },
                ValidItemFunc = item => item.IsAir || !item.IsAir && item.ammo == AmmoID.Bullet
            };
            SentryGunPanel.Append(_vanillaItemSlot4);

            Append(SentryGunPanel);
        }

        private void CloseButtonClicked(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(SoundID.MenuClose);
            Visible = false;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
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
        }

        public override void Update(GameTime gameTime)
        {
            var modPlayer = Main.LocalPlayer.GetModPlayer<TF2_Player>();
            SentryHealth.SetText($"Sentry Health: {modPlayer.SentryHealth}/{modPlayer.SentryHealthMax}");
            SentryAmmo.SetText($"Sentry Reserve: {modPlayer.SentryCurrentAmmo}/{modPlayer.SentrySpawnAmmo}");
        }
    }
}