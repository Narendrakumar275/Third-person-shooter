using UnityEngine;
public class Playerhdeath : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    private Animator animator;
    public bool death01=false;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetBool("death01" , true);
    }
}
