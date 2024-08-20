using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropArea : MonoBehaviour
{
    public List<DropCondition> _dropConditions = new List<DropCondition>();
    public event Action<DraggableComponent> _onDropHandler;

    public bool Accepts(DraggableComponent draggable)
    {
        return _dropConditions.TrueForAll(condition => condition.Check(draggable));
    }
    public void Drop(DraggableComponent draggable)
    {
        _onDropHandler?.Invoke(draggable);
    }
}