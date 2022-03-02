using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Items.Pyro
{
	public class PyroPlayer : ModPlayer
	{
		public static PyroPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<PyroPlayer>();
		}
		public float PhlogCurrentCharge;
		public float PhlogChargeMax = 100;
		public float PhlogChargeRate;
		internal int PhlogChargeTimer = 0;

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			PhlogChargeRate = 1f;
			PhlogCurrentCharge = 0;
		}

		public override void PostUpdateMiscEffects()
		{
			UpdateResource();
		}

		private void UpdateResource()
		{
			PhlogCurrentCharge = Utils.Clamp(PhlogCurrentCharge, 0, PhlogChargeMax);
		}
	}
}