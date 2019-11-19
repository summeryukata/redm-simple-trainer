using CitizenFX.Core.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client.Menus
{
    internal class Weapons : Drawing
    {
        static void GiveWeapon(int ped, string weapName, int ammoCount, bool equip, int group, bool leftHanded, float condition)
            => Function.Call((Hash)0x5E3BDDBCB83F3D84, ped, GenHash(weapName), ammoCount, equip, true, group, false, 1056964608, 1065353216, leftHanded, condition);

        static void GiveAmmoOfTypeToPed(int ped, string ammoType, int ammoCount)
            => Function.Call((Hash)0x106A811C6D3035F3, ped, GenHash(ammoType), ammoCount, 752097756);

        static int selectedWeapIdx = 0;
        static int selectedAmmoType = 0;

        public static async Task Draw()
        {
            SetMenuTitle("Weapons", "pew pew (but slo mo)");

            int weap = AddArray("Weapon", ref selectedWeapIdx, NameArrays.WeaponNames, NameArrays.WeaponNames.Count());
            int ammo = AddArray("Ammo Type", ref selectedAmmoType, NameArrays.AmmoTypes, NameArrays.AmmoTypes.Count());
            int remove = AddMenuEntry("Remove Weapons");

            int ped = Function.Call<int>(Hash.PLAYER_PED_ID);

            if (IsEntryPressed(weap))
            {
                await Main.PerformRequest(GenHash(NameArrays.WeaponModelNames[selectedWeapIdx]));

                GiveWeapon(ped, NameArrays.WeaponNames[selectedWeapIdx], 100, false, 1, false, 0.0f);

                Scripts.Toast.AddToast($"Gave a {NameArrays.WeaponNames[selectedWeapIdx]}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(ammo))
            {
                GiveAmmoOfTypeToPed(ped, NameArrays.AmmoTypes[selectedAmmoType], 100);
                Scripts.Toast.AddToast($"Gave ammo of type {NameArrays.AmmoTypes[selectedAmmoType]}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(remove))
            {
                Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, ped, 1, 1);
                Scripts.Toast.AddToast($"Removed all weapons!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            await Task.FromResult(0);
        }
    }
}
