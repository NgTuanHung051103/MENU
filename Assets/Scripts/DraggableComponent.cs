using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using ExtensionMethods;

public class DraggableComponent : MonoBehaviour, IInitializePotentialDragHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<PointerEventData> OnBeginDragHandler;
    public event Action<PointerEventData> OnDragHandler;
    public event Action<PointerEventData, bool> OnEndDragHandler;

    public bool _followCursor { get; set; } = true;
    public Vector3 _startPosition;
    public bool _canDrag { get; set; } = true;
    private RectTransform _rectTransform;
    private Canvas _canvas;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_canDrag)
        {
            return;
        }
        OnBeginDragHandler?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_canDrag)
        {
            return;
        }
        transform.position = Input.mousePosition;
        OnDragHandler?.Invoke(eventData);

        if (_followCursor)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_canDrag)
        {
            return;
        }

        var results = new List<RaycastResult>();

        //result là gameobject hướng tới
        EventSystem.current.RaycastAll(eventData, results);

        GameObject dropAreaObject = null;
        DropArea dropArea = null;
        foreach (var result in results)
        {
            dropAreaObject = result.gameObject;
            dropArea = dropAreaObject.GetComponent<DropArea>();

            if (dropArea != null)
            {
                break;
            }
        }

        if (dropArea != null && dropAreaObject != null && dropArea.Accepts(this))
        {
            if(dropArea.Accepts(this))
            {
                dropArea.Drop(this);
                OnEndDragHandler?.Invoke(eventData, true);

                // Xử lý disable list trên
                if(ItemController.Instance.CheckEquipedItem(dropAreaObject.gameObject.name))
                {
                    ItemController.Instance.EquipSelectedItem(transform.gameObject.name);
                }
                return;
            }
        }

        var equipedItem = ItemController.Instance._listEquipedItem?
            .FirstOrDefault(p => p.GetChildItem() != null && p.GetChildItem().name == this.name);

        // Dành riêng cho phần active lại box va trả lại sản phẩm về list trên
        if (equipedItem != null)
        {
            FakeInventoryController.Instance.InitializeItems(this.name);
            ItemController.Instance.RemoveEquipedItem(this.name);
            return;
        }

        _rectTransform.anchoredPosition = _startPosition;
        OnEndDragHandler?.Invoke(eventData, false);
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        _startPosition = _rectTransform.anchoredPosition;
    }
}