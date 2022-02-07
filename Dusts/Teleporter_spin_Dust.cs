using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TF2_Content.Dusts
{
	public class Teleporter_spin_dust : ModDust
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "TF2_Content/Dusts/Teleporter_spin_Dust";
			return base.Autoload(ref name, ref texture);
		}
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.noLight = false;
			dust.scale = 1f;
			dust.frame = new Rectangle(0, 0, 36, 2);
			dust.velocity = Vector2.Zero;
		}

		public override bool Update(Dust dust)
		{
			dust.scale -= 0.01f;
			if (dust.scale <= 0.5f)
			{
				dust.active = false;
			}

			return false;
		}
	}
}