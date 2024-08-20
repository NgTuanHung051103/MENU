using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEditor;
using Common;
//This class purpose is only to initialize the EquipmentSlot objects from the inventory (in a very ugly way :)) ) Do not use this class as a template for your inventory
public class FakeInventoryController : MonoBehaviour
{
    public static FakeInventoryController Instance { get; set; }
    [SerializeField] private GameObject inventoryItemsContainer;
    public GameObject PrefabSelectedItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeItems();
    }

    public void InitializeItems()
    {
        var list = ItemController.Instance.GetSelectedItemList();

        string spritePath = string.Empty;
        Sprite? sprite = null;
        foreach (SelectedItemRecord item in list)
        {
            if(!IsCreateSelectedItem(item.Name))
            {
                GameObject selectedItem = Instantiate(PrefabSelectedItem, inventoryItemsContainer.transform);
                selectedItem.name = item.Name;
                GameObject child = selectedItem.GetChildItem();
                if (child != null)
                {
                    child.name = item.Name;
                    spritePath = Constant.PathImageItem + item.Name + "." + Constant.FileImagePNG;
                    sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    if (sprite != null)
                    {
                        child.GetComponent<UnityEngine.UI.Image>().sprite = sprite;
                    }
                }
            }
        }
    }
    private bool IsCreateSelectedItem(string nameObject)
    {
        foreach(Transform child in inventoryItemsContainer.transform)
        {
            if( nameObject == child.name)
            {
                return true;
            }
        }
        return false;
    }

    public void RemoveSelectedItem(string nameObject)
    {
        foreach (Transform child in inventoryItemsContainer.transform)
        {
            if (nameObject == child.name)
            {
                Destroy(child.gameObject);
                return;
            }
        }
    }
}