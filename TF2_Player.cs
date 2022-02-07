using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Buffs;

namespace TF2_Content
{
    class TF2_Player : ModPlayer
    {
        //Any code using this is adapted from the weapon out mod, it's all used for heavy's fists.
        public const int useStyle = 102115116; //http://www.unit-conversion.info/texttools/ascii/ with fst to ASCII numbers

        public int SentrySpawnAmmo = 100;
        public int SentryCurrentAmmo = 100;
        public int SentryHealth = 100;
        public int SentryHealthMax = 750;

        public int DispenserMaterial = 50;
        public int DispenserHealth = 100;
        public int DispenserHealthMax = 500;

        public int TeleporterHealth = 100;
        public int TeleporterHealthMax = 250;

        public int DispTeleExit = 0; // 0 is Dispenser, 1 is Teleporter Entrace, 2 is teleporter exit.

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (player.HasBuff(ModContent.BuffType<Ubercharge>()))
                return false;
            else
                return true;
        }
        public override void PreUpdate()
        {
            if (player.HasBuff(ModContent.BuffType<Ubercharge>()) && Main.netMode != NetmodeID.Server)
            {
                Filters.Scene.Activate("UberGlow");
                Filters.Scene["UberGlow"].GetShader().UseProgress(60);
            }
            else if (!player.HasBuff(ModContent.BuffType<Ubercharge>()) && Filters.Scene["UberGlow"].Active && Main.netMode != NetmodeID.Server)
            {
                Filters.Scene["UberGlow"].Deactivate();
            }
        }

        public override void PostUpdate()
        {
            FistBodyFrame();
            ShowFistHandOn();
        }

        //this code is adapted from Weapon Out, and used for heavy's fists.
        private void ShowFistHandOn()
        {
            if (player.itemAnimation > 0)
            {
                if (player.HeldItem.useStyle == useStyle)
                {
                    if (player.HeldItem.handOnSlot > 0)
                    {
                        player.handon = player.HeldItem.handOnSlot;
                        player.cHandOn = 0;
                    }
                    if (player.HeldItem.handOffSlot > 0)
                    {
                        player.handoff = player.HeldItem.handOffSlot;
                        player.cHandOff = 0;
                    }
                }
            }
        }

        private void FistBodyFrame()
        {
            // Don't show when not attacking
            if (player.itemAnimation == 0) return;

            // Don't apply to non fists
            if (player.HeldItem.useStyle != useStyle) return;

            // Animation normal
            float anim = player.itemAnimation / (float)player.itemAnimationMax;



            //wind up animation
            if (anim > 0.7f) player.bodyFrame.Y = player.bodyFrame.Height * 10;
            else if (anim > 0.66f)
            {
                player.bodyFrame.Y = player.bodyFrame.Height * 17;
            }
            //punch
            else if (anim > 0.3f)
            {
                if (Math.Abs(player.itemRotation) > Math.PI / 4 && Math.Abs(player.itemRotation) < 3 * Math.PI / 4)
                {
                    if (player.itemRotation * player.direction * player.gravDir > 0)
                    {
                        //Down low
                        player.bodyFrame.Y = player.bodyFrame.Height * 4;
                    }
                    else
                    {
                        //Up high
                        player.bodyFrame.Y = player.bodyFrame.Height * 2;
                    }
                }
                else
                {
                    //along the middle
                    player.bodyFrame.Y = player.bodyFrame.Height * 3;
                }
            }
            //wind back
            else player.bodyFrame.Y = player.bodyFrame.Height * 17;
        }
    }
}
