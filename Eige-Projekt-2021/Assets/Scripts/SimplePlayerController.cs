using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SimplePlayerController : MonoBehaviour
{
    [FormerlySerializedAs("accelerationForce")] [SerializeField]
    private float acceleration = 1F;

    [SerializeField] private float jumpSpeed = 1F;
    [SerializeField] private float maxSpeed = 1F;
    [SerializeField] private Camera mainCamera;

    private Rigidbody _rigidbody;
    private CapsuleCollider _capsule;
    private Vector3 _rayCastDelta;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _capsule = GetComponent<CapsuleCollider>();
        _rayCastDelta = new Vector3(0, _capsule.height + _capsule.radius, 0);
    }

    void Update()
    {
        var selfPos = transform.position;
        var campos = new Vector3(0, selfPos.y + 7, selfPos.z - 10);
        campos = Vector3.Lerp(mainCamera.transform.position, campos, 0.01F);
        var camTransform = mainCamera.transform;
        camTransform.position = campos;
        var dir = new Vector3(0, selfPos.y, selfPos.z) - campos;
        camTransform.forward = dir;
    }

    private void FixedUpdate()
    {
        var inX = Input.GetAxis("Horizontal");
        var inZ = Input.GetAxis("Vertical");

        var dir = new Vector3(inX, 0, inZ);
        var compensatedSpeed = Time.fixedDeltaTime * acceleration;
        dir = dir.normalized * compensatedSpeed;

        _rigidbody.velocity += dir;
        var clamped = Vector3.ClampMagnitude(_rigidbody.velocity, maxSpeed);
        _rigidbody.velocity = new Vector3(clamped.x, _rigidbody.velocity.y, clamped.z);

        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out var hit, 1.6F))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                var current = _rigidbody.velocity;
                var vel = new Vector3(current.x, jumpSpeed, current.z);
                _rigidbody.velocity = vel;
            }
        }
    }

}