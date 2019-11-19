using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class PlayerDetails : Drawing
    {
        public static int SelectedPlayer = -1;
        public static string SelectedPlayerName = "";

        public static async Task Draw()
        {
            SetMenuTitle($"{SelectedPlayerName} is a player", "and they're connected");

            int tpToPlayer = AddMenuEntry("Teleport to player");
            int tpToVeh = AddMenuEntry("Teleport to vehicle");

            int pedId = Function.Call<int>(Hash.PLAYER_PED_ID);
            int targetPed = Function.Call<int>(Hash.GET_PLAYER_PED, SelectedPlayer);
            Vector3 targetPos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, targetPed, 0);
            int targetVeh = Function.Call<int>(Hash.GET_VEHICLE_PED_IS_USING, targetPed);

            if (IsEntryPressed(tpToPlayer))
            {
                if (Function.Call<bool>(Hash.IS_PED_SITTING_IN_ANY_VEHICLE, pedId))
                {
                    Function.Call(Hash.SET_ENTITY_COORDS, Function.Call<int>(Hash.GET_VEHICLE_PED_IS_USING, pedId), targetPos.X, targetPos.Y, targetPos.Z, 1, 0, 0, 1);
                }
                else
                {
                    Function.Call(Hash.SET_ENTITY_COORDS, pedId, targetPos.X, targetPos.Y, targetPos.Z, 1, 0, 0, 1);
                }

                Scripts.Toast.AddToast($"Teleported to {SelectedPlayerName}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(tpToVeh))
            {
                if (Function.Call<bool>(Hash.DOES_ENTITY_EXIST, targetPed))
                {
                    if (Function.Call<bool>(Hash.IS_PED_IN_ANY_VEHICLE, targetPed, false))
                    {
                        if (!Function.Call<bool>(Hash.GET_VEHICLE_DOORS_LOCKED_FOR_PLAYER, targetVeh, pedId))
                        {
                            int seatNum = 0 + Function.Call<int>(Hash.GET_VEHICLE_NUMBER_OF_PASSENGERS, targetVeh);
                            int passNum = Function.Call<int>(Hash.GET_VEHICLE_MAX_NUMBER_OF_PASSENGERS, targetVeh);

                            while (seatNum < passNum)
                            {
                                if (Function.Call<bool>(Hash.IS_VEHICLE_SEAT_FREE, targetVeh, seatNum))
                                {
                                    Function.Call(Hash.SET_ENTITY_COORDS, pedId, targetPos.X, targetPos.Y, targetPos.Z, 1, 0, 0, 1);
                                    Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, pedId);
                                    Function.Call(Hash.SET_PED_INTO_VEHICLE, pedId, targetVeh, seatNum);
                                    Function.Call(Hash.NETWORK_SET_IN_SPECTATOR_MODE, 0, targetPed);
                                    break;
                                }
                                else
                                {
                                    seatNum++;
                                }
                            }
                        }
                    }
                }

                Scripts.Toast.AddToast($"Teleported to {SelectedPlayerName}'s vehicle!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
