using HarmonyLib;
using System;
using UnityEngine;

namespace BeastMaster
{
    [HarmonyPatch(typeof(Character), nameof(Character.Awake))]
    public class CharacterAwakePatch
    {
        static void Postfix(Character __instance)
        {
            __instance.gameObject.AddComponent<PlayerPet>();
        }
    }


    [HarmonyPatch(typeof(Character), nameof(Character.Teleport), new Type[] { typeof(Vector3), typeof(Vector3) })]
    public class CharacterTeleport
    {
        static void Postfix(Character __instance, Vector3 _pos, Vector3 _rot)
        {
            PlayerPet playerPet = __instance.gameObject.GetComponent<PlayerPet>();

            if (playerPet != null && playerPet.HasActivePet)
            {
                BeastMaster.Log.LogMessage($"Warping {playerPet.ActivePet.PetName} with {playerPet.Character.Name}");
                playerPet.ActivePet.Teleport(_pos, _rot);
            }
        }
    }
}