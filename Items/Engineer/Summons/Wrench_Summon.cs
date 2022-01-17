using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using TF2_Content.Buffs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TF2_Content.Items.Engineer.Summons
{
    class Wrench_Summon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engineering Wrench");
            Tooltip.SetDefault("Summons a sentry that levels up the more you summon." +
                "\nUse right click to toggle between buffing or mini sentries." +
                "\n1 Summon = Level 1 Sentry" +
                "\n2 Summons = Level 2 Sentry" +
                "\n3 Summons = Level 3 Sentry" +
                "\n>3 Buff Sentry or Summon Mini Sentries" +
                "\nEach buff increases defense, damage and firing speed." +
                "\nUse alt fire to summon mini sentries.");
        }

        public override void SetDefaults()
        {
            item.damage = 300;
            item.knockBack = 3f;
            item.width = 32;
            item.height = 32;
            item.useTime = 36;
            item.useAnimation = 36;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.sellPrice(gold: 30);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item44;
            item.knockBack = 10f;

            item.noMelee = true;
            item.summon = true;
            item.buffType = ModContent.BuffType<Sentry_Buff>();
            item.shoot = ModContent.ProjectileType<Sentry_SummonTierOne>();
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            player.AddBuff(item.buffType, 2);
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierOne>()] > 0)
            {
                type = ModContent.ProjectileType<Sentry_SummonTierTwo>();
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierTwo>()] > 0)
            {
                type = ModContent.ProjectileType<Sentry_SummonTierThree>();
            }
            else if (player.ownedProjectileCounts[ModContent.ProjectileType<Sentry_SummonTierThree>()] > 0)
            {
                type = ModContent.ProjectileType<Sentry_SummonTierThree_Buff>();
            }
            if (player.altFunctionUse == 2)
            {
                type = ModContent.ProjectileType<Sentry_SummonMini>();
            }
            Projectile.NewProjectile(Main.MouseWorld, new Vector2(0, 0), type, item.damage, item.knockBack, player.whoAmI);
            return false;
        }
    }
}