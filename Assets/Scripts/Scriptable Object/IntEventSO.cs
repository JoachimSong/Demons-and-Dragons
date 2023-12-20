using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/IntEventSO")]
public class IntEventSO : ScriptableObject
{
    public UnityAction<int> OnEventRaised;
    public void RaiseEvent(int num)
    {
        OnEventRaised?.Invoke(num);
    }
}
