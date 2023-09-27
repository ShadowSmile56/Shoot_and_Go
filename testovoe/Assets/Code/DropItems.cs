using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItems : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public float dropRate = 0.5f;

    private void OnDestroy()
    {
        // Выполняйте логику выпадения предметов только при уничтожении объекта
        if (Random.value < dropRate)
        {
            DropRandomItem();
        }
    }

    private void DropRandomItem()
    {
        int randomIndex = Random.Range(0, itemPrefabs.Length);
        GameObject itemPrefab = itemPrefabs[randomIndex];
        Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }
}