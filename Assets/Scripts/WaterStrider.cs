using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WaterStrider : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 3f;

    [Header("Dash")]
    public float dashForce = 10f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 1f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private TMP_Text foodText;
    [SerializeField] private LoseScreen loseScreen;
    [SerializeField] private WinScreen winScreen;

    private Rigidbody rb;
    private Vector3 moveDirection;

    private bool isDashing;
    private float dashCooldownTimer;
    private int foodCount = 0;
    private const int WIN_FOOD = 10;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateFoodUI();
    }

    void Update()
    {
        ReadInput();
        HandleCameraRotation();
        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && dashCooldownTimer <= 0f)
            StartDash();

        if (!isDashing)
            dashCooldownTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (isDashing) return;

        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    // ---------------- INPUT ----------------
    void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        moveDirection = (forward * v + right * h).normalized;
    }

    // ----------- CAMERA-BASED ROTATION -----------
    void HandleCameraRotation()
    {
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = targetRotation;
    }

    // ---------------- ANIMATION ----------------
    void HandleAnimation()
    {
        bool isMoving = moveDirection.sqrMagnitude > 0.01f;
        animator.SetBool("IsMoving", isMoving);
    }

    // ---------------- DASH ----------------
    void StartDash()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;

        animator.SetTrigger("Dash");

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        Invoke(nameof(StopDash), dashDuration);
    }

    void StopDash()
    {
        isDashing = false;
    }

    // ---------------- FOOD + WIN ----------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("food"))
        {
            foodCount++;
            UpdateFoodUI();

            if (foodCount >= WIN_FOOD)
            {
                winScreen.Show();
                gameObject.SetActive(false);
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("enemy"))
        {
            loseScreen.Show();
            gameObject.SetActive(false);
        }
    }

    void UpdateFoodUI()
    {
        if (foodText != null)
            foodText.text = ": " + foodCount;
    }

  
}