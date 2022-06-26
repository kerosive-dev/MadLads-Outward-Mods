using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BeastMaster
{
    /// <summary>
    /// Maybe skip using an SL_Effect all together.
    /// </summary>
    public class SL_SpawnPet : SL_Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SpawnPet);
        public Type GameModel => typeof(SLEx_SpawnPet);

        public string PetName;

        public override void ApplyToComponent<T>(T component)
        {
            SLEx_SpawnPet comp = component as SLEx_SpawnPet;
            comp.PetName = PetName;
        }

        public override void SerializeEffect<T>(T effect)
        {

        }
    }

    public class SLEx_SpawnPet : Effect, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_SpawnPet);
        public Type GameModel => typeof(SLEx_SpawnPet);

        public string PetName;

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {
            if (!BeastMaster.PetManager.CharacterHasPet(_affectedCharacter))
            {
                PetType petType = BeastMaster.PetManager.GetPetDefinitionByName(PetName);

                if (petType != null)
                {
                    PetController petController = BeastMaster.PetManager.CreatePetFor(_affectedCharacter, petType, OutwardHelpers.GetPositionAroundCharacter(_affectedCharacter), Vector3.zero);
                    petController.SetOwner(_affectedCharacter);
                }
                else
                {
                    BeastMaster.Log.LogMessage($"SLEx_SpawnPet Could not find species with Species Name : {PetName}, in the list of defintions.");
                }
            }
            else
            {
                BeastMaster.Log.LogMessage($"SLEx_SpawnPet {_affectedCharacter.Name} already has an active pet.");
            }
        }
    }
}