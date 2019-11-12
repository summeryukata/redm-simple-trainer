using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client
{
    public class Noclip
    {
        static Control ms_toggleControl = Control.SelectItemWheel;
        static bool ms_toggled = false;
        static bool ms_justToggled = false;
        static float ms_currentSpeed = 5.0f;
        static bool ms_shouldDrawHelp = true;

        static int ms_speedIdx = 0;
        static float[] ms_speeds = new[]
        {
            5.0f,
            10.0f,
            25.0f,
            50.0f,
            100.0f
        };

        public static bool Enabled => ms_toggled;

        public static async Task Tick()
        {
            Keyboard.DisableControlActionWrap(2, ms_toggleControl, true);

            if (Keyboard.IsDisabledControlJustPressedWrap(2, ms_toggleControl))
            {
                ms_toggled = !ms_toggled;
                ms_justToggled = true;
            }

            var ped = Function.Call<int>(Hash.PLAYER_PED_ID);

            void ToggleFreeze(bool val) => Function.Call(Hash.FREEZE_ENTITY_POSITION, ped, val);
            void ToggleAlpha(bool val) => Function.Call(Hash.SET_ENTITY_ALPHA, ped, val ? 0 : 255, false);
            void ToggleCol(bool val) => Function.Call(Hash.SET_ENTITY_COLLISION, ped, val, val);

            void SwitchSpeed()
            {
                ms_speedIdx++;

                if (ms_speedIdx >= ms_speeds.Count())
                {
                    ms_speedIdx = 0;
                }

                ms_currentSpeed = ms_speeds[ms_speedIdx];
            }

            void DrawHintMenu()
            {
                Globals.g_menu_optionCount = 0;
                Drawing.SetMenuTitle($"Noclip (Speed: {ms_currentSpeed}m/s)", "we fylin now baby");
                Drawing.AddMenuEntry("W = Forwards");
                Drawing.AddMenuEntry("S = Backwards");
                Drawing.AddMenuEntry("A = Left");
                Drawing.AddMenuEntry("D = Right");
                Drawing.AddMenuEntry("E = Up");
                Drawing.AddMenuEntry("Q = Down");
                Drawing.AddMenuEntry("LShift = Speed");
                Drawing.AddMenuEntry("H = Hide this text");
                Drawing.StyleMenu();
            }

            if (ms_toggled)
            {
                if (ms_justToggled)
                {
                    ToggleFreeze(true);
                    ToggleAlpha(true);
                    ToggleCol(false);
                    ms_justToggled = false;
                }

                Function.Call(Hash.CLEAR_PED_TASKS_IMMEDIATELY, ped);

                Keyboard.DisableControlActionWrap(2, Control.MoveUpOnly, true);
                Keyboard.DisableControlActionWrap(2, Control.MoveDownOnly, true);
                Keyboard.DisableControlActionWrap(2, Control.Enter, true);
                Keyboard.DisableControlActionWrap(2, Control.Cover, true);
                Keyboard.DisableControlActionWrap(2, Control.MoveLeftOnly, true);
                Keyboard.DisableControlActionWrap(2, Control.MoveRightOnly, true);
                Keyboard.DisableControlActionWrap(2, Control.Sprint, true);
                Keyboard.DisableControlActionWrap(2, Control.Whistle, true);

                if (ms_shouldDrawHelp) DrawHintMenu();

                Vector3 rotation = Vector3.Multiply(Function.Call<Vector3>(Hash._GET_GAMEPLAY_CAM_ROT, 0), (float)Math.PI / 180.0f);
                Vector3 forward = new Vector3((float)(-Math.Sin(rotation.Z) * Math.Abs(Math.Cos(rotation.X))),
                    (float)(Math.Cos(rotation.Z) * Math.Abs(Math.Cos(rotation.X))),
                    (float)Math.Sin(rotation.X));
                forward.Normalize();

                Vector3 right = new Vector3(forward.Y, -forward.X, 0);

                Vector3 camPos = Function.Call<Vector3>(Hash.GET_GAMEPLAY_CAM_COORD);
                Vector3 pedPos = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, ped);
                Vector3 offset = (pedPos - camPos);

                Vector3 up = new Vector3(0, 0, 1);

                float frameTime = Function.Call<float>(Hash.GET_FRAME_TIME);

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.MoveUpOnly))
                {
                    camPos += forward * ms_currentSpeed * frameTime;
                }

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.MoveDownOnly))
                {
                    camPos += (forward * -1) * ms_currentSpeed * frameTime;
                }

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.MoveLeftOnly))
                {
                    camPos += (right * -1) * ms_currentSpeed * frameTime;
                }

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.MoveRightOnly))
                {
                    camPos += right * ms_currentSpeed * frameTime;
                }

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.Enter))
                {
                    camPos += up * ms_currentSpeed * frameTime;
                }

                if (Keyboard.IsDisabledControlPressedWrap(2, Control.Cover))
                {
                    camPos += (up * -1) * ms_currentSpeed * frameTime;
                }

                Vector3 pos = camPos + offset;

                Function.Call(Hash.SET_ENTITY_COORDS_NO_OFFSET, ped, pos.X, pos.Y, pos.Z, true, true, true);

                if (Keyboard.IsDisabledControlJustPressedWrap(2, Control.Sprint))
                {
                    SwitchSpeed();
                }

                if (Keyboard.IsDisabledControlJustPressedWrap(2, Control.Whistle))
                {
                    ms_shouldDrawHelp = !ms_shouldDrawHelp;
                }
            }
            else
            {
                if (ms_justToggled)
                {
                    ToggleFreeze(false);
                    ToggleAlpha(false);
                    ToggleCol(true);

                    ms_justToggled = false;
                }
            }

            await Task.FromResult(0);
        }
    }
    
}
