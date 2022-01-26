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

        /*public bool StartedFlash = false;
        public int UberTimer;

        public override void PreUpdate()
        {
            if (Main.netMode != NetmodeID.Server) // This all needs to happen client-side!
            {
                if (player.HasBuff(ModContent.BuffType<Ubercharge>()))
                {
                    Filters.Scene.Activate("UberGlow");

                }
                else if(Filters.Scene.HasActiveFilter())
                    Filters.Scene["UberGlow"].Deactivate();
            }
        }*/
    }
}
