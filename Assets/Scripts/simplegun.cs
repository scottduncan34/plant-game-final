using UnityEngine;

public class SimpleGun : MonoBehaviour
{
    public Camera playerCamera;
    public float range = 100f;
    public float damage = 10f;

    [Header("Fire Mode")]
    public bool isFullAuto = true;
    public float fireRate = 10f; // bullets per second
    private float nextFireTime = 0f;

    public ParticleSystem muzzleFlash;
    public AudioSource gunAudio;

    void Awake()
    {
        //assign camera
        if (playerCamera == null)
        {
            playerCamera = GetComponentInParent<Camera>();
            if (playerCamera == null)
                playerCamera = Camera.main;
        }

        //assign audio source
        if (gunAudio == null)
            gunAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!gameObject.activeInHierarchy)
            return;


        if (isFullAuto)
        {
            //full auto
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
        else
        {
            //semi auto
            if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                nextFireTime = Time.time + 1f / fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        //visual + audio effects
        if (muzzleFlash) muzzleFlash.Play();
        if (gunAudio) gunAudio.Play();

        //center-of-screen ray
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 0.1f);

        //shoot ray
        if (Physics.Raycast(ray, out RaycastHit hit, range))
        {
            Debug.Log("Hit: " + hit.transform.name);
            //revent player from damaging the plant
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Plant"))
                return;

            //damage objects with health component
            Health health = hit.transform.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
