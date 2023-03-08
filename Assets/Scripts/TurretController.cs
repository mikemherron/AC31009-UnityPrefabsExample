using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public GameObject bulletPrefab;

    public GameObject fireTarget;

    public float fireDelay = 0f;

    public AudioSource hitSound;

    private float timeToNextFire = 0f;
    
    public SpriteRenderer spriteRenderer;

    public int health = 3;

    public float fireSpeed = 0.5f;

    void Start()
    {
        timeToNextFire = fireDelay * Random.value;
    }

    // Update is called once per frame
    void Update()
    {
        timeToNextFire -= Time.deltaTime;
        if (timeToNextFire <= 0f) {
            timeToNextFire = fireDelay;
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Vector2 direction = fireTarget.transform.position - transform.position;
            direction.Normalize();

            BulletController controller = bullet.GetComponent<BulletController>();
            controller.direction = direction;
            controller.speed = fireSpeed;
            controller.firedBy = gameObject;
            
            bullet.GetComponent<SpriteRenderer>().color = Color.red;
        }        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Turret hit by bullet");
        if (collision.gameObject.tag == "Bullet") {
            BulletController bullet = collision.gameObject.GetComponent<BulletController>();
            if (bullet.firedBy.tag == "Player") {
                Destroy(collision.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
