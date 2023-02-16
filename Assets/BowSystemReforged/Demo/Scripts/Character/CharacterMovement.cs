using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.Mathematics;
using UnityEngine;

namespace WeaponizationSystemReforged.Demo.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        private CharacterController controller;
        private CharacterAnimatiorControl charAnimCtrl;
        private IAimWeaponIK aimWeaponIK;

        [Header("Movement Settings:")]
        [SerializeField] private float walkingSpeed = 2.7f;
        [SerializeField] private float directionalWalkingSpeed = 1.5f;
        [SerializeField] private float runningSpeed = 6;
        [SerializeField] private float jumpHeight = 1.0f;
        [SerializeField] private float gravityValue = -9.81f;
        [SerializeField] private float rotationSmoothness = 10;

        [Header("Input Names")]
        [SerializeField] private string inpRun = "Debug Multiplier";
        [SerializeField] private string impJump = "Jump";
        [SerializeField] private string impHorizontal = "Horizontal";
        [SerializeField] private string impVertical = "Vertical";
        [SerializeField] private string impFire1 = "Fire1";
        [SerializeField] private string impFire2 = "Fire2";
        [SerializeField] private string inpChangeCharMode = "Enable Debug Button 1";

        // State Machine
        [SerializeField] 
        private CharacterMovementMode characterMovementMode = CharacterMovementMode.Default;
        public enum CharacterMovementMode { 
            Default,BowSystem
        }

        // Specific character speed variables for animation and movement
        private float currentSpeed = 0;
        private float playerTrueSpeed = 0;
        private float playerSpeed = 0;

        // Character physics variables
        private Vector3 playerVelocity;
        private bool groundedPlayer;
        private quaternion characterRotation;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            charAnimCtrl = GetComponent<CharacterAnimatiorControl>();

            if (GetComponent<IAimWeaponIK>() == null) {
                gameObject.AddComponent<AimWeaponIK>();
            }
            aimWeaponIK = GetComponent<IAimWeaponIK>();
        }

        void Update()
        {
            // Inputs
            bool preparingShot = Input.GetButton(impFire2) && charAnimCtrl.IsBowKeeped;
            bool isRunning = Input.GetButton(inpRun) && !preparingShot;
            bool isChangeCharMode = Input.GetButtonDown(inpChangeCharMode);

            // Get ground collision and height control
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }

            // Get axis directional movement
            float axisHorizontal = Input.GetAxis(impHorizontal);
            float axisVertical = Input.GetAxis(impVertical);
            Vector3 move = new Vector3(axisHorizontal, 0, axisVertical);
            
            // Character state machine
            CharacterStateMachine(move, isChangeCharMode, isRunning, preparingShot, groundedPlayer);

            // Set animation parameters
            charAnimCtrl.InVertical = axisVertical;
            charAnimCtrl.InHorizontal = axisHorizontal;
            charAnimCtrl.IsRunning = isRunning;

            // Character jump control
            if (Input.GetButtonDown(impJump) && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            // Character gravity control
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }

        private void CharacterStateMachine(Vector3 move,bool isChangeCharMode, bool isRunning,bool preparingShot,bool groundedPlayer)
        {
            switch (characterMovementMode)
            {
                case CharacterMovementMode.Default:
                    charAnimCtrl.IsBowKeeped = false;
                    charAnimCtrl.IsPreparingShot = false;
                    aimWeaponIK.WeightAim = 0;

                    playerTrueSpeed = WalkSetup(move, isRunning, playerTrueSpeed, groundedPlayer);
                    
                    StateMachineChanger(isChangeCharMode, CharacterMovementMode.BowSystem);

                    break;
                case CharacterMovementMode.BowSystem:
                    charAnimCtrl.IsBowKeeped = true;
                    
                    charAnimCtrl.IsPreparingShot = preparingShot;
                    aimWeaponIK.WeightAim = preparingShot ? charAnimCtrl.AimWeightCourve : 0;

                    if (Input.GetButtonDown(impFire1) && charAnimCtrl.IsPreparingShot) {
                        charAnimCtrl.TriggerNowShootArrow();
                    }

                    if (isRunning && !preparingShot || !isRunning && !preparingShot)
                    {
                        playerTrueSpeed = WalkSetup(move, isRunning, playerTrueSpeed, groundedPlayer);
                        
                    } else {
                        playerTrueSpeed = PreparingShotSetup(move, preparingShot, playerTrueSpeed,groundedPlayer);
                        
                    }
                    StateMachineChanger(isChangeCharMode, CharacterMovementMode.Default);
                    break;
                default:
                    break;
            }
            // Set the player speed to the animatior component.
            charAnimCtrl.InSpeed = playerTrueSpeed;
        }

        private float WalkSetup(Vector3 move,bool isRunning,float playerTrueSpeed,bool groundedPlayer) {
            // Character movement
            if (move != Vector3.zero && groundedPlayer)
            {
                Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.TransformDirection(move), Vector3.up);
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                characterRotation = Quaternion.Slerp(characterRotation, rotation, Time.deltaTime * rotationSmoothness);
                transform.rotation = characterRotation;
            }
            if (groundedPlayer) {
                // Character speed calculation for running and walking in Default State.
                currentSpeed = isRunning ? runningSpeed : walkingSpeed;
                playerSpeed = Mathf.Lerp(playerSpeed, currentSpeed, Time.deltaTime * 10);

                playerTrueSpeed = new Vector2(move.x, move.z).magnitude * playerSpeed;
                if (playerTrueSpeed < 0) playerTrueSpeed *= -1;
            }
            // Apply character movement.
            controller.Move(transform.forward * Time.deltaTime * playerTrueSpeed);
            return playerTrueSpeed;
        }

        private float PreparingShotSetup(Vector3 move, bool preparingShot, float playerTrueSpeed,bool groundedPlayer) {
            // Character movement
            if ((move != Vector3.zero || preparingShot) && groundedPlayer)
            {
                Vector3 relativeDir = transform.position - Camera.main.transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativeDir, Vector3.up);
                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                characterRotation = Quaternion.SlerpUnclamped(characterRotation, rotation, Time.deltaTime * rotationSmoothness);
                transform.rotation = characterRotation;
            }
            if (groundedPlayer) {
                // Character speed calculation for Preparing Shot in Bow Mode State.
                currentSpeed = directionalWalkingSpeed;
                playerSpeed = Mathf.Lerp(playerSpeed, currentSpeed, Time.deltaTime * 10);

                playerTrueSpeed = new Vector2(move.x, move.z).magnitude * playerSpeed;
                if (playerTrueSpeed < 0) playerTrueSpeed *= -1;
            }
            //Apply character movement.
            controller.Move(transform.TransformDirection(move) * Time.deltaTime * playerTrueSpeed);
            return playerTrueSpeed;
        }

        private void StateMachineChanger(bool isChangeCharMode, CharacterMovementMode newState) {
            if (isChangeCharMode)
            {
                characterMovementMode = newState;
            }
        }
    }
}
