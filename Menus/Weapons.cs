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

            int weap = AddArray("Weapon", ref selectedWeapIdx, weaponNames, weaponNames.Count());
            int ammo = AddArray("Ammo Type", ref selectedAmmoType, ammoTypes, ammoTypes.Count());
            int remove = AddMenuEntry("Remove Weapons");

            int ped = Function.Call<int>(Hash.PLAYER_PED_ID);

            if (IsEntryPressed(weap))
            {
                await Main.PerformRequest(GenHash(weaponModelNames[selectedWeapIdx]));

                GiveWeapon(ped, weaponNames[selectedWeapIdx], 100, false, 1, false, 0.0f);

                Toast.AddToast($"Gave a {weaponNames[selectedWeapIdx]}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(ammo))
            {
                GiveAmmoOfTypeToPed(ped, ammoTypes[selectedAmmoType], 100);
                Toast.AddToast($"Gave ammo of type {ammoTypes[selectedAmmoType]}!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            if (IsEntryPressed(remove))
            {
                Function.Call(Hash.REMOVE_ALL_PED_WEAPONS, ped, 1, 1);
                Toast.AddToast($"Removed all weapons!", 3000, 0.25f + (0.3f / 2), GetCurrentActiveY());
            }

            StyleMenu();

            await Task.FromResult(0);
        }

        // auto-generated
        static string[] weaponModelNames = new[]
        {
"s_interact_jug_pickup",
"s_interact_lantern03x_pickup",
"s_interact_torch",
"w_melee_brokenSword01",
"w_melee_fishingpole02",
"w_melee_hatchet01",
"w_melee_hatchet02",
"w_melee_hatchet03",
"w_melee_hatchet04",
"w_melee_hatchet05",
"w_melee_hatchet06",
"w_melee_hatchet06",
"w_melee_hatchet07",
"w_melee_hatchet07",
"w_melee_knife01",
"w_melee_knife02",
"w_melee_knife03",
"w_melee_knife05",
"w_melee_knife14",
"w_melee_knife16",
"w_melee_knife17",
"w_melee_knife18",
"w_melee_machete01",
"w_melee_tomahawk01",
"w_melee_tomahawk02",
"w_pistol_m189901",
"w_pistol_mauser01",
"w_pistol_semiauto01",
"w_pistol_volcanic01",
"W_REPEATER_CARBINE01",
"w_repeater_henry01",
"w_repeater_pumpaction01",
"w_repeater_winchester01",
"w_revolver_cattleman01",
"w_revolver_doubleaction01",
"w_revolver_lemat01",
"w_revolver_schofield01",
"w_rifle_boltaction01",
"W_RIFLE_CARCANO01",
"w_rifle_rollingblock01",
"w_rifle_springfield01",
"w_shotgun_doublebarrel01",
"w_shotgun_pumpaction01",
"w_shotgun_repeating01",
"w_shotgun_sawed01",
"w_shotgun_semiauto01",
"W_SP_BowArrow",
"w_throw_dynamite01",
"w_throw_molotov01",
        };
        static string[] weaponNames = new[]
        {
"WEAPON_MOONSHINEJUG",
"WEAPON_MELEE_LANTERN_ELECTRIC",
"WEAPON_MELEE_TORCH",
"WEAPON_MELEE_BROKEN_SWORD",
"WEAPON_FISHINGROD",
"WEAPON_MELEE_HATCHET",
"WEAPON_MELEE_CLEAVER",
"WEAPON_MELEE_ANCIENT_HATCHET",
"WEAPON_MELEE_HATCHET_VIKING",
"WEAPON_MELEE_HATCHET_HEWING",
"WEAPON_MELEE_HATCHET_DOUBLE_BIT",
"WEAPON_MELEE_HATCHET_DOUBLE_BIT_RUSTED",
"WEAPON_MELEE_HATCHET_HUNTER",
"WEAPON_MELEE_HATCHET_HUNTER_RUSTED",
"WEAPON_MELEE_KNIFE_JOHN",
"WEAPON_MELEE_KNIFE",
"WEAPON_MELEE_KNIFE_JAWBONE",
"WEAPON_THROWN_THROWING_KNIVES",
"WEAPON_MELEE_KNIFE_MINER",
"WEAPON_MELEE_KNIFE_CIVIL_WAR",
"WEAPON_MELEE_KNIFE_BEAR",
"WEAPON_MELEE_KNIFE_VAMPIRE",
"WEAPON_MELEE_MACHETE",
"WEAPON_THROWN_TOMAHAWK",
"WEAPON_THROWN_TOMAHAWK_ANCIENT",
"WEAPON_PISTOL_M1899",
"WEAPON_PISTOL_MAUSER",
"WEAPON_PISTOL_SEMIAUTO",
"WEAPON_PISTOL_VOLCANIC",
"WEAPON_REPEATER_CARBINE",
"WEAPON_REPEATER_HENRY",
"WEAPON_RIFLE_VARMINT",
"WEAPON_REPEATER_WINCHESTER",
"WEAPON_REVOLVER_CATTLEMAN",
"WEAPON_REVOLVER_DOUBLEACTION",
"WEAPON_REVOLVER_LEMAT",
"WEAPON_REVOLVER_SCHOFIELD",
"WEAPON_RIFLE_BOLTACTION",
"WEAPON_SNIPERRIFLE_CARCANO",
"WEAPON_SNIPERRIFLE_ROLLINGBLOCK",
"WEAPON_RIFLE_SPRINGFIELD",
"WEAPON_SHOTGUN_DOUBLEBARREL",
"WEAPON_SHOTGUN_PUMP",
"WEAPON_SHOTGUN_REPEATING",
"WEAPON_SHOTGUN_SAWEDOFF",
"WEAPON_SHOTGUN_SEMIAUTO",
"WEAPON_BOW",
"WEAPON_THROWN_DYNAMITE",
"WEAPON_THROWN_MOLOTOV",
        };

        // some of these might not be ammo types
        static string[] ammoTypes = new[]
        {
"AMMO_PISTOL",
"AMMO_PISTOL_SPLIT_POINT",
"AMMO_PISTOL_EXPRESS",
"AMMO_PISTOL_EXPRESS_EXPLOSIVE",
"AMMO_PISTOL_HIGH_VELOCITY",
"AMMO_REVOLVER",
"AMMO_REVOLVER_SPLIT_POINT",
"AMMO_REVOLVER_EXPRESS",
"AMMO_REVOLVER_EXPRESS_EXPLOSIVE",
"AMMO_REVOLVER_HIGH_VELOCITY",
"AMMO_RIFLE",
"AMMO_RIFLE_SPLIT_POINT",
"AMMO_RIFLE_EXPRESS",
"AMMO_RIFLE_EXPRESS_EXPLOSIVE",
"AMMO_RIFLE_HIGH_VELOCITY",
"AMMO_22",
"AMMO_REPEATER",
"AMMO_REPEATER_SPLIT_POINT",
"AMMO_REPEATER_EXPRESS",
"AMMO_REPEATER_EXPRESS_EXPLOSIVE",
"AMMO_REPEATER_HIGH_VELOCITY",
"AMMO_SHOTGUN",
"AMMO_SHOTGUN_BUCKSHOT_INCENDIARY",
"AMMO_SHOTGUN_SLUG",
"AMMO_SHOTGUN_SLUG_EXPLOSIVE",
"AMMO_ARROW",
"AMMO_TURRET",
"ML_UNARMED",
"AMMO_MOLOTOV",
"AMMO_MOLOTOV_VOLATILE",
"AMMO_DYNAMITE",
"AMMO_DYNAMITE_VOLATILE",
"AMMO_THROWING_KNIVES",
"AMMO_THROWING_KNIVES_IMPROVED",
"AMMO_THROWING_KNIVES_POISON",
"AMMO_THROWING_KNIVES_JAVIER",
"AMMO_THROWING_KNIVES_CONFUSE",
"AMMO_THROWING_KNIVES_DISORIENT",
"AMMO_THROWING_KNIVES_DRAIN",
"AMMO_THROWING_KNIVES_TRAIL",
"AMMO_THROWING_KNIVES_WOUND",
"AMMO_TOMAHAWK",
"AMMO_TOMAHAWK_ANCIENT",
"AMMO_TOMAHAWK_HOMING",
"AMMO_TOMAHAWK_IMPROVED",
"AMMO_HATCHET",
"AMMO_HATCHET_ANCIENT",
"AMMO_HATCHET_CLEAVER",
"AMMO_HATCHET_DOUBLE_BIT",
"AMMO_HATCHET_DOUBLE_BIT_RUSTED",
"AMMO_HATCHET_HEWING",
"AMMO_HATCHET_HUNTER",
"AMMO_HATCHET_HUNTER_RUSTED",
"AMMO_HATCHET_VIKING",
"AMMO_ARROW_FIRE",
"AMMO_ARROW_DYNAMITE",
"AMMO_ARROW_SMALL_GAME",
"AMMO_ARROW_IMPROVED",
"AMMO_ARROW_POISON",
"AMMO_ARROW_CONFUSION",
"AMMO_ARROW_DISORIENT",
"AMMO_ARROW_DRAIN",
"AMMO_ARROW_TRAIL",
"AMMO_ARROW_WOUND",
        };
    }
}
