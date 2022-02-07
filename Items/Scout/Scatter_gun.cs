using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Scout
{
    class Scatter_gun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scattergun");
            Tooltip.SetDefault("\"I'm runnin' circles around ya!\"");
        }
        public override void SetDefaults()
        {
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 16f;
            item.useTime = 20;
            item.useAnimation = 20;
            item.damage = 125;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/scatter_gun_shoot");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 5 + Main.rand.Next(2);
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
                Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
}
