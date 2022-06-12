using UnityEngine;
using System.Collections;
using Assets.Scripts;

abstract public class PickableObject : ObjectToSave
{
    protected bool isDestroyed = false;

    abstract protected void Effect();
    abstract protected void Destroy();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player.gameObject)
        {
            Effect();

            isDestroyed = true;
            Destroy();
        }
    }


    #region Load & save region
    override public void LoadFromSaveObject(SaveObjectBase saveObject)
    {
        Debug.Log("I'm ADD MAX LIFE and i Load:" + saveObject.Activated);
        if (saveObject.Activated)
        {
            Destroy();
            this.isDestroyed = true;
        }
    }

    override public SaveObjectBase GetSaveObject()
    {
        Debug.Log("I'm ADD MAX LIFE and i save:" + this.isDestroyed + " And my GUID is: " + GuidString);
        SaveObjectBase saveObjectBase = new SaveObjectBase()
        {
            Activated = this.isDestroyed,
            GuidString = this.GuidString,
        };
        return saveObjectBase;
    }
    #endregion

}
