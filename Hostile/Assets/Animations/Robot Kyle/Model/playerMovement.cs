using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float normalSpeed = 5f;
    public float runningSpeed = 9f;
    private float speed;
    private Vector3 fallingVelocity;

    bool grounded;

    public float jumpHeight = 1f;

    public float groundLerp;
    public float airLerp;
    private Vector3 oldMotion;
    private Vector3 motion;
    public Animator animator;

    private void Start()
    {
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is on the ground
        //grounded = Physics.CheckSphere(groundCheck.position, groundOffset, groundMask);
        grounded = controller.isGrounded;

        //Get 2D movements inputs
        float x = Input.GetAxisRaw("Vertical");
        float y = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Turn", y);
        animator.SetFloat("Forward", x);

        //normalized direction to avoid faster diagonal movement
        Vector3 direction = (x * transform.forward + y * transform.right).normalized;
        oldMotion = motion;
        motion = direction * speed * Time.deltaTime;

        //Lerp motion to smooth start and stop of player motion
        float lerpValue = (grounded) ? groundLerp : airLerp;
        motion = Vector3.Lerp(motion, oldMotion, lerpValue);

        if (grounded)
        {
            //Set waking speed
            speed = (Input.GetKey(KeyCode.LeftShift)) ? runningSpeed : normalSpeed;

            //Get Jump Input
            if (Input.GetButtonDown("Jump"))
                StartCoroutine(Jump());
            //Resets fallingVelocity if player fell on the ground and isn't jumping
            else if (grounded && fallingVelocity.y < 0f)
                fallingVelocity.y = -4f;
        }
    }

    private IEnumerator Jump()
    {
        //preventing wall jitter
        controller.slopeLimit = 90f;
        speed *= 0.6f;
        fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);

        do { 
            yield return null;
        } while (!grounded);

        controller.slopeLimit = 45f;
    }

    private void FixedUpdate()
    {
        
        if (!grounded)
            //Update fallingVelocity 
            fallingVelocity += (Physics.gravity * Time.deltaTime);

        //Apply summed velocity
        controller.Move(motion);
        controller.Move(fallingVelocity * Time.deltaTime);
    }
}