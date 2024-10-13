using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Game game;
    public UIController UI;
    public PlayerAnimations playerAnimations;

    public Rigidbody2D rb;

    public Vector3 movementDirection;
    public Vector3 facingDirection;
    public Vector3 mousePosition;
    public Vector3 lastPosition;

    float lastPosTime;
    public float speed;
    public float rotationSpeed;

    public int hp;
    public bool invincible;
    public bool mouseMoving;

    float dashCD;
    float dashSpeed;
    float dashDeceleration;
    bool dashing;
    bool canDash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movementDirection = Vector3.zero;
        mousePosition = Vector3.zero;
        lastPosition = transform.position;

        lastPosTime = 0f;
        speed = 8f;
        rotationSpeed = 15f;
        hp = 3;

        invincible = false;
        mouseMoving = true;

        dashCD = 0f;
        dashSpeed = 50f;
        dashDeceleration = 5f;
        dashing = false;
        canDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        lastPosTime += Time.deltaTime;
        dashCD -= Time.deltaTime;

        if (!canDash && dashCD <= 0)
        {
            canDash = true;
            UI.DashUI(true);
        }

        GetInput();

        if (lastPosTime > 1.0f)
        {
            lastPosition = transform.position;
        }

        // decelerate after dashing
        if (dashing)
        {
            dashSpeed -= dashSpeed * dashDeceleration * Time.deltaTime;
            if (dashSpeed < speed)
            {
                Debug.Log("STOPPED DASHING");
                dashing = false;
            }
        }

        //kill player if HP reaches 0
        if (hp <= 0)
        {
            lastPosition = new Vector2(2000.0f, 2000.0f);
            //spriteRenderer.enabled = false;
            playerAnimations.PlayerDied();
            enabled = false;
        }
    }

    void GetInput()
    {
        // get movement input
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // get character's facing direction
        mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        facingDirection = new Vector3(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y, 0f); ;

        transform.up = facingDirection;

        // dash mechanics
        if (Input.GetKeyDown(KeyCode.Space) && !dashing && dashCD <= 0f)
        {
            Debug.Log("DASH");
            dashing = true;
            dashSpeed = 50f;
            dashCD = 1.8f;
            canDash = false;
            UI.DashUI(false);
        }
    }

    private void FixedUpdate()
    {
        // dash in direction of mouse
        if (dashing)
        {
            rb.velocity = movementDirection * dashSpeed;
        }
        else
        {
            rb.velocity = movementDirection * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // hit by enemy: lose health and get temporary invincibility
        if (!invincible && collision != null && collision.gameObject.tag == "Enemy")
        {
            Debug.Log("TOOK DAMAGE!");
            hp -= 1;
            game.hp = hp;
            game.LoseHP();
            invincible = true;
            speed -= 3f;

            Invoke("Invincibility", 2.0f);
        }
    }

    void Invincibility()
    {
        invincible = false;
        speed += 3f;
    }
}
