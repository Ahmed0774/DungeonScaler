using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float bulletForce = 20f;

    [SerializeField] float reloadTime;
    [SerializeField] Camera cam;
    Vector2 mousePos;

    PlayerController player;

    float lookAngle;

    bool shoot = false;
    int shootTimer = 0;

    // Update is called once per frame
    void Start()
    {
        player = GetComponent<PlayerController>();
        shootTimer = (int)reloadTime;
    }
    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        

        if (Input.GetButtonDown("Fire1"))
        {
            shoot = true;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            shoot = false;
        }
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - new Vector2(firePoint.position.x, firePoint.position.y);
        lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        firePoint.rotation = Quaternion.Euler(0,0, lookAngle);
        firePoint.position = gameObject.transform.position;

        if (shoot && shootTimer > reloadTime * player.GetAttackSpeed())
        {
            Debug.Log("Atk speed is: " +reloadTime * player.GetAttackSpeed());
            Shoot();
        }

        shootTimer++;
    }
    private void Shoot()
    {
        float angleStep = player.GetProjectileSpread() / player.GetProjectileCount();
        float aimingAngle = firePoint.rotation.eulerAngles.z;
        float centeringOffset = (player.GetProjectileSpread() / 2) - (angleStep / 2); //offsets every projectile so the spread is                                                                                                                         //centered on the mouse cursor

        for (int i = 0; i < player.GetProjectileCount(); i++)
        {
            float currentBulletAngle = angleStep * i;

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimingAngle + currentBulletAngle - centeringOffset));
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bullet.transform.up * bulletForce, ForceMode2D.Impulse);
        }
        shootTimer = 0;
    }
}

