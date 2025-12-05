using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterStrider : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float speed = 0.8f;
    private Vector3 moveVector;

    public float dashForce = 10f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        moveVector.z = Input.GetAxis("Vertical");
        rb.MovePosition(rb.position + moveVector * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashCooldownTimer <= 0)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                StopDash();
            }
        }
        else
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        Vector3 movementDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

        if (movementDirection == Vector3.zero)
        {
            movementDirection = transform.forward;
        }

        rb.AddForce(movementDirection * dashForce, ForceMode.Impulse);
    }

    void StopDash()
    {
        isDashing = false;
    }
}