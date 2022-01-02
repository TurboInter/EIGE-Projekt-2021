using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour
{
    [SerializeField] private float accelerationForce = 1F;
    [SerializeField] private float maxSpeed = 1F;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        var inX = Input.GetAxis("Horizontal");
        var inZ = Input.GetAxis("Vertical");

        var dir = new Vector3(inX, 0, inZ);
        var compensatedSpeed = Time.fixedDeltaTime * accelerationForce;
        dir = dir.normalized * compensatedSpeed;

        if (_rigidbody.velocity.magnitude < maxSpeed)
        {
            _rigidbody.velocity += Vector3.ClampMagnitude(_rigidbody.velocity, accelerationForce);
        }
        
    }
}