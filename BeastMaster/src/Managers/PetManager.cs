using BeastMaster;
using SideLoader;
using UnityEngine;

namespace BeastMaster
{
    public static class PetManager
    {
        public static SL_Character Rhino;

        internal static void Init()
        {
            var pack = SL.GetSLPack("madlads-beastmasterClass");

            Rhino = pack.CharacterTemplates["beastmaster.testpet"];
            Rhino.OnSpawn += OnPetSpawn;
        }

        private static void OnPetSpawn(Character petCharacter, string rpcData)
        {
            var charUID = rpcData;

            if (!PhotonNetwork.isNonMasterClientInRoom)
            {
                var owner = CharacterManager.Instance.GetCharacter(charUID);
                /*CharacterVisuals petPrefab = OutwardHelpers.GetFromAssetBundle<CharacterVisuals>("madlads-beastmasterClass", "madlad-pets", "Rhino");
                petCharacter.m_visualsHolder = petPrefab;*/

                var tele = petCharacter.gameObject.AddComponent<PetTeleport>();
                tele.m_character = petCharacter;
                tele.TargetCharacter = owner.transform;
            }
        }
    }
}
