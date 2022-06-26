using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeastMaster
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public partial class BeastMaster : BaseUnityPlugin
    {
        public const string GUID = "madlads.beastMaster";
        public const string NAME = "BeastMaster";
        public const string VERSION = "1.0.0";

        public static bool Debug = true;

        internal static ManualLogSource Log;
        public static Canvas MainCanvas
        {
            get; private set;
        }
       
        public static PetManager PetManager
        {
            get; private set;
        }


        public static string RootFolder;


        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;
            RootFolder = this.Info.Location.Replace("BeastMaster.dll", "");
            PetManager = new PetManager(RootFolder);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            new Harmony(GUID).PatchAll();
        }

        private void SceneManager_sceneLoaded(Scene Scene, LoadSceneMode LoadMode)
        {
            if (Scene.name == "MainMenu_Empty")
            {
                PetManager.DestroyAllPetInstances();
            }
        }
    }
}
