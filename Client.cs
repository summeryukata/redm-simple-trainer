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
        MENU_WEAPONS,
        MENU_PLAYERLIST,
        MENU_PLAYERLIST_DETAILS
    };

    public class Client : BaseScript
    {
        public Client()
        {
            Tick += FirstTick;
        }

        public static Vector3 SpawnLocation;

        async Task FirstTick()
        {
            await Delay(500);

            //await ChangeModel.SetModel("a_c_fox_01");

            SpawnLocation = new Vector3(-262.849f, 793.404f, 118.587f);

            if (Storage.TryGet("SpawnLocation", out Vector3 spawnLocation))
            {
                SpawnLocation = spawnLocation;
            }

            Function.Call(Hash.NETWORK_SET_FRIENDLY_FIRE_OPTION, true);
            Function.Call(Hash._SET_MINIMAP_REVEALED, true);
            Function.Call(Hash.SET_ENTITY_COORDS, PlayerPedId(), SpawnLocation.X, SpawnLocation.Y, SpawnLocation.Z, 1, 0, 0, 1);

            Tick -= FirstTick;

            Tick += BaseTick;
            Tick += Noclip.Tick;
            Tick += Toast.Tick;
            Tick += GamerTag.Tick;
            //Tick += Commands.DrawTexture;
        }

        private static async Task BaseTick()
        {
            Function.Call((Hash)0x4759cc730f947c81); // ped pop enable  
            Function.Call((Hash)0x1ff00db43026b12f); // veh?

            bool pauseActive = Function.Call<bool>(Hash.IS_PAUSE_MENU_ACTIVE);

            if (!pauseActive)
            {
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

                    Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME);
                }

                if (!Noclip.Enabled)
                {
                    Keyboard.MonitorKeys();
                }
            }
            else
            {
                Noclip.Enabled = false;
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

                case MenuId.MENU_PLAYERLIST:
                    await Menus.PlayerList.Draw();
                    break;

                case MenuId.MENU_PLAYERLIST_DETAILS:
                    await Menus.PlayerDetails.Draw();
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
