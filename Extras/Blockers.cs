using HarmonyLib;
using LocationCustomization;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace LocationTuner.Extras
{
    internal class Blockers
    {
        private static LCConfig _config;
        private static IMonitor _monitor;
        private readonly static Dictionary<string, GameLocationCustomizations> _customizationCache = new();
        public Blockers(string manifestId, LCConfig config, IMonitor monitor)
        {
            _config = config;
            _monitor = monitor;

            var harmony = new Harmony(manifestId);

            harmony.Patch(
                AccessTools.Method(typeof(GameLocation), "addBirdies",
                new[] { typeof(double), typeof(bool) }),
                new HarmonyMethod(GetType(), nameof(NoBirdies)));
            harmony.Patch(
                AccessTools.Method(typeof(GameLocation), "addButterflies",
                new[] { typeof(double), typeof(bool) }),
                new HarmonyMethod(GetType(), nameof(NoButterflies)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addBunnies",
                 new[] { typeof(double), typeof(bool) }),
                 new HarmonyMethod(GetType(), nameof(NoBunnies)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addSquirrels",
                 new[] { typeof(double), typeof(bool) }),
                 new HarmonyMethod(GetType(), nameof(NoSquirrels)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addWoodpecker",
                 new[] { typeof(double), typeof(bool) }),
                 new HarmonyMethod(GetType(), nameof(NoWoodpeckers)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addOwl",
                 new Type[] { }),
                 new HarmonyMethod(GetType(), nameof(NoOwls)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addOpossums",
                 new[] { typeof(double), typeof(bool) }),
                 new HarmonyMethod(GetType(), nameof(NoOpossums)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "addClouds",
                 new[] { typeof(double), typeof(bool) }),
                 new HarmonyMethod(GetType(), nameof(NoClouds)));
            harmony.Patch(
                AccessTools.Method(typeof(GameLocation), "addFrog",
                new Type[] { }),
                new HarmonyMethod(GetType(), nameof(NoAddFrogs)));
            harmony.Patch(
                AccessTools.Method(typeof(GameLocation), "addJumperFrog",
                new Type[] { typeof(Vector2) }),
                new HarmonyMethod(GetType(), nameof(NoAddFrogs)));
            harmony.Patch(
                AccessTools.Method(typeof(Farm), "addCrows",
                new Type[] { }),
                new HarmonyMethod(GetType(), nameof(NoCrows)));
            harmony.Patch(
                AccessTools.Method(typeof(Farm), "doSpawnCrow",
                new Type[] { typeof(Vector2) }),
                new HarmonyMethod(GetType(), nameof(NoCrows)));
            harmony.Patch(
                AccessTools.Method(typeof(Utility), "performLightningUpdate",
                new Type[] { typeof(int) }),
                new HarmonyMethod(GetType(), nameof(NoLightning)));
            harmony.Patch(
                 AccessTools.Method(typeof(GameLocation), "SeedsIgnoreSeasonsHere",
                 new Type[] { }),
                 new HarmonyMethod(GetType(), nameof(SeedsIgnoreSeasonsHere)));

            harmony.PatchAll();
        }
        public static void ClearCache()
        {
            _customizationCache.Clear();
        }
        private static bool TryGetCustomizations(GameLocation location, out GameLocationCustomizations customizations)
        {
            customizations = null;
            if (location == null)
                return false;

            if (_customizationCache.TryGetValue(location.Name, out customizations))
                return true;

            if (location.modData.TryGetValue(GameLocationCustomizations.ModDataKey, out string value))
            {
                try
                {
                    customizations = new GameLocationCustomizations(value);
                    _customizationCache.Add(location.Name, customizations);
                    return true;
                }
                catch (Exception ex) { }
            }
            return false;
        }
        private static void LogCritter(bool blockEnabled, string location, string critter)
        {
            _monitor.Log($"Blocking: {blockEnabled} of '{critter}' in {location}", LogLevel.Debug);
        }
        public static bool SeedsIgnoreSeasonsHere(GameLocation __instance,out bool __result)
        {

            __result = false;
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                if (customizations.SeedsIgnoreSeason.HasValue)
                {
                    __result=customizations.SeedsIgnoreSeason.Value;
                    return false;
                }
            }
            return true;
        }
        public static bool NoLightning(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoLightning, __instance.Name, "Lightning");
                return !customizations.NoLightning;
            }
            return true;
        }
        public static bool NoAddFrogs(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoFrogs, __instance.Name, "Frogs");
                return !customizations.NoFrogs;
            }
            return true;
        }
        public static bool NoCrows(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoCrows, __instance.Name, "Crows");
                return !customizations.NoCrows;
            }
            return true;
        }
        public static bool NoClouds(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoClouds, __instance.Name, "Clouds");
                return !customizations.NoClouds;
            }
            return true;
        }

        public static bool NoOpossums(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoOpossums, __instance.Name, "Opossums");
                return !customizations.NoOpossums;
            }
            return true;
        }
        public static bool NoOwls(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoOwls, __instance.Name, "Owls");
                return !customizations.NoOwls;
            }
            return true;
        }
        public static bool NoWoodpeckers(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoWoodpeckers, __instance.Name, "Woodpeckers");
                return !customizations.NoWoodpeckers;
            }
            return true;
        }
        public static bool NoSquirrels(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoSquirrels, __instance.Name, "Squirrels");
                return !customizations.NoSquirrels;
            }
            return true;
        }
        public static bool NoBunnies(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoBunnies, __instance.Name, "Bunnies");
                return !customizations.NoBunnies;
            }
            return true;
        }
        public static bool NoBirdies(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoBirdies, __instance.Name, "Birdies");
                return !customizations.NoBirdies;
            }
            return true;
        }
        public static bool NoButterflies(GameLocation __instance)
        {
            if (TryGetCustomizations(__instance, out GameLocationCustomizations customizations))
            {
                LogCritter(customizations.NoButterflies, __instance.Name, "Butterflies");
                return !customizations.NoButterflies;
            }
            return true;
        }
    }
}
