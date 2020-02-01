using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
    
    
public class Player_Movement : MonoBehaviour
{
    
    public CharacterController character;
    
    
    //value of speed and jump  ###  private after test in unity
    public float crouchSpeed = 4.0f;
    public float walkingSpeed = 6.0f;
    public float runningSpeed = 9.0f;
    public float jumpHeight = 2.2f;
    public float gravity = 10.0f;

    //modified valued every frame
    private PlayerControls controls;
    private Vector3 fallingVelocity;
    private float speed;
    public float groundLerp;
    public float airLerp;
    [SerializeField] private bool walking = true;
    [SerializeField] private bool crouch = false;
    
    public Animator animator;
    private Vector3 movement;
    
    //called before Start
    private void Awake()
    {
        controls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        groundLerp = 0.8f;
        airLerp = 0.95f;
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        if (controls.InGame.SpeedSwap.triggered)
        {
            SpeedChange();
        }
        else if (controls.InGame.Crouch.triggered)
        {
            Crouching();
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

        Vector2 moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
        Vector3 currentMovement = (moveDirection.y * transform.forward + moveDirection.x * transform.right).normalized;


        if (character.isGrounded)
        {
            movement = Vector3.Lerp(currentMovement * speed * Time.deltaTime, movement, groundLerp);
            animator.SetFloat("Speed", moveDirection.x);
            animator.SetFloat("TurnSpeed", moveDirection.y);

            if(controls.InGame.Jump.triggered){
                StartCoroutine(Jump());
            }
            else if (fallingVelocity.y < 0)
            {
                fallingVelocity.y = -gravity;
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
        speed /= 2;
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
            }
            else
            {
                walking = true;
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
            crouch = true;
        }
    }

    //enables controls input
    private void OnEnable() => controls.InGame.Enable(); 
    private void OnDisable() => controls.InGame.Disable();
        //controls.Menu.disable might be used here
}
