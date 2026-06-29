using UnityEngine;
using System.Collections.Generic;

/// Oyun başladığında 50 bloklu parkur yolunu otomatik oluşturur.
public class PlatformGenerator : MonoBehaviour
{
    [Header("Platform Prefab — Inspector'da atayın")]
    public GameObject platformPrefab;       // basit Cube prefab
    public GameObject checkpointPrefab;     // yeşil/sarı işaretli Cube prefab (isteğe bağlı)
    public GameObject finishPrefab;         // bitiş platformu (isteğe bağlı)

    [Header("Ayarlar")]
    public int totalPlatforms = 50;
    public int checkpointEvery = 10;

    [Header("Rastgele Offset Limitleri")]
    public float minX = -2f;
    public float maxX = 2f;
    public float minY = -0.5f;
    public float maxY = 1.5f;
    public float minZ = 2f;
    public float maxZ = 4f;

    readonly List<Vector3> checkpointPositions = new();

    void Awake()
    {
        GeneratePlatforms();
    }

    void GeneratePlatforms()
    {
        Vector3 current = Vector3.zero;

        for (int i = 1; i <= totalPlatforms; i++)
        {
            bool isCheckpoint = (i % checkpointEvery == 0);
            bool isFinish     = (i == totalPlatforms);

            // Offset hesapla
            float ox = Random.Range(minX, maxX);
            float oy = Random.Range(minY, maxY);
            float oz = Random.Range(minZ, maxZ);
            current += new Vector3(ox, oy, oz);

            // Hangi prefabı kullan
            GameObject prefab = platformPrefab;
            if (isFinish && finishPrefab)        prefab = finishPrefab;
            else if (isCheckpoint && checkpointPrefab) prefab = checkpointPrefab;

            GameObject go = Instantiate(prefab, current, Quaternion.identity, transform);
            go.SetActive(true);
            go.name = isFinish ? "Finish" : isCheckpoint ? $"Checkpoint_{i / checkpointEvery}" : $"Platform_{i}";

            // Platform boyutu: 2x0.5x2
            go.transform.localScale = new Vector3(2f, 0.5f, 2f);

            if (isCheckpoint && !isFinish)
            {
                checkpointPositions.Add(current);
                int cpIndex = i / checkpointEvery;
                CheckpointTrigger ct = go.AddComponent<CheckpointTrigger>();
                ct.checkpointIndex = cpIndex;
                ct.checkpointPosition = current;
            }

            if (isFinish)
            {
                FinishTrigger ft = go.AddComponent<FinishTrigger>();
            }
        }

        // Başlangıç platformu (zemin)
        GameObject start = Instantiate(platformPrefab, Vector3.zero, Quaternion.identity, transform);
        start.SetActive(true);
        start.name = "StartPlatform";
        start.transform.localScale = new Vector3(4f, 0.5f, 4f);
        start.transform.position = new Vector3(0f, -0.25f, 0f);
    }
}
