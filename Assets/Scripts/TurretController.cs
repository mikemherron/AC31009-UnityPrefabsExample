using UnityEngine;

public class TurretController : MonoBehaviour
{
  public GameObject bulletPrefab;
  public float fireDelay = 0f;
  public int health = 3;
  public float fireSpeed = 0.5f;

  private float timeToNextFire = 0f;
  private GameObject fireTarget;

  void Start()
  {
    timeToNextFire = fireDelay * Random.value;

    // Note this has changed from the last demo - we now look up the
    // player by tag rather than requiring the reference to be provided
    // in the editor. This makes it easier to reuse the turret prefab at the
    // expense of a static reference to the player.
    fireTarget = GameObject.FindGameObjectWithTag("Player");
  }

  void Update()
  {
    timeToNextFire -= Time.deltaTime;
    if (timeToNextFire <= 0f)
    {
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
    if (collision.gameObject.tag == "Bullet")
    {
      BulletController bullet = collision.gameObject.GetComponent<BulletController>();
      if (bullet.firedBy.tag == "Player")
      {
        Destroy(collision.gameObject);
        health--;
        if (health <= 0)
        {
          Destroy(gameObject);
        }
      }
    }
  }
}
