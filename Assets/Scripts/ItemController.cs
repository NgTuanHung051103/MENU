using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public static ItemController Instance { get; set; }
    public List<GameObject> listEquipedItem;
    [SerializeField] private List<SelectedItemRecord> _listSelectedItem;

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

        if (listEquipedItem == null)
        {
            listEquipedItem = new List<GameObject>();
        }
    }

    private void AddItem(SelectedItemRecord item)
    {
        _listSelectedItem.Add(item);
    }

    private void RemoveItem(string nameObject)
    {
        // Đảm bảo danh sách được khởi tạo và không null
        var findedItem = _listSelectedItem?.FirstOrDefault(p => p.Name == nameObject);
        if (findedItem != null)
        {
            _listSelectedItem.Remove(findedItem);
            FakeInventoryController.Instance.RemoveSelectedItem(nameObject);
        }
    }

    public void UpdateEquipedItem(GameObject newObject, string oldObjectName)
    {
        if(newObject.name == oldObjectName)
        {
            return;
        }
        RemoveItem(oldObjectName);
        AddItem(new SelectedItemRecord(newObject.name, newObject));
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
}