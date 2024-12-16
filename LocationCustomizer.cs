using StardewModdingAPI.Events;
using StardewValley.GameData.Locations;
using StardewModdingAPI;
using Newtonsoft.Json;
using StardewValley.Menus;
using StardewValley;
using LocationCustomization.Menu;
using LocationTuner.Extras;


namespace LocationCustomization
{
    internal class LocationCustomizer
    {
        private IModHelper modHelper;
         private bool menuUp = false;
        private IMonitor monitor;
        private LCConfig config;
        private Blockers blockers;
        public LocationCustomizer(IModHelper helper, IMonitor monitor, LCConfig config)
        {
            modHelper = helper;
            this.monitor = monitor;
            this.config = config;
            // add hook for Data/Location editing
            helper.Events.Content.AssetRequested += Content_AssetRequested;
            helper.Events.Content.AssetReady += Content_AssetReady;
            // add hook for applying customizations after the save has loaded
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
            // add hook for checking to see if the Menu Hotkey was pressed
            helper.Events.GameLoop.UpdateTicked += GameLoop_UpdateTicked;
            blockers = new Blockers(helper.ModRegistry.ModID, config, monitor);
        }

        private void Content_AssetReady(object? sender, AssetReadyEventArgs e)
        {
            if(e.NameWithoutLocale.Name == "Data/Locations")
            {
                EditDataLocations(Game1.locationData);
            }
        }

        /// <summary>
        /// Check for pressing of the menu hotkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_UpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (Game1.hasLoadedGame && Game1.activeClickableMenu == null)
            {
                if (!menuUp && config.Keybinding.JustPressed() && Game1.currentLocation.IsOutdoors)
                {
                    GameLocationCustomizations customizations = new GameLocationCustomizations(Game1.currentLocation.Name, modHelper);
                    menuUp = true;
                    ExpansionOptionsMenu menu = new ExpansionOptionsMenu(Game1.uiViewport.Width / 2 - (800 + IClickableMenu.borderWidth * 2) / 2, Game1.uiViewport.Height / 2 - (600 + IClickableMenu.borderWidth * 2) / 2, 800 + IClickableMenu.borderWidth * 2, 600 + IClickableMenu.borderWidth * 2, false, customizations, MenuClosed,config.AdvancedEnabled);

                    menu.behaviorBeforeCleanup += behaviorBeforeCleanup;
                    Game1.activeClickableMenu = menu;
                }
            }
        }
        private void behaviorBeforeCleanup(IClickableMenu menu)
        {
            MenuClosed();
        }
        /// <summary>
        /// Called when the Option menu is closed
        /// </summary>
        private void MenuClosed()
        {
            menuUp = false;
            Blockers.ClearCache();
        }
        /// <summary>
        /// Handle customizing locations once the save has loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameLoop_SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            var locations = GetCustomizedLocations();
            foreach (var location in locations)
            {
                try
                {
                    GameLocationCustomizations customization = JsonConvert.DeserializeObject<GameLocationCustomizations>(location.modData[GameLocationCustomizations.ModDataKey]);

                    customization?.ApplyCustomizations();
                }
                catch { }
            }
            EditDataLocations(Game1.locationData);
            modHelper.GameContent.InvalidateCache("Data/Locations");
        }
        private void EditDataLocations(IDictionary<string, LocationData> locationData)
        {
            var locations = GetCustomizedLocations();

            foreach (var location in locations)
            {
                try
                {
                    GameLocationCustomizations? customization = JsonConvert.DeserializeObject<GameLocationCustomizations>(location.modData[GameLocationCustomizations.ModDataKey]);
                    string? locationName = customization?.GetLocationName();

                    if (customization != null && locationData.TryGetValue(locationName, out var locData))
                    {
                        if (customization.CanHaveGreenRainSpawns.HasValue)
                            locData.CanHaveGreenRainSpawns = customization.CanHaveGreenRainSpawns.Value;
                        if (customization.CanPlantHere.HasValue)
                            locData.CanPlantHere = customization.CanPlantHere.Value;
                        if (customization.CanBuildHere.HasValue && customization.CanBuildHere.Value)
                        {
                            if (locData.CreateOnLoad == null)
                                locData.CreateOnLoad = new CreateLocationData();

                            locData.CreateOnLoad.AlwaysActive = true;
                        }

                        if (customization.FirstDayWeedMultiplier.HasValue)
                            locData.FirstDayWeedMultiplier = customization.FirstDayWeedMultiplier.Value;
                        if (customization.MinDailyWeeds.HasValue)
                            locData.MinDailyWeeds = customization.MinDailyWeeds.Value;
                        if (customization.MaxDailyWeeds.HasValue)
                            locData.MaxDailyWeeds = customization.MaxDailyWeeds.Value;

                        if (customization.MinDailyForageSpawn.HasValue)
                            locData.MinDailyForageSpawn = customization.MinDailyForageSpawn.Value;
                        if (customization.MaxDailyForageSpawn.HasValue)
                            locData.MaxDailyForageSpawn = customization.MaxDailyForageSpawn.Value;
                        if (customization.MaxSpawnedForageAtOnce.HasValue)
                            locData.MaxSpawnedForageAtOnce = customization.MaxSpawnedForageAtOnce.Value;

                        if (customization.ChanceForClay.HasValue)
                            locData.ChanceForClay = customization.ChanceForClay.Value;
                    }
                }
                catch (Exception ex)
                {
                    monitor.Log(ex.ToString(), LogLevel.Error);
                }
            }

        }
        /// <summary>
        /// Handle Data/Location customization elements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_AssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.Name == "Data/Locations")
            {
                e.Edit(asset =>
                {
                    Dictionary<string, LocationData> locationData = (Dictionary<string, LocationData>)asset.Data;
                    EditDataLocations(locationData);
                });
            }
        }
        /// <summary>
        /// Scaan all GameLocations to fined ones with customization data
        /// </summary>
        /// <returns>Returns a list of GameLocations that have customization data</returns>
        private List<GameLocation> GetCustomizedLocations()
        {
            return Game1.locations.Where(p => p.modData.ContainsKey(GameLocationCustomizations.ModDataKey)).ToList();
        }
    }
}
