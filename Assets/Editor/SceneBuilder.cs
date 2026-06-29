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

        RenderSettings.skybox = null;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.15f, 0.15f, 0.15f);
        RenderSettings.fog = false;

        var light = new GameObject("Directional Light").AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.2f;
        light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        var platformPrefab = MakeCube("PlatformPrefab", new Color(0.4f, 0.6f, 1f));
        var cpPrefab       = MakeCube("CheckpointPrefab", new Color(1f, 0.85f, 0f));
        var finPrefab      = MakeCube("FinishPrefab", new Color(0f, 1f, 0.4f));

        var gen = new GameObject("PlatformGenerator").AddComponent<PlatformGenerator>();
        gen.platformPrefab   = platformPrefab;
        gen.checkpointPrefab = cpPrefab;
        gen.finishPrefab     = finPrefab;

        var playerGo = new GameObject("Player");
        try { playerGo.tag = "Player"; } catch { }
        playerGo.transform.position = new Vector3(0f, 2f, 0f);
        var cc = playerGo.AddComponent<CharacterController>();
        cc.height = 1.8f;
        cc.center = new Vector3(0f, 0.9f, 0f);

        var camHolder = new GameObject("CameraHolder");
        camHolder.transform.SetParent(playerGo.transform, false);
        camHolder.transform.localPosition = new Vector3(0f, 1.6f, 0f);

        var camGo = new GameObject("Main Camera");
        camGo.transform.SetParent(camHolder.transform, false);
        try { camGo.tag = "MainCamera"; } catch { }
        var cam = camGo.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        camGo.AddComponent<AudioListener>();

        var pc = playerGo.AddComponent<PlayerController>();
        pc.cameraHolder = camHolder.transform;

        var voidPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        voidPlane.name = "VoidPlane";
        voidPlane.transform.position = new Vector3(0f, -9f, 0f);
        voidPlane.transform.localScale = new Vector3(200f, 1f, 200f);
        Object.DestroyImmediate(voidPlane.GetComponent<Collider>());
        var voidMat = new Material(Shader.Find("Standard"));
        voidMat.color = Color.black;
        voidMat.SetFloat("_Glossiness", 0f);
        voidPlane.GetComponent<Renderer>().sharedMaterial = voidMat;

        var gm = new GameObject("GameManager").AddComponent<GameManager>();
        gm.player        = pc;
        gm.startSpawnPos = new Vector3(0f, 2f, 0f);

        Debug.Log("Scene ready! Press Play.");
    }

    static GameObject MakeCube(string name, Color color)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.SetActive(false);
        var mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        go.GetComponent<Renderer>().sharedMaterial = mat;
        return go;
    }
}
