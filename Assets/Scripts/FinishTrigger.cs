using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    bool triggered;

    void Start()
    {
        GameObject zone = new GameObject("FinishZone");
        zone.transform.SetParent(transform);
        zone.transform.localPosition = Vector3.up * 1.5f;
        BoxCollider bc = zone.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = new Vector3(1f, 2f, 1f);
        TriggerZone tz = zone.AddComponent<TriggerZone>();
        tz.onEnter = OnZoneEnter;
    }

    void OnZoneEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;
        triggered = true;
        GameManager.Instance?.TriggerWin();
    }
}
