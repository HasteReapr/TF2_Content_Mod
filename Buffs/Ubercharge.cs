using Terraria.ModLoader;
using Terraria;
using TF2_Content.Items.Medic;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;

namespace TF2_Content.Buffs
{
    class Ubercharge : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Ubercharge");
            Description.SetDefault("\"I am bullet proof!\"");
        }

        int timer = 0;

        public override void Update(Player player, ref int buffIndex)
        {
            var medic = player.GetModPlayer<MedicPlayer>();
            medic.CurrentUber -= 0.208333333333333f;

            player.statLife += 1;

            if(Main.netMode != NetmodeID.Server && !Filters.Scene["UberGlow"].Active)
                Filters.Scene.Activate("UberGlow", Vector2.Zero).GetShader().UseColor(1.0f, 0.5f, 0f);
        }
    }
}
