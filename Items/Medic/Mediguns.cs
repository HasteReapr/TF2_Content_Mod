using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using TF2_Content;

namespace TF2_Content.Items.Medic
{
	// This class handles everything for our custom damage class
	// Any class that we wish to be using our custom damage class will derive from this class, instead of ModItem
	public abstract class Mediguns : ModItem
	{
		public override bool CloneNewInstances => true;
		public int UberCost = 100;

		public virtual void SafeSetDefaults()
		{
		}

		public sealed override void SetDefaults()
		{
			SafeSetDefaults();
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.summon = false;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += MedicPlayer.ModPlayer(player).HealRateAdd;
			mult *= MedicPlayer.ModPlayer(player).HealRateMult;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Heal Rate" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] splitText = tt.text.Split(' ');
				string damageValue = splitText.First();
				string damageWord = splitText.Last();
				tt.text = damageValue + " Heal Rate " + damageWord;
			}

			if (UberCost > 0)
			{
				tooltips.Add(new TooltipLine(mod, "Mediguns Uber Cost", $"Requires {UberCost} Ubercharge"));
			}
		}

		/*public override bool CanUseItem(Player player)
		{
			var modPlayer = player.GetModPlayer<MedicPlayer>();

			if (modPlayer.CurrentUber >= UberCost)
			{
				modPlayer.CurrentUber -= UberCost;
				return true;
			}
			return false;
		}*/
	}
}