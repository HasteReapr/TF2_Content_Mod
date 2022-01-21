using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Items.Medic.Projectiles;

namespace TF2_Content.Items.Medic
{
    class SyringeGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Syringe Gun");
            Tooltip.SetDefault("Those needles aren't full of medicine...\nSyringe gun has unlimited ammo.");
        }

        public override void SetDefaults()
        {
            item.damage = 32;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ModContent.ProjectileType<Syringe>();
            item.shootSpeed = 24;
            item.autoReuse = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0);
        }

        int shotsLeft = 125;

        public override void HoldItem(Player player)
        {
            if(shotsLeft >= 125)
            {
                shotsLeft = 125;
            }
            if(shotsLeft <= 0)
            {
                shotsLeft++;
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            shotsLeft--;
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            //Main.NewText($"{shotsLeft}");
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            if(shotsLeft <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
