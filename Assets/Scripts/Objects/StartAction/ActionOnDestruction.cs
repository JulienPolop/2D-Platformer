using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class ActionOnDestruction : MonoBehaviour
{

    public UnityEvent methods;

    private void OnDestroy()
    {
        methods.Invoke();
    }


}
