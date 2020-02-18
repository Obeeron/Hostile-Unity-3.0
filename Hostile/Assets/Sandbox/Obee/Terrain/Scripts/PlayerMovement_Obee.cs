using System;
using System.Collections;
using UnityEngine;
public class PlayerMovement_Obee : MonoBehaviour
{
    private CharacterController controller;

    public float normalSpeed = 4.5f;
    public float runningSpeed = 7.5f;
    private float speed;
    private Vector3 fallingVelocity;

    bool grounded;
    public float jumpHeight = 1f;

    [Range(0f, 1f)]
    public float groundLerp = 0.7f;
    [Range(0f, 1f)]
    public float airLerp = 0.95f;
    [Range(0f, 1f)]
    public float airSpeedMultiplier = 0.6f;
    private Vector3 motion;

    Vector3 direction; 

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        grounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;           //Check if player is on the ground

        float x = Input.GetAxisRaw("Vertical");     //Get 2D movements inputs
        float y = Input.GetAxisRaw("Horizontal");
        
        direction = (x * transform.forward + y * transform.right).normalized;       //normalized direction to avoid faster diagonal movement

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
        float lerpValue = (grounded) ? groundLerp : airLerp;            //Lerp motion to smooth start and stop of player motion
        motion = Vector3.Lerp(direction * speed, motion, lerpValue);

        if (!grounded)
            fallingVelocity += (Physics.gravity * Time.fixedDeltaTime); //Update fallingVelocity 

        controller.Move(motion * Time.fixedDeltaTime);                  //Apply summed velocity
        controller.Move(fallingVelocity * Time.fixedDeltaTime);
    }
}
