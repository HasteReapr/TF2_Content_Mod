﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Sniper
{
    class SMG : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("SMG");
            Tooltip.SetDefault("Sniper's trusty sidearm.");
        }
        public override void SetDefaults()
        {
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 16f;
            item.useTime = 5;
            item.useAnimation = 10;
            item.damage = 25;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(0, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }
    }
}
