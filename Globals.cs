using CitizenFX.Core.Native;
using System.Collections.Generic;
using CitizenFX.Core;
using System.Text;

namespace client
{
    public class Globals
    {
        public static int GenHash(string name)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(name.ToLowerInvariant());

            uint num = 0u;

            for (int i = 0; i < bytes.Length; i++)
            {
                num += (uint)bytes[i];
                num += num << 10;
                num ^= num >> 6;
            }
            num += num << 3;
            num ^= num >> 11;

            return (int)(num + (num << 15));
        }

        public static bool g_browseJustOpen = false;
        public static bool g_editFirstOpen = false;

        // styling
        public static float g_titleMenuText = 0.01f + 0.020f;

        public static float g_trainerCentre = 0.155f;
        public static bool g_titleCentered = false;

        public static int g_titleTextRed = 255;
        public static int g_titleTextGreen = 255;
        public static int g_titleTextBlue = 255;
        public static int g_titleTextAlpha = 255;
        public static int[] g_menuTextColour = { g_titleTextRed, g_titleTextGreen, g_titleTextBlue, g_titleTextAlpha };

        public static int g_titleTextFont = 7;
        public static int g_backgroundRed = 0;
        public static int g_backgroundGreen = 0;
        public static int g_backgroundBlue = 0;
        public static int g_backgroundAlpha = 200;

        public static int g_optionsFont = 10;
        public static int g_optionsRed = 255;
        public static int g_optionsGreen = 255;
        public static int g_optionsBlue = 255;
        public static int g_optionsAlpha = 210;

        public static int g_activeRed = 255;
        public static int g_activeGreen = 255;
        public static int g_activeBlue = 255;
        public static int g_activeOpacity = 125;

        public static int g_highlightRed = 230;
        public static int g_highlightGreen = 22;
        public static int g_highlightBlue = 232;
        public static int g_highlightOpacity = 255;

        public static int g_indicatorRed = 255;
        public static int g_indicatorGreen = 44;
        public static int g_indicatorBlue = 55;
        public static int g_indicatorAlpha = 255;

        // functions
        public static MenuId g_menu_subMenu = MenuId.MENU_NOTOPEN;

        public static int g_menu_subMenuLevel;
        public static MenuId[] g_menu_lastSubMenu = new MenuId[20];
        public static int[] g_menu_lastOption = new int[20];
        public static int g_menu_currentOption;
        public static int g_menu_optionCount;
        public static bool g_menu_newTimerTick = true;
        public static bool g_menu_optionPress = false;
        public static int g_menu_delayCounter;

        public static bool right_press = false;
        public static bool left_press = false;

        // frame functions
        public static bool g_noReload = false;

        public static bool g_rainbowFlares = false;
        public static bool g_playerVehicleLocked = false;
        public static bool g_ingameCoords = false;
        public static bool g_driftAngle = false;
        public static int g_currentTimeHours = 12;
        public static int g_currentTimeMinutes = 0;
        public static bool g_persistWeather = false;
        public static int g_vehicleType;
        public static int g_colourType = 0;

        public static int previousCurrentTime = 0;
        public static bool previousPersistWeather = false;
        public static bool previousVehicleLocked = false;

        public static int g_currentWeatherIdx = 0;
        public static int previousWeatherIdx = 0;
        public static string[] g_weathers = new[] { "OVERCAST", "RAIN", "FOG", "SNOWLIGHT", "THUNDER", "BLIZZARD", "SNOW", "MISTY", 
                                                    "SUNNY", "HIGHPRESSURE", "SLEET", "DRIZZLE", "SNOWCLEARING", "OVERCASTDARK", 
                                                    "THUNDERSTORM", "SANDSTORM", "HURRICANE", "HAIL", "WHITEOUT", "GROUNDBLIZZARD"};

        public static bool g_debugControls = false;
        public static bool g_infiniteStamina = false;
    }
}
