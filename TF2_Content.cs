using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TF2_Content.UI;
using TF2_Content.Items.Engineer.Summons;
using TF2_Content.Items;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;

namespace TF2_Content
{
    public class TF2_Content : Mod
    {
        internal SentryUI SentryUI;
        private UserInterface _UserInterface;

        internal UberchargeBeamUI UberchargeBeamUI;
        private UserInterface _UberBar;

        internal MiniUberUI miniUberUI;
        private UserInterface _MiniUberBar;

        internal PhlogChargeUI PhlogChargeUI;
        private UserInterface _PhlogCharge;

        internal MiniPhlogUI miniPhlogUI;
        private UserInterface _miniPhlog;

        internal PDAUI PDAUI;
        private UserInterface _PDAUI;

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> UberRef = new Ref<Effect>(GetEffect("Effects/UberShader")); //Path to the effect
                GameShaders.Armor.BindShader(ModContent.ItemType<UberDye>(), new ArmorShaderData(UberRef, "Ubercharged"));
                //Item the dye is binded too                        The ref     The pass name
                Filters.Scene["UberGlow"] = new Filter(new ScreenShaderData(UberRef, "Uberscreen"), EffectPriority.VeryHigh);
                Filters.Scene["UberGlow"].Load();
            }

            if (!Main.dedServ)
            {
                SentryUI = new SentryUI();
                SentryUI.Activate();
                _UserInterface = new UserInterface();
                _UserInterface.SetState(SentryUI);

                UberchargeBeamUI = new UberchargeBeamUI();
                UberchargeBeamUI.Activate();
                _UberBar = new UserInterface();
                _UberBar.SetState(UberchargeBeamUI);

                miniUberUI = new MiniUberUI();
                miniUberUI.Activate();
                _MiniUberBar = new UserInterface();
                _MiniUberBar.SetState(miniUberUI);

                PhlogChargeUI = new PhlogChargeUI();
                PhlogChargeUI.Activate();
                _PhlogCharge = new UserInterface();
                _PhlogCharge.SetState(PhlogChargeUI);

                miniPhlogUI = new MiniPhlogUI();
                miniPhlogUI.Activate();
                _miniPhlog = new UserInterface();
                _miniPhlog.SetState(miniPhlogUI);

                PDAUI = new PDAUI();
                PDAUI.Activate();
                _PDAUI = new UserInterface();
                _PDAUI.SetState(PDAUI);
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            var player = new Player();
            if (SentryUI.Visible)
            {
                _UserInterface?.Update(gameTime);
            }

            if (PDAUI.Visible)
            {
                _PDAUI?.Update(gameTime);
            }

            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierOne>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierTwo>()] > 0 || player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] > 0)
            {
                SentryUI.Visible = false;
            }
            _UberBar?.Update(gameTime);
            _MiniUberBar?.Update(gameTime);
            _PhlogCharge?.Update(gameTime);
            _miniPhlog?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TF_Content: Sentry Gun UI",
                    delegate
                    {
                        if (SentryUI.Visible)
                        {
                            _UserInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TF_Content: PDA UI",
                    delegate
                    {
                        if (PDAUI.Visible)
                        {
                            _PDAUI.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }

            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "TF2_Content: Ubercharge Bar",
                    delegate
                    {
                        _UberBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "TF2_Content: Mini Ubercharge Bar",
                    delegate
                    {
                        _MiniUberBar.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );

                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                   "TF2_Content: Phlogistinator Bar",
                   delegate
                   {
                       _PhlogCharge.Draw(Main.spriteBatch, new GameTime());
                       return true;
                   },
                   InterfaceScaleType.UI)
               );

                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                   "TF2_Content: Mini Phlogistinator Bar",
                   delegate
                   {
                       _miniPhlog.Draw(Main.spriteBatch, new GameTime());
                       return true;
                   },
                   InterfaceScaleType.UI)
               );
            }
        }
    }
}