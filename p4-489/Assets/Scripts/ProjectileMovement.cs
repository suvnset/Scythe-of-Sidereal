using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public Game game;
    public UIController UI;
    public PlayerAttack playerAttack;
    public PlayerMovement playerMovement;
    public Transform scytheTransform;

    public Collider2D col;
    public SpriteRenderer sprite;

    Vector3 target;
    Vector3 direction;

    float rotationSpeed;
    float throwSpeed;
    float chargeTime;

    bool charging;
    bool travelling;
    public bool landed;

    void Start()
    {
        target = Vector3.zero;
        direction = Vector3.zero;

        rotationSpeed = 100f;
        throwSpeed = 20f;
        chargeTime = 0f;

        charging = false;
        travelling = false;
        landed = false;
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector3(playerMovement.mousePosition.x - transform.position.x, playerMovement.mousePosition.y - transform.position.y, 0f).normalized;

        // start rotating and count rotation time
        if (charging)
        {
            Debug.DrawRay(transform.position, direction, Color.red);

            transform.position = scytheTransform.position;
            rotationSpeed += Time.deltaTime * 1000;
            Mathf.Clamp(throwSpeed, 100f, 2000f);
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

            chargeTime += Time.deltaTime;
        }

        // travel to target area
        else if (travelling)
        {
            Debug.DrawRay(transform.position, direction, Color.red);

            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, target, throwSpeed * Time.deltaTime);

            // projectile has landed
            if (transform.position == target)
            {
                Land();
            }
        }
    }

    public void Land()
    {
        landed = true;
        travelling = false;
        rotationSpeed = 100f;
    }

    public void StartCharge()
    {
        charging = true;
        sprite.enabled = true;
    }

    public bool EndCharge()
    {
        travelling = true;
        col.enabled = true;

        Debug.Log("Charged for " + chargeTime + " seconds, target is " + direction.x + ", " + direction.y);

        // throw if charge time is more than 1.5 seconds
        if (chargeTime >= 4.5f)
        {
            Debug.Log("THROW LVL 3");
            target = transform.position + direction * 18;

        }
        else if (chargeTime >= 3f)
        {
            Debug.Log("THROW LVL 2");
            target = transform.position + direction * 12;

        }
        else if (chargeTime >= 1.5f)
        {
            Debug.Log("THROW LVL 1");
            target = transform.position + direction * 6;
        }
        else
        {
            Debug.Log("THROW CANCELLED");
            travelling = false;
            sprite.enabled = false;
            col.enabled = false;
        }

        chargeTime = 0f;
        charging = false;
        return travelling;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && landed && collision.gameObject.tag == "Player")
        {
            UI.WeaponUI(true);
            playerAttack.missingWeapon = false;
            sprite.enabled = false;
            col.enabled = false;
            landed = false;
        }

        if (collision != null && travelling && collision.gameObject.tag == "Wall")
        {
            Debug.Log("COLLIDED WITH WALL");
            Land();
        }
    }
}
