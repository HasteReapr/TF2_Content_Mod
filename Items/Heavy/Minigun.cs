using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TF2_Content.Items.Heavy
{
    class Minigun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minigun");
            Tooltip.SetDefault("\"It costs $400,000 to fire this gun... for twelve seconds.\"\n25% chance not to consume ammo.");
        }

        public override void SetDefaults()
        {
            item.damage = 75;
            item.useTime = 0;
            item.useAnimation = 2;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.shoot = ProjectileID.PurificationPowder;
            item.useAmmo = AmmoID.Bullet;
            item.shootSpeed = 24;
            item.autoReuse = true;
            item.UseSound = SoundID.Item11;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 5);
        }

        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= 0.25f;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;
            return true;
        }

        public override bool UseItem(Player player)
        {
            player.moveSpeed *= 0.25f;
            player.maxRunSpeed *= 0.25f;
            player.accRunSpeed *= 0.1f;
            return true;
        }
    }
}
