using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float BaseSpeed = 5f;
    private const float WalkSpeedModifier = 0.4f;
    private const float NonCombatRunSpeedModifier = 1.25f;
    private const float CombatRunSpeedModifier = 1.4f;
    private const float DashSpeedModifier = 2.5f;
    private const float DashCooldown = 1.75f;
    private const float RollSpeedModifier = 1f;
    private const float HardLandHeight = 3f;
    private const float RotationSpeed = 0.1f;
    private const float JumpSpeed = 7f;
    private const float WalkAccelerator = 6f;
    private const float RunAccelerator = 3f;
    private const float DashAccelerator = 70f;

    private float rotateVelocity;

    private Rigidbody rb;
    private Player player;
    private Vector3 moveDirection;
    private Vector2 inputVector;
    private bool walkToggle;
    private bool isJumping;
    private bool isFalling;
    private float fallingSource;
    private float lastJumpTime;
    private float lastDashTime = 0;
    private int dashCount = 0;

    [SerializeField] private Animator animator;

    public bool IsGrounded
    {
        get
        {
            return Physics.Raycast(transform.position, Vector3.down, 1f, Physics.AllLayers);
        }
    }

    public bool SpeedCheated = false;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        player = Player.Instance;
        walkToggle = true;

        player.InputAction.Player.Jump.performed += Jump;
        player.InputAction.Player.WalkToggle.performed += ToggleWalk;
        player.InputAction.Player.Dash.performed += Dash;
    }

    private void Update()
    {
        inputVector = player.InputAction.Player.Movement.ReadValue<Vector2>();

        float fallingSpeed = -rb.velocity.y;
        //Debug.Log(dashCount);

        if(IsGrounded && Time.time - lastJumpTime >= 1f)
        {
            animator.SetBool("isGrounded", true);
            animator.SetBool("isFalling", false); 
            animator.SetBool("isJumping", false);

            isJumping = false;

            if(isFalling)
            {
                float fallingHeight = fallingSource - transform.position.y;
                Debug.Log(fallingHeight);

                animator.SetBool("isHardLand", false);
                animator.SetBool("isLightLand", false);
                animator.SetBool("isRoll", false);

                if(fallingHeight > HardLandHeight)
                {
                    //player.DisableAction();
                    animator.SetBool("isHardLand", true);
                }
                else
                {
                    animator.SetBool("isLightLand", true);
                }
            }

            isFalling = false;
        }
        else
        {
            animator.SetBool("isGrounded", false);
            
            if((isJumping && fallingSpeed > 0) || fallingSpeed > 1) // if we're going down after jumping or falling off cliffs
            {
                if(!isFalling)
                {
                    fallingSource = transform.position.y;
                    //Debug.Log(fallingSource);
                }

                animator.SetBool("isFalling", true);
                isFalling = true;
            }
        }
    }
    private void FixedUpdate()
    {
        moveDirection = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if (moveDirection != Vector3.zero)
        {
            Move();
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCombatRun", false);
        }
    }

    private void Move()
    {
        float targetAngle = Mathf.Atan2(this.moveDirection.x, this.moveDirection.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotateVelocity, RotationSpeed);

        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        Vector3 moveDirection = (Quaternion.Euler(0, targetAngle, 0) * Vector3.forward).normalized;

        if(walkToggle)
        {
            rb.AddForce(WalkAccelerator * BaseSpeed * WalkSpeedModifier * moveDirection, ForceMode.Force);
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCombatRun", false);
        }
        else if(player.IsCombat)
        {
            rb.AddForce(RunAccelerator * BaseSpeed * CombatRunSpeedModifier * moveDirection, ForceMode.Force);
            animator.SetBool("isCombatRun", true);
            animator.SetBool("isWalking", false);
        }
        else
        {
            rb.AddForce(RunAccelerator * BaseSpeed * NonCombatRunSpeedModifier * moveDirection, ForceMode.Force);
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
            animator.SetBool("isJumping", true);
            isJumping = true;
            lastJumpTime = Time.time;
        }
    }

    private void ToggleWalk(InputAction.CallbackContext context)
    {
        walkToggle = !walkToggle;
    }

    private void Dash(InputAction.CallbackContext context)
    {
        if(Time.time - lastDashTime > DashCooldown)
        {
            dashCount = 0;
        }

        // if second dash < 1 second or dash in cooldown
        if ((dashCount < 2 && Time.time - lastDashTime < 1f) || (Time.time - lastDashTime < DashCooldown && dashCount == 2)) return;

        animator.SetBool("isDashing", true);
        rb.AddForce(DashAccelerator * BaseSpeed * DashSpeedModifier * transform.forward, ForceMode.Force);
        lastDashTime = Time.time;
        dashCount++;
    }

    public void CheatSpeed()
    {
        if (SpeedCheated) return;

        BaseSpeed *= 2.5f;
        SpeedCheated = true;
    }
}
