using LocationCustomization;
using StardewValley;
using System.Reflection;


namespace LocationTuner.API
{
    public class LocationTunerAPI : ILocationTunerAPI
    {
        private bool GetCustomizations(string locationName, out GameLocationCustomizations customizations)
        {
            customizations = null;
            if (Game1.getLocationFromName(locationName) != null)
            {
                GameLocation location = Game1.getLocationFromName(locationName);
                if (location.modData.TryGetValue(GameLocationCustomizations.ModDataKey, out string value))
                {
                    customizations= new GameLocationCustomizations(value);
                    return true;
                }
            }

            return false;
        }
        public Dictionary<string, object?> GetAllValues(string locationName)
        {
            Dictionary<string, object?> values = new();

            if(GetCustomizations(locationName,out GameLocationCustomizations customizations))
            {
                foreach (PropertyInfo prop in customizations.GetType().GetProperties().OrderBy(p => p.Name))
                {
                    values.Add(prop.Name, prop.GetValue(customizations));
                }
            }

            return values;
        }
            
        public bool NoCrows(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoCrows;
                return true;
            }
            return false;
        }
        public bool SeedsIgnoreSeasons(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.SeedsIgnoreSeason;
                return true;
            }
            return false;
        }
        //
        public bool NoBirdies(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoBirdies;
                return true;
            }
            return false;
        }

        public bool NoButterflies(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoButterflies;
                return true;
            }
            return false;
        }

        public bool NoBunnies(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoBunnies;
                return true;
            }
            return false;
        }

        public bool NoSquirrels(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoSquirrels;
                return true;
            }
            return false;
        }

        public bool NoWoodpeckers(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoWoodpeckers;
                return true;
            }
            return false;
        }

        public bool NoOwls(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoOwls;
                return true;
            }
            return false;
        }

        public bool NoOpossums(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoOpossums;
                return true;
            }
            return false;
        }

        public bool NoClouds(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoClouds;
                return true;
            }
            return false;
        }

        public bool NoFrogs(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoFrogs;
                return true;
            }
            return false;
        }

        public bool NoDebris(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoDebris;
                return true;
            }
            return false;
        }

        public bool NoLightning(string locationName, out bool value)
        {
            value = false;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.NoLightning;
                return true;
            }
            return false;
        }

        public bool SeasonOverride(string locationName, out string value)
        {
            value = "";
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.SeasonOverride;
                return true;
            }
            return false;
        }

        public bool ContextId(string locationName, out string? value)
        {
            value = "";
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.ContextId;
                return true;
            }
            return false;
        }

        public bool CanHaveGreenRainSpawns(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.CanHaveGreenRainSpawns;
                return true;
            }
            return false;
        }

        public bool CanPlantHere(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.CanPlantHere;
                return true;
            }
            return false;
        }

        public bool CanBuildHere(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.CanBuildHere;
                return true;
            }
            return false;
        }

        public bool MinDailyWeeds(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.MinDailyWeeds;
                return true;
            }
            return false;
        }

        public bool MaxDailyWeeds(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.MaxDailyWeeds;
                return true;
            }
            return false;
        }

        public bool FirstDayWeedMultiplier(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.FirstDayWeedMultiplier;
                return true;
            }
            return false;
        }

        public bool MinDailyForageSpawn(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.MinDailyForageSpawn;
                return true;
            }
            return false;
        }

        public bool MaxDailyForageSpawn(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.MaxDailyForageSpawn;
                return true;
            }
            return false;
        }

        public bool MaxSpawnedForageAtOnce(string locationName, out int? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.MaxSpawnedForageAtOnce;
                return true;
            }
            return false;
        }

        public bool ChanceForClay(string locationName, out float? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.ChanceForClay;
                return true;
            }
            return false;
        }

        public bool DirtDecayChance(string locationName, out float? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.DirtDecayChance;
                return true;
            }
            return false;
        }

        public bool AllowGiantCrops(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.AllowGiantCrops;
                return true;
            }
            return false;
        }

        public bool AllowGrassSurviveInWinter(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.AllowGrassSurviveInWinter;
                return true;
            }
            return false;
        }

        public bool AllowGrassGrowInWinter(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.AllowGrassGrowInWinter;
                return true;
            }
            return false;
        }

        public bool EnableGrassSpread(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.EnableGrassSpread;
                return true;
            }
            return false;
        }

        public bool AllowTreePlanting(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.AllowTreePlanting;
                return true;
            }
            return false;
        }

        public bool SkipWeedGrowth(string locationName, out bool? value)
        {
            value = null;
            if (GetCustomizations(locationName, out GameLocationCustomizations customizations))
            {
                value = customizations.SkipWeedGrowth;
                return true;
            }
            return false;
        }
    }
}
