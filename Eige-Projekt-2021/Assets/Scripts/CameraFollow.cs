using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float distanceAway = 5f;
    [SerializeField] private float distanceUp = 2f;
    [SerializeField] private float smooth = 100f;
    [SerializeField] private Transform followedObject = null;
    
    Vector3 toPosition = Vector3.zero;

    private void LateUpdate()
    {
        toPosition = followedObject.position + Vector3.up * distanceUp - followedObject.forward * distanceAway;
        transform.position = Vector3.Lerp(transform.position, toPosition, smooth * Time.deltaTime);
        transform.LookAt(followedObject);
    }
}
