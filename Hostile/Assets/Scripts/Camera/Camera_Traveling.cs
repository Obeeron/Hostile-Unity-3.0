using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Traveling : MonoBehaviour
{
    #region Variables
    [Header("Traveling Properties")]
    [Range(0f,1f)]
    public float transitionLerpSpeed = 0.1f;

    private float minimumDistance = 0.02f;
    #endregion

    public void TravelingTo(Transform target)
    {
        StopAllCoroutines();
        StartCoroutine(Traveling(target));
    }

    IEnumerator Traveling(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > minimumDistance)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, transitionLerpSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, transitionLerpSpeed);
            yield return new WaitForEndOfFrame();
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
