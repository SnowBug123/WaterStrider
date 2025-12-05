using UnityEngine;
using TMPro;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    //public Rigidbody body;
    //public float speed;
    //public Vector3 forwardVector;
    //public Vector3 rightVector;
    //public float dashForse;

    //private void Update()
    //{
    //    float forvard = Input.GetAxisRaw("Vertical") * speed;
    //    float right = Input.GetAxisRaw("Horizontal") * speed;

    //    body.AddForce(new Vector3(right, 0, forvard) * Time.deltaTime);
    //}










    //[Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;     // скорость обычного перемещения
    [SerializeField] private float _dashDistance = 3f;  // длина рывка
    [SerializeField] private float _dashCooldown = 1f;  // перезарядка рывка

    private bool _canDash = true;
    private Vector3 _dashTarget;

    //[Header("References")]
    //[SerializeField] private Transform _cameraPivot;   // объект камеры или pivot камеры
    //[SerializeField] private Animator _animator;       // аниматор водомерки

    //[Header("Environment")]
    //[SerializeField] private LayerMask _waterLayer;    // слой воды
    //[SerializeField] private float _waterCheckDistance = 1f;

    //[Header("Food UI")]
    [SerializeField] private TMP_Text _foodText;            // ссылка на текст UI
    private int _foodCount = 0;

    private void Start()
    {
        UpdateFoodUI();
    }

    private void Update()
    {
        Move();
        TryDash();
    }

    //// ------------ ПРОВЕРКА: на воде ли мы? ------------
    //private bool IsOnWater()
    //{
    //    Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
    //    return Physics.Raycast(ray, _waterCheckDistance, _waterLayer);
    //}

    //// ------------ ОБЫЧНОЕ ДВИЖЕНИЕ ------------
    private void Move()
    {
    //    //if (!IsOnWater()) return;  // нельзя двигаться, если не на воде

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(horizontal, 0, vertical);
        input = Vector3.ClampMagnitude(input, 1f); // нормализация

    //    // движение относительно камеры
    //    Vector3 camForward = _cameraPivot.forward;
    //    camForward.y = 0;
    //    camForward.Normalize();

    //    Vector3 camRight = _cameraPivot.right;
    //    camRight.y = 0;
    //    camRight.Normalize();

    //    Vector3 moveDir = camForward * input.z + camRight * input.x;

    //    transform.position += moveDir * _moveSpeed * Time.deltaTime;

    //    // Поворот в сторону движения (если есть движение)
    //    if (moveDir.magnitude > 0.1f)
    //        transform.rotation = Quaternion.LookRotation(moveDir);
    }

    //// ------------ РЫВОК ТОЛЬКО ВПЕРЁД ------------
    private void TryDash()
    {
    //    //if (!IsOnWater()) return;

        if (Input.GetKeyDown(KeyCode.Space) && _canDash)
        {
            _canDash = false;

            // Направление рывка = направление камеры по горизонтали
    //        Vector3 dashDir = _cameraPivot.forward;
    //        dashDir.y = 0;
    //        dashDir.Normalize();

    //        _dashTarget = transform.position + dashDir * _dashDistance;

    //    //    // Воспроизведение анимации рывка
         //   if (_animator != null)
         //       _animator.SetTrigger("Dash");

          //  StartCoroutine(DashMovement());
          //  Invoke(nameof(ResetDash), _dashCooldown);
        }
    }

    private System.Collections.IEnumerator DashMovement()
    {
        float time = 0f;
        float duration = 0.15f; // длительность рывка

        Vector3 startPos = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, _dashTarget, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = _dashTarget;
    }

    private void ResetDash()
    {
        _canDash = true;
    }

    //// ------------ СБОР ЕДЫ ------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("food"))
        {
            _foodCount++;
            UpdateFoodUI();

            // анимация поедания
           // if (_animator != null)
           //     _animator.SetTrigger("Eat");

            Destroy(other.gameObject);
        }
    }

    private void UpdateFoodUI()
    {
        if (_foodText != null)
            _foodText.text = "Food: " + _foodCount;
    }
}