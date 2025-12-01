using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [Header("Zombie Prefabs")]
    public GameObject normalZombiePrefab;
    public GameObject objectiveZombiePrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 3f;
    public int maxZombiesAlive = 10;

    [Header("Spawn Points")]
    public Transform[] normalZombieSpawnPoints;
    public Transform[] objectiveZombieSpawnPoints;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            TrySpawnZombie();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void TrySpawnZombie()
    {
        int currentZombieCount = GameObject.FindGameObjectsWithTag("Zombie").Length;

        if (currentZombieCount >= maxZombiesAlive)
            return;

        //randomly decide which type to spawn
        bool spawnObjectiveZombie = Random.value > 0.5f;

        if (spawnObjectiveZombie && objectiveZombiePrefab && objectiveZombieSpawnPoints.Length > 0)
        {
            Transform point = objectiveZombieSpawnPoints[Random.Range(0, objectiveZombieSpawnPoints.Length)];
            Instantiate(objectiveZombiePrefab, point.position, point.rotation);
        }
        else if (normalZombiePrefab && normalZombieSpawnPoints.Length > 0)
        {
            Transform point = normalZombieSpawnPoints[Random.Range(0, normalZombieSpawnPoints.Length)];
            Instantiate(normalZombiePrefab, point.position, point.rotation);
        }
    }
}
