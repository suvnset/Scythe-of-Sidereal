using UnityEngine;

public class Spawner : MonoBehaviour
{
    Game game;

    [SerializeField] GameObject weakEnemy;
    [SerializeField] GameObject midEnemy;
    [SerializeField] Vector3 spawnOffset = new Vector3(2f, 2f, 0f); // Offset above the floor for enemy spawn
    GameObject[] enemies;

    bool active;
    float timeLimit;
    float timeToSpawn;
    float minSpawnDelay;
    float maxSpawnDelay;

    GameObject currEnemy;
    Camera mainCamera;
    Transform playerTransform;
    GameObject[] wave1Floors;  // Array to store all floor objects with "Wave1" tag

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();

        mainCamera = Camera.main;
        playerTransform = GameObject.FindWithTag("Player").transform;

        enemies = new GameObject[2];

        if (weakEnemy != null)
        {
            enemies[0] = weakEnemy;
            currEnemy = weakEnemy;
        }

        if (midEnemy != null)
        {
            enemies[1] = midEnemy;
        }

        active = false;
        timeLimit = 0f;
        timeToSpawn = 1f;
        minSpawnDelay = 1f;
        maxSpawnDelay = 5f;

        // Find all floors with the "Wave1" tag
        wave1Floors = GameObject.FindGameObjectsWithTag("Wave1");
    }

    void Update()
    {
        if (active)
        {
            // Time until wave ends
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0f)
            {
                active = false;
            }

            // Time until next enemy spawns
            timeToSpawn -= Time.deltaTime;
            if (timeToSpawn <= 0f)
            {
                SpawnAttacker();
                timeToSpawn = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
        }
    }

    public void SpawnAttacker()
    {
        Debug.Log("spawning enemy");

        // Get random position on a "Wave1" floor
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(currEnemy, spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        if (wave1Floors.Length == 0)
        {
            Debug.LogWarning("No objects with tag 'Wave1' found.");
            return Vector3.zero;
        }

        // Choose a random floor from the list of "Wave1" floors
        GameObject randomFloor = wave1Floors[Random.Range(0, wave1Floors.Length)];

        // Get the position of the floor and apply the spawn offset
        Vector3 floorPosition = randomFloor.transform.position;
        Vector3 spawnPosition = floorPosition + spawnOffset;

        spawnPosition.z = 0; // Ensure the enemy spawns at the correct Z axis for 2D games
        return spawnPosition;
    }

    public void Activate(float time)
    {
        active = true;
        timeLimit = time;
        Debug.Log("SPAWNER ACTIVATED");
    }

    public void ChangeEnemyType(int type)
    {
        currEnemy = enemies[type];
    }

    public void ChangeSpawnDelay(float min, float max)
    {
        minSpawnDelay = min;
        maxSpawnDelay = max;
    }
}
