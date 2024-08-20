using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ExtensionMethods;

public class PickableComponent : MonoBehaviour, ISelectHandler
{
    public GameObject targetContainer;
    private string _oldName = string.Empty;
    private Dictionary<string, GameObject> existingCopies = new Dictionary<string, GameObject>();
    public void OnSelect(BaseEventData eventData)
    {
        if (targetContainer == null)
        {
            return;
        }

        GameObject oldObject = targetContainer.GetChildItem();

        if (oldObject != null)
        {
            _oldName = oldObject.name;

            DestroyAllChildren(targetContainer);
        }

        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        
        // Sao chép đối tượng
        GameObject copiedObject = Instantiate(selectedObject, targetContainer.transform);

        copiedObject.name = selectedObject.GetComponent<Image>().sprite.name;
        Destroy(copiedObject.GetComponent<Button>());
        // Sao chép các thuộc tính RectTransform
        RectTransform selectedRect = selectedObject.GetComponent<RectTransform>();
        RectTransform copiedRect = copiedObject.GetComponent<RectTransform>();

        // Sao chép các thuộc tính RectTransform
        copiedRect.sizeDelta = selectedRect.sizeDelta;
        copiedRect.anchoredPosition = selectedRect.anchoredPosition;
        copiedRect.pivot = selectedRect.pivot;
        copiedRect.anchorMin = selectedRect.anchorMin;
        copiedRect.anchorMax = selectedRect.anchorMax;
        copiedRect.localRotation = selectedRect.localRotation;
        copiedRect.localScale = new Vector2(1f,1f);

        ItemController.Instance.UpdateEquipedItem(copiedObject, _oldName);
    }

    void DestroyAllChildren(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}