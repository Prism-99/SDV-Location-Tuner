using HarmonyLib;
using Newtonsoft.Json;
using System.Reflection;
using StardewModdingAPI;
using StardewValley;


namespace LocationCustomization
{
    internal class GameLocationCustomizations
    {
        public const string ModDataKey = "prism99.locTuner.customizations";
        private IModHelper? _modHelper = null;
        public GameLocationCustomizations() { LocationName = ""; }
        public GameLocationCustomizations(string modDataValue)
        {
            ParseModDataValue(modDataValue);
        }
        public GameLocationCustomizations(string locationName, IModHelper modHelper, bool getDefaults = false)
        {
            _modHelper = modHelper;
            LocationName = locationName;
            GameLocation? source = Game1.getLocationFromName(locationName);
            
            if (source != null)
            {
                if (getDefaults)
                {
                    LoadDefaultValues(source);
                }
                else
                {
                    if (source.modData.TryGetValue(ModDataKey, out string values))
                    {
                        ParseModDataValue(values);
                    }
                }
            }
        }
        private void LoadDefaultValues(GameLocation source)
        {
            Lazy<Season?> over = (Lazy<Season?>)Traverse.Create(source).Field("seasonOverride").GetValue();
            SeasonOverride = over.Value == null ? "None" : over.ToString();

        }
        private void ParseModDataValue(string value)
        {
            try
            {
                GameLocationCustomizations? current = JsonConvert.DeserializeObject<GameLocationCustomizations>(value);
                if (current != null)
                {
                    foreach (PropertyInfo prop in current.GetType().GetProperties().OrderBy(p => p.Name))
                    {
                        GetType().GetProperty(prop.Name).SetValue(this, prop.GetValue(current, null));
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// Returns the location name of the customization
        /// Checks for Farm location and returns the
        /// composite name for the Farm
        /// </summary>
        /// <returns></returns>
        public string GetLocationName()
        {
            if (LocationName == "Farm")
                return "Farm_" + Game1.GetFarmTypeKey();
            else
                return LocationName;
        }
        /// <summary>
        /// Apply customizations
        /// </summary>
        public void ApplyCustomizations()
        {
            GameLocation? source = Game1.getLocationFromName(LocationName);
            if (source != null)
            {
                if (SeasonOverride == "none")
                    Traverse.Create(source).Field("seasonOverride").SetValue(new Lazy<Season?>());
                else
                {
                    if (Enum.TryParse(typeof(Season), SeasonOverride, true, out object ov))
                    {
                        Traverse.Create(source).Field("seasonOverride").SetValue(new Lazy<Season?>((Season)ov));
                        source.seasonUpdate();
                        source.updateSeasonalTileSheets();
                    }
                }
                if (!string.IsNullOrEmpty(ContextId) && Game1.locationContextData.ContainsKey(ContextId))
                    source.locationContextId = ContextId;

                if (AllowGiantCrops.HasValue)
                {
                    if (AllowGiantCrops.Value)
                        source.map.Properties["AllowGiantCrops"] = "T";
                    else
                        source.map.Properties.Remove("AllowGiantCrops");
                }
                if (AllowGrassSurviveInWinter.HasValue)
                {
                    if (AllowGrassSurviveInWinter.Value)
                        source.map.Properties["AllowGrassSurviveInWinter"] = "T";
                    else
                        source.map.Properties.Remove("AllowGrassSurviveInWinter");
                }
                if (AllowGrassGrowInWinter.HasValue)
                {
                    if (AllowGrassGrowInWinter.Value)
                        source.map.Properties["AllowGrassGrowInWinter"] = "T";
                    else
                        source.map.Properties.Remove("AllowGrassGrowInWinter");
                }
                if (AllowTreePlanting.HasValue)
                {
                    if (AllowTreePlanting.Value)
                        source.map.Properties["ForceAllowTreePlanting"] = "T";
                    else
                        source.map.Properties.Remove("ForceAllowTreePlanting");
                }
                if (SkipWeedGrowth.HasValue)
                {
                    if (SkipWeedGrowth.Value)
                        source.map.Properties["skipWeedGrowth"] = "T";
                    else
                        source.map.Properties.Remove("skipWeedGrowth");
                }
                if (EnableGrassSpread.HasValue)
                {
                    if (EnableGrassSpread.Value)
                        source.map.Properties["EnableGrassSpread"] = "T";
                    else
                        source.map.Properties.Remove("EnableGrassSpread");
                }
                if (CanBuildHere.HasValue)
                {
                    if (CanBuildHere.Value)
                    {
                        source.map.Properties["CanBuildHere"] = "T";
                        source.map.Properties["LooserBuildRestrictions"] = "T";
                        source.isAlwaysActive.Value = true;
                    }
                    else
                        source.map.Properties.Remove("CanBuildHere");
                }
                if (DirtDecayChance.HasValue)
                {
                    source.map.Properties["DirtDecayChance"] = DirtDecayChance.Value.ToString();
                }
                source.ignoreDebrisWeather.Value = NoDebris;
            }
        }
        /// <summary>
        /// Removes the customizations from the GameLocation's modData
        /// </summary>
        public void ClearCustomizations()
        {
            GameLocation? source = Game1.getLocationFromName(LocationName);
            if (source != null)
            {
                source.modData.Remove(ModDataKey);
                if (_modHelper != null)
                    _modHelper.GameContent.InvalidateCache("Data/Locations");
            }
        }
        /// <summary>
        /// Save the customizations to the modData of the GameLocation
        /// </summary>
        public void SaveCustomizations()
        {
            GameLocation? source = Game1.getLocationFromName(LocationName);
            if (source != null)
            {
                ApplyCustomizations();
                string data = JsonConvert.SerializeObject(this);
                source.modData[ModDataKey] = data;
                if (_modHelper != null)
                    _modHelper.GameContent.InvalidateCache("Data/Locations");
            }
        }
        public bool NoBirdies { get; set; } = false;
        public bool NoButterflies { get; set; } = false;
        public bool NoBunnies { get; set; } = false;
        public bool NoSquirrels { get; set; } = false;
        public bool NoWoodpeckers { get; set; } = false;
        public bool NoOwls { get; set; } = false;
        public bool NoOpossums { get; set; } = false;
        public bool NoClouds { get; set; } = false;
        public bool NoFrogs { get; set; } = false;
        public bool NoCrows { get; set; } = false;
        public bool NoDebris { get; set; } = false;
        public bool NoLightning { get; set; } = false;
        public bool? SeedsIgnoreSeason {  get; set; } = null;
        public string LocationName { get; set; }
        public string SeasonOverride { get; set; } = "none";
        public string? ContextId { get; set; }
        public bool? CanHaveGreenRainSpawns { get; set; } = null;
        public bool? CanPlantHere { get; set; } = null;
        public bool? CanBuildHere { get; set; } = null;
        public int? MinDailyWeeds { get; set; } = null;
        public int? MaxDailyWeeds { get; set; } = null;
        public int? FirstDayWeedMultiplier { get; set; } = null;
        public int? MinDailyForageSpawn { get; set; } = null;
        public int? MaxDailyForageSpawn { get; set; } = null;
        public int? MaxSpawnedForageAtOnce { get; set; } = null;
        public float? ChanceForClay { get; set; } = null;
        public float? DirtDecayChance { get; set; } = null;
        public bool? AllowGiantCrops { get; set; } = null;
        public bool? AllowGrassSurviveInWinter { get; set; } = null;
        public bool? AllowGrassGrowInWinter { get; set; } = null;
        public bool? EnableGrassSpread { get; set; } = null;
        public bool? AllowTreePlanting { get; set; } = null;
        public bool? SkipWeedGrowth { get; set; } = null;
    }
}
