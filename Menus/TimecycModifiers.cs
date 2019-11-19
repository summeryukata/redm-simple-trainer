using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class TimecycModifiers : Drawing
    {
        static int fileIdx = 0;
        static int prevFileIdx = 0;
        static int selectedMod;
        static int[] selectedIdxes = new int[] { 0, 0, 0, 0 };

        static float strength;

        static bool firstOpen = true;

        static int totalMods = 0;

        public static async Task Draw()
        {
            if (firstOpen)
            {
                totalMods = NameArrays.TimecycMods1.Count() + NameArrays.TimecycMods2.Count() + NameArrays.TimecycMods3.Count() + NameArrays.TimecycMods4.Count();

                firstOpen = false;
            }

            SetMenuTitle("Timecycle Modifiers", $"there's {totalMods} of them");

            AddArray("File", ref fileIdx, new[] { "1", "2", "3", "4" }, 4);

            if (fileIdx != prevFileIdx)
            {
                selectedMod = 0;
                prevFileIdx = fileIdx;
            }

            string[] selectedMods;

            switch (fileIdx)
            {
                default:
                case 0:
                    selectedMods = NameArrays.TimecycMods1;
                    break;
                case 1:
                    selectedMods = NameArrays.TimecycMods2;
                    break;
                case 2:
                    selectedMods = NameArrays.TimecycMods3;
                    break;
                case 3:
                    selectedMods = NameArrays.TimecycMods4;
                    break;
            }

            // lazy and slow...
            AddArray("Modifier", ref selectedMod, selectedMods, selectedMods.Count());

            AddFloat("Strength", ref strength, 0, 1, 0.1f);

            int clear = AddMenuEntry("Clear modifiers");

            if (strength != 0.0f)
            {
                // this doesn't need to be looped.
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER, selectedMods[selectedMod]);
                Function.Call(Hash.SET_TIMECYCLE_MODIFIER_STRENGTH, strength);
            }

            if (IsEntryPressed(clear))
            {
                strength = 0.0f;

                Function.Call(Hash.CLEAR_TIMECYCLE_MODIFIER);

                Scripts.Toast.AddToast("Cleared modifiers!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
