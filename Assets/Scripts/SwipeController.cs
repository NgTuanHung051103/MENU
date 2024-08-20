using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] int _maxPage;
    int _currentPage;
    Vector3 _targetPosition;
    [SerializeField] Vector3 _pageStep;
    [SerializeField] RectTransform _levelPagesRect;
    [SerializeField] float _tweenTime;
    [SerializeField] LeanTweenType _tweenType;
    float _dragThreshould;
    private void Awake()
    {
        _currentPage = 2;
        _targetPosition = _levelPagesRect.localPosition;
        _dragThreshould = Screen.width / 15;
    }

    public void Next()
    {
        if (_currentPage < _maxPage)
        {
            _currentPage++;
            _targetPosition += _pageStep;
            MovePage();
        }
    }
    public void Previous()
    {
        if (_currentPage > 1)
        {
            _currentPage--; ;
            _targetPosition -= _pageStep;
            MovePage();
        }
    }
    void MovePage()
    {
        _levelPagesRect.LeanMoveLocal(_targetPosition, _tweenTime).setEase(_tweenType);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > _dragThreshould)
        {
            if (eventData.position.x > eventData.pressPosition.x)
            {
                Previous();
            }
            else
            {
                Next();
            }
        }
        else
        {
            MovePage();
        }
    }
}