using UnityEngine;

namespace BeastMaster
{
    public class BasePetState : BaseState<PetController>
    {
        public override void OnEnter(PetController PetController)
        {

        }

        public override void OnExit(PetController PetController)
        {

        }

        public override void OnFixedUpdate(PetController PetController)
        {

        }

        public override void OnUpdate(PetController PetController)
        {
            UpdateAnimator(PetController);

            if (!PetController.NavMesh.isOnNavMesh)
            {
                return;
            }
        }


        public void UpdateAnimator(PetController PetController)
        {
            float forwardVel = Vector3.Dot(PetController.NavMesh.velocity.normalized, PetController.transform.forward);
            float sideVel = Vector3.Dot(PetController.NavMesh.velocity.normalized, PetController.transform.right);

            PetController.Animator.SetFloat("Move X", sideVel, 5f, 5f);
            PetController.Animator.SetFloat("Move Z", forwardVel, 5f, 5f);
        }
    }
}