using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб противника
    public Transform[] spawnPoints; // Массив точек спавна
    public int numberOfEnemies = 3; // Количество противников
    private List<int> usedSpawnPoints = new List<int>();
    private void Start()
    {
        usedSpawnPoints.Clear(); // Сброс занятых мест при старте игры
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomSpawnPointIndex;
            do
            {
                randomSpawnPointIndex = Random.Range(0, spawnPoints.Length);
            } while (usedSpawnPoints.Contains(randomSpawnPointIndex));

            usedSpawnPoints.Add(randomSpawnPointIndex);

            Transform spawnPoint = spawnPoints[randomSpawnPointIndex];
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}