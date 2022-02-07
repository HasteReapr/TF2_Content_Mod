using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.AllClass
{
    class Shotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shotgun");
            Tooltip.SetDefault("\"Bang! Pow! Boom!\"");
        }
        public override void SetDefaults()
        {
            item.useAmmo = AmmoID.Bullet;
            item.shoot = ProjectileID.PurificationPowder;
            item.shootSpeed = 16f;
            item.useTime = 45;
            item.useAnimation = 45;
            item.damage = 125;
            item.autoReuse = true;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/shotgun_shoot");
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int numberProjectiles = 7;
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
