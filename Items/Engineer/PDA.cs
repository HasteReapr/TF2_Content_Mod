using Terraria.ModLoader;
using Terraria.ID;
using TF2_Content.UI;
using Terraria;
using TF2_Content.Items.Engineer.Summons;
using Microsoft.Xna.Framework;

namespace TF2_Content.Items.Engineer
{
    class PDA : ModItem
    {
        public override string Texture => "TF2_Content/Items/Placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Engineer's PDA");
            Tooltip.SetDefault("Right click to change what building is placed.");
        }

        int[] PDAShoot =
        {
            ModContent.ProjectileType<Dispenser_Summon>(),
        };

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 15;
            item.useAnimation = 15;
            item.damage = 0;
            item.knockBack = 0;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = PDAShoot[Main.LocalPlayer.GetModPlayer<TF2_Player>().DispTeleExit];
            if(Main.LocalPlayer.ownedProjectileCounts[ModContent.ProjectileType<Dispenser_Summon>()] > 0 && Main.LocalPlayer.GetModPlayer<TF2_Player>().DispTeleExit == 0)
            {
                Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, item.damage, item.knockBack, player.whoAmI, 1);
            }
            else
            {
                Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, item.damage, item.knockBack, player.whoAmI);
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool UseItem(Player player)
        {
            if(player.altFunctionUse == 2)
            {
                PDAUI.Visible = !PDAUI.Visible;
                item.shoot = ProjectileID.PurificationPowder;
                item.useStyle = ItemUseStyleID.HoldingUp;
            }
            else
            {
                item.useStyle = ItemUseStyleID.SwingThrow;
            }
            return true;
        }
    }
}
