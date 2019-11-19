using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Main : Drawing
    {
        static int selectedVehicle = 0;

        static int spawned = -1;

        public static async Task PerformRequest(int hash)
        {
            if (Function.Call<bool>(Hash.IS_MODEL_IN_CDIMAGE, hash) && Function.Call<bool>(Hash.IS_MODEL_VALID, hash))
            {
                Debug.WriteLine($"Requesting model {hash}");

                Function.Call(Hash.REQUEST_MODEL, hash);

                while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
                {
                    await BaseScript.Delay(50);
                }
            }
            else
            {
                Debug.WriteLine("Requested model not valid");
            }
        }

        public static async Task<int> CreateVehicle(int hash, Vector3 pos, float head)
        {
            if (spawned != -1)
            {
                Function.Call(Hash.DELETE_VEHICLE, spawned);
                spawned = -1;
            }

            await PerformRequest(hash);

            spawned = Function.Call<int>(Hash.CREATE_VEHICLE, hash, pos.X, pos.Y, pos.Z, 0, 0, 0, head - 90, true, false, 0, 0);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, spawned, true, true);

            return spawned;
        }

        public static async Task Draw()
        {
            SetMenuTitle("owo", "what's this?");

            int list = AddMenuOption("Player Listing", MenuId.MENU_PLAYERLIST);

            AddMenuOption("Player", MenuId.MENU_PLAYER);
            AddMenuOption("Util", MenuId.MENU_MISC);
            AddMenuOption("Timecyc Mods", MenuId.MENU_MODIFIERS);
            AddMenuOption("Weapons", MenuId.MENU_WEAPONS);

            string zero = g_currentTimeMinutes < 10 ? "0" : "";

            AddInt("Time", ref g_currentTimeMinutes, 0, 60, 1, additionalValuePre: $"{g_currentTimeHours}:{zero}");
            AddArray("Weather", ref g_currentWeatherIdx, g_weathers, g_weathers.Count());

            int v = AddArray("Vehicles", ref selectedVehicle, NameArrays.Vehicles, NameArrays.Vehicles.Count());

            int w;

            if (spawned != -1)
            {
                w = AddMenuEntryMultiline("Warp in to vehicle", "Press enter to");
            }
            else
            {
                w = 0;
            }

            var pedId = Function.Call<int>(Hash.PLAYER_PED_ID);

            if (IsEntryPressed(list))
            {
                // sadly, not yet
                //Menus.PlayerList.Players = (int[])API.GetActivePlayers();

                List<int> players = new List<int>();

                for (int i = 0; i < 256; i++)
                {
                    if (Function.Call<bool>(Hash.NETWORK_IS_PLAYER_CONNECTED, i))
                    {
                        players.Add(i);
                    }
                }

                Menus.PlayerList.Players = players.ToArray();
            }

            if (IsEntryPressed(v))
            {
                var coors = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, pedId);
                var forward = Function.Call<Vector3>(Hash.GET_ENTITY_FORWARD_VECTOR, pedId);
                forward.Normalize();
                var head = Function.Call<float>(Hash.GET_ENTITY_HEADING, pedId);
                var veh = NameArrays.Vehicles[selectedVehicle];
                var hash = GenHash(veh);

                await CreateVehicle(hash, coors + (forward * 3), head);

                Scripts.Toast.AddToast($"Spawned a {veh}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(w))
            {
                Function.Call(Hash.SET_PED_INTO_VEHICLE, pedId, spawned, -1);
            }

            await Task.FromResult(0);
        }
    }
}