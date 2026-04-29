using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;
    public Animator Pistol_B;

    //SFX
    public AudioClip Gunshoot;
    public AudioClip ReloadSound;
    public AudioClip emptyGunClick;

    //Weapon type
    public bool isGlock17 = true;
    public bool isM16A4 = false;
    public GameObject Glock_17;
    public GameObject M16A4;

    //parst of player!

    //Shoting setings
    public bool isShoting, readyToShoot;
    bool allowReset = true;
    public float shotingDelay;
    public float reloadTime = 2f; 
    public int magCapacity = 17;
    public int mag = 17;

    //Burst Setings
    public int shootsPerBurst = 3;
    public int currentBurst;

    //Spread
    public float spreadIntensity;

    //Bullet setings
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 40f;
    public float bulletLifeTime = 3f;

    public enum shootingMode
    {
        Single,
        Burst,
        Auto
    }

    public shootingMode currentShootingMode;

    public void Awake()
    {
        readyToShoot = true;
        currentBurst = shootsPerBurst;

        if (isGlock17)
        {
            Glock_17.SetActive(true);
            M16A4.SetActive(false);
           
        }
        else if (isM16A4)
        {
            Glock_17.SetActive(false);
            M16A4.SetActive(true);
           
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Alpha1) && isM16A4 == true)
        {
            Glock_17.SetActive(true);
            M16A4.SetActive(false);

            isGlock17 = true;  
            isM16A4 = false;

            
        }
        if (Input.GetKey(KeyCode.Alpha2) && isGlock17 == true)
        {
            Glock_17.SetActive(false);
            M16A4.SetActive(true);

            isGlock17 = false;  
            isM16A4 = true;

           
        }


        // Reload input
        if (Input.GetKeyDown(KeyCode.R) && mag < magCapacity && readyToShoot)
        {
            reloadMagazine();
        }

        if (currentShootingMode == shootingMode.Auto)
        {
            isShoting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == shootingMode.Burst ||
            currentShootingMode == shootingMode.Single)
        {
            isShoting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Added "mag > 0" so you can't shoot without bullets
        if (readyToShoot && isShoting && mag > 0)
        {
            currentBurst = shootsPerBurst;
            FireWepon();
        }

        if(readyToShoot && isShoting && mag <= 0)
        {
            AudioSource.PlayClipAtPoint(emptyGunClick, transform.position);
        }
    }

    private void reloadMagazine()
    {
        readyToShoot = false; // Stop shooting while reloading

        if (ReloadSound != null)
        {
            AudioSource.PlayClipAtPoint(ReloadSound, transform.position);
        }

        Invoke("FinishReload", reloadTime);
    }

    private void FinishReload()
    {
        mag = magCapacity;
        readyToShoot = true;
    }

    private void FireWepon()
    {


        Pistol_B.SetBool("shoter", true);
        readyToShoot = false;
        mag--; // Decrease ammo

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //SFX
        AudioSource.PlayClipAtPoint(Gunshoot, transform.position);
        Pistol_B.SetTrigger("shoter");

        //GOD creates bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        //Make the bullet face the direction of shoting
        bullet.transform.forward = shootingDirection;

        //Gun shoots :)
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);

        //GOD deletes bullet
        StartCoroutine(DestroyBullet(bullet, bulletLifeTime));

        //check if whe are done shoting
        if (allowReset)
        {
            // Fixed: Only invoke ResetShoot if we are NOT in the middle of a burst
            if (currentShootingMode != shootingMode.Burst || currentBurst <= 1)
            {
                Invoke("ResetShoot", shotingDelay);
                allowReset = false;
            }
        }

        //Burst
        if (currentShootingMode == shootingMode.Burst && currentBurst > 1 && mag > 0)
        {
            currentBurst--;
            Invoke("FireWepon", shotingDelay);
        }
    }

    private void ResetShoot()
    {
        readyToShoot = true;
        allowReset = true;
        Pistol_B.SetBool("shoter", false);
    }

    private IEnumerator DestroyBullet(GameObject bullet, float Life)
    {
        yield return new WaitForSeconds(Life);
        Destroy(bullet);
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        //shoting from the midel of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        // Fixed: Added negative spread for real randomness
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }
}
