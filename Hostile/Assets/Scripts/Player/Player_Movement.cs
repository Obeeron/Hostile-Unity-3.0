using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
    
    
public class Player_Movement : MonoBehaviour
{
    
    CharacterController character;
    
    //private after test in unity
    public float walkingSpeed = 6.0f;
    public float runningSpeed = 9.0f;
    public float jumpSpeed = 9.0f;
    public float jumpHeight = 1.5f;
    public float gravity = 20.0f;

    private PlayerControls controls;
    private float speed;
    private bool walking = true;
    
    private Vector3 movement = Vector3.zero;
    
    //called before Start
    private void Awake()
    {
        controls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // movement
        if (controls.InGame.SpeedSwap.triggered)
        {
            SpeedChange();
        }
        if (walking){speed = walkingSpeed;}else{speed = runningSpeed;}

        if (character.isGrounded)
        {
            var moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            movement = new Vector3{
                x = moveDirection.x,
                z = moveDirection.y
            }.normalized;
            movement *= speed;

            // if(controls.InGame.Jump.triggered){
            //    Jump();
            // }
        }
        else
        {
            /*var moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            movement = new Vector3(){
                x = moveDirection.x,
                z = moveDirection.y
            }.normalized;

            movement.x *= walkingSpeed*0.6f;
            movement.z *= walkingSpeed*0.6f;*/
        }
        
        movement.y -= gravity * Time.deltaTime;
        character.Move(movement * Time.deltaTime);
    }
    
    public void SpeedChange() => walking = !walking;

    public void Jump(){
        if(character.isGrounded){
         movement.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        }
    }
    //enables controls input
    private void OnEnable() => controls.InGame.Enable(); 
    private void OnDisable() => controls.InGame.Disable();
        //controls.Menu.disable might be used here
}
