using UnityEngine;

public class aimcamera : MonoBehaviour
{
    [Header("Player Reference")]
    public Transform player;

    [Header("Camera References")]
    public Camera normalCamera;
    public Camera aimCamera;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Camera Settings")]
    public float distance = 3f;
    public float height = 1.5f;
    public float rightOffset = 0.8f;

    [Header("Sensitivity")]
    public float normalSensitivity = 3f;
    public float aimSensitivity = 1.5f;

    [Header("Smoothing")]
    public float positionSmoothTime = 0.08f;
    public float rotationSmoothSpeed = 10f;

    public float minY = -20f;
    public float maxY = 60f;

    float yaw;
    float pitch;

    Vector3 currentVelocity;

    Animator anim;

    void Start()
    {
        anim = GetComponentInParent<Animator>();

        aimCamera.gameObject.SetActive(false);
        crosshair.SetActive(false);

        yaw = player.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isAiming =
            anim.GetBool("holdbazooka") ||
            anim.GetBool("holdgun") ||
            anim.GetBool("gunaim") ||
            anim.GetBool("bazookaim");

        float sensitivity = isAiming ? aimSensitivity : normalSensitivity;

        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            player.position +
            Vector3.up * height -
            targetRotation * Vector3.forward * distance +
            targetRotation * Vector3.right * rightOffset;

        // 🔥 Smooth Position
        normalCamera.transform.position = Vector3.SmoothDamp(
            normalCamera.transform.position,
            targetPosition,
            ref currentVelocity,
            positionSmoothTime
        );

        aimCamera.transform.position = normalCamera.transform.position;

        // 🔥 Smooth Rotation
        normalCamera.transform.rotation = Quaternion.Lerp(
            normalCamera.transform.rotation,
            targetRotation,
            rotationSmoothSpeed * Time.deltaTime
        );

        aimCamera.transform.rotation = normalCamera.transform.rotation;

        if (isAiming)
        {
            normalCamera.gameObject.SetActive(false);
            aimCamera.gameObject.SetActive(true);
            crosshair.SetActive(true);
        }
        else
        {
            normalCamera.gameObject.SetActive(true);
            aimCamera.gameObject.SetActive(false);
            crosshair.SetActive(false);
        }
    }
}