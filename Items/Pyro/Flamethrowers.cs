using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content;
using TF2_Content.Items.Pyro.Projectiles;

namespace TF2_Content.Items.Pyro
{
	// This class handles everything for our custom damage class
	// Any class that we wish to be using our custom damage class will derive from this class, instead of ModItem
	public abstract class Flamethrowers : ModItem
	{
		public override bool CloneNewInstances => true;
		public bool Airblast = true;
		public int AirblastTimer = 30;

		public virtual void SafeSetDefaults()
		{
		}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 5;
			item.useAnimation = 15;
			item.shoot = ProjectileID.Flames;
			item.shootSpeed = 8;
			item.autoReuse = true;
			item.crit = 2;
			item.noMelee = true;
		}


		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}

		public override bool AltFunctionUse(Player player)
        {
			return Airblast;
        }

		public override bool UseItem(Player player)
		{
			if (player.altFunctionUse == 2 && Airblast)
			{
				item.useTime = 30;
				item.useAnimation = 30;
				item.knockBack = 50;
				item.shoot = ModContent.ProjectileType<Airblast>();
			}
			else
			{
				SetDefaults();
			}
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if(player.altFunctionUse == 2 && Airblast)
            {
				Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
				if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
				{
					position += muzzleOffset;
				}
				Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<Airblast>(), 1, 75 * player.direction, player.whoAmI);
			}
            else if (Airblast)
            {
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
				speedX = perturbedSpeed.X;
				speedY = perturbedSpeed.Y;
				Projectile.NewProjectile(position, player.DirectionTo(Main.MouseWorld) * 8, ProjectileID.Flames, item.damage, item.knockBack, player.whoAmI);
			}
			return !Airblast;
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (Airblast)
				tooltips.Add(new TooltipLine(mod, "Flamethrowers Airblast", "Right click to airblast."));
			else
				tooltips.Add(new TooltipLine(mod, "Flamethrowers No Airblast", "No airblast."));
		}
	}
}