using LocationCustomization;
using StardewModdingAPI;
using GenericModConfigMenu;
using SDV_Realty_Core;
using StardewModdingAPI.Events;

namespace LocationTuner.GMCM
{
    internal class GMCM_Integration
    {
        private IManifest manifest;
        private IModHelper modHelper;
        private static LCConfig config;
        public GMCM_Integration(IManifest manifest, IModHelper modHelper, LCConfig modConfig)
        {
            this.manifest = manifest;
            this.modHelper = modHelper;
            config = modConfig;
            modHelper.Events.GameLoop.GameLaunched += GameLoop_GameLaunched; ;
        }

        private void GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            RegisterMenu();
        }

        private static void ResetGuiVars()
        {
            config = new LCConfig();
        }
        public void RegisterMenu()
        {
            var configMenu = modHelper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
              mod: manifest,
              reset: ResetGuiVars,
              save: () => modHelper.WriteConfig(config),
               titleScreenOnly: false
          );

            configMenu.AddKeybindList(
                mod: manifest,
                name: () => "Activation Key",
                tooltip: () => "",
                 getValue: () => config.Keybinding,
                 setValue: value => config.Keybinding = value
             );
        //    configMenu.AddBoolOption(
        //      mod: manifest,
        //      name: () => "No Birds",
        //      tooltip: () => "",
        //      getValue: () => config.NoBirdies,
        //      setValue: value => config.NoBirdies = value
        //  );
        //    configMenu.AddBoolOption(
        //     mod: manifest,
        //     name: () => "No Butterflies",
        //     tooltip: () => "",
        //     getValue: () => config.NoButterflies,
        //     setValue: value => config.NoButterflies = value
        // );
        //    configMenu.AddBoolOption(
        //     mod: manifest,
        //     name: () => "No Bunnies",
        //     tooltip: () => "",
        //     getValue: () => config.NoBunnies,
        //     setValue: value => config.NoBunnies = value
        // );
        //    configMenu.AddBoolOption(
        //       mod: manifest,
        //       name: () => "No Squirrels",
        //       tooltip: () => "",
        //       getValue: () => config.NoSquirrels,
        //       setValue: value => config.NoSquirrels = value
        //);
        //    configMenu.AddBoolOption(
        //       mod: manifest,
        //       name: () => "No Woodpeckers",
        //       tooltip: () => "",
        //       getValue: () => config.NoWoodpeckers,
        //       setValue: value => config.NoWoodpeckers = value
        //    );
        //    configMenu.AddBoolOption(
        //        mod: manifest,
        //        name: () => "No Owls",
        //        tooltip: () => "",
        //        getValue: () => config.NoOwls,
        //        setValue: value => config.NoOwls = value
        //    );
        //    configMenu.AddBoolOption(
        //        mod: manifest,
        //        name: () => "No Opossums",
        //        tooltip: () => "",
        //        getValue: () => config.NoOpossums,
        //        setValue: value => config.NoOpossums = value
        //    );
        //    configMenu.AddBoolOption(
        //        mod: manifest,
        //        name: () => "No Clouds",
        //        tooltip: () => "",
        //        getValue: () => config.NoClouds,
        //        setValue: value => config.NoClouds = value
        //    );
            configMenu.AddBoolOption(
                mod: manifest,
                name: () => I18n.LC_GMCM_Adv(),
                tooltip: () => "",
                getValue: () => config.AdvancedEnabled,
                setValue: value => config.AdvancedEnabled = value
            );


        }
    }
}
