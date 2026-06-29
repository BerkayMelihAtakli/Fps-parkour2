using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    bool triggered;

    void Start()
    {
        var zone = new GameObject("FinishZone");
        zone.transform.SetParent(transform);
        zone.transform.localPosition = Vector3.up * 1.5f;
        var bc = zone.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = new Vector3(1f, 2f, 1f);
        zone.AddComponent<TriggerZone>().onEnter = col =>
        {
            if (triggered || !col.CompareTag("Player")) return;
            triggered = true;
            GameManager.Instance?.TriggerWin();
        };
    }
}
