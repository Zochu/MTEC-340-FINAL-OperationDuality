using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float groundCheckDistance;
    public float moveSpeedMultiply;
    [SerializeField] float moveSpeed;
    [SerializeField] float moveSpeedOrigin;
    [SerializeField] float sprintMultiply;
    [SerializeField] float playerHeight;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    public bool isGrounded;
    [SerializeField] Transform playerPosition;
    [SerializeField] LayerMask groundLayer;

    LineRenderer _lineRenderer;

    CharacterController controller;
    ControllerColliderHit colliderhit;

    public float wallRunSpeed;
    public bool doubleJumpReady;// { get; private set; }
    public bool jumpReady { get; private set; }
    public bool limitSpeed = true;

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }

    public bool isCollidingWall { get; private set; } = false;

    public Vector3 moveDirection;

    public Rigidbody rb;

    [Space(20)]
    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event footStepStartEvent;
    [SerializeField] public AK.Wwise.Event footStepStopEvent;
    public bool isMovingAk;
    [SerializeField] AK.Wwise.Event jumpEvent;
    [SerializeField] AK.Wwise.Event doubleJumpEvent;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        moveSpeed = moveSpeedOrigin;
        isMovingAk = false;
        //controller = GetComponent<CharacterController>();
        //controller.radius = 0.4f;
        //Debug.Log(groundLayer.value);
        //_lineRenderer = GetComponent<LineRenderer>();
        //_lineRenderer.SetPosition(0, transform.position);
        //_lineRenderer.SetPosition(1, transform.forward);

    }

    private void Update()
    {
        //_lineRenderer.SetPosition(0, transform.position);
        //_lineRenderer.SetPosition(1, transform.forward);
        PlayerInput();
        //RaycastHit groundHit;
        //isGrounded = Physics.BoxCast(playerPosition.position, playerPosition.localScale*2, Vector3.down, out RaycastHit groundHit, playerPosition.rotation, groundCheckDistance, groundLayer);
        //Debug.Log(groundHit.collider.name);
        //isGrounded = Physics.Raycast(playerPosition.position, Vector3.down, out RaycastHit groundHit, groundCheckDistance, groundLayer);
        isGrounded = Physics.SphereCast(playerPosition.position, 0.5f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
        //Debug.Log(groundHit.transform.gameObject.name);
        //if(groundHit.collider != null) Debug.Log(groundHit.collider.name);

        Debug.DrawRay(playerPosition.position, Vector3.down, Color.red);

        if (isGrounded && limitSpeed)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxisRaw("Vertical") > 0 && isGrounded)
            moveSpeed = moveSpeedOrigin * sprintMultiply * moveSpeedMultiply;
        if ((Input.GetKeyUp(KeyCode.LeftShift) || Input.GetAxisRaw("Vertical") <= 0) && isGrounded)
            moveSpeed = moveSpeedOrigin * moveSpeedMultiply;
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJumpReady) && !WallRunning.isWallRunning)
            Jump();

        if (WallRunning.isWallRunning)
            moveSpeed = wallRunSpeed * moveSpeedMultiply;
        else
            moveSpeed = moveSpeedOrigin * moveSpeedMultiply;
    }

    private void FixedUpdate()
    {
        PlayerMove();
        SpeedLimit();
    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void PlayerMove()
    {
        moveDirection = playerPosition.forward * verticalInput + playerPosition.right * horizontalInput;
        if(isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * moveSpeedMultiply, ForceMode.Force);
        if (moveDirection != Vector3.zero && !isMovingAk && isGrounded)
        {
            isMovingAk = true;
            footStepStartEvent.Post(gameObject);
        }
        else if (moveDirection == Vector3.zero || !isGrounded)
        {
            isMovingAk = false;
            footStepStopEvent.Post(gameObject);
        }
        //else 
        //{
        //    if (controller.isGrounded)
        //    {
        //        rb.AddForce(colliderhit.normal, ForceMode.Force);
        //        // Respond differently depending on whether the
        //        // player is facing the contact point or not.
        //        //if (Vector3.Dot(movement, colliderhit.normal) < 0)
        //        //    movement = colliderhit.normal * moveSpeed;
        //        //else
        //        //    movement += colliderhit.normal * moveSpeed;
        //    }
        //}
    }

    private void SpeedLimit()
    {
        Vector3 vel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if((vel.magnitude > moveSpeed * moveSpeedMultiply) && limitSpeed)
        {
            Vector3 limitedVel = vel.normalized * moveSpeed * moveSpeedMultiply;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
        //Debug.Log(vel.magnitude);
        //Debug.Log(limitSpeed);
    }

    private void Jump()
    {
        if (!isGrounded && !doubleJumpReady) return;
        if (isGrounded)
        {
            isGrounded = false;
            doubleJumpReady = true;
            jumpEvent.Post(gameObject);
        }else if(doubleJumpReady)
        {
            doubleJumpReady = false;
            doubleJumpEvent.Post(gameObject);
        }

            
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        //doubleJumpReady = isGrounded; // Not Sure Why this is working??????
    }


    private void OnCollisionEnter(Collision collision)
    {

        //if (collision.gameObject.layer == 6)//groundLayer.value)
        //{
        //    //Debug.Log("ghei;p");
        //    doubleJumpReady = false;
        //    //isGrounded = true;
        //}

        if (!limitSpeed)
        {
            //Debug.Log("Wall");
            isCollidingWall = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (!isGrounded)
    //        rb.AddForce(collision.contact.normal, ForceMode.Force);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.layer == 6) doubleJumpReady = true;
    //    if(collision.gameObject.layer == 6)
    //    {
    //        isCollidingWall = false;
    //    }
    //}
}
