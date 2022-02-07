using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Heavy
{
    class Fists : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fists");
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.damage = 150;
            item.useAnimation = 24;
            item.knockBack = 4.5f;

            item.useTime = 24;
            item.shoot = ModContent.ProjectileType<Projectiles.FistsProjectile>(); // this is just the hitbox for the item.
            item.shootSpeed = 1;

            item.value = Item.sellPrice(0, 5, 0, 0);

            item.UseSound = SoundID.Item7;
            item.useStyle = TF2_Player.useStyle; // This makes it animate like fists.
            item.autoReuse = true;
            item.width = 20;
            item.height = 20;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 26;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset;
            }
            return true;
        }
    }
}
