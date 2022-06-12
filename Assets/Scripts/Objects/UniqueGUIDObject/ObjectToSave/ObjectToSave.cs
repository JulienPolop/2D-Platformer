using UnityEngine;
using System.Collections;
using Assets.Scripts;

public abstract class ObjectToSave : UniqueGuidObject
{
    abstract public void LoadFromSaveObject(SaveObjectBase saveObject);
    abstract public SaveObjectBase GetSaveObject();
}
