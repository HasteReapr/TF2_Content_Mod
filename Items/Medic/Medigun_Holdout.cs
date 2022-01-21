using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TF2_Content.Items.Medic.Projectiles;

namespace TF2_Content.Items.Medic
{
	public class Medigun_Holdout : ModProjectile
	{
		public override string Texture => "TF2_Content/Items/Medic/Medigun";

		public const float MaxCharge = 180f;
		
		public const float DamageStart = 30f;

		private const float AimResponsiveness = 0.25f;

		private const int SoundInterval = 20;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Medigun");
			ProjectileID.Sets.NeedsUUID[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.LastPrism);
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, true);
			PlaySounds();

			UpdatePlayerVisuals(player, rrp);

			if (projectile.owner == Main.myPlayer)
			{
				UpdateAim(rrp, player.HeldItem.shootSpeed);

				bool stillInUse = player.channel && !player.noItems && !player.CCed;

				if (stillInUse)
				{
					FireBeams();
				}
				else if (!stillInUse)
				{
					projectile.Kill();
				}
			}

			projectile.timeLeft = 2;
		}

		private void PlaySounds()
		{
			Main.PlaySound(SoundID.Item15, projectile.position);
		}

		private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
		{
			projectile.Center = playerHandPos - new Vector2(10 * projectile.spriteDirection, 0);
			//projectile.rotation = projectile.velocity.ToRotation();
			projectile.rotation -= MathHelper.PiOver2 * projectile.spriteDirection;
			projectile.spriteDirection = projectile.direction;

			player.ChangeDir(projectile.direction);
			player.heldProj = projectile.whoAmI;
			player.itemTime = 2;
			player.itemAnimation = 2;

			// If you do not multiply by projectile.direction, the player's hand will point the wrong direction while facing left.
			player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
		}

		private void UpdateAim(Vector2 source, float speed)
		{
			Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
			if (aim.HasNaNs())
			{
				aim = -Vector2.UnitY;
			}

			aim = Vector2.Normalize(Vector2.Lerp(Vector2.Normalize(projectile.velocity), aim, AimResponsiveness));
			aim *= speed;

			if (aim != projectile.velocity)
			{
				projectile.netUpdate = true;
			}
			projectile.velocity = aim;
		}

		private void FireBeams()
		{
			Vector2 beamVelocity = Vector2.Normalize(projectile.velocity);
			if (beamVelocity.HasNaNs())
			{
				beamVelocity = -Vector2.UnitY;
			}

			int uuid = Projectile.GetByUUID(projectile.owner, projectile.whoAmI);

			int damage = projectile.damage;
			float knockback = projectile.knockBack;
			for (int b = 0; b < 1; ++b)
			{
				Projectile.NewProjectile(projectile.Center, beamVelocity, ModContent.ProjectileType<Medigun_Projectile>(), damage, knockback, projectile.owner, b, uuid);
			}

			projectile.netUpdate = true;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			SpriteEffects effects = projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			int frameHeight = texture.Height / Main.projFrames[projectile.type];
			int spriteSheetOffset = frameHeight * projectile.frame;
			Vector2 sheetInsertPosition = (projectile.Center + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition).Floor();

			spriteBatch.Draw(texture, sheetInsertPosition, new Rectangle?(new Rectangle(0, spriteSheetOffset, texture.Width, frameHeight)), Color.White, projectile.rotation, new Vector2(texture.Width / 2f, frameHeight / 2f), projectile.scale, effects, 0f);
			return false;
		}
	}
}