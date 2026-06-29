using UnityEngine;
using UnityEditor;

public class SceneBuilder
{
    [MenuItem("Tools/Build FPS Parkour Scene")]
    static void Build()
    {
        foreach (var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            if (go.transform.parent == null)
                Object.DestroyImmediate(go);

        // Siyah ortam
        RenderSettings.skybox = null;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.15f, 0.15f, 0.15f);
        RenderSettings.fog = false;

        // Işık
        GameObject lightGo = new GameObject("Directional Light");
        Light light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Platform prefabları
        GameObject platformPrefab = MakeCube("PlatformPrefab", new Color(0.4f, 0.6f, 1f));
        GameObject cpPrefab       = MakeCube("CheckpointPrefab", new Color(1f, 0.85f, 0f));
        GameObject finPrefab      = MakeCube("FinishPrefab", new Color(0f, 1f, 0.4f));

        // Platform Generator
        GameObject genGo = new GameObject("PlatformGenerator");
        PlatformGenerator gen = genGo.AddComponent<PlatformGenerator>();
        gen.platformPrefab   = platformPrefab;
        gen.checkpointPrefab = cpPrefab;
        gen.finishPrefab     = finPrefab;

        // Player
        GameObject playerGo = new GameObject("Player");
        // Tag'i güvenli şekilde set et
        try { playerGo.tag = "Player"; } catch { }
        playerGo.transform.position = new Vector3(0f, 2f, 0f);

        CharacterController cc = playerGo.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.center = new Vector3(0f, 0.9f, 0f);

        // Camera Holder
        GameObject camHolder = new GameObject("CameraHolder");
        camHolder.transform.SetParent(playerGo.transform, false);
        camHolder.transform.localPosition = new Vector3(0f, 1.6f, 0f);

        // Ana Kamera
        GameObject camGo = new GameObject("Main Camera");
        camGo.transform.SetParent(camHolder.transform, false);
        camGo.transform.localPosition = Vector3.zero;
        camGo.transform.localRotation = Quaternion.identity;
        try { camGo.tag = "MainCamera"; } catch { }

        Camera cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.nearClipPlane = 0.1f;
        cam.farClipPlane = 500f;
        camGo.AddComponent<AudioListener>();

        // PlayerController — en sona ekle
        PlayerController pc = playerGo.AddComponent<PlayerController>();
        pc.cameraHolder = camHolder.transform;

        // Void düzlemi (görsel, collider yok)
        GameObject voidPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        voidPlane.name = "VoidPlane";
        voidPlane.transform.position = new Vector3(0f, -9f, 0f);
        voidPlane.transform.localScale = new Vector3(200f, 1f, 200f);
        Object.DestroyImmediate(voidPlane.GetComponent<Collider>());
        Material voidMat = new Material(Shader.Find("Standard"));
        voidMat.color = Color.black;
        voidMat.SetFloat("_Glossiness", 0f);
        voidPlane.GetComponent<Renderer>().sharedMaterial = voidMat;

        // Game Manager
        GameObject gmGo = new GameObject("GameManager");
        GameManager gm = gmGo.AddComponent<GameManager>();
        gm.player        = pc;
        gm.startSpawnPos = new Vector3(0f, 2f, 0f);

        Debug.Log("Sahne hazır! Play tuşuna bas.");
    }

    static GameObject MakeCube(string name, Color color)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.SetActive(false);
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }
}
