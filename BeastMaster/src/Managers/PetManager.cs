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
            BeastMaster.Log.LogMessage("Getting the template!");
            var pack = SL.GetSLPack("madlads-beastmasterClass");

            Rhino = pack.CharacterTemplates["beastmaster.testpet"];
            BeastMaster.Log.LogMessage($"This is the UID : {Rhino.UID}");
            Rhino.OnSpawn += OnPetSpawn;
        }

        private static void OnPetSpawn(Character petCharacter, string rpcData)
        {
            var charUID = rpcData;

            if (!PhotonNetwork.isNonMasterClientInRoom)
            {
                var owner = CharacterManager.Instance.GetCharacter(charUID);

                var tele = petCharacter.gameObject.AddComponent<PetTeleport>();
                tele.m_character = petCharacter;
                tele.TargetCharacter = owner.transform;
            }
        }
    }
}
