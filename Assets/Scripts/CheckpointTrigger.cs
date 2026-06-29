using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int checkpointIndex;
    public Vector3 checkpointPosition;
    bool activated;

    void Start()
    {
        var zone = new GameObject("CPZone");
        zone.transform.SetParent(transform);
        zone.transform.localPosition = Vector3.up * 1.5f;
        var bc = zone.AddComponent<BoxCollider>();
        bc.isTrigger = true;
        bc.size = new Vector3(1f, 2f, 1f);
        zone.AddComponent<TriggerZone>().onEnter = col =>
        {
            if (activated || !col.CompareTag("Player")) return;
            activated = true;
            GameManager.Instance?.ActivateCheckpoint(checkpointIndex, checkpointPosition);
            GetComponent<Renderer>().material.color = Color.green;
        };
    }
}
