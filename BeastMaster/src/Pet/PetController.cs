using System;
using UnityEngine;
using UnityEngine.AI;

namespace BeastMaster
{
    public class PetController : MonoBehaviour
    {
        #region Properties
        public Character CharacterOwner
        {
            get; private set;
        }
        public Animator Animator
        {
            get; private set;
        }
        public CharacterController Controller
        {
            get; private set;
        }
        public NavMeshAgent NavMesh
        {
            get; private set;
        }
        public Item BagContainer
        {
            get; private set;
        }

        public PetType PetType
        {
            get; private set;
        }

        public string PetName
        {
            get; set;
        }

        public string SLPackName
        {
            get; set;
        }

        public string AssetBundleName
        {
            get; set;
        }

        public string PrefabName
        {
            get; set;
        }

        public string PetUID
        {
            get; set;
        }

        #endregion

        //PetType Movement Settings
        public float MoveSpeed { get; private set; }
        public float RotateSpeed { get; private set; }
        public float LeashDistance = 6f;
        public float CombatLeashDistance = 1.5f;
        //A Point is randomly chosen in LeashPointRadius around player to leash to.
        public float LeashPointRadius = 2.3f;
        public float TargetStopDistance = 1.4f;
        public float MoveToRayCastDistance = 20f;
        public LayerMask MoveToLayerMask => LayerMask.GetMask("LargeTerrainEnvironment", "WorldItems");

      
        public float DistanceToOwner => CharacterOwner != null ? Vector3.Distance(transform.position, CharacterOwner.transform.position) : 0f;

        public PetCompanionInteractions petCompanionInteraction { get; private set; }
        public InteractionActivator interactionActivator { get; private set; }
        public InteractionTriggerBase interactionTriggerBase { get; private set; }

        public StackBasedStateMachine<PetController> PetSM
        {
            get; private set;
        }

        public void Awake()
        {
            Animator = GetComponent<Animator>();
            Controller = GetComponent<CharacterController>();
            NavMesh = gameObject.AddComponent<NavMeshAgent>();
            SetupInteractionComponents();

            NavMesh.stoppingDistance = 1f;
            NavMesh.enabled = false;
            PetUID = Guid.NewGuid().ToString();

            SetupSM();
        }

        private void SetupSM()
        {
            PetSM = new StackBasedStateMachine<PetController>(this);
            PetSM.AddState("Base", new PetState());
            PetSM.PushState("Base");
        }



        private void SetupInteractionComponents()
        {
            BeastMaster.Log.LogMessage($"Creating Interaction Components...");
            petCompanionInteraction = gameObject.AddComponent<PetCompanionInteractions>();
            interactionActivator = gameObject.AddComponent<InteractionActivator>();
            interactionTriggerBase = gameObject.AddComponent<InteractionTriggerBase>();

            //interactionActivator.AddBasicInteractionOverride(petCompanionInteraction);
            interactionActivator.m_defaultHoldInteraction = petCompanionInteraction;
            interactionTriggerBase.DetectionColliderRadius = 1f;
        }

        #region Setters

        public void SetOwner(Character mountTarget)
        {
            CharacterOwner = mountTarget;
        }

   
        public void SetPetTypes(PetType petTypes)
        {
            PetType = petTypes;
            SetMoveSpeed(PetType.MoveSpeed);
            SetRotationSpeed(PetType.RotateSpeed);
            SetNavMeshMoveSpeed(PetType.MoveSpeed);
            SetNavMeshAcceleration(PetType.Acceleration);
        }
       

        public void SetNavMeshMoveSpeed(float newSpeed)
        {
            if (NavMesh != null)
            {
                NavMesh.speed = newSpeed;

            }
        }

        public void SetNavMeshAcceleration(float acceleration)
        {
            if (NavMesh != null)
            {
                NavMesh.acceleration = acceleration;
            }
        }

        public void SetMoveSpeed(float newSpeed)
        {
            MoveSpeed = newSpeed;
        }
        public void SetRotationSpeed(float newSpeed)
        {
            RotateSpeed = newSpeed;
        }
     
        public void SetCharacterCameraOffset(Character _affectedCharacter, Vector3 NewOffset)
        {
            _affectedCharacter.CharacterCamera.Offset = NewOffset;
        }
        #endregion

        public void Update()
        {
            //todo move to state machine, probably stack based
            if (PetSM != null)
            {
                PetSM.Update();
            }

            ///Bag takes way too long to instantiate and set up (multiple frames) resulting in IsKinematic being disabled by the RigidbodySuspender, so for now, force the settings.
            if (BagContainer)
            {
                BagContainer.transform.localPosition = new Vector3(-0.0291f, 0.11f, -0.13f);
                BagContainer.transform.localEulerAngles = new Vector3(2.3891f, 358.9489f, 285.6735f);

                if (BagContainer.m_rigidBody)
                {
                    BagContainer.m_rigidBody.isKinematic = true;
                    BagContainer.m_rigidBody.useGravity = false;
                    BagContainer.m_rigidBody.freezeRotation = true;
                }
            }
        }

        public void FixedUpdate()
        {
            if (PetSM != null)
            {
                //never likely to be used but you never know..
                PetSM.FixedUpdate();
            }
        }

        public void DisplayNotification(string text)
        {
            if (CharacterOwner != null)
            {
                CharacterOwner.CharacterUI.ShowInfoNotification(text);
            }
        }

        public void DisplayImportantNotification(string text)
        {
            if (CharacterOwner != null)
            {
                CharacterOwner.CharacterUI.NotificationPanel.ShowNotification(text);
            }
        }

        public void PlayTriggerAnimation(string name)
        {
            Animator.SetTrigger(name);
        }

        public void PlayPetAnimation(PetAnimations animation)
        {
            switch (animation)
            {
                case PetAnimations.PetVictory:
                    PlayTriggerAnimation("PetVictory");
                    break;
                case PetAnimations.PetAtk:
                    PlayTriggerAnimation("PetAtk");
                    break;
                case PetAnimations.PetGetHit:
                    PlayTriggerAnimation("PetGetHit");
                    break;
            }
        }

        public void EnableNavMeshAgent()
        {
            NavMesh.enabled = true;
        }

        public void DisableNavMeshAgent()
        {
            NavMesh.enabled = false;
        }

        public void Teleport(Vector3 Position, Vector3 Rotation)
        {
            BeastMaster.Log.LogMessage($"Teleporting {PetName} to {Position} {Rotation}");
            DisableNavMeshAgent();
            transform.position = Position;
            transform.rotation = Quaternion.Euler(Rotation);
            EnableNavMeshAgent();
        }

    }

    public enum PetAnimations
    {
        PetVictory,
        PetAtk,
        PetGetHit
    }
}