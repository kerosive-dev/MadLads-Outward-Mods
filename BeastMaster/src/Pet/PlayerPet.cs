using UnityEngine;

namespace BeastMaster
{
    public class PlayerPet : MonoBehaviour
    {
        public PetController ActivePet
        {
            get; private set;
        }

        public bool HasActivePet
        {
            get
            {
                return ActivePet != null;
            }
        }

        public Character Character => GetComponent<Character>();

        public void SetActivePet(PetController newMount)
        {
            ActivePet = newMount;
        }
    }
}
