using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Photon.Pun;

namespace Joueur
{
    public class Player_Movement : MonoBehaviour
    {
        public UnityEvent onJump;

        private float groundLerp = 0.8f;
        private float airLerp = 0.96f;
        
        CharacterController character;

#pragma warning disable 649
        [SerializeField] private PlayerData Data;
#pragma warning restore 649
        private PlayerControls controls;
        protected Animator animator;
        private Vector3 movement;
        private Vector3 currentMovement;
        private Vector3 fallingVelocity;
        private bool isThereAnimator = true;
        public Animator animatorArms;

        void OnEnable()
        {
            controls = new PlayerControls();
            controls.InGame.Enable();
            animator = this.gameObject.GetComponent<Animator>();
            if (animator == null)
                isThereAnimator = false;
            character = GetComponent<CharacterController>();
        }
        
        // Update is called once per frame
        void Update()
        {
            //this.gameObject.transform.rotation = new Quaternion(camera.transform.localRotation.x, camera.transform.localRotation.y, 0,0);
            // change l'état du joueur en fonction de la touche pressée
            if (controls.InGame.SpeedSwap.triggered)
            {
                stateSwap();
            }
            else if (controls.InGame.Crouch.triggered)
            {
                crouchSwap();
            }

            if (Data.Hunger < Data.MaxHunger * 0.1f || Data.Stamina <= 0f) //if the player is hungry or worn, state switch to run state
            {
                if (Data.speedState == PlayerData.State.running)
                    Data.speedState = PlayerData.State.walking;
            }

            Vector2 moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            currentMovement = (moveDirection.y * transform.forward + moveDirection.x * transform.right).normalized;
            
            if (character.isGrounded)
            {
                //for animation
                if (isThereAnimator)
                {
                    if(Data.speedState == PlayerData.State.walking && moveDirection.y > 0)
                    {
                        animator.SetFloat("TurnSpeed", 0.5f);
                        animatorArms.SetFloat("TurnSpeed", 0.5f);
                    }
                    else
                    if (Data.speedState == PlayerData.State.running && moveDirection.y > 0)
                    {
                        animator.SetFloat("TurnSpeed", 1f);
                        animatorArms.SetFloat("TurnSpeed", 1f);
                    }
                    else
                    {
                        animator.SetFloat("TurnSpeed", moveDirection.y);
                        animatorArms.SetFloat("TurnSpeed", moveDirection.y);
                    }
                        

                    animator.SetFloat("Speed", moveDirection.x);
                    animatorArms.SetFloat("Speed", moveDirection.x);


                    animator.SetFloat("JumpLeg", moveDirection.x);
                    animator.SetFloat("Jump", moveDirection.y);
                }
                if (controls.InGame.Jump.triggered){
                    if (Data.Stamina > 0f)
                    {
//                        fallingVelocity.x = currentMovement.x * Data.speed;
//                        fallingVelocity.z = currentMovement.z * Data.speed;
                        StartCoroutine(Jump());
                    }
                }
                else if (Math.Abs(fallingVelocity.y - Physics.gravity.y) > 0.0001f)
                {
                    fallingVelocity.y = Physics.gravity.y;
                }

                if (moveDirection == Vector2.zero)
                    Data.isIdle = true;
                else
                    Data.isIdle = false;
            }
        }

        private void FixedUpdate()
        {
            if (!character.isGrounded)
            {
                float multiplier = 1f;
                if (fallingVelocity.y < 0f) 
                {
                    multiplier = 2.0f;//modify this multiplier to change the curve of the fall. closer to 0 mean curve symetrical else fall way faster.
                }
                fallingVelocity.y += (Time.fixedDeltaTime * multiplier * Physics.gravity.y);// the longer the player fall, the faster he falls.
                movement = Vector3.Lerp(Time.fixedDeltaTime * (Data.speed * 1.4f)* currentMovement, movement, airLerp);
            }
            else
            {
                movement = Vector3.Lerp(Time.fixedDeltaTime * Data.speed* currentMovement, movement, groundLerp);
            }

            character.Move(movement);
            character.Move(fallingVelocity * Time.fixedDeltaTime);
        
        }

        private IEnumerator Jump()
        {
            onJump?.Invoke();
            if (isThereAnimator)
                animator.SetBool("OnGround", false);

            character.slopeLimit = 90f;
            fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * 2.0f);
            Data.isOnJump = true;
            do
            {
                yield return null;
            } while (!character.isGrounded);
            if (isThereAnimator)
                animator.SetBool("OnGround", true);
            
            Data.isOnJump = false;
            character.slopeLimit = 45f;
        }

        private void stateSwap()
        {
            switch (Data.speedState)
            {
                case PlayerData.State.walking:
                    Data.speedState = PlayerData.State.running;
                    break;
                case PlayerData.State.running:
                    Data.speedState = PlayerData.State.walking;
                    break;
                default:
                    break;
            }
        }

        private void crouchSwap()
        {
            if (Data.speedState == PlayerData.State.crouching)
            {
                Data.speedState = PlayerData.State.walking;
                if (isThereAnimator)
                        animator.SetBool("Crouch", false);
            }
            else
            {
                Data.speedState = PlayerData.State.crouching;
                if (isThereAnimator)
                {
                    animator.SetBool("Crouch", true);
                    animator.SetFloat("Forward", movement.x);
                    animator.SetFloat("Turn", movement.y);
                }
            }
        }

        private void OnDisable() => controls.InGame.Disable();
    }
}