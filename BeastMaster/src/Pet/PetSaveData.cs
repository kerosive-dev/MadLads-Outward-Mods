using SideLoader.SaveData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeastMaster
{
    public class PetSaveData : PlayerSaveExtension
    {
        //Current Active Mount For Player
        public PetInstanceData ActivePetInstance = new PetInstanceData();

        //Any stored mounts
        public List<PetInstanceData> StoredPets = new List<PetInstanceData>();


        // Serialize your class from the character / world here.
        // The instance members will contain the last loaded save data, if any was found.
        public override void Save(Character character, bool isWorldHost)
        {
            PlayerPet playerPet = character.gameObject.GetComponent<PlayerPet>();

            if (playerPet != null && playerPet.ActivePet != null)
            {
                SaveActivePet(character, playerPet);
            }
            else
            {
                this.ActivePetInstance = null;
            }
        }

        // Your class is loaded from an XML save, apply it to the character / world.
        // The instance members contain the last loaded save data.
        public override void ApplyLoadedSave(Character character, bool isWorldHost)
        {
            //needs redoing
            PlayerPet playerPet = character.gameObject.GetComponent<PlayerPet>();

 
            if (this.ActivePetInstance != null)
            {
                LoadActivePet(character, playerPet);
            }
        }


        private void SaveActivePet(Character character, PlayerPet playerPet)
        {
            if (playerPet.HasActivePet)
            {
                BeastMaster.Log.LogMessage("Saving Active PetType Data");
                this.ActivePetInstance = BeastMaster.PetManager.CreateInstanceDataFromPet(playerPet.ActivePet);
            }
        }

        private void LoadActivePet(Character character, PlayerPet playerPet)
        {
            BeastMaster.Log.LogMessage("Creating PetType From Save Data");
            character.StartCoroutine(LateLoading(character, playerPet));
        }


        private IEnumerator LateLoading(Character character, PlayerPet playerPet)
        {
            yield return new WaitForSeconds(10f);
            PetController basicMountController = BeastMaster.PetManager.CreatePetFromInstanceData(character, this.ActivePetInstance);
            yield break;
        }
    }

    [System.Serializable]
    public class PetInstanceData
    {
        public string PetName;
        public string PetUID;
        public PetType PetType;
        public Vector3 Position;
        public Vector3 Rotation;

        public List<BasicSaveData> ItemSaveData;
    }
}
