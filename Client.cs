using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core.Native;

namespace client
{
    public enum MenuId
    {
        MENU_NOTOPEN,
        MENU_MAIN,
        MENU_TELEPORTS,
        MENU_MISC,
        MENU_WEATHER,
        MENU_VEHICLES,
        MENU_CHANGE_MODEL,
        MENU_PLAYER,
        MENU_MODIFIERS,
        MENU_WEAPONS
    };

    public class Client : BaseScript
    {
        public static Client Instance;

        public Client()
        {
            Instance = this;
            Tick += DoTick;
            Tick += Noclip.Tick;
        }

        static bool firstTick = true;

        private static async Task DoTick()
        {
            if (firstTick)
            {
                await Delay(500);

                //await ChangeModel.SetModel("a_c_fox_01");

                Function.Call(Hash.SET_ENTITY_COORDS, PlayerPedId(), -1452.17f, -2329.4f, 42.9603f, 1, 0, 0, 1);

                firstTick = false;
            }

            Function.Call((Hash)0x4759cc730f947c81); // ped pop enable  
            Function.Call((Hash)0x1ff00db43026b12f); // veh?

            Keyboard.DisableControlActionWrap(2, Control.Map, true);

            if (Globals.g_menu_subMenu != MenuId.MENU_NOTOPEN)
            {
                Keyboard.DisableControlActionWrap(2, Control.VehNextRadioTrack, true);
                Keyboard.DisableControlActionWrap(2, Control.VehPrevRadioTrack, true);
                Keyboard.DisableControlActionWrap(2, Control.VehNextRadio, true);
                Keyboard.DisableControlActionWrap(2, Control.VehPrevRadio, true);
                Keyboard.DisableControlActionWrap(2, Control.RadioWheelUd, true);
                Keyboard.DisableControlActionWrap(2, Control.RadioWheelLr, true);
                Keyboard.DisableControlActionWrap(2, Control.VehHeadlight, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendAccept, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendCancel, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendUp, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendDown, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendRight, true);
                Keyboard.DisableControlActionWrap(2, Control.FrontendLeft, true);
                Keyboard.DisableControlActionWrap(2, Control.ScriptPadLeft, true);
                Keyboard.DisableControlActionWrap(2, Control.VehSelectNextWeapon, true);
                Keyboard.DisableControlActionWrap(2, Control.VehSelectPrevWeapon, true);
                Keyboard.DisableControlActionWrap(2, Control.SelectWeaponSpecial, true);
                Keyboard.DisableControlActionWrap(2, Control.SelectWeaponUnarmed, true);
            }

            if (!Noclip.Enabled)
            {
                Keyboard.MonitorKeys();
            }            

            Globals.g_menu_optionCount = 0;

            switch (Globals.g_menu_subMenu)
            {
                case MenuId.MENU_MAIN:
                    await Menus.Main.Draw();
                    break;

                case MenuId.MENU_CHANGE_MODEL:
                    await Menus.ChangeModel.Draw();
                    break;

                case MenuId.MENU_MISC:
                    await Menus.Misc.Draw();
                    break;

                case MenuId.MENU_PLAYER:
                    await Menus.Player.Draw();
                    break;

                case MenuId.MENU_MODIFIERS:
                    await Menus.Modifiers.Draw();
                    break;

                case MenuId.MENU_WEAPONS:
                    await Menus.Weapons.Draw();
                    break;

                default:
                    break;
            }

            await Frame.RunFunctions();

            Globals.left_press = false;
            Globals.right_press = false;
            Globals.g_menu_optionPress = false;

            await Task.FromResult(0);
        }
    }
}
