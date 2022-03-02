using Terraria.ModLoader.Config;

namespace TF2_Content
{
    class TF2Config
    {
		public class MiniUIConfigs : ModConfig
		{
			public override ConfigScope Mode => ConfigScope.ClientSide;

			[Label("Phlogistinator Mini Cursor UI")]
			public bool ShowMiniPhlogUI;

			[Label("Ubercharge Mini Cursor UI")]
			public bool ShowMiniUberUI;

			public override void OnChanged()
			{
				UI.MiniUberUI.canShow = ShowMiniUberUI;
				UI.MiniPhlogUI.canShow = ShowMiniPhlogUI;
			}
		}
	}
}
