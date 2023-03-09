using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Rather than use our own health properties, we
    // now have a component, HealthController, that 
    // manages our health. We can use this component
    // on any game object that needs health!
    private HealthController healthController;

    public GameObject bulletPrefab;

    public float moveSpeed = 0f;
    public Rigidbody2D rigidBody;
    public Vector2 movement;
    public Animator animator;
    public AudioSource hitSound;
    
    void Start()
    {
        // Get the HealthController component from this game object
        // programmatically rather than needing it to be set in the
        // inspector. For components that always go together, this
        // can be preferred to linking them in the inspector.
        healthController = GetComponent<HealthController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            direction.Normalize();
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletController controller = bullet.GetComponent<BulletController>();
            controller.direction = direction;
            controller.firedBy = gameObject;
            Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate() 
    {
        rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet") {
            BulletController controller = collision.gameObject.GetComponent<BulletController>();
            if (controller.firedBy.tag == "Turret") {
                Destroy(collision.gameObject);
                // We can now use the HealthController component
                // to manage our health rather than having to
                // manage it ourselves.
                healthController.TakeDamage();
                 if(healthController.health > 0) {
                    hitSound.Play();
                 }
            }
        }
    }
}