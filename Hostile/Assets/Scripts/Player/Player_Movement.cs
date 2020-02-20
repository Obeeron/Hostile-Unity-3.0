using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Joueur
{
    public class Player_Movement : MonoBehaviour
    {
        public UnityEvent onJump;

        private float gravity = 3.0f;
        private float groundLerp = 0.8f;
        private float airLerp = 0.95f;
        
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
                    animator.SetFloat("Speed", moveDirection.x);
                    animator.SetFloat("TurnSpeed", moveDirection.y);

                    animator.SetFloat("JumpLeg", moveDirection.x);
                    animator.SetFloat("Jump", moveDirection.y);
                }
                if (controls.InGame.Jump.triggered){
                    if (Data.Stamina > 0f)
                    {
                        StartCoroutine(Jump());
                    }
                }
                else if (fallingVelocity.y < 0f)
                {
                    fallingVelocity.y = -gravity;
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
                fallingVelocity += (Physics.gravity * Time.fixedDeltaTime);
                movement = Vector3.Lerp(currentMovement * (Data.speed/1.8f)* Time.fixedDeltaTime, movement, airLerp);
            }
            else
            {
                movement = Vector3.Lerp(currentMovement * Data.speed* Time.fixedDeltaTime, movement, groundLerp);
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
            do
            {
                yield return null;
            } while (!character.isGrounded);
            if (isThereAnimator)
                animator.SetBool("OnGround", true);

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