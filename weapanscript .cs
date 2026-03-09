using UnityEngine;

public class Weapanscript : MonoBehaviour
{
    [SerializeField] Rigidbody bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float bulletSpeed = 90f;
    [SerializeField] float reloadTime = 0.3f;
    [SerializeField] float bulletLife = 3f;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] Camera aimCamera;

    float nextFireTime;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + reloadTime;
        }
    }

    void Fire()
    {
        if (muzzleFlash != null)
            muzzleFlash.Play();

        Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 200f;
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;

        Rigidbody bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        bullet.velocity = direction * bulletSpeed;

        Destroy(bullet.gameObject, bulletLife);
    }
}