using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    public static bool isWallRunning;
    [SerializeField] Transform playerPosition;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float maxWallRunTime;    
    [SerializeField] float wallDistance;
    //[SerializeField] float minHeight;
    [SerializeField] float wallJumpTime;

    PlayerMovement playerMovement;
    PlayerCamera cam;

    bool wallJumping;
    bool wallLeft;
    bool wallRight;
    float wallJumpTimer;
    float wallRunTimer;
    Vector3 wallNormal;
    Vector3 wallForward;
    RaycastHit hitLeftWall;
    RaycastHit hitRightWall;
    Rigidbody rb;

    [Header("Wwise")]
    [SerializeField] AK.Wwise.Event footStepEvent;
    [SerializeField] AK.Wwise.Event footStepStopEvent;
    [SerializeField] AK.Wwise.Switch onWallSwitch;
    [SerializeField] AK.Wwise.Switch onGroundSwitch;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        cam = GameObject.Find("MainCamera").GetComponent<PlayerCamera>();
        wallRunTimer = maxWallRunTime;
        isWallRunning = false;
    }

    private void Update()
    {
        CheckWall();
        LetsRun();
        //Debug.Log(OKtoRun());
        //Debug.Log("Left: " + wallLeft);
        //Debug.Log("Right: " + wallRight);
        //Debug.Log("Ready to Run: " + OKtoRun());
    }

    private void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -playerPosition.right, out hitLeftWall, 1, wallLayer);
        //wallLeft = Physics.BoxCast(transform.position, playerPosition.localScale, Vector3.left, transform.rotation, 1, wallLayer);
        wallRight = Physics.Raycast(transform.position, playerPosition.right, out hitRightWall, 1, wallLayer);
        //Debug.Log("wLeft: " + wallLeft);
    }

    //private void CheckWall()
    //{
    //    Vector3 origin = transform.position;
    //    Vector3 direction = playerPosition.forward;
    //    float distance = wallDistance;

    //    // Cast raycasts to the left and right of the player position
    //    Vector3 leftOrigin = origin - playerPosition.right * 0.5f;
    //    Vector3 leftDirection = Quaternion.AngleAxis(-30f, transform.up) * direction;
    //    bool leftHit = Physics.Raycast(leftOrigin, leftDirection, out hitLeftWall, distance, wallLayer);

    //    Vector3 rightOrigin = origin + playerPosition.right * 0.5f;
    //    Vector3 rightDirection = Quaternion.AngleAxis(30f, transform.up) * direction;
    //    bool rightHit = Physics.Raycast(rightOrigin, rightDirection, out hitRightWall, distance, wallLayer);

    //    // Use the closest wall hit as the detected wall
    //    if (leftHit && rightHit)
    //    {
    //        if (hitLeftWall.distance < hitRightWall.distance)
    //        {
    //            wallLeft = true;
    //            wallRight = false;
    //        }
    //        else
    //        {
    //            wallLeft = false;
    //            wallRight = true;
    //        }
    //    }
    //    else if (leftHit)
    //    {
    //        wallLeft = true;
    //        wallRight = false;
    //    }
    //    else if (rightHit)
    //    {
    //        wallLeft = false;
    //        wallRight = true;
    //    }
    //    else
    //    {
    //        wallLeft = false;
    //        wallRight = false;
    //    }
    //}



    private void FixedUpdate()
    {
        if (isWallRunning)
            WallRunningMovement();
        else
            rb.useGravity = true;
    }

    private bool OKtoRun()
    {
        return !Physics.BoxCast(playerPosition.position, playerPosition.localScale, Vector3.down, playerPosition.rotation, 2.7f, groundLayer);
    }

    private void LetsRun()
    {
        if((wallLeft || wallRight) && Input.GetAxisRaw("Vertical") > 0 && OKtoRun() && !wallJumping && !playerMovement.isGrounded)
        {
            //Debug.Log("1");
            if (!isWallRunning) StartRunning();
            if (wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;
            if (wallRunTimer <= 0)
            {
                wallJumping = true;
                wallJumpTimer = wallJumpTime;
            }
        }
        else if(wallJumping)
        {
            if (isWallRunning)
                StopRunning();

            if (wallJumpTimer > 0)
                wallJumpTimer -= Time.deltaTime;

            if (wallJumpTimer <= 0)
                wallJumping = false;
        }
        else
        {
            if(isWallRunning)
                StopRunning();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isWallRunning)
            WallJump();

        //Debug.Log("wallLeft: " + wallLeft);
        //Debug.Log("wallRight: " + wallRight);

    }

    private void StartRunning()
    {
        
        isWallRunning = true;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        onWallSwitch.SetValue(gameObject);
        footStepEvent.Post(gameObject);

        cam.Fov(80);
        if (wallLeft)
            cam.Tilt(-5f);
        if (wallRight)
            cam.Tilt(5f);
    }


    private void WallRunningMovement()
    {
        rb.useGravity = false;
        wallNormal = wallRight ? hitRightWall.normal : hitLeftWall.normal;
        wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((playerPosition.forward - wallForward).magnitude > (playerPosition.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * playerMovement.wallRunSpeed, ForceMode.Force);
    }

    private void StopRunning()
    {
        isWallRunning = false;
        wallRunTimer = maxWallRunTime;
        footStepStopEvent.Post(gameObject);
        onGroundSwitch.SetValue(gameObject);
        cam.Fov(65);
        cam.Tilt(0);
    }

    private void WallJump()
    {
        //Debug.Log(wallNormal);
        rb.mass = 1f;
        wallJumping = true;
        wallJumpTimer = wallJumpTime;
        Vector3 jumpForce = playerPosition.up * wallJumpForce + wallNormal * wallJumpSideForce;

        rb.velocity = new Vector3 (rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(jumpForce, ForceMode.Impulse);
    }
}
