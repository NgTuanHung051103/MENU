using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : EquipmentSlot
{
    protected override void Awake()
    {
        base.Awake();
        _dropArea._dropConditions.Add(new IsWeaponCondition());
    }
}
