using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TF2_Content.UI;
using TF2_Content.Items.Engineer.Summons;

namespace TF2_Content
{
	public class TF2_Content : Mod
	{
		internal SentryUI SentryUI;
        private UserInterface _UserInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                SentryUI = new SentryUI();
                SentryUI.Activate();
                _UserInterface = new UserInterface();
                _UserInterface.SetState(SentryUI);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            var player = new Player();
            if (SentryUI.Visible)
            {
                _UserInterface?.Update(gameTime);
            }
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierOne>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierTwo>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] > 0)
            {
                SentryUI.Visible = false;
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TF_Content: Sentry Gun UI",
                    delegate {
                        if (SentryUI.Visible)
                        {
                            _UserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}