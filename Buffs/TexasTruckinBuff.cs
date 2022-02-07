using Terraria;
using Terraria.ModLoader;

namespace TF2_Content.Buffs
{
    class TexasTruckinBuff : ModBuff
    {
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Texas Truckin");
			Description.SetDefault("Who's the farmboy now?");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<Mounts.TexasTruckinMount>(), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
