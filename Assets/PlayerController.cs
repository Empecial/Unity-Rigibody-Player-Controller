using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Controls _Controls;


    public Rigidbody rb;
    public GameObject camHolder;
    public float speed, sensitivity,maxForce,jumpForce;
    private Vector2 move, look;
    private float lookRotation;

    public bool grounded;
    
    
    private void Awake()
    {
        _Controls = new Controls();

        _Controls.Player.Enable();

        _Controls.Player.Move.performed += OnMove;
        _Controls.Player.Look.performed += OnLook;
        _Controls.Player.Jump.performed += OnJump;

        Cursor.lockState = CursorLockMode.Locked;
    }

   

    public void OnMove(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext ctx)
    {
        look = ctx.ReadValue<Vector2>();
    }


    private void FixedUpdate()
    {
        Move();

    }

    void Jump()
    {
        Vector3 jumpForces = Vector3.zero;

        if (grounded)
        {
            jumpForces = Vector3.up * jumpForce;


        }

        rb.AddForce(jumpForces, ForceMode.VelocityChange);

    }

    public void SetGrounded(bool state)
    {
        grounded = state;

    }


    public void OnJump(InputAction.CallbackContext ctx)
    {
        Jump();

    }

    void Move()
    {
        //find target velocity

        Vector3 CurrentVelocity = rb.velocity;
        Vector3 TargetVelocity = new Vector3(move.x, 0, move.y);
        TargetVelocity *= speed;

        //always move in correct direction

        TargetVelocity = transform.TransformDirection(TargetVelocity);


        //calculate forces
        Vector3 velocityChange = (TargetVelocity - CurrentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0, velocityChange.z);


        //limit force
        Vector3.ClampMagnitude(velocityChange, maxForce);

        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Look();
    }
    
    void Look()
    {
        //Turning
        transform.Rotate(Vector3.up * look.x * sensitivity);

        //Looking
        lookRotation += (-look.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.eulerAngles = new Vector3(lookRotation, camHolder.transform.eulerAngles.y, camHolder.transform.eulerAngles.z);

    }


    private void OnEnable()
    {
        _Controls.Enable();
    }

    private void OnDisable()
    {
        _Controls.Disable();
    }
}
