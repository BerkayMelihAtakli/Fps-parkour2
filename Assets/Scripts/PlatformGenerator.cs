using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject checkpointPrefab;
    public GameObject finishPrefab;

    public int totalPlatforms = 50;
    public int checkpointEvery = 10;

    public float minX = -3f, maxX = 3f;
    public float minY = -0.5f, maxY = 1.5f;
    public float minZ = 4f, maxZ = 7f;

    void Awake()
    {
Vector3 current = Vector3.zero;

        for (int i = 1; i <= totalPlatforms; i++)
        {
            bool isCheckpoint = i % checkpointEvery == 0;
            bool isFinish = i == totalPlatforms;

            current += new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));

            GameObject prefab = isFinish && finishPrefab ? finishPrefab :
                                isCheckpoint && checkpointPrefab ? checkpointPrefab : platformPrefab;

            GameObject go = Instantiate(prefab, current, Quaternion.identity, transform);
            go.SetActive(true);
            go.transform.localScale = new Vector3(2f, 0.5f, 2f);
            go.name = isFinish ? "Finish" : isCheckpoint ? $"Checkpoint_{i / checkpointEvery}" : $"Platform_{i}";

            if (isCheckpoint && !isFinish)
            {
                var ct = go.AddComponent<CheckpointTrigger>();
                ct.checkpointIndex = i / checkpointEvery;
                ct.checkpointPosition = current;
            }

            if (isFinish) go.AddComponent<FinishTrigger>();
        }

        GameObject start = Instantiate(platformPrefab, new Vector3(0f, -0.25f, 0f), Quaternion.identity, transform);
        start.SetActive(true);
        start.name = "StartPlatform";
        start.transform.localScale = new Vector3(4f, 0.5f, 4f);
    }
}
