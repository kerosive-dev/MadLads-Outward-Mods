using BepInEx;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using SideLoader;

namespace BeastMaster
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency(SL.GUID, BepInDependency.DependencyFlags.HardDependency)]

    public class BeastMaster : BaseUnityPlugin
    {
        public static BeastMaster Instance;
        public const string GUID = "madlads.beastmaster";
        public const string NAME = "BeastMaster";
        public const string VERSION = "1.0.0";

        internal static ManualLogSource Log;

        internal void Awake()
        {
            Log = this.Logger;
            Log.LogMessage($"{NAME} {VERSION} loading...");

            SL.OnPacksLoaded += SL_OnPacksLoaded;

            var harmony = new Harmony(GUID);
            harmony.PatchAll();
        }

        private void SL_OnPacksLoaded()
        {
            PetManager.Init();
            SkillManager.Init();
        }
    }
}
