using StardewModdingAPI;


namespace SDV_Realty_Core
{
    internal static class I18n
    {
        private static ITranslationHelper Translations;
        public static void Init(ITranslationHelper translations)
        {
            Translations = translations;
        }
        #region "LandCustomizer"
        public static string LC_No_Lightning()
        {
            return GetByKey("lc.no.lightning");
        }
        public static string LC_No_Debris()
        {
            return GetByKey("lc.no.debris");
        }
        public static string LC_No_Frogs()
        {
            return GetByKey("lc.no.frogs");
        }
        public static string LC_No_Crows()
        {
            return GetByKey("lc.no.crows");
        }
        public static string LC_No_Opossums()
        {
            return GetByKey("lc.no.opossums");
        }
        public static string LC_No_Owls()
        {
            return GetByKey("lc.no.owls");
        }
        public static string LC_No_Woodpeckers()
        {
            return GetByKey("lc.no.woodpeckers");
        }
        public static string LC_No_Squirrels()
        {
            return GetByKey("lc.no.squirrels");
        }
        public static string LC_No_Bunnies()
        {
            return GetByKey("lc.no.bunnies");
        }
        public static string LC_No_Butterflies()
        {
            return GetByKey("lc.no.butterflies");
        }
        public static string LC_No_Birds()
        {
            return GetByKey("lc.no.birds");
        }
        public static string LC_No_Clouds()
        {
            return GetByKey("lc.no.clouds");
        }
        public static string LC_GMCM_Adv()
        {
            return GetByKey("lc.gmcm.advanced");
        }
        public static string LC_CanBuild()
        {
            return GetByKey("lc.can.build");
        }
        public static string LC_CanBuild_TT()
        {
            return GetByKey("lc.can.build.tt");
        }
        public static string LC_Drop_Default()
        {
            return GetByKey("lc.drop.default");
        }
        public static string LC_Drop_Yes()
        {
            return GetByKey("lc.drop.yes");
        }
        public static string LC_Drop_No()
        {
            return GetByKey("lc.drop.no");
        }


        public static  string LC_Season_None()
        {
            return GetByKey("lc.season.none");
        }
        public static string LC_Season_Spring()
        {
            return GetByKey("lc.season.spring");
        }
        public static string LC_Season_Summer()
        {
            return GetByKey("lc.season.summer");
        }
        public static string LC_Season_Fall()
        {
            return GetByKey("lc.season.fall");
        }
        public static string LC_Season_Winter()
        {
            return GetByKey("lc.season.winter");
        }


        public static string LC_Button_Save()
        {
            return GetByKey("lc.button.save");
        }
        public static string LC_Button_Cancel()
        {
            return GetByKey("lc.button.cancel");
        }
        public static string LC_SeedSeason()
        {
            return GetByKey("lc.seed.season");
        }
        public static string LC_Clay()
        {
            return GetByKey("lc.clay");
        }
        public static string LC_Clay_TT()
        {
            return GetByKey("lc.clay.tt");
        }

        public static string LC_DirtDecay()
        {
            return GetByKey("lc.dirt.decay");
        }
        public static string LC_DirtDecay_TT()
        {
            return GetByKey("lc.dirt.decay.tt");
        }
        public static string LC_MaxForage()
        {
            return GetByKey("lc.max.forage");
        }
        public static string LC_MaxForage_TT()
        {
            return GetByKey("lc.max.forage.tt");
        }
        public static string LC_MinForage()
        {
            return GetByKey("lc.min.forage");
        }
        public static string LC_MinForage_TT()
        {
            return GetByKey("lc.min.forage.tt");
        }

        public static string LC_MaxWeeds()
        {
            return GetByKey("lc.max.weeds");
        }
        public static string LC_MaxWeeds_TT()
        {
            return GetByKey("lc.max.weeds.tt");
        }
        public static string LC_MinWeeds()
        {
            return GetByKey("lc.min.weeds");
        }
        public static string LC_MinWeeds_TT()
        {
            return GetByKey("lc.min.weeds.tt");
        }
        public static string LC_FirstDay()
        {
            return GetByKey("lc.first.day");
        }
        public static string LC_FirstDay_TT()
        {
            return GetByKey("lc.first.day.tt");
        }
        public static string LC_SkipWeed()
        {
            return GetByKey("lc.skip.weed");
        }
        public static string LC_TreePlanting()
        {
            return GetByKey("lc.tree.planting");
        }
        public static string LC_GrassSpreadWinter()
        {
            return GetByKey("lc.grass.spread.winter");
        }
        public static string LC_GrassSpread()
        {
            return GetByKey("lc.grass.spread");
        }
        public static string LC_WinterGrassSurvive()
        {
            return GetByKey("lc.grass.sur.winter");
        }
        public static string LC_WinterGrassSurvive_TT()
        {
            return GetByKey("lc.grass.sur.winter.tt");
        }

        public static string LC_GiantCrops()
        {
            return GetByKey("lc.giant.crops");
        }
        public static string LC_GiantCrops_TT()
        {
            return GetByKey("lc.giant.crops.tt");
        }

        public static string LC_CanPlant()
        {
            return GetByKey("lc.can.plant");
        }
        public static string LC_CanPlant_TT()
        {
            return GetByKey("lc.can.plant.tt");
        }

        public static string LC_GreenSpawn()
        {
            return GetByKey("lc.green.spawn");
        }
        public static string LC_GreenSpawn_TT()
        {
            return GetByKey("lc.green.spawn.tt");
        }

        public static string LC_SeasonOverride()
        {
            return GetByKey("lc.season.override");
        }
        public static string LC_SeasonOverride_TT()
        {
            return GetByKey("lc.season.override.tt");
        }
        #endregion
      
        private static Translation GetByKey(string key, object tokens = null)
        {
            if (Translations == null)
                throw new InvalidOperationException($"You must call {nameof(I18n)}.{nameof(Init)} from the mod's entry method before reading translations.");
            return Translations.Get(key, tokens);
        }
    }
}
