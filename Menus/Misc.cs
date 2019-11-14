using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Misc : Drawing
    {
        static int balloon = -1;

        public static async Task Draw()
        {
            SetMenuTitle("Utils", "miscellaneous things~");

            AddBool("Display coords?", ref g_ingameCoords);
            AddBool("Debug controls?", ref g_debugControls);

            /*int balloon = AddMenuEntry("Spawn balloon");
            int flyForwards = AddMenuEntry("Fly 100 metres forward");*/

            int defaultSpawn = AddMenuEntry("Set default spawn");

            StyleMenu();

            /*if (IsEntryPressed(balloon))
            {
                Vector3 pos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, API.PlayerPedId());
                balloon = await Main.CreateVehicle(GenHash("hotAirBalloon01"), pos, 0);
            }

            if (IsEntryPressed(flyForwards))
            {
                if (balloon != -1 && Function.Call<bool>(Hash.DOES_ENTITY_EXIST, balloon))
                {
                    int ped = API.PlayerPedId();
                    Vector3 pos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, ped);
                    Vector3 rotation = Vector3.Multiply(Function.Call<Vector3>(Hash.GET_ENTITY_ROTATION, ped), (float)Math.PI / 180.0f);
                    Vector3 forward = new Vector3((float)(-Math.Sin(rotation.Z) * Math.Abs(Math.Cos(rotation.X))),
                        (float)(Math.Cos(rotation.Z) * Math.Abs(Math.Cos(rotation.X))),
                        (float)Math.Sin(rotation.X));
                    forward.Normalize();

                    Vector3 target = (pos + (forward * 100));
                    float heading = Function.Call<float>(Hash.GET_ENTITY_HEADING, ped);

                    Function.Call(Hash.TASK_FLY_TO_COORD, balloon, target.X, target.Y, target.Z, 50.0f, 0, heading, 0); // last is unk
                }
            }*/

            if (IsEntryPressed(defaultSpawn))
            {
                Vector3 vec = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, Function.Call<int>(Hash.PLAYER_PED_ID));
                Storage.Set("SpawnLocation", vec);

                Client.SpawnLocation = vec;

                Toast.AddToast("Spawn location set!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
