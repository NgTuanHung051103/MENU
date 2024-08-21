
using UnityEngine;
using ExtensionMethods;
using UnityEditor;
using Common;
using ExtensionMethods;
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

    public void InitializeItems(string? nameObject = null)
    {
        var list = ItemController.Instance._listSelectedItem;

        string spritePath = string.Empty;
        Sprite? sprite = null;

        //Tạo mới đùng item kéo từ dưới lên
        if(!string.IsNullOrEmpty(nameObject))
        {
            foreach (SelectedItemRecord item in list)
            {
                // chưa tồn tại ở list trên và list dưới ||| ném từ dưới lên trên 
                if (item.Name == nameObject)
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
                    item.ChildNewObject = child;
                    item.NewObject = selectedItem;
                }
            }
            return;
        }
        else
        {
            foreach (SelectedItemRecord item in list)
            {
                // chưa tồn tại ở list trên và list dưới
                if (!IsCreateSelectedItem(item.Name) && !ItemController.Instance.CheckExistInEquipedItem(item.Name))
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
                    item.ChildNewObject = child;
                    item.NewObject = selectedItem;
                }
            }
        }
    }

    /// <summary>
    /// if created item = true
    /// if did not create item = false
    /// </summary>
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

    /// <summary>
    /// Kiểm tra xem có phải trường hợp kéo từ equip lên trên hay không?
    /// </summary>
    /// <param name="nameObject"></param>
    /// <returns></returns>
    private bool IsRemoveEquipedBox(string nameObject)
    {
        foreach (Transform child in inventoryItemsContainer.transform)
        {
            if (child.gameObject.activeSelf && child.gameObject.name == nameObject)
            {
                var childOfChild = child.gameObject.GetChildItem();

                return childOfChild == null;
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