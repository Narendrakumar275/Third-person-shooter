using UnityEngine;

public class Enemyhdeath : MonoBehaviour
{
    public int enemyHealth = 100;
    public int enemyDamage = 30;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            TakeDamage(30);
        }
    }
    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;

        if (enemyHealth <= 0)
        {
            Debug.Log("Enemy is dead");
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
