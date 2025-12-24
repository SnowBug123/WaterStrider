using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;      // WaterStrider
    public Transform pivot;       // CameraPivot

    public float mouseSensitivity = 3f;
    public float minY = -30f;
    public float maxY = 60f;

    private float xRotation;
    private float yRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        pivot.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.position = pivot.position;
        transform.rotation = pivot.rotation;
    }
}