using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffState
{
    Fire,
    Poision,
    Curse,
    None,
}

public class EnemySpawaner : MonoBehaviour
{
    [SerializeField] private int _startEnemyCount;
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 4f;

    [SerializeField] private float kingSpawnRate = 10f;
    [SerializeField] private int kingEnemyHealth = 1000;
    [SerializeField] private int nomalEnemyHealth = 100;

    private Queue<GameObject> _enemySpawningPool = new Queue<GameObject>();

    private void Awake()
    {
        SetEnemyPool();
    }

    private void SetEnemyPool()
    {
        for (int i = 0; i < 4; ++i)
        {
            _enemySpawningPool.Enqueue(transform.GetChild(i).gameObject);
            StartCoroutine(SpawnEnemy(0f));
        }
    }

    public void ReturnToPool(GameObject enemy)
    {
        _enemySpawningPool.Enqueue(enemy);
        float respawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        StartCoroutine(SpawnEnemy(respawnTime));
    }

    private IEnumerator SpawnEnemy(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        GameObject spawnedEnemy = _enemySpawningPool.Dequeue();

        float rate = Random.Range(0f, 100f);
        if (rate <= kingSpawnRate)
        {
            spawnedEnemy.GetComponent<Enemy>().MaxHealth = kingEnemyHealth;
            Debug.Log("\"King\" µÓ¿Â");
        }
        else
        {
            spawnedEnemy.GetComponent<Enemy>().MaxHealth = nomalEnemyHealth;
        }

        spawnedEnemy.SetActive(true);
    }
}
