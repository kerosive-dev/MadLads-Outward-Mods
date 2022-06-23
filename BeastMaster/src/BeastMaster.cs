using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SideLoader;
using UnityEngine;
using BeastMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BeastMaster
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BeastMaster : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "madlads.beastMaster";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "BeastMaster";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.1";

        // For accessing your BepInEx Logger from outside of this class (eg Plugin.Log.LogMessage("");)
        internal static ManualLogSource Log;

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Log = this.Logger;
            Log.LogMessage($"Hello world from {NAME} {VERSION}!");

            // Any config settings you define should be set up like this:
            ExampleConfig = Config.Bind("ExampleCategory", "ExampleSetting", false, "This is an example setting.");

            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();
        }

        // Update is called once per frame. Use this only if needed.
        // You also have all other MonoBehaviour methods available (OnGUI, etc)
        internal void Update()
        {

        }
    }
}
