using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Slot : MonoBehaviour
{
    private Inventory inventory;
    public int index;
    public Button deleteButton;
    private Button itemButton;
    public GameObject itemPrefab;
    public GameObject newItem;
    public Text itemCountText;
    public int itemCount;
    public bool deleteButtonVisibility = false;
    private void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();


    }
    private void Update()
    {
        if (transform.childCount <= 0)
        {
            inventory.isFull[index] = false;
        }
        if (itemCount == 1)
        {
            itemCountText.text = string.Empty;
        }
    }
    public void AddItem(GameObject itemPrefab)
    {
        this.itemPrefab = itemPrefab;
        itemCount = 1;
        //itemCountText.text = itemCount.ToString();
        // �������� ������� � ���������� ��� ������� � ��������
        newItem = Instantiate(itemPrefab, transform);
        newItem.transform.localPosition = Vector3.zero;
        newItem.transform.SetParent(transform);

        // ����� ������ �������� � ��������� ����� OnItemClicked � ������� �������
        Button itemButton = newItem.GetComponent<Button>();
        itemButton.onClick.AddListener(OnItemClicked);


    }
    public bool IsEmpty()
    {
        return itemPrefab == null;
    }

    public GameObject GetItemPrefab()
    {
        return itemPrefab;
    }

    public void IncreaseItemCount()
    {
        itemCount++;
        itemCountText.text = itemCount.ToString();
    }
    public void DecreaseItemCount()
    {
        if (itemCount > 0)
        {
            itemCount--;
            itemCountText.text = itemCount.ToString();
        }
    }
    public void DropItem()
    {

        // ���������, ���� �� �������� � ���� �����
        if (!IsEmpty())
        {
            // ��������� ���������� ���������
            DecreaseItemCount();

            newItem.GetComponent<Spawn>().SpawnDroppedItem();
            //deleteButton.gameObject.SetActive(false);


            // ���������� �������� ������ �������� �� �����
            // ��������, ������ ���������� ������ ���� ��������� ��������
            if (itemCount == 0)
            {
                Destroy(transform.GetChild(0).gameObject);
                itemPrefab = null;
                itemCountText.text = string.Empty;
                inventory.isFull[index] = false;
                deleteButton.gameObject.SetActive(false);
                deleteButtonVisibility = false;
            }
        }
    }
    public void SetDeleteButtonVisibility(bool visible)
    {
        deleteButton.gameObject.SetActive(visible);
    }
    private void OnItemClicked()
    {
        // �������� �����, ������� ��������� ���������� ������ ��������
        if (!deleteButtonVisibility)
        {
            inventory.SetDeleteButtonVisibility(index, true);
            deleteButtonVisibility = true;
        }
        else
        {
            inventory.SetDeleteButtonVisibility(index, false);
            deleteButtonVisibility = false;
        }
    }
    public void AddItemByName(string itemName)
    {
        // 1. ������� ��������� �������� �� ������ ��� ����� (��������������, ��� �������� �������� � ��������)
        GameObject itemPrefab2 = Resources.Load<GameObject>("Prefabs/" + itemName);

        if (itemPrefab2 == null)
        {
            Debug.LogError("Item with name " + itemName + " not found.");
            return;
        }

        // 2. ������� ��������� �������� � ���������� ��� ��� �������� ������ �����
        GameObject newItem2 = Instantiate(itemPrefab2, transform);
        newItem2.transform.localPosition = Vector3.zero;
        newItem2.transform.SetParent(transform);

        // 3. �������� ��������� ���� (���� ����), ����� ���������� ���������� ���������
        newItem = newItem2;
        itemCount++;
        itemCountText.text = itemCount.ToString();
        this.itemPrefab = itemPrefab2;
        Button itemButton = newItem.GetComponent<Button>();
        itemButton.onClick.AddListener(OnItemClicked);
    }
}
