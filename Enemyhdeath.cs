using UnityEngine;

public class Enemyhdeath : MonoBehaviour
{
    public float health = 100f;
    public Animator animator;
    public float destroyDelay = 3f;

    bool isDead = false;

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Enemy Died");

        if (animator != null)
        {
            animator.SetTrigger("death01");
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Destroy(gameObject, destroyDelay);
    }
}
