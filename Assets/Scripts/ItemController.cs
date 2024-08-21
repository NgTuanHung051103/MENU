using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance { get; set; }

    //list item ở dưới
    public List<GameObject> _listEquipedItem;

    // list item ở trên
    [SerializeField] public List<SelectedItemRecord> _listSelectedItem;

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

        // Khởi tạo danh sách nếu chưa được khởi tạo
        if (_listSelectedItem == null)
        {
            _listSelectedItem = new List<SelectedItemRecord>();
        }

        if (_listEquipedItem == null)
        {
            _listEquipedItem = new List<GameObject>();
        }
    }

    #region Select Item
    private void AddItem(SelectedItemRecord item)
    {
        _listSelectedItem.Add(item);
    }

    /// <summary>
    /// Xóa item (image), ở cả ô trên và ô dưới ( thực tế chỉ tồn tại 1 trong 2 ô )
    /// </summary>
    /// <param name="nameObject"></param>
    private void RemoveItem(string nameObject)
    {
        // Remove if this item was equiped
        RemoveEquipedItem(nameObject);

        RemoveSelectedItem(nameObject);
    }

    /// <summary>
    /// Xóa gameobject cũ đã từng chọn
    /// </summary>
    /// <param name="nameObject">Tên object mới chọn</param>
    /// <param name="oldNameObject">Tên object đã chọn trước đó</param>
    public void UpdateEquipedItem(string nameObject, string oldNameObject)
    {
        RemoveItem(oldNameObject);
        AddItem(new SelectedItemRecord(nameObject, null));
        FakeInventoryController.Instance.InitializeItems();
    }

    private bool CheckExistItem(string nameObject)
    {
        return _listSelectedItem != null && _listSelectedItem.Any(p => p.Name == nameObject);
    }

    public List<SelectedItemRecord> GetSelectedItemList()
    {
        return _listSelectedItem;
    }
    #endregion

    #region Equip item
    /// <summary>
    /// Check the action is equip or remove
    ///  if equiped item = true
    ///  if remove item = false
    /// </summary>
    public bool CheckEquipedItem(string nameObject)
    {
        return _listEquipedItem.Any( p => p.name == nameObject);
    }

    /// <summary>
    /// Kiểm tra item có ở trong SelectedBox ko thông qua tên item ở bên trong SelectedBox
    /// </summary>
    public bool CheckExistInEquipedItem(string nameObject)
    {
        return _listEquipedItem.Any(p => p.GetChildItem() != null && p.GetChildItem().name == nameObject);
    }

    /// <summary>
    /// Change status of selected box when equip or remove item
    /// </summary>
    /// <param name="nameObject">name of selected box need to change status</param>
    /// <param name="isDisplay">Status of selected box</param>
    public void ChangeActiveSelectedBox(string nameObject, bool isDisplay)
    {
        GameObject disabledBox = _listSelectedItem
            .Where(p => p.Name == nameObject)
            .Select(p => p.NewObject)
            .FirstOrDefault();

        if (disabledBox != null)
        {
            disabledBox.SetActive(isDisplay);
        }
    }

    public void RemoveDisableBox(GameObject equipedBox)
    {
        equipedBox.GetComponent<DropArea>()._dropConditions.RemoveAll(p => p is DisableDropCondition);
    }

    public void RemoveEquipedItem(string equipedItemName)
    {
        var equipedItem = _listEquipedItem?
            .FirstOrDefault(p => p.GetChildItem() != null
                && p.GetChildItem().name == equipedItemName
            );

        if (equipedItem != null)
        {
            RemoveDisableBox(equipedItem);
            Destroy(equipedItem.GetChildItem());
        }
    }

    public void RemoveSelectedItem(string selectedItemName)
    {
        var selectedItem = _listSelectedItem?.FirstOrDefault(p => p.Name == selectedItemName);
        if (selectedItem != null)
        {
            _listSelectedItem.Remove(selectedItem);
            FakeInventoryController.Instance.RemoveSelectedItem(selectedItemName);
        }
    }

    /// <summary>
    /// xóa item ở trên tại hierarchy
    /// </summary>
    public void EquipSelectedItem(string selectedItemName)
    {
        var selectedItem = _listSelectedItem?.FirstOrDefault(p => p.Name == selectedItemName);
        if (selectedItem != null)
        {
            FakeInventoryController.Instance.RemoveSelectedItem(selectedItemName);
        }
    }

    #endregion
}