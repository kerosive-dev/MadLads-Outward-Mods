using SideLoader;
using System.Collections.Generic;
using UnityEngine;

namespace BeastMaster
{
    //The Base Unmounted state
    public class PetState : BasePetState
    {
        private Vector3 Target = Vector3.zero;
        private bool StayStill = false;
        private bool ActiveCombat = false;

        public override void OnEnter(PetController PetController)
        {
            PetController.EnableNavMeshAgent();

            if (BeastMaster.Debug)
            {
                PetController.DisplayNotification($"{PetController.PetName}, entered Leash State");
            }
        }

        public override void OnExit(PetController PetController)
        {
            Target = Vector3.zero;
            StayStill = false;
            if (BeastMaster.Debug)
            {
                PetController.DisplayNotification($"{PetController.PetName}, left Leash State");
            }
        }

        public override void OnFixedUpdate(PetController PetController)
        {

        }

        public override void OnUpdate(PetController PetController)
        {
            base.OnUpdate(PetController);
            CheckIsOwnerInCombat(PetController);

            if (ActiveCombat == false)
            {
                if (!StayStill)
                {
                    if (Target != Vector3.zero)
                    {
                        MoveToTargetPosition(PetController, Target);
                    }
                    else
                    {
                        if (PetController.DistanceToOwner > PetController.LeashDistance)
                        {
                            MoveToOwner(PetController);
                        }
                    }

                }
                else
                {
                    PetController.NavMesh.isStopped = true;
                    Target = Vector3.zero;
                }
            }
            else
            {
                List<Character> charList = new();
                CharacterManager.Instance.FindCharactersInRange(PetController.CharacterOwner.CenterPosition, 300f, ref charList);

                for (int i = 0; i < charList.Count; i++)
                {
                    bool IsEnemy = !charList[i].IsAlly(PetController.CharacterOwner);
                    bool IsAlive = !charList[i].IsDead;
                    if (IsEnemy && IsAlive)
                    {
                        if (ActiveCombat)
                        {
                            Target = charList[i].transform.position;
                            float distanceToTarget = Vector3.Distance(PetController.transform.position, charList[i].transform.position);

                            if (distanceToTarget <= PetController.LeashDistance)
                            {
                                MoveToTargetPosition(PetController, Target);
                            }
                            else
                            {
                                
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }


        private void MoveToOwner(PetController PetController)
        {
            StayStill = true;

            Vector3 LeashPosition = PetController.CharacterOwner.transform.position + (Vector3)Random.insideUnitCircle * PetController.LeashPointRadius;

            if (LeashPosition != Vector3.zero)
            {
                MoveToTargetPosition(PetController, LeashPosition);
            }
        }

        private void MoveToTargetPosition(PetController PetController, Vector3 MoveToTarget)
        {
            StayStill = false;

            if (MoveToTarget != Vector3.zero)
            {
                float distanceToTarget = Vector3.Distance(PetController.transform.position, MoveToTarget);

                if (distanceToTarget <= PetController.LeashDistance)
                {
                    PetController.NavMesh.isStopped = true;
                }
                else
                {
                    PetController.NavMesh.SetDestination(MoveToTarget);
                    PetController.NavMesh.isStopped = false;
                }
            }
        }

        private void CheckIsOwnerInCombat(PetController PetController)
        {
            if (PetController.CharacterOwner.InCombat)
            {
                ActiveCombat = true;
            }
            else
            {
                ActiveCombat = false;
            }
        }
    }
}