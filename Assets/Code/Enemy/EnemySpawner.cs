using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public float spawnRate = 5f;
    public float spawnDistance = 10f;
    public GameObject[] enemies;

    void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnRate, spawnRate);
    }

	void SpawnEnemy()
    {
        Vector3 location = Random.onUnitSphere;
        location *= 10;
		GameObject.Instantiate(enemies[Random.Range(0, enemies.Length)], randomPointOnCircle(15) + Vector3.up, Quaternion.identity);
    }

    Vector3 randomPointOnCircle(float scale)
    {
        Vector3 loc = Random.onUnitSphere;
        loc.y = 0;
        loc.Normalize();
        loc *= scale;
        return loc;
    }
}