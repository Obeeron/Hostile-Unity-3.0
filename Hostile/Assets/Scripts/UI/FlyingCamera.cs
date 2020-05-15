using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlyingCamera : MonoBehaviour
{
    public LightingManager dayLightCycle;
    public Terrain terrain;
    public Transform targetPoint;
    public AnimationCurve curve;

    private Light light;
    System.Random rdm;

    [Header("Speeds")]
    public float speed = 1f;
    public float rotationSpeed = 10f;

    [Header("Other")]
    public float minDistance = 10f;
    public float maxDistance = 20f;
    public float minHeight = 10f;
    public float maxHeight = 60f;
    public float minAngle = 20f;
    public float maxAngle = 90f;

    float rdmDistance;
    float rdmAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponentInChildren<Light>();
        rdm = new System.Random();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, targetPoint.position)<=6f)
        {
            rdmDistance = randomRange(minDistance, maxDistance);
            rdmAngle = randomRange(minAngle, maxAngle)*((rdm.Next()%2==0)?1:-1);

            targetPoint.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y,0f);
            targetPoint.Rotate(Vector3.up*rdmAngle);
            targetPoint.Translate(Vector3.forward*rdmDistance);
            
            //HEIGHT
            float terrainHeight = terrain.terrainData.GetHeight((int)targetPoint.position.x,(int)targetPoint.position.z);
            targetPoint.position = new Vector3(targetPoint.position.x, 
                                                terrainHeight + minHeight + curve.Evaluate((float)rdm.NextDouble())*(maxHeight-minHeight),
                                                targetPoint.position.z);

            if(targetPoint.position.x >= 900 || targetPoint.position.x <= 100 || targetPoint.position.z >= 900 || targetPoint.position.z <= 100)
                targetPoint.position = new Vector3(500,100,500);
        }
        light.intensity = (dayLightCycle.IsDay)? 0 : 1.5f;
        Vector3 targetDir = targetPoint.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.fixedDeltaTime * rotationSpeed);
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
    }

    float randomRange(float min, float max)
    {
        return min + (float)rdm.NextDouble()*(max-min);
    }
}
