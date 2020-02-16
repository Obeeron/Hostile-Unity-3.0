
using UnityEngine;

public class basicmove : MonoBehaviour
{
    public Rigidbody player;
    public int forwardForce = 4000;
    public float sideForce;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("q"))
            player.AddForce(-transform.right * sideForce * Time.deltaTime, ForceMode.VelocityChange);
        if (Input.GetKey("d"))
            player.AddForce(transform.right * sideForce * Time.deltaTime, ForceMode.VelocityChange);
        if (Input.GetKey("z"))
            player.AddForce(transform.forward * forwardForce * Time.deltaTime, ForceMode.VelocityChange);
        if (Input.GetKey("s"))
            player.AddForce(-transform.forward * forwardForce * Time.deltaTime, ForceMode.VelocityChange);


    }
}

