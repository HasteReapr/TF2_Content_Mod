using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Items.Medic
{
	public class MedicPlayer : ModPlayer
	{
		public static MedicPlayer ModPlayer(Player player)
		{
			return player.GetModPlayer<MedicPlayer>();
		}

		public float HealRateAdd;
		public float HealRateMult = 1f;
		
		public float CurrentUber;
		public const float UberChargeMax = 100;
		public float UberChargeRate;
		internal int UberChargeTimer = 0;
		public static readonly Color HealExampleResource = new Color(187, 91, 201);

		public override void UpdateDead()
		{
			ResetVariables();
		}

		private void ResetVariables()
		{
			HealRateAdd = 0;
			HealRateMult = 1f;
			UberChargeRate = 1f;
			CurrentUber = 0;
		}

		public override void PostUpdateMiscEffects()
		{
			UpdateResource();
		}

		private void UpdateResource()
		{
			CurrentUber = Utils.Clamp(CurrentUber, 0, UberChargeMax);
		}
	}
}