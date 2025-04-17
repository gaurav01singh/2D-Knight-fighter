using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject EnemyPF1;
    public GameObject EnemyPF2;
    public Transform spawner1;
    public Transform spawner2;
    public float spawnRate = 2f;
    public int enemyCount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SpawnEnemies()); // Start controlled spawning
        }
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyCount > 0)
        {   
            Spawn();
            enemyCount--; // Decrease enemy count
            yield return new WaitForSeconds(spawnRate); // Wait before spawning next enemy
        }
    }

    void Spawn()
    {
        // Choose a random spawn point
        Transform spawnPoint = Random.value > 0.5f ? spawner1 : spawner2;
        GameObject EnemyPF = Random.value > 0.5f ? EnemyPF1 : EnemyPF2;

        // Instantiate enemy at the chosen spawn point
        Instantiate(EnemyPF, spawnPoint.position, spawnPoint.rotation);
    }
}
