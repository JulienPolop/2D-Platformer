using UnityEngine;
using System.Collections;
using System;

public class UniqueGuidObject : MonoBehaviour
{
    public string GuidString;

    protected void OnValidate()
    {
        if (GuidString == null || GuidString == Guid.Empty.ToString() || GuidString == String.Empty)
        {
            GuidString = Guid.NewGuid().ToString();
        }
    }
}
