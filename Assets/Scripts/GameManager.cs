using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController player;
    public Vector3 startSpawnPos = new Vector3(0f, 2f, 0f);

    int currentCheckpoint;
    Vector3 respawnPos;
    bool isGameOver;
    bool showDeath;
    bool showRespawnChoice;
    float deathTimer;
    float elapsedTime;
    float finishTime;

    public bool IsGameOver => isGameOver;

    // GUI stilleri — Awake'de oluştur
    GUIStyle styleHUD;
    GUIStyle styleDeath;
    GUIStyle styleWinBox;
    GUIStyle styleWinText;
    GUIStyle styleCross;
    bool stylesReady;

    void Awake()
    {
        Instance = this;
        respawnPos = startSpawnPos;
    }

    void Update()
    {
        if (!isGameOver && !showRespawnChoice)
            elapsedTime += Time.deltaTime;

        if (showDeath)
        {
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0f) showDeath = false;
        }
    }

    public void ActivateCheckpoint(int index, Vector3 pos)
    {
        if (index <= currentCheckpoint) return;
        currentCheckpoint = index;
        respawnPos = pos + Vector3.up * 2f;
    }

    public void RespawnPlayer()
    {
        if (isGameOver) return;
        if (currentCheckpoint > 0)
        {
            // Has a checkpoint — ask the player
            showRespawnChoice = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // No checkpoint yet — respawn at start silently
            player.TeleportTo(startSpawnPos);
            showDeath = true;
            deathTimer = 1f;
        }
    }

    void ContinueFromCheckpoint()
    {
        showRespawnChoice = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.TeleportTo(respawnPos);
        showDeath = true;
        deathTimer = 1f;
    }

    void RestartFromBeginning()
    {
        showRespawnChoice = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentCheckpoint = 0;
        respawnPos = startSpawnPos;
        elapsedTime = 0f;
        player.TeleportTo(startSpawnPos);
        showDeath = true;
        deathTimer = 1f;
    }

    public void TriggerWin()
    {
        if (isGameOver) return;
        isGameOver = true;
        finishTime = elapsedTime;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BuildStyles()
    {
        if (stylesReady) return;
        stylesReady = true;

        styleHUD = new GUIStyle(GUI.skin.label)
        {
            fontSize = 26,
            fontStyle = FontStyle.Bold
        };
        styleHUD.normal.textColor = Color.white;

        styleCross = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        styleCross.normal.textColor = Color.white;

        styleDeath = new GUIStyle(GUI.skin.box)
        {
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        styleDeath.normal.textColor = Color.red;

        styleWinBox = new GUIStyle(GUI.skin.box);
        styleWinBox.normal.background = MakeTex(2, 2, new Color(0f, 0.15f, 0f, 0.92f));

        styleWinText = new GUIStyle(GUI.skin.label)
        {
            fontSize = 42,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            wordWrap = true
        };
        styleWinText.normal.textColor = new Color(0.2f, 1f, 0.4f);
    }

    void OnGUI()
    {
        BuildStyles();

        float sw = Screen.width;
        float sh = Screen.height;

        // Timer
        string timeStr = FormatTime(isGameOver ? finishTime : elapsedTime);
        GUI.Label(new Rect(20, 20, 350, 40), $"Checkpoint: {currentCheckpoint} / 5", styleHUD);
        GUI.Label(new Rect(20, 55, 350, 40), $"Time: {timeStr}", styleHUD);

        // Crosshair
        GUI.Label(new Rect(sw / 2f - 20, sh / 2f - 20, 40, 40), "+", styleCross);

        // Fall message
        if (showDeath)
        {
            GUI.Box(new Rect(sw / 2f - 260, sh / 2f - 45, 520, 90),
                "You fell! Respawning...", styleDeath);
        }

        // Respawn choice dialog
        if (showRespawnChoice)
        {
            float dw = 560, dh = 260;
            float dx = sw / 2f - dw / 2f;
            float dy = sh / 2f - dh / 2f;

            GUIStyle dialogBox = new GUIStyle(GUI.skin.box);
            dialogBox.normal.background = MakeTex(2, 2, new Color(0.1f, 0.1f, 0.1f, 0.95f));
            GUI.Box(new Rect(dx, dy, dw, dh), GUIContent.none, dialogBox);

            GUIStyle title = new GUIStyle(GUI.skin.label)
            {
                fontSize = 30,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            title.normal.textColor = Color.white;
            GUI.Label(new Rect(dx, dy + 20, dw, 50), "You fell!", title);

            GUIStyle sub = new GUIStyle(title) { fontSize = 22, fontStyle = FontStyle.Normal };
            sub.normal.textColor = new Color(0.8f, 0.8f, 0.8f);
            GUI.Label(new Rect(dx, dy + 70, dw, 40), $"Last checkpoint: {currentCheckpoint} / 5", sub);

            if (GUI.Button(new Rect(dx + 30, dy + 140, 230, 70), $"Continue from\nCheckpoint {currentCheckpoint}"))
                ContinueFromCheckpoint();

            if (GUI.Button(new Rect(dx + dw - 260, dy + 140, 230, 70), "Restart\nfrom Beginning"))
                RestartFromBeginning();
        }

        // Win screen
        if (isGameOver)
        {
            float bw = 600, bh = 320;
            float bx = sw / 2f - bw / 2f;
            float by = sh / 2f - bh / 2f;

            GUI.Box(new Rect(bx, by, bw, bh), GUIContent.none, styleWinBox);
            GUI.Label(new Rect(bx, by + 30, bw, 100),
                "CONGRATULATIONS!\nCourse Complete!", styleWinText);
            GUI.Label(new Rect(bx, by + 150, bw, 60),
                $"Your time: {FormatTime(finishTime)}", new GUIStyle(styleWinText) { fontSize = 32 });

            if (GUI.Button(new Rect(bx + bw / 2f - 120, by + 230, 240, 60), "Play Again"))
                RestartGame();
        }
    }

    static string FormatTime(float t)
    {
        int m = (int)(t / 60);
        int s = (int)(t % 60);
        int ms = (int)((t * 100) % 100);
        return m > 0 ? $"{m}m {s:00}.{ms:00}s" : $"{s}.{ms:00}s";
    }

    static Texture2D MakeTex(int w, int h, Color col)
    {
        Color[] pixels = new Color[w * h];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = col;
        Texture2D tex = new Texture2D(w, h);
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }
}
