using UnityEngine;
using System;

public class TriggerZone : MonoBehaviour
{
    public Action<Collider> onEnter;

    void OnTriggerEnter(Collider other)
    {
        onEnter?.Invoke(other);
    }
}
