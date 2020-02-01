using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
    

namespace Joueur
{
    public class Player_Movement : MonoBehaviour
    {
        
        CharacterController character;
        
        
        //values of speed and jump
        [SerializeField] private float crouchSpeed = 0.8f;
        [SerializeField] private float walkingSpeed = 6.0f;
        [SerializeField] private float runningSpeed = 15.0f;
        [SerializeField] private float noStaminaSpeed = 3f;
        [SerializeField] private float jumpHeight = 2.2f;
        [SerializeField] private float gravity = 10.0f;

        //values of stmaina
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
        private float groundLerp;
        private float airLerp;
        
        public Animator animator;
        private Vector3 movement;
        public Player_Stats player;
        
        //called before Start
        private void Awake()
        {
            controls = new PlayerControls();
        }

        // Start is called before the first frame update
        void Start()
        {
            character = GetComponent<CharacterController>();
            groundLerp = 0.8f;
            airLerp = 0.95f;
            //initialise les statistiques du joueur. Sera dans un autre fichiers lorsque le choix de points de compétences sera mis en place.
            player.Choosing(0,0,0,0);
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
            if (player.IsWorn || player.IsHungry)
            {
                speed = noStaminaSpeed;
            }
            else
            {
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
                movement = Vector3.Lerp(currentMovement * speed * Time.deltaTime, movement, groundLerp);
                animator.SetFloat("Speed", moveDirection.x);
                animator.SetFloat("TurnSpeed", moveDirection.y);

                if(controls.InGame.Jump.triggered){
                    StartCoroutine(Jump());
                    hasJumped = true;
                }
                else if (fallingVelocity.y < 0)
                {
                    fallingVelocity.y = -gravity;
                }

                //modifying stamina
                if (hasJumped)
                {
                    player.HasStamina -= jumpStamina;
                    staminaTimer = 0f;
                }
                if (running && moveDirection != Vector2.zero)
                {
                    player.HasStamina -= runningStamina * Time.deltaTime;
                    staminaTimer = 0f;
                }
                if (!hasJumped && (!running || moveDirection == Vector2.zero))
                {
                    if (staminaTimer >= 2f)
                    {
                        player.HasStamina += regenStamina * Time.deltaTime;
                    }
                    else
                    {
                        staminaTimer += Time.deltaTime;
                    }
                    
                }
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
            character.Move(fallingVelocity * Time.deltaTime);
        }
        
        private IEnumerator Jump()
        {
            character.slopeLimit = 90f;
            speed /= 1.8f;
            fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            do
            {
                yield return null;
            } while (!character.isGrounded);
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

        // une seule vitesse en étant accroupi? ou deux?
        private void Crouching()
        {
            if (crouch)
            {
                walking = true;
                crouch = false;
            }
            else
            {
                walking = false;
                running = false;
                crouch = true;
            }
        }

        //enables controls input
        private void OnEnable() => controls.InGame.Enable(); 
        private void OnDisable() => controls.InGame.Disable();
            //controls.Menu.disable might be used here
    }
}
