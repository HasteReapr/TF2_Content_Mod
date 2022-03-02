using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;

namespace TF2_Content.Items.Pyro
{
    class Phlogistinator : Flamethrowers
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phlogistinator");
            Tooltip.SetDefault("Scientists are still trying to figure out why this tool is so hated.\n" +
                "Build charge by dealing 1000 damage.\n" +
                "On right click, if charge is 100%, triples your damage output for 6 seconds.\n" +
                "Will not build charge if damage bonus is active.");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 300;
            Airblast = false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            var modplayer = player.GetModPlayer<PyroPlayer>();
            if(player.altFunctionUse == 2 && modplayer.PhlogCurrentCharge == modplayer.PhlogChargeMax)
            {
                //add the kritz buff for 8 seconds, can be used with any weapon because fuck balance amiright
                //alternatively make a buff like the kritz buff, but make it only for the phlog and when you switch off the buff still counts down but doesnt give the damage bonus, or the weapon glow
            }
            return true;
        }
    }
}
