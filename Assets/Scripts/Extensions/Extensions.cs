using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
    public static class Extensions
    {
        public static GameObject GetChildItem(this GameObject parentObject)
        {
            if (parentObject.transform.childCount > 0)
            {
                return parentObject.transform.GetChild(0).gameObject;
            }
            else
            {
                return null;
            }
        }
    }
}
