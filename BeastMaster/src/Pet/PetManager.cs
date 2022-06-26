using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace BeastMaster
{
    public class PetManager
    {
        private List<PetType> PetTypeData = new List<PetType>();

        public Dictionary<Character, PetController> PetControllers
        {
            get; private set;
        }

        private string RootFolder;
        private string SpeciesFolder => RootFolder + "PetType/";

        public PetManager(string rootFolder)
        {
            RootFolder = rootFolder;

            BeastMaster.Log.LogMessage($"Initalising PetManager at : {RootFolder}");
            PetControllers = new Dictionary<Character, PetController>();
            LoadAllPetTypeDataFiles();
        }

        private void LoadAllPetTypeDataFiles()
        {
            BeastMaster.Log.LogMessage($"PetManager Initalising Species Definitions..");
            PetTypeData.Clear();

            if (!HasFolder("PetType"))
            {
                BeastMaster.Log.LogMessage($"PetManager PetType Folder does not exist, creating..");
                Directory.CreateDirectory(RootFolder + "PetType/");
            }

            string[] filePaths = Directory.GetFiles(SpeciesFolder, "*.xml");
            BeastMaster.Log.LogMessage($"PetManager PetType Definitions [{filePaths.Length}] Found.");


            foreach (var item in filePaths)
            {
                BeastMaster.Log.LogMessage($"PetManager PetType Reading {item} data.");
                PetType petTypes = Deserialize<PetType>(item);

                if (!HasSpeciesDefinition(petTypes.PetTypeName))
                {
                    PetTypeData.Add(petTypes);
                    BeastMaster.Log.LogMessage($"PetManager PetType Added {petTypes.PetTypeName} data.");
                }
            }
        }

        public bool HasSpeciesDefinition(string PetType)
        {
            return PetTypeData.Find(x => x.PetTypeName == PetType) != null ? true : false;
        }

        public PetType GetPetDefinitionByName(string PetType)
        {
            if (PetTypeData != null)
            {
                return PetTypeData.Find(x => x.PetTypeName == PetType);
            }

            return null;
        }

        public static void Serialize(PetType item, string path)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            StreamWriter writer = new StreamWriter(path);
            serializer.Serialize(writer.BaseStream, item);
            writer.Close();
        }

        public static T Deserialize<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(path);
            T deserialized = (T)serializer.Deserialize(reader.BaseStream);
            reader.Close();
            return deserialized;
        }

        private bool HasFolder(string FolderLocation)
        {
            return Directory.Exists(FolderLocation);
        }

        #region Controller

        public PetController CreatePetFor(Character _affectedCharacter, PetType PetTypes, Vector3 Position, Vector3 Rotation, string bagUID = "")
        {
            GameObject Prefab = OutwardHelpers.GetFromAssetBundle<GameObject>(PetTypes.SLPackName, PetTypes.AssetBundleName, PetTypes.PrefabName);
            GameObject PetInstance = null;

            if (Prefab == null)
            {
                BeastMaster.Log.LogMessage($"CreatePetForCharacter PrefabName : {PetTypes.PrefabName} from AssetBundle was null.");
                return null;
            }

            if (PetInstance == null)
            {
                PetInstance = GameObject.Instantiate(Prefab, Position, Quaternion.Euler(Rotation));
                GameObject.DontDestroyOnLoad(PetInstance);

                PetController petController = PetInstance.AddComponent<PetController>();
                petController.PetName = PetTypes.GetRandomName();

                petController.SetOwner(_affectedCharacter);
                petController.SetPetTypes(PetTypes);


                PlayerPet playerPet = _affectedCharacter.gameObject.GetComponent<PlayerPet>();

                if (playerPet)
                {
                    playerPet.SetActivePet(petController);
                }

                PetControllers.Add(_affectedCharacter, petController);

                petController.Teleport(Position, Rotation);
                return (petController);
            }

            return null;
        }

        public PetController CreatePetFromInstanceData(Character character, PetInstanceData petInstanceData, bool SetAsActive = true)
        {
            PetController petController = CreatePetFor(character, petInstanceData.PetType, petInstanceData.Position, petInstanceData.Rotation);
            petController.PetName = petInstanceData.PetName;
            petController.PetUID = petInstanceData.PetUID;

            if (SetAsActive)
            {
                character.GetComponent<PlayerPet>().SetActivePet(petController);
            }

            return petController;
        }

        public PetInstanceData CreateInstanceDataFromPet(PetController playerPet)
        {
            BeastMaster.Log.LogMessage($"Creating Instance Data For {playerPet.PetName}");
            PetInstanceData petInstanceData = new PetInstanceData();
            petInstanceData.PetName = playerPet.PetName;
            petInstanceData.PetUID = playerPet.PetUID;
            petInstanceData.PetType = playerPet.PetType;
            petInstanceData.Position = playerPet.transform.position;
            petInstanceData.Rotation = playerPet.transform.eulerAngles;
            return petInstanceData;
        }

        public bool CharacterHasPet(Character character)
        {
            if (PetControllers.ContainsKey(character))
            {
                return true;
            }

            return false;
        }
        public PetController GetControllerForCharacter(Character _affectedCharacter)
        {
            if (PetControllers.ContainsKey(_affectedCharacter))
            {
                return PetControllers[_affectedCharacter];
            }

            return null;
        }

        public void DestroyAllPetInstances()
        {
            BeastMaster.Log.LogMessage($"Destroying All PetType Instances...");

            if (PetControllers != null)
            {
                foreach (var pet in PetControllers.ToList())
                {

                    BeastMaster.Log.LogMessage($"Destroying and unregistring from UI for {pet.Value.PetName} of {pet.Key.Name}");
                    DestroyPet(pet.Key, pet.Value);
                }

                PetControllers.Clear();

                BeastMaster.Log.LogMessage($"All PetType Instances Destroyed Successfully.");
            }
        }
        public void DestroyActivePet(Character character)
        {
            BeastMaster.Log.LogMessage($"Destroying Active PetType instance for {character.Name}");

            if (PetControllers != null)
            {
                if (PetControllers.ContainsKey(character))
                {
                    DestroyPet(character, PetControllers[character]);
                }
            }
        }
        public void DestroyPet(Character character, PetController petController)
        {
            BeastMaster.Log.LogMessage($"Destroying PetType instance for {character.Name}");
            petController.DisableNavMeshAgent();
            GameObject.Destroy(petController.gameObject);
            PetControllers.Remove(character);
        }

        #endregion
    }
}