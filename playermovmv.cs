using System.Diagnostics;
using UnityEngine;

public class playermovmv : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 8f;
    public float layerBlendSpeed = 5f;

    public Transform cam;
    public GameObject rifleObject;
    public GameObject gunObject;
    public GameObject granedObject;
    public GameObject sniperObject;
    bool rifleactive = false;
    bool gunactive = false;
    bool granedactive = false;
    bool sniperactive = false;

    Animator anim;
    int movementLayerIndex;
    int idlesLayerIndex;

    public bool isRunning;
    void Awake()
    {

        rifleObject.SetActive(false);
        gunObject.SetActive(false);
        granedObject.SetActive(false);
        sniperObject.SetActive(false);
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        if (cam == null) cam = Camera.main.transform;

        rifleactive = false;
        gunactive = false;
        granedactive = false;
        sniperactive = false;
        movementLayerIndex = anim.GetLayerIndex("movement");
        idlesLayerIndex = anim.GetLayerIndex("Idles");
        anim.SetBool("holdgun", false);
        anim.SetBool("holdbazooka", false);
        anim.SetBool("gunaim", false);
        anim.SetBool("bazookaim", false);
        anim.SetBool("granedaim", false);
        anim.SetBool("holdgraned", false);
        anim.SetBool("holdsniper", false);
        anim.SetBool("sniperaim", false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rifleactive = !rifleactive;
            gunactive = false;
            
            rifleObject.SetActive(rifleactive);
            gunObject.SetActive(false);

            anim.SetBool("holdbazooka", rifleactive);
            anim.SetBool("holdgun", false);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            gunactive = !gunactive;
            rifleactive = false;

            gunObject.SetActive(gunactive);
            rifleObject.SetActive(false);

            anim.SetBool("holdgun", gunactive);
            anim.SetBool("holdbazooka", false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            granedactive = !granedactive;

            granedObject.SetActive(granedactive);

            anim.SetBool("holdgraned", granedactive);
        }
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            sniperactive = !sniperactive;

            sniperObject.SetActive(sniperactive);

            anim.SetBool("holdsniper", sniperactive);
            anim.SetBool("sniperaim", false);
        }
        if (rifleactive)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("bazookaim", true);
                anim.SetBool("holdbazooka", false);
            }
            else
            {
                anim.SetBool("bazookaim", false);
                anim.SetBool("holdbazooka", true);
            }
        }
        else
        {
            anim.SetBool("bazookaim", false);
        }
        if (gunactive)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("gunaim", true);
                anim.SetBool("holdgun", false);
            }
            else
            {
                anim.SetBool("gunaim", false);
                anim.SetBool("holdgun", true);
            }
        }
        else
        {
            anim.SetBool("gunaim", false);
        }
        if (granedactive)
        {
            if (Input.GetMouseButton(0))
                {
                    anim.SetBool("granedaim", true);
                    anim.SetBool("holdgraned", false);
                }
                else
                {
                    anim.SetBool("granedaim", false);
                    anim.SetBool("holdgraned", true);
                }
        }
            else
            {
                anim.SetBool("granedaim", false);
            }
        if (sniperactive)
        {            if (Input.GetMouseButton(0))
                {
                    anim.SetBool("sniperaim", true);
                    anim.SetBool("holdsniper", false);
                }
                else
                {
                    anim.SetBool("sniperaim", false);
                    anim.SetBool("holdsniper", true);
                }
        }
            else
            {
                anim.SetBool("sniperaim", false);
            }
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;

        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDir = camForward * v + camRight * h;

        float magnitude = Mathf.Clamp01(new Vector2(h, v).magnitude);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        float speedValue = isRunning ? 2f : 1f;

        anim.SetFloat("Speed", magnitude * speedValue, 0.2f, Time.deltaTime);
        anim.SetFloat("Horizontal", h, 0.15f, Time.deltaTime);
        anim.SetFloat("Vertical", v, 0.15f, Time.deltaTime);

        float targetIdle = magnitude < 0.01f ? 1f : 0f;
        float targetMove = 1f - targetIdle;

        anim.SetLayerWeight(idlesLayerIndex,
            Mathf.Lerp(anim.GetLayerWeight(idlesLayerIndex), targetIdle, Time.deltaTime * layerBlendSpeed));

        anim.SetLayerWeight(movementLayerIndex,
            Mathf.Lerp(anim.GetLayerWeight(movementLayerIndex), targetMove, Time.deltaTime * layerBlendSpeed));

        if (magnitude > 0.01f)
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            transform.position += moveDir.normalized * currentSpeed * Time.deltaTime;
        }

        Quaternion targetRot = Quaternion.LookRotation(camForward);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );
    }
}
