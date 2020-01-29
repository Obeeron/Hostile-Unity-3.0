using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
    
    
public class Player_Movement : MonoBehaviour
{
    
    CharacterController character;
    
    public float walkingSpeed = 6.0f;
    public float runningSpeed = 9.0f;
    public float jumpSpeed = 30.0f;
    public float gravity = 20.0f;

    private PlayerControls controls;
    private float speed;
    public bool walking = true;
    
    public bool wasd = false;
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
        if (walking){speed = walkingSpeed;}else{speed = runningSpeed;}

        if (character.isGrounded)
        {
            var moveDirection = controls.InGame.Movement.ReadValue<Vector2>();
            movement = new Vector3{
                x = moveDirection.x,
                z = moveDirection.y
            }.normalized;
            movement *= speed;

            if(controls.InGame.Jump.triggered){
                movement.y = jumpSpeed;
            }
        }

        movement.y -= gravity * Time.deltaTime;
        character.Move(movement * Time.deltaTime);
    }
    
    public void speedChange() => walking = !walking;
    private void OnEnable(){
        controls.InGame.Enable();
    } 
    private void OnDisable() => controls.InGame.Disable();
        //controls.Menu.disable might be used here
}
