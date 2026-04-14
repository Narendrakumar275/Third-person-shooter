using UnityEngine;

public class jumps : MonoBehaviour
{
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    float yVelocity;

    CharacterController controller;
    Animator anim;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (anim != null)
            {
                anim.SetTrigger("jump");
            }
        }

        yVelocity += gravity * Time.deltaTime;

        Vector3 move = new Vector3(0, yVelocity, 0);
        controller.Move(move * Time.deltaTime);
    }

    public float GetYVelocity()
    {
        return yVelocity;
    }
}