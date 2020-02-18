using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementPlayer : MonoBehaviour
{
    private float Speed;
    private float gravity = 3.0f;
    private float staminaTimer = 0.0f;
    private float groundLerp = 0.8f;
    private float airLerp = 0.95f;
    
    CharacterController character;
#pragma warning disable 649
    [SerializeField] private PlayerData Data;
#pragma warning restore 649
    private PlayerControls controls;
    protected Animator animator;
    private Vector3 movement;
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
        bool hasJumped = false;
        // change l'état du joueur en fonction de la touche pressée
        if (controls.InGame.SpeedSwap.triggered)
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
        else if (controls.InGame.Crouch.triggered)
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

        if (Data.Hunger < Data.MaxHunger * 0.1f || Data.Stamina <= 0f) //if the player is hungry or worn, state switch to run state
        {
            if (Data.speedState == PlayerData.State.running)
                Data.speedState = PlayerData.State.walking;
        }

        Vector2 moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
        Vector3 currentMovement = (moveDirection.y * transform.forward + moveDirection.x * transform.right).normalized;
        
        if (character.isGrounded)
        {
            //modifying speed
            switch (Data.speedState)
            {
                case PlayerData.State.crouching:
                    Speed = 3.0f;
                    break;
                case PlayerData.State.running:
                    Speed = 9.0f;
                    break;
                default:
                    Speed = 5.0f;
                    break;
            }

            movement = Vector3.Lerp(currentMovement * Speed* Time.deltaTime, movement, groundLerp);

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
                    hasJumped = true;
                }
            }
            else if (fallingVelocity.y < 0f)
            {
                fallingVelocity.y = -gravity;
            }

            if (hasJumped)
            {
                if (Data.Stamina >= 10f)
                    Data.Stamina -= 10f;
                else
                    Data.Stamina =0f;
                staminaTimer = 0f;
            }
            if (Data.speedState == PlayerData.State.running && moveDirection != Vector2.zero)
            {
                if (Data.Stamina - 5.0f * Time.deltaTime >= 0f)
                    Data.Stamina -= 5.0f * Time.deltaTime;
                else
                    Data.Stamina = 0f;
                staminaTimer = 0f;
            }
            if (!hasJumped && (Data.speedState != PlayerData.State.running || moveDirection == Vector2.zero))
            {
                if (staminaTimer >= 1f)
                {
                    if (Data.Hunger <= 0f)
                    {
                        float newStamina = Data.Stamina + (7.5f * Time.deltaTime);
                        if (newStamina >= Data.MaxStamina)
                        {
                            Data.Stamina = Data.MaxStamina;
                        }
                        else
                        {
                            Data.Stamina = newStamina;
                        }
                    }
                    else 
                    {
                        float newStamina = Data.Stamina + (15f * Time.deltaTime);
                        if (newStamina >= Data.MaxStamina)
                        {
                            Data.Stamina = Data.MaxStamina;
                        }
                        else
                        {
                            Data.Stamina = newStamina;
                        }
                    }
                }
                else
                {
                    staminaTimer += Time.deltaTime;
                }
            }
        }
        else
        {
            movement = Vector3.Lerp(currentMovement * Speed * Time.deltaTime, movement, airLerp);
        }
    }

    private void FixedUpdate()
    {
        if (!character.isGrounded)
            fallingVelocity += (Physics.gravity * Time.fixedDeltaTime);

        character.Move(movement);
        character.Move(fallingVelocity * Time.fixedDeltaTime);
    }

    private IEnumerator Jump()
    {
        if (isThereAnimator)
            animator.SetBool("OnGround", false);

        character.slopeLimit = 90f;
        Speed /= 1.8f;
        fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * 2.0f);
        do
        {
            yield return null;
        } while (!character.isGrounded);
        if (isThereAnimator)
            animator.SetBool("OnGround", true);

        character.slopeLimit = 45f;
    }

    private void OnDisable() => controls.InGame.Disable();
}
