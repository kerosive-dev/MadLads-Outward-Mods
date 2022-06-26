namespace BeastMaster
{
    public class PetCompanionInteractions : InteractionBase
    {
        private PetController PetController => GetComponentInParent<PetController>();
        public override string DefaultHoldText => PetController != null ? $"Pet {PetController.PetName}" : "Pet Companion";

        public override void Activate(Character _character)
        {
            PlayerPet playerPet = _character.GetComponent<PlayerPet>();

            PetController.PlayPetAnimation(PetAnimations.PetVictory);
           
        }
    }
}
