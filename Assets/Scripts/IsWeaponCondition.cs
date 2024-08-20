using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsWeaponCondition : DropCondition
{
    public override bool Check(DraggableComponent draggable)
    {
        return draggable.GetComponent<Weapon>() != null;
    }
}
