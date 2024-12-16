using LocationCustomization;
using LocationTuner.Extras;
using LocationTuner.GMCM;
using LocationTuner.API;
using SDV_Realty_Core;
using StardewModdingAPI;

namespace LocationTuner
{
    internal class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            I18n.Init(helper.Translation);
            LCConfig config=helper.ReadConfig<LCConfig>();
            LocationCustomizer locationCustomizer = new LocationCustomizer(helper, Monitor, config);
            GMCM_Integration gmcm=new GMCM_Integration(ModManifest,helper,config);
        }

        public override object? GetApi()
        {
            return new LocationTunerAPI();
        }
    }
}
