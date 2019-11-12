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
        internal static void DrawText(string text, int font, float x, float y, float scaleX, float scaleY, int r, int g, int b, int a, bool center)
        {
            Function.Call(Hash.SET_TEXT_SCALE, scaleX, scaleY);
            Function.Call((Hash)0x50a41ad966910f03, r, g, b, a);
            Function.Call((Hash)0xd79334a4bb99bad1, Function.Call<long>((Hash)0xFA925AC00EB830B9, 10, "LITERAL_STRING", text), x, y);
        }

        internal static void ChangeSubmenu(MenuId submenu)
        {
            g_menu_lastSubMenu[g_menu_subMenuLevel] = g_menu_subMenu;
            g_menu_lastOption[g_menu_subMenuLevel] = g_menu_currentOption;
            g_menu_currentOption = 1;
            g_menu_subMenu = submenu;
            g_menu_subMenuLevel++;
        }

        internal static void DrawTexture(string textureStreamed, string textureName, float x, float y, float width, float height, float rotation, int r, int g, int b, int a)
        {
            if (!Function.Call<bool>(Hash.HAS_STREAMED_TEXTURE_DICT_LOADED, textureStreamed))
            {
                Function.Call(Hash.REQUEST_STREAMED_TEXTURE_DICT, textureStreamed, false);
            }
            else
            {
                Function.Call(Hash.DRAW_SPRITE, textureStreamed, textureName, x, y, width, height, rotation, r, g, b, a);
            }
        }

        internal static void SetMenuTitle(string title, string subtitle)
        {
            DrawText($"~bold~{title}", g_titleTextFont, 0.002f, 0.065f, 0.75f, 0.75f, g_titleTextRed, g_titleTextGreen, g_titleTextBlue, g_titleTextAlpha, true);
            DrawText(subtitle, 1, 0.002f, 0.105f, 0.50f, 0.40f, g_titleTextRed, g_titleTextGreen, g_titleTextBlue, g_titleTextAlpha, false);
        }

        private static int maxOptionCount = 18;

        internal static int AddMenuEntry(string option)
        {
            g_menu_optionCount++;

            if (g_menu_currentOption <= maxOptionCount && g_menu_optionCount <= maxOptionCount)
            {
                DrawText(option, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.126f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha, g_titleCentered);
            }
            else if ((g_menu_optionCount > (g_menu_currentOption - maxOptionCount)) && g_menu_optionCount <= g_menu_currentOption)
            {
                DrawText(option, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.126f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha, g_titleCentered);
            }

            return g_menu_optionCount;
        }

        internal static int AddMenuEntryMultiline(string line1, string line2)
        {
            g_menu_optionCount++;

            if (g_menu_currentOption <= maxOptionCount && g_menu_optionCount <= maxOptionCount)
            {
                DrawText(line1, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.132f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha, g_titleCentered);
                DrawText(line2, g_optionsFont, g_titleMenuText, (g_menu_optionCount * 0.035f + 0.12f), 0.1f, 0.25f, g_optionsRed, g_optionsGreen, g_optionsBlue, 200, g_titleCentered);
            }
            else if ((g_menu_optionCount > (g_menu_currentOption - maxOptionCount)) && g_menu_optionCount <= g_menu_currentOption)
            {
                DrawText(line1, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.132f), 0.1f, 0.4f, g_optionsRed, g_optionsGreen, g_optionsBlue, g_optionsAlpha, g_titleCentered);
                DrawText(line2, g_optionsFont, g_titleMenuText, ((g_menu_optionCount - (g_menu_currentOption - maxOptionCount)) * 0.035f + 0.12f), 0.1f, 0.25f, g_optionsRed, g_optionsGreen, g_optionsBlue, 200, g_titleCentered);
            }

            return g_menu_optionCount;
        }

        internal static int AddInt(string option, ref int value, int min, int max, int step = 1)
        {
            int count = AddMenuEntry($"{option}: ~b~&lt; ~s~{value} ~b~&gt;");

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
                        value = value + step;
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
                        value = value - step;
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
                        thisChanges = thisChanges + step;
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
                        thisChanges = thisChanges - step;
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
                        value = value + step;
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
                        value = value - step;
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
                        value = value + 1;
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
                        value = value - 1;
                    }

                    left_press = false;
                }
            }

            return count;
        }

        internal static int AddMenuOption(string option, MenuId submenu)
        {
            int count = AddMenuEntry(option);

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

            float width = 0.2f;

            // background box
            DrawRect(0.25f / 2, 1.0f / 2, 0.25f, 1.0f, 0, 0, 0, 200);

            float fromX = width / 2.0f;

            if (g_menu_optionCount > maxOptionCount)
            {
                if (g_menu_currentOption > maxOptionCount)
                {
                    DrawRect(fromX, ((maxOptionCount * 0.035f) + 0.1415f), width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);

                    DrawRect(fromX, 0.156f, width, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }
                else
                {
                    DrawRect(fromX, ((g_menu_currentOption * 0.035f) + 0.1415f), width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);

                    DrawRect(fromX, 0.156f, width, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }
                if (g_menu_currentOption != g_menu_optionCount)
                {
                    DrawRect(fromX, 0.79f, width, 0.005f, g_indicatorRed, g_indicatorGreen, g_indicatorBlue, g_indicatorAlpha);
                }
            }
            else
            {
                activeY = DiffTrack(((g_menu_currentOption * 0.035f) + 0.1415f), activeY, 15.0f, Function.Call<float>(Hash.GET_FRAME_TIME, new InputArgument[0]));

                DrawRect(fromX, activeY, width, 0.035f, g_activeRed, g_activeGreen, g_activeBlue, g_activeOpacity);
            }
        }

        internal static int AddBool(string option, ref bool value, bool msg = false)
        {
            int count = 0;

            if (value)
            {
                count = AddMenuEntry($"{option}: ~b~On");
            }
            else
            {
                count = AddMenuEntry($"{option}: ~r~Off");
            }

            if (g_menu_currentOption == g_menu_optionCount && g_menu_optionPress)
            {
                value = !value;
            }

            return count;
        }
    }
}
