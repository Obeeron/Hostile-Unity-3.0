using System;
using System.Collections;
using UnityEngine;
public class PlayerMovement_Obee : MonoBehaviour
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
    public float airSpeedMultiplier;
    private Vector3 motion;

    private void Start()
    {
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;           //Check if player is on the ground

        float x = Input.GetAxisRaw("Vertical");     //Get 2D movements inputs
        float y = Input.GetAxisRaw("Horizontal");
        
        Vector3 direction = (x * transform.forward + y * transform.right).normalized;       //normalized direction to avoid faster diagonal movement

        float lerpValue = (grounded) ? groundLerp : airLerp;                                //Lerp motion to smooth start and stop of player motion
        motion = Vector3.Lerp(direction * speed * Time.deltaTime, motion, lerpValue);

        if (grounded)
        {
            speed = (Input.GetKey(KeyCode.LeftShift)) ? runningSpeed : normalSpeed;  //Set waking speed

            if (Input.GetButtonDown("Jump"))        //Get Jump Input
                StartCoroutine(Jump());
            else if (fallingVelocity.y < 0f)        //Resets fallingVelocity if player fell on the ground and isn't jumping
                fallingVelocity.y = -2f;
        }
    }

    private IEnumerator Jump()
    {
        controller.slopeLimit = 90f;            //preventing wall jitter
        speed *= airSpeedMultiplier;            //reducing midair speed
        fallingVelocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);

        do
        {
            yield return null;
        } while (!grounded);

        controller.slopeLimit = 45f;
    }

    private void FixedUpdate()
    {

        if (!grounded)
            fallingVelocity += (Physics.gravity * Time.deltaTime);  //Update fallingVelocity 

        controller.Move(motion);                                    //Apply summed velocity
        controller.Move(fallingVelocity * Time.deltaTime);
    }
}
