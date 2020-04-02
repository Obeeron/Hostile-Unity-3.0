using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Traveling : MonoBehaviour
{
    #region Variables
    [Header("Traveling Properties")]
    [Min(0.1f)]
    public float transitionTime = 1;

    #endregion

    public void TravelingTo(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(Traveling(target));
    }

    IEnumerator Traveling(Transform target)
    {
        float t = 0.0f;
        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * (Time.timeScale/transitionTime);

            transform.position = Vector3.Lerp(startingPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startingRot, target.rotation, t);
            yield return new WaitForEndOfFrame();
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
