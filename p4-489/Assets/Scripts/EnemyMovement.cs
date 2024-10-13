using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Game game;

    [SerializeField] int health;
    GameObject player;
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;
    BreadcrumbManager breadcrumbManager;

    SpriteRenderer spriteRenderer;
    Collider2D col;

    Vector2 movementDirection;
    Vector2 targetPosition;
    Vector2 spawnPosition;

    bool invincible;
    float speed;
    float rotationSpeed;

    [SerializeField] float detectionRadius = 5f;
    [SerializeField] LayerMask obstaclesLayer;
    [SerializeField] float obstacleAvoidanceDistance = 1.5f;

    [SerializeField] float idleMoveRadius = 3f;
    [SerializeField] float idleMoveDelay = 2f;
    private float idleMoveTimer;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();

        movementDirection = Vector2.zero;
        invincible = false;
        speed = 3f;
        rotationSpeed = 15f;

        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerAttack = player.GetComponent<PlayerAttack>();
        breadcrumbManager = player.GetComponent<BreadcrumbManager>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        spawnPosition = transform.position;
        idleMoveTimer = idleMoveDelay;
    }

    void Update()
{
    targetPosition = GetBreadcrumbPosition();

    if (targetPosition == Vector2.zero)
    {
        HandleIdleMovement();
        Debug.Log("Enemy idle, no breadcrumb found");
    }
    else
    {
        idleMoveTimer = idleMoveDelay;
        Debug.Log("Moving toward breadcrumb at: " + targetPosition);
    }

    Vector2 newDirection = AvoidObstacles();
    if (newDirection == Vector2.zero)
    {
        newDirection = (targetPosition - (Vector2)transform.position).normalized;
    }

    transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + newDirection, speed * Time.deltaTime);
    RotateToMovement(newDirection);
}


    Vector2 GetBreadcrumbPosition()
    {
        // Get the latest breadcrumb dropped by the player
        GameObject latestBreadcrumb = breadcrumbManager.GetLatestBreadcrumb();
        if (latestBreadcrumb != null)
        {
            return latestBreadcrumb.transform.position;
        }

        // If no breadcrumb exists, return zero vector (enemy will idle)
        return Vector2.zero;
    }

    void HandleIdleMovement()
    {
        idleMoveTimer -= Time.deltaTime;
        if (idleMoveTimer <= 0f)
        {
            targetPosition = spawnPosition + Random.insideUnitCircle * idleMoveRadius;
            idleMoveTimer = idleMoveDelay;
        }
    }

    void RotateToMovement(Vector2 movementDirection)
    {
        if (movementDirection.sqrMagnitude > 0.01f)
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.forward, movementDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, quat, rotationSpeed * Time.deltaTime);
        }
    }

    private Vector2 AvoidObstacles()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, movementDirection, obstacleAvoidanceDistance, obstaclesLayer);

        if (hit.collider != null)
        {
            Vector2 avoidDirection = Vector2.Perpendicular(movementDirection);
            RaycastHit2D sideHit = Physics2D.Raycast(transform.position, avoidDirection, obstacleAvoidanceDistance, obstaclesLayer);

            if (sideHit.collider != null)
            {
                avoidDirection = -avoidDirection;
            }

            return avoidDirection.normalized;
        }

        return Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!invincible && collision.tag == "Weapon")
        {
            playerAttack.KillEnemy();

            if (collision.gameObject.name == "Special Beam")
            {
                Destroy(gameObject);
            }

            Debug.Log("HIT!");
            health -= 1;

            if (health <= 0)
            {
                game.IncreaseKillCount();
                Destroy(gameObject);
                Debug.Log("Died of death.");
            }

            invincible = true;
            col.enabled = false;
            spriteRenderer.color = Color.gray;
            speed = 0f;

            Invoke("Invincibility", 1.0f);
        }
    }

    void Invincibility()
    {
        invincible = false;
        col.enabled = true;
        speed = 4f;
        spriteRenderer.color = Color.red;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
