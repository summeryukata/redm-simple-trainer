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
            while (!Function.Call<bool>(Hash.HAS_MODEL_LOADED, hash))
            {
                Function.Call(Hash.REQUEST_MODEL, hash);
                await BaseScript.Delay(50);
            }
        }

        private static async Task CreateVehicle(int hash, Vector3 pos, float head)
        {
            if (spawned != -1)
            {
                Function.Call(Hash.DELETE_VEHICLE, spawned);
                spawned = -1;
            }

            await PerformRequest(hash);

            spawned = Function.Call<int>(Hash.CREATE_VEHICLE, hash, pos.X, pos.Y, pos.Z, 0, 0, 0, head - 90, false, false, 0, 0);
            Function.Call(Hash.SET_ENTITY_AS_MISSION_ENTITY, spawned, true, true);
        }

        public static async Task Draw()
        {
            SetMenuTitle("hi!", "this is native menu");

            int list = AddMenuOption("Player Listing", MenuId.MENU_PLAYERLIST);

            AddMenuOption("Player", MenuId.MENU_PLAYER);
            AddMenuOption("Util", MenuId.MENU_MISC);
            AddMenuOption("Timecyc Mods", MenuId.MENU_MODIFIERS);
            AddMenuOption("Weapons", MenuId.MENU_WEAPONS);

            string zero = g_currentTimeMinutes < 10 ? "0" : "";

            AddInt("Time", ref g_currentTimeMinutes, 0, 60, 1, additionalValuePre: $"{g_currentTimeHours}:{zero}");
            AddArray("Weather", ref g_currentWeatherIdx, g_weathers, g_weathers.Count());

            int v = AddArray("Vehicles", ref selectedVehicle, vehicles, vehicles.Count());

            int w;

            if (spawned != -1)
            {
                w = AddMenuEntryMultiline("Warp in to vehicle", "Press enter to");
            }
            else
            {
                w = 0;
            }

            StyleMenu();

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
                var veh = vehicles[selectedVehicle];
                var hash = GenHash(veh);

                await CreateVehicle(hash, coors + (forward * 3), head);

                Toast.AddToast($"Spawned a {veh}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(w))
            {
                Function.Call(Hash.SET_PED_INTO_VEHICLE, pedId, spawned, -1);
            }

            await Task.FromResult(0);
        }

        // auto-generated
        static string[] vehicles = new[]
        {
            "privateopensleeper02x",
            "privateopensleeper01x",
            "steamerDummy",
            "armoredCar01x",
            "armoredCar03x",
            "privatebaggage01x",
            "smuggler02",
            "keelboat",
            "boatSteam02x",
            "midlandrefrigeratorCar",
            "midlandboxcar05x",
            "caboose01x",
            "canoe",
            "canoeTreeTrunk",
            "cart01",
            "cart02",
            "cart03",
            "cart04",
            "cart05",
            "cart06",
            "cart07",
            "cart08",
            "coach2",
            "coach3",
            "coach3_cutscene",
            "coach4",
            "coach5",
            "coach6",
            "buggy01",
            "buggy02",
            "buggy03",
            "ArmySupplyWagon",
            "chuckwagon000x",
            "supplywagon",
            "supplywagon2",
            "logwagon",
            "logwagon2",
            "coal_wagon",
            "chuckwagon002x",
            "gatling_gun",
            "gatlingMaxim02",
            "handcart",
            "horseBoat",
            "hotAirBalloon01",
            "hotchkiss_cannon",
            "mineCart01x",
            "northflatcar01x",
            "privateflatcar01x",
            "northpassenger01x",
            "northpassenger03x",
            "privatepassenger01x",
            "oilWagon01x",
            "oilWagon02x",
            "pirogue",
            "pirogue2",
            "policeWagon01x",
            "policeWagongatling01x",
            "privateCoalCar01x",
            "NorthCoalCar01x",
            "winterSteamer",
            "wintercoalcar",
            "privateboxcar04x",
            "privateboxcar02x",
            "privateboxcar01x",
            "coalHopper01x",
            "privateObservationcar",
            "privateArmoured",
            "privateDining01x",
            "privateRooms01x",
            "privateSteamer01x",
            "northSteamer01x",
            "GhostTrainSteamer",
            "GhostTrainCoalCar",
            "GhostTrainPassenger",
            "GhostTrainCaboose",
            "rcBoat",
            "rowboat",
            "rowboatSwamp",
            "rowboatSwamp02",
            "ship_guama02",
            "turbineboat",
            "ship_nbdGuama",
            "ship_nbdGuama2",
            "skiff",
            "stagecoach001x",
            "stagecoach002x",
            "stagecoach003x",
            "stagecoach004x",
            "stagecoach005x",
            "stagecoach006x",
            "trolley01x",
            "TugBoat2",
            "wagon02x",
            "wagon03x",
            "wagon04x",
            "wagon05x",
            "wagon06x",
            "wagonCircus01x",
            "wagonCircus02x",
            "wagonDoc01x",
            "wagonPrison01x",
            "wagonWork01x",
            "wagonDairy01x",
            "wagonTraveller01x",
            "breach_cannon",
            "utilliwag",
            "gatchuck",
            "gatchuck_2",
        };
    }
}