using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace TF2_Content.Items.Spy
{
    class Knife : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Butterfly Knife");
            Tooltip.SetDefault("\"Why won't it insta-kill?\"\nWhen stabbing an NPC, there is a 10% chance that you deal 25% of that npcs HP.");
        }

        public override void SetDefaults()
        {
            item.damage = 25;
            item.useTime = 20;
            item.useAnimation = 20;
            item.height = 16;
            item.width = 16;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.melee = true;
            item.autoReuse = true;
            item.crit *= 0;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            if (Main.rand.NextBool(10))
            {
                target.StrikeNPC((int)(target.lifeMax * 0.25f), item.knockBack, player.direction, true);
            }
        }
    }
}
