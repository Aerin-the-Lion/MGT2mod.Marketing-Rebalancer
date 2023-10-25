using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using HarmonyLib;
using UnityEngine.UI;

namespace MarketingRebalancer
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInProcess("Mad Games Tycoon 2.exe")]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGuid = "aerin.Mad_Games_Tycoon_2.plugins.MarketingRebalancer";
        public const string PluginName = "MarketingRebalancer";
        public const string PluginVersion = "1.0.0.0";

        // ****** Setting ******
        // Hypeの調整
        public static ConfigEntry<float> CFG_MAX_HYPE { get; private set; }
        public static ConfigEntry<bool> CFG_IS_ENABLED { get; private set; }
        public static ConfigEntry<int> CFG_OVERHYPE { get; private set; }
        public static ConfigEntry<int> CFG_BOMBASTIC_ADS { get; private set; }
        //Posters
        public static ConfigEntry<float> CFG_POSTERS_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_POSTERS_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_POSTERS_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_POSTERS_PRICE { get; private set; }

        //Game magazines
        public static ConfigEntry<float> CFG_GAMEMAGAZINES_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_GAMEMAGAZINES_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_GAMEMAGAZINES_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_GAMEMAGAZINES_PRICE { get; private set; }

        //Radio
        public static ConfigEntry<float> CFG_RADIO_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_RADIO_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_RADIO_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_RADIO_PRICE { get; private set; }

        //Internet Ads
        public static ConfigEntry<float> CFG_INTERNETADS_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_INTERNETADS_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_INTERNETADS_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_INTERNETADS_PRICE { get; private set; }

        //Top Streamer
        public static ConfigEntry<float> CFG_TOPSTREAMER_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_TOPSTREAMER_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_TOPSTREAMER_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_TOPSTREAMER_PRICE { get; private set; }

        //TV
        public static ConfigEntry<float> CFG_TV_MAXHYPE { get; private set; }
        public static ConfigEntry<float> CFG_TV_HYPE_PROM { get; private set; }
        public static ConfigEntry<float> CFG_TV_WORKPOINTS { get; private set; }
        public static ConfigEntry<float> CFG_TV_PRICE { get; private set; }

        public void LoadConfig()
        {
            string textIsEnable = "0. MOD Settings";
            string text1 = "1. Basic Game Settings";
            string text2 = "2. Additional Game Settings";
            string text3 = "3. Advanced Game Settings";

            CFG_IS_ENABLED = Config.Bind<bool>(textIsEnable, "Activate the MOD", true, "If you need to disable the mod, Toggle to Disabled");
            CFG_MAX_HYPE = Config.Bind<float>(text2, "Maximum Hype Limit", 100f, "Default : 100");
            CFG_BOMBASTIC_ADS = Config.Bind<int>(text2, "Border line of Bombastic advertisement", 100, "If your game crosses the borderline and the game's review so badly, your fans will be disappointed like original overhype does. Default : 100");
            CFG_OVERHYPE = Config.Bind<int>(text2, "Customized Overhype", 100, "Overhype of Special Marketing adds additional points for hype. **Original overhype makes forcibly 200 hypes but it adds additional points instead. Default : 100");
            //Posters
            CFG_POSTERS_MAXHYPE = Config.Bind<float>(text1, "Posters : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 24 }));
            CFG_POSTERS_HYPE_PROM = Config.Bind<float>(text1, "Posters : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 23 }));
            CFG_POSTERS_WORKPOINTS = Config.Bind<float>(text1, "Posters : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 22 }));
            CFG_POSTERS_PRICE = Config.Bind<float>(text1, "Posters : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 21 }));

            //Game magazines
            CFG_GAMEMAGAZINES_MAXHYPE = Config.Bind<float>(text1, "Game magazines : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 20 }));
            CFG_GAMEMAGAZINES_HYPE_PROM = Config.Bind<float>(text1, "Game magazines : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 19 }));
            CFG_GAMEMAGAZINES_WORKPOINTS = Config.Bind<float>(text1, "Game magazines : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 18 }));
            CFG_GAMEMAGAZINES_PRICE = Config.Bind<float>(text1, "Game magazines : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 17 }));

            //Radio
            CFG_RADIO_MAXHYPE = Config.Bind<float>(text1, "Radio : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 16 }));
            CFG_RADIO_HYPE_PROM = Config.Bind<float>(text1, "Radio : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 15 }));
            CFG_RADIO_WORKPOINTS = Config.Bind<float>(text1, "Radio : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 14 }));
            CFG_RADIO_PRICE = Config.Bind<float>(text1, "Radio : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 13 }));

            //Internet ads
            CFG_INTERNETADS_MAXHYPE = Config.Bind<float>(text1, "Internet Ads : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 12 }));
            CFG_INTERNETADS_HYPE_PROM = Config.Bind<float>(text1, "Internet Ads : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 11 }));
            CFG_INTERNETADS_WORKPOINTS = Config.Bind<float>(text1, "Internet Ads : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 10 }));
            CFG_INTERNETADS_PRICE = Config.Bind<float>(text1, "Internet ads : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 9 }));

            //Top Streamer
            CFG_TOPSTREAMER_MAXHYPE = Config.Bind<float>(text1, "Top Streamer : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 8 }));
            CFG_TOPSTREAMER_HYPE_PROM = Config.Bind<float>(text1, "Top Streamer : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 7 }));
            CFG_TOPSTREAMER_WORKPOINTS = Config.Bind<float>(text1, "Top Streamer : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 6 }));
            CFG_TOPSTREAMER_PRICE = Config.Bind<float>(text1, "Top Streamer : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 5 }));

            //TV
            CFG_TV_MAXHYPE = Config.Bind<float>(text1, "TV : Maximum Hype : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 4 }));
            CFG_TV_HYPE_PROM = Config.Bind<float>(text1, "TV : Hype Promotion : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 3 }));
            CFG_TV_WORKPOINTS = Config.Bind<float>(text1, "TV : Work Points : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 2 }));
            CFG_TV_PRICE = Config.Bind<float>(text1, "TV : Price : (Number)x", 1, new ConfigDescription("Magnification Value. If you setted 2, it will be 2 times. Default : 1", new AcceptableValueRange<float>(0, 100), new ConfigurationManagerAttributes { ShowRangeAsPercent = false, Order = 1 }));

            Config.SettingChanged += delegate (object sender, SettingChangedEventArgs args)
            {
            };
        }

        void Awake()
        {
            LoadConfig();
            Harmony.CreateAndPatchAll(typeof(MarketingRebalancer));

        }

        void Update()
        {
            //UpdateCount++;
            //Debug.Log("Update Count : " + UpdateCount);
        }


    }

}