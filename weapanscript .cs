using UnityEngine;

public class weapanscript : MonoBehaviour
{
    public float damage = 20f;
    public float range = 100f;

    public Camera cam;
    public AudioSource audioSource;
    public AudioClip shootSound;

    public GameObject shootEffect;
    public GameObject enemyHitEffect;
    public GameObject groundHitEffect;

    public Transform firePoint;

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (shootSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        if (shootEffect != null && firePoint != null)
        {
            GameObject effect = Instantiate(shootEffect, firePoint.position, firePoint.rotation);
            Destroy(effect, 0.5f);
        }

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            GameObject impactEffect = null;

            if (hit.transform.CompareTag("enemy"))
            {
                impactEffect = enemyHitEffect;

                Enemyhdeath Enemy = hit.transform.GetComponent<Enemyhdeath>();
                if (Enemy != null)
                {
                    Enemy.TakeDamage(damage);
                }
            }
            else
            {
                impactEffect = groundHitEffect;
            }

            if (impactEffect != null)
            {
                GameObject impact = Instantiate(
                    impactEffect,
                    hit.point,
                    Quaternion.LookRotation(hit.normal)
                );
                Destroy(impact, 1f);
            }
        }
    }
}
