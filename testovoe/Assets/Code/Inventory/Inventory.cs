using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Networking;
public class Inventory : MonoBehaviour
{
    //public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    public bool[] isFull;
    public GameObject[] slots;
    public GameObject inventory;
    public GameObject deleteButton;
    private bool inventoryOn;
    public string inventoryFilePath;
    [System.Serializable]
    public class SlotData
    {
        public int slotIndex;
        public string itemName;
        public int itemCount;
    }

    [System.Serializable]
    public class InventoryItem
    {
        public List<SlotData> slotsData;  // ���������� � ������
    }
    private void Start()
    {
        inventoryOn = false;
        SetDeleteButtonsVisibility(false);
        inventory.SetActive(false);
        //InventorySaveLoadManager.LoadInventoryFromJson(this, "inventoryData.json");
        LoadInventory();
    }
    public void Update()
    {
        SaveInventory();
        //InventorySaveLoadManager.SaveInventoryToJson(this, "inventoryData.json");
    }
    public void Chest()
    {
        if (!inventoryOn)
        {
            inventoryOn = true;
            inventory.SetActive(true);
        }
        else
        {
            inventoryOn = false;
            inventory.SetActive(false);
        }
    }
    public void SetDeleteButtonsVisibility(bool visible)
    {
        foreach (GameObject slot in slots)
        {
            // ������� ������ �������� � ������� ����� � ���������� ���������
            Button deleteButton = slot.GetComponent<Slot>().deleteButton;
            if (deleteButton != null)
            {
                deleteButton.gameObject.SetActive(visible);
            }
        }
    }
    public void SetDeleteButtonVisibility(int slotIndex, bool visible)
    {
        GameObject slot = slots[slotIndex];
        Button deleteButton = slot.GetComponent<Slot>().deleteButton;
        if (deleteButton != null)
        {
            deleteButton.gameObject.SetActive(visible);
        }
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }

    private void Awake()
    {

    }
    public void SaveInventory()
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.slotsData = new List<SlotData>();

        for (int i = 0; i < slots.Length; i++)
        {
            Slot slot = slots[i].GetComponent<Slot>();

            if (slot != null)
            {
                // ���������, ��� � ����� ���� ������� � ��������� ����
                GameObject itemPrefab = slot.GetItemPrefab();
                int itemCount = slot.itemCount;

                if (itemPrefab != null)
                {
                    SlotData slotData = new SlotData
                    {
                        slotIndex = i,
                        itemName = itemPrefab.name,
                        itemCount = itemCount
                    };
                    inventoryItem.slotsData.Add(slotData);
                }
            }
        }

        // ���������� ���� � ����� � ����� StreamingAssets
        string savePath = Path.Combine(Application.dataPath, "StreamingAssets/inventoryData.json");

        // ������������ ������ � JSON � ��������� �� � ����
        string jsonData = JsonUtility.ToJson(inventoryItem);
        System.IO.File.WriteAllText(savePath, jsonData);
    }
    public void LoadInventory()
    {
        PlayerData savedPlayerData = SaveLoadManager.LoadPlayerData();
       
#if UNITY_EDITOR
            if (savedPlayerData.isdead == false)
            {
                inventoryFilePath = Path.Combine(Application.streamingAssetsPath, "inventoryData.json");
            }
            else
            {
                inventoryFilePath = Path.Combine(Application.streamingAssetsPath, "inventoryEmpty.json");
            }

#else
            if (savedPlayerData.isdead == false)
            {
                inventoryFilePath = Path.Combine(Application.streamingAssetsPath, "inventoryData.json");
            }
            else
            {
                inventoryFilePath = Path.Combine(Application.streamingAssetsPath, "inventoryEmpty.json");
            }
#endif

            // ���������, ��� ���� ��������� � StreamingAssets, �, ���� ���, ���������� ���
            if (inventoryFilePath.Contains("://"))
            {
                // ��� ������ � StreamingAssets �� Android ����� ������������ WWW ��� ������
                WWW www = new WWW(inventoryFilePath);
                while (!www.isDone) { } // ���� ���������� ��������
                string jsonData = www.text;
                ProcessInventoryData(jsonData);
            }
            else
            {
                // ��� ������ � StreamingAssets �� ������ ���������� ����������� ������� ������
                if (System.IO.File.Exists(inventoryFilePath))
                {
                    string jsonData = System.IO.File.ReadAllText(inventoryFilePath);
                    ProcessInventoryData(jsonData);
                }
            }
        
    }

    private void ProcessInventoryData(string jsonData)
    {
        if (!string.IsNullOrEmpty(jsonData))
        {
            InventoryItem inventoryItem = JsonUtility.FromJson<InventoryItem>(jsonData);
            if (inventoryItem != null && inventoryItem.slotsData != null)
            {
                foreach (var slotData in inventoryItem.slotsData)
                {
                    int slotIndex = slotData.slotIndex;
                    if (slotIndex >= 0 && slotIndex < slots.Length)
                    {
                        Slot slot = slots[slotIndex].GetComponent<Slot>();
                        if (slot != null)
                        {
                            GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + slotData.itemName);
                            if (itemPrefab != null)
                            {
                                slot.AddItemByName(slotData.itemName);
                                slot.itemCount = slotData.itemCount;
                                isFull[slotIndex] = true;
                                slot.itemCountText.text = slot.itemCount.ToString();
                            }
                        }
                    }
                }
            }
        }
    }

    /* public void LoadInventory()
     {
         PlayerData savedPlayerData = SaveLoadManager.LoadPlayerData();
         if (savedPlayerData.isdead == false) { 
         if (System.IO.File.Exists("inventoryData.json"))
         {
             string jsonData = System.IO.File.ReadAllText("inventoryData.json");
             InventoryItem inventoryItem = JsonUtility.FromJson<InventoryItem>(jsonData);
             if (inventoryItem != null && inventoryItem.slotsData != null)
             {
                 foreach (var slotData in inventoryItem.slotsData)
                 {
                     int slotIndex = slotData.slotIndex;
                     if (slotIndex >= 0 && slotIndex < slots.Length)
                     {
                         Slot slot = slots[slotIndex].GetComponent<Slot>();
                         if (slot != null)
                         {

                             GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + slotData.itemName);
                             {
                                 slot.AddItemByName(slotData.itemName);
                                 slot.itemCount = slotData.itemCount;
                                 isFull[slotIndex] = true;
                                 slot.itemCountText.text = slot.itemCount.ToString();
                             }
                         }
                     }
                 }
             }
         }
         }
         else
         {
             if (System.IO.File.Exists("inventoryEmpty.json"))
             {
                 string jsonData = System.IO.File.ReadAllText("inventoryEmpty.json");
                 InventoryItem inventoryItem = JsonUtility.FromJson<InventoryItem>(jsonData);
                 if (inventoryItem != null && inventoryItem.slotsData != null)
                 {
                     foreach (var slotData in inventoryItem.slotsData)
                     {
                         int slotIndex = slotData.slotIndex;
                         if (slotIndex >= 0 && slotIndex < slots.Length)
                         {
                             Slot slot = slots[slotIndex].GetComponent<Slot>();
                             if (slot != null)
                             {

                                 GameObject itemPrefab = Resources.Load<GameObject>("Prefabs/" + slotData.itemName);
                                 {
                                     slot.AddItemByName(slotData.itemName);
                                     slot.itemCount = slotData.itemCount;
                                     isFull[slotIndex] = true;
                                     slot.itemCountText.text = slot.itemCount.ToString();
                                 }
                             }
                         }
                     }
                 }
             }
         }
     }*/

}