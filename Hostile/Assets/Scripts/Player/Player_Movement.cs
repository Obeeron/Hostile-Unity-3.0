using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
    
namespace Joueur
{
    public class Player_Movement : Player_Stats
    {
        
        CharacterController character;
        
        
        //values of speed and jump
        [SerializeField] private float crouchSpeed = 3.0f;
        [SerializeField] private float walkingSpeed = 5.0f;     // les animations change la vitesse
        [SerializeField] private float runningSpeed = 9.0f;
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float gravity = 3.0f;

        //values of stamina
        private float staminaTimer = 0.0f;
        private float jumpStamina = 10.0f;
        private float runningStamina = 5.0f;
        private float regenStamina = 20.0f;

        //modified valued every frame
        [SerializeField] private bool walking = true;
        [SerializeField] private bool running = false;
        [SerializeField] private bool crouch = false;
        private PlayerControls controls;
        private Vector3 fallingVelocity;
        private float speed;
        private float groundLerp = 0.8f;
        private float airLerp = 0.95f;
        
        protected Animator animator;
        private Vector3 movement;
        protected Player_Stats playerStats;
        
        //called before Start
        private void Awake()
        {
            controls = new PlayerControls();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            controls.InGame.Enable();
            playerStats = this.gameObject.GetComponent<Player_Stats>();
            animator = this.gameObject.GetComponent<Animator>();
            character = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
            bool hasJumped = false;
            // change la vitesse en fonction de la touche
            if (controls.InGame.SpeedSwap.triggered)
            {
                SpeedChange();
            }
            else if (controls.InGame.Crouch.triggered)
            {
                Crouching();
            }

            // définition de la vitesse en fonction de la stamina et du choix du joueur
            if (playerStats.IsWorn || playerStats.IsHungry)
            {
                running = false;
                walking = true;
                speed = walkingSpeed;
            }
            else
            {
                if (playerStats.IsHungry)
                {
                    regenStamina = 10f;
                }
                else
                {
                    regenStamina = 20f;
                }
                if (walking)
                {
                    speed = walkingSpeed;
                }
                else{
                    if (crouch)
                    {
                        speed = crouchSpeed;
                    }
                    else 
                    {
                        speed = runningSpeed;
                    }
                }
            }
                

            Vector2 moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            Vector3 currentMovement = (moveDirection.y * transform.forward + moveDirection.x * transform.right).normalized;


            if (character.isGrounded)
            {
                /*if (fallingVelocity.y < -20f) //degats de chute not fully functionnal
                {
                    player.HasLife -= 5f;
                }*/
                movement = Vector3.Lerp(currentMovement * speed * Time.deltaTime, movement, groundLerp);
                animator.SetFloat("Speed", moveDirection.x);
                animator.SetFloat("TurnSpeed", moveDirection.y);

                animator.SetFloat("JumpLeg", moveDirection.x);
                animator.SetFloat("Jump", moveDirection.y);

                if (controls.InGame.Jump.triggered){
                    if (!playerStats.IsWorn)//checking stamina
                    {
                        StartCoroutine(Jump());
                        hasJumped = true;
                    }
                }
                else if (fallingVelocity.y < 0)
                {
                    fallingVelocity.y = -gravity;
                }

                //modifying stamina
                if (hasJumped)
                {
                    playerStats.HasStamina -= jumpStamina; //perte de stamina à cause du saut
                    staminaTimer = 0f;
                }
                if (running && moveDirection != Vector2.zero)
                {
                    playerStats.HasStamina -= runningStamina * Time.deltaTime; //perte de stamina en fonction du temps passé à courir
                    staminaTimer = 0f;
                }
                if (!hasJumped && (!running || moveDirection == Vector2.zero)) //regain de stamina si le joueur ne court pas ou est à l'arret
                {
                    if (staminaTimer >= 1.3f)
                    {
                        playerStats.HasStamina += regenStamina * Time.deltaTime;
                    }
                    else
                    {
                        staminaTimer += Time.deltaTime;
                    }
                }
                //end of modifying stamina
            }
            else
            {
                movement = Vector3.Lerp(currentMovement * speed * Time.deltaTime, movement, airLerp);
            }
        }

        private void FixedUpdate()
        {
            if (!character.isGrounded){fallingVelocity += (Physics.gravity * Time.deltaTime);} 

            character.Move(movement);
            character.Move(fallingVelocity * Time.fixedDeltaTime);
        }
        
        private IEnumerator Jump()
        {
            animator.SetBool("OnGround", false);
            character.slopeLimit = 90f;
            speed /= 1.8f;
            fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            do
            {
                yield return null;
            } while (!character.isGrounded);
            animator.SetBool("OnGround", true);
            character.slopeLimit = 45f;
        }

        private void SpeedChange()
        {
            if (!crouch)
            {
                if (walking)
                {
                    walking = false;
                    running = true;
                }
                else
                {
                    walking = true;
                    running = false;
                }
            }
        }

        private void Crouching()
        {
            if (crouch)
            {
                animator.SetBool("Crouch", false);
                walking = true;
                running = false;
                crouch = false;
            }
            else
            {
                walking = false;
                running = false;
                crouch = true;
                animator.SetBool("Crouch", true);
                animator.SetFloat("Forward", movement.x);
                animator.SetFloat("Turn", movement.y);
            }
        }

        //enables controls input
        // private void OnEnable() => controls.InGame.Enable(); 
        private void OnDisable() => controls.InGame.Disable();
            //controls.Menu.disable might be used here
    }
}
