using UnityEngine;
using SideLoader;


namespace BeastMaster
{
    public class SummonPet : SpawnSLCharacter
    {
        internal new void Awake()
        {
            base.Awake();
            this.GenerateRandomUIDForSpawn = true;
            this.TryFollowCaster = true;
            this.SpawnOffset = new Vector3(1, 0.25f, 1);
        }

        public override void ActivateLocally(Character _affectedCharacter, object[] _infos)
        {

            // custom health & mana cost for casting
            _affectedCharacter.Stats.UseBurntHealth = false;
            float healthcost = 0.5f * _affectedCharacter.Stats.MaxHealth;
            _affectedCharacter.Stats.UseBurnStamina = false;
            float staminacost = 0.5f * _affectedCharacter.Stats.MaxStamina;
            _affectedCharacter.Stats.ReceiveDamage(healthcost);
            _affectedCharacter.Stats.UseBurntHealth = true;
            _affectedCharacter.Stats.UseStamina(staminacost, 0f);
            _affectedCharacter.Stats.UseBurnStamina = true;

            // only host should do this
            if (!PhotonNetwork.isNonMasterClientInRoom)
            {
                var template = PetManager.Rhino;
                this.SLCharacter_UID = template.UID;

                SL.Log(this.SLCharacter_UID); ;

                CustomCharacters.Templates.TryGetValue(this.SLCharacter_UID, out charTemplate);

                this.ExtraRpcData = _affectedCharacter.UID.ToString();

                base.ActivateLocally(_affectedCharacter, _infos);
            }
        }
    }
}