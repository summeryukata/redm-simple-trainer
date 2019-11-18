using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using System.Drawing;

namespace client
{
    internal class Drawing : Globals
    { 
        internal static void CorrectAspectRatio(ref float x, ref float y)
        {
            const float height = 1080f;
            float ratio = 16f / 9f;
            var width = height * ratio;

            x = x / width * 1000;
            y = y / height * 1000;
        }

        internal static void DrawText(string text, int font, float x, float y, float scaleX, float scaleY, int r, int g, int b, int a, bool center = false)
        {
            Function.Call(Hash.SET_TEXT_SCALE, scaleX, scaleY);
            Function.Call((Hash)0x50a41ad966910f03, r, g, b, a);
            Function.Call(Hash.SET_TEXT_CENTRE, center);
            Function.Call((Hash)0xADA9255D, font);
            // LITERAL_STRING, PLAYER_STRING
            Function.Call(Hash._DRAW_TEXT, Function.Call<long>(Hash._CREATE_VAR_STRING, 10, "LITERAL_STRING", text), x, y);
        }

        internal static void ChangeSubmenu(MenuId submenu)
        {
            g_menu_lastSubMenu[g_menu_subMenuLevel] = g_menu_subMenu;
            g_menu_lastOption[g_menu_subMenuLevel] = g_menu_currentOption;
            g_menu_currentOption = 1;
            g_menu_subMenu = submenu;
            g_menu_subMenuLevel++;
        }

        internal static void DrawTexture(string textureStreamed, string textureName, float x, float y, float width, float height, float rotation, int r, int g, int b, int a, bool p11)
        {
            if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureStreamed))
            {
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureStreamed, false);
            }
            else
            {
                Function.Call(Hash.DRAW_SPRITE, textureStreamed, textureName, x, y, width, height, rotation, r, g, b, a, p11);
            }
        }

        internal static void SetMenuTitle(string title, string subtitle)
        {
            DrawText(title, g_titleTextFont, 0.25f / 2, 0.065f, 0.75f, 0.75f, g_titleTextRed, g_titleTextGreen, g_titleTextBlue, g_titleTextAlpha, true);
            DrawText(subtitle, g_optionsFont, 0.25f / 2, 0.105f, 0.50f, 0.40f, g_titleTextRed, g_titleTextGreen, g_titleTextBlue, g_titleTextAlpha, true);
        }

        private static readonly int maxOptionCount = 18;

        internal static int AddMenuEntry(string option)
        {
            g_menu_optionCount++;

            if (g_menu_currentOption <= maxOptionCount && g_menu_optionCount <= maxOptionCount)
            {
                DrawText(option, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.126f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha);
            }
            else if ((g_menu_optionCount > (g_menu_currentOption - maxOptionCount)) && g_menu_optionCount <= g_menu_currentOption)
            {
                DrawText(option, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.126f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha);
            }

            return g_menu_optionCount;
        }

        internal static int AddMenuEntryMultiline(string line1, string line2)
        {
            g_menu_optionCount++;

            if (g_menu_currentOption <= maxOptionCount && g_menu_optionCount <= maxOptionCount)
            {
                DrawText(line1, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.132f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha);
                DrawText(line2, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.12f), 0.1f, 0.25f, g_optionsRed, g_optionsGreen, g_optionsBlue, 200);
            }
            else if ((g_menu_optionCount > (g_menu_currentOption - maxOptionCount)) && g_menu_optionCount <= g_menu_currentOption)
            {
                DrawText(line1, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.132f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha);
                DrawText(line2, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.12f), 0.1f, 0.25f, g_optionsRed, g_optionsGreen, g_optionsBlue, 200);
            }

            return g_menu_optionCount;
        }

        internal static int AddInt(string option, ref int value, int min, int max, int step = 1, string additionalValuePre = "", string additionalValuePost = "")
        {
            int count = AddMenuEntry($"{option}: ~b~&lt; ~s~{additionalValuePre}{value}{additionalValuePost} ~b~&gt;");

            if (g_menu_currentOption == g_menu_optionCount)
            {
                if (right_press)
                {
                    if (value >= max)
                    {
                        value = min;
                    }
                    else
                    {
                        value += step;
                    }

                    right_press = false;
                }
                else if (left_press)
                {
                    if (value <= min)
                    {
                        value = max;
                    }
                    else
                    {
                        value -= step;
                    }

                    left_press = false;
                }
            }

            return count;
        }

        internal static int AddDouble(string option, double thisCurrent, out double thisChanges, double min, double max, double step = 1.0)
        {
            thisChanges = thisCurrent;

            int count = AddMenuEntry($"{option}: ~b~&lt; ~s~{thisChanges} ~b~&gt;");

            if (g_menu_currentOption == g_menu_optionCount)
            {
                if (right_press)
                {
                    if (thisChanges >= max)
                    {
                        thisChanges = min;
                    }
                    else
                    {
                        thisChanges += step;
                    }

                    right_press = false;
                }
                else if (left_press)
                {
                    if (thisChanges <= min)
                    {
                        thisChanges = max;
                    }
                    else
                    {
                        thisChanges -= step;
                    }

                    left_press = false;
                }
            }

            return count;
        }

        internal static int AddFloat(string option, ref float value, float min, float max, float step = 1.0f)
        {
            int count = AddMenuEntry($"{option} ~b~&lt; ~s~{value} ~b~&gt;");

            if (g_menu_currentOption == g_menu_optionCount)
            {
                if (right_press)
                {
                    if (value >= max)
                    {
                        value = min;
                    }
                    else
                    {
                        value += step;
                    }

                    right_press = false;
                }
                else if (left_press)
                {
                    if (value <= min)
                    {
                        value = max;
                    }
                    else
                    {
                        value -= step;
                    }

                    left_press = false;
                }
            }

            return count;
        }

        internal static int AddArray(string option, ref int value, string[] names, int size)
        {
            int count = AddMenuEntry($"{option}: ~b~&lt; ~s~{names[value]} ~b~&gt;");

            if (g_menu_currentOption == g_menu_optionCount)
            {
                if (right_press)
                {
                    if (value >= (size - 1))
                    {
                        value = 0;
                    }
                    else
                    {
                        value += 1;
                    }

                    right_press = false;
                }
                else if (left_press)
                {
                    if (value <= 0)
                    {
                        value = size - 1;
                    }
                    else
                    {
                        value -= 1;
                    }

                    left_press = false;
                }
            }

            return count;
        }

        internal static int AddMenuOption(string option, MenuId submenu)
        {
            float sizeX = 0.030f;
            float sizeY = sizeX;

            CorrectAspectRatio(ref sizeX, ref sizeY);

            float x = g_menuTextWidth + (sizeX / 2);
            float y = ((g_menu_optionCount + 1) * 0.035f + 0.126f) + sizeY / 2;

            int count = AddMenuEntry(option);
            DrawTexture("pausemenu_textures", "SELECTION_ARROW_RIGHT", x, y, sizeX, sizeY, 0f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha, true);

            if (g_menu_currentOption == g_menu_optionCount && g_menu_optionPress)
            {
                ChangeSubmenu(submenu);
            }

            return count;
        }

        internal static bool IsEntryPressed(int entry)
        {
            if (entry == g_menu_currentOption && g_menu_optionPress)
            {
                return true;
            }

            return false;
        }

        internal static void DrawRect(float fromX, float fromY, float width, float height, int r, int g, int b, int a)
        {
            Function.Call(Hash.DRAW_RECT, fromX, fromY, width, height, r, g, b, a);
        }

        internal static float DiffTrack(float tgt, float cur, float rate, float deltaTime)
        {
            float diff = tgt - cur;
            float step = (diff * rate * deltaTime);

            if (Math.Abs(diff) <= 0.00001f)
            {
                return cur;
            }

            if (Math.Abs(step) > Math.Abs(diff))
            {
                return cur;
            }

            return cur + step;
        }

        internal static float GetCurrentActiveY()
        {
            return (g_menu_currentOption * 0.035f) + 0.1415f;
        }


        internal static float activeY = 0.1415f;

        internal static void StyleMenu()
        {
            Function.Call(Hash.SET_CINEMATIC_BUTTON_ACTIVE, 0);
            Keyboard.DisableControlActionWrap(2, Control.NextCamera, true);
            Keyboard.DisableControlActionWrap(2, Control.Phone, true);
            Keyboard.DisableControlActionWrap(2, Control.CinematicCam, true);
            Keyboard.DisableControlActionWrap(2, Control.SelectCharacterFranklin, true);
            Keyboard.DisableControlActionWrap(2, Control.SelectCharacterMichael, true);
            Keyboard.DisableControlActionWrap(2, Control.SelectCharacterTrevor, true);
            Keyboard.DisableControlActionWrap(2, Control.SelectCharacterMultiplayer, true);
            Keyboard.DisableControlActionWrap(2, Control.CharacterWheel, true);
            Keyboard.DisableControlActionWrap(2, Control.MeleeAttack, true);
            Keyboard.DisableControlActionWrap(2, Control.MeleeGrappleAttack, true);
            Keyboard.DisableControlActionWrap(2, Control.MultiplayerInfo, true);
            Keyboard.DisableControlActionWrap(2, Control.Phone, true);

            // background box
            //DrawRect(0.25f / 2, 1.0f / 2, 0.25f, 1.0f, 0, 0, 0, 200);
            DrawTexture("menu_textures", "translate_bg_1a", 0.25f / 2, 1.0f / 2, 0.25f, 1.0f, 0.0f, 0, 0, 0, 200, true);

            // title box
            DrawTexture("menu_textures", "translate_bg_1a", g_menuTextLeft, 0.05f + (0.1f / 2), g_menuTextWidth + 0.02f, 0.1f, 0.0f, 203, 16, 16, 100, true);
        }

        internal static void StyleSelectedOption()
        {
            string selectedTxd = "boot_flow";
            string selectedTex = "SELECTION_BOX_BG_1D";

            activeY = DiffTrack(((g_menu_currentOption * 0.035f) + 0.14f), activeY, 15.0f, Function.Call<float>(Hash.GET_FRAME_TIME, new InputArgument[0]));

            if (g_menu_optionCount > maxOptionCount)
            {
                if (g_menu_currentOption > maxOptionCount)
                {
                    activeY = ((maxOptionCount * 0.035f) + 0.1415f);

                    //DrawRect(fromX, ((maxOptionCount * 0.035f) + 0.1415f), width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);
                    DrawTexture(selectedTxd, selectedTex, g_menuTextLeft, activeY, g_menuTextWidth, 0.035f, 0.0f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity, true);

                    DrawRect(g_menuTextLeft, 0.156f, g_menuTextWidth, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }
                else
                {
                    //DrawRect(fromX, ((g_menu_currentOption * 0.035f) + 0.1415f), width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);
                    DrawTexture(selectedTxd, selectedTex, g_menuTextLeft, activeY, g_menuTextWidth, 0.035f, 0.0f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity, true);

                    DrawRect(g_menuTextLeft, 0.156f, g_menuTextWidth, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }

                if (g_menu_currentOption != g_menu_optionCount)
                {
                    DrawRect(g_menuTextLeft, 0.79f, g_menuTextWidth, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }
            }
            else
            {
                //DrawRect(fromX, activeY, width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);
                DrawTexture(selectedTxd, selectedTex, g_menuTextLeft, activeY, g_menuTextWidth, 0.035f, 0.0f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity, true);
            }
        }

        static Dictionary<string, Action<bool>> boolCallbacks = new Dictionary<string, Action<bool>>();

        internal static async Task BoolCallbacks()
        {
            foreach (var cb in boolCallbacks)
            {
                cb.Value?.Invoke(true);
            }

            await Task.FromResult(0);
        }

        internal static int AddBool(string option, ref bool value, bool msg = true, Action<bool> cb = null)
        {
            int count;

            float sizeX = 0.030f;
            float sizeY = sizeX;

            CorrectAspectRatio(ref sizeX, ref sizeY);

            float x = g_menuTextWidth + (sizeX / 2);
            float y = ((g_menu_optionCount + 1) * 0.035f + 0.126f) + sizeY / 2;

            count = AddMenuEntry(option);
            DrawTexture("generic_textures", "tick_box", x, y, sizeX, sizeY, 0, 255, 255, 255, 255, true);

            if (value)
            {
                if (!boolCallbacks.ContainsKey(option))
                {
                    boolCallbacks.Add(option, cb);
                }

                DrawTexture("generic_textures", "tick", x, y, sizeX, sizeY, 0, 203, 16, 16, 255, true);
            }
            else
            {
                if (boolCallbacks.ContainsKey(option))
                {
                    cb?.Invoke(false);
                    boolCallbacks.Remove(option);
                }
            }

            if (g_menu_currentOption == g_menu_optionCount && g_menu_optionPress)
            {
                value = !value;

                if (msg) Toast.AddToast($"{option} is now {value}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            return count;
        }
    }
}