using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private Inventory inventory;
    public GameObject slotButton;

    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!TryStackItem(slotButton))
            {
                for (int i = 0; i < inventory.slots.Length; i++)
                {
                    if (inventory.isFull[i] == false)
                    {
                        inventory.isFull[i] = true;
                        inventory.slots[i].GetComponent<Slot>().AddItem(slotButton);
                        Destroy(gameObject);
                        break;
                    }
                }
            }
        }
    }
    private bool TryStackItem(GameObject itemPrefab)
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Slot slot = inventory.slots[i].GetComponent<Slot>();
            if (slot.IsEmpty())
            {
                continue;
            }

            if (slot.GetItemPrefab() == slotButton)
            {
                // Увеличьте количество предметов в инвентаре
                slot.IncreaseItemCount();
                Destroy(gameObject); // Уничтожьте подобранный предмет
                return true;
            }
        }
        return false;
    }
}