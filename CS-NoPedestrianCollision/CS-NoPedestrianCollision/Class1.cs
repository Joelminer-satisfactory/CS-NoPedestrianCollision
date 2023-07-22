using ICities;
using CitiesHarmony.API;
using HarmonyLib;
using System;

namespace NoPedestrianCollision
{
    public class DisablePedestrianCheck : LoadingExtensionBase, IUserMod
    {
        public string Name { get { return "No Pedestrian Collision"; } }
        public string Description { get { return "This mod disables the collision check for pedestrians, meaning they will drive through them without stopping"; } }
        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }
    public class Patcher
    {
        private const string HarmonyId = "yourname.YourModName";
        private static bool patched = false;

        public static void PatchAll()
        {
            if (patched) return;

            patched = true;
            var harmony = new Harmony(HarmonyId);
            harmony.PatchAll(typeof(Patcher).Assembly);
        }

        public static void UnpatchAll()
        {
            if (!patched) return;

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
            patched = false;
        }

        [HarmonyPatch(typeof(CarAI), "CheckOtherVehicles")]
        class MyPatch
        {
            public static void Prefix(ref int lodPhysics)
            {
                if (lodPhysics == 0)
                {
                    lodPhysics = (Int32)1;
                }
            }
        }
    }
}