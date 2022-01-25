using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Rigidbody))] //Zwingt das Objekt die RequiredComponent zu besitzen
public class PlayerBehaviour : MonoBehaviour
{
    [System.Serializable]
    public class MoveSettings
    {
        public float runVelocity = 12f;
        public float rotateVelocity = 100f;
        public float jumpVelocity = 8f;
        public float distanceToGround = 1.3f;
        public LayerMask ground;
        
    }
    
    [System.Serializable]
    public class InputSettings
    {
        public string FORWARD_AXIS = "Vertical";
        public string SIDEWAYS_AXIS = "Horizontal";
        public string TURN_AXIS = "Mouse X";
        public string JUMP_AXIS = "Jump";
    }

    public Transform spawnpoint;

    [SerializeField] private MoveSettings moveSettings = null;
    [SerializeField] private InputSettings inputSettings = null;

    private Rigidbody playerRigidbody = null;
    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;
    private float forwardInput = 0f, sidewaysInput = 0f, turnInput = 0f, jumpInput = 0f;

    private void Awake()
    {
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0f;
        targetRotation = transform.rotation;
        playerRigidbody = gameObject.GetComponent<Rigidbody>(); //Funktioniert auch ohne gameObject Referenz
        
    }

    private void Update()
    {
        GetInput(); 
        Turn();
    }

    private void FixedUpdate()
    {
        Run();
        Jump();
    }

    private void GetInput() //Setzt die entsprechenden Variablen, kein Returntype
    {
        if (inputSettings.FORWARD_AXIS.Length != 0)
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        if (inputSettings.SIDEWAYS_AXIS.Length != 0)
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        if (inputSettings.JUMP_AXIS.Length != 0)
            jumpInput = Input.GetAxis(inputSettings.JUMP_AXIS);
        if (inputSettings.TURN_AXIS.Length != 0)
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
    }

    private void Turn()
    {
        if (Mathf.Abs(turnInput) > 0) //Mathf wegen float value
        {
            targetRotation *= Quaternion.AngleAxis(moveSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    private void Run()
    {
        velocity.x = sidewaysInput * moveSettings.runVelocity;
        velocity.y = playerRigidbody.velocity.y;
        velocity.z = forwardInput * moveSettings.runVelocity; //Kein Deltatime, wegen fixed 50Fps der Physics engine
        
        playerRigidbody.velocity = transform.TransformDirection(velocity); //Transformiert den Vektor in den richtigen Space
    }

    private void Jump()
    {
        if (jumpInput != 0 && Grounded())
        {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, moveSettings.jumpVelocity, playerRigidbody.velocity.z);
        }
    }
    
    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground);
    }

    void Spawn()
    {
        transform.position = spawnpoint.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeathZone"))
        {
            Spawn();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("DynamicPlatform"))
        {
            transform.SetParent(null);
            //transform.localScale = beforeScale;
        }
    }

    void onDeath() => Spawn();
    
    
}
