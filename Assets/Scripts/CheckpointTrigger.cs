using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;
    public Vector3 checkpointPosition;

    bool activated;

    void Start()
    {
        // Platform üstüne trigger zone ekle
        GameObject zone = new GameObject("CPZone");
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
        if (activated) return;
        if (!other.CompareTag("Player")) return;
        activated = true;
        GameManager.Instance?.ActivateCheckpoint(checkpointIndex, checkpointPosition);
        GetComponent<Renderer>().material.color = Color.green;
    }
}
