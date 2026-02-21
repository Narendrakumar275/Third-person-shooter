using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform player;

    public float distance = 1.2f;   
    public float height = 1.5f;
    public float mouseSensitivity = 3f;

    public float minY = -20f;
    public float maxY = 60f;

    public float smoothTime = 0.08f;

    float yaw;
    float pitch;
    Vector3 currentVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = player.eulerAngles.y;
    }

    void Update()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPos = player.position + Vector3.up * height - rotation * Vector3.forward * distance;

        transform.position = Vector3.SmoothDamp(transform.position,targetPos,ref currentVelocity,smoothTime);

        transform.LookAt(player.position + Vector3.up * height);
    }
}
