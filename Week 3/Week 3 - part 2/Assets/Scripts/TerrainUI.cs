using UnityEngine;
using UnityEngine.InputSystem;

// Simple blocky slider panel for the terrain. Tab = hide/show.
public class TerrainUI : MonoBehaviour
{
    public TerrainGenerator terrain;

    bool show = true;
    bool changed;
    bool ready;

    Texture2D panelTex, trackTex, thumbTex, btnTex;
    GUIStyle panelStyle, labelStyle, trackStyle, thumbStyle, btnStyle;

    void Start()
    {
        if (terrain == null) terrain = FindFirstObjectByType<TerrainGenerator>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            show = !show;
    }

    Texture2D Block(Color c)
    {
        Texture2D t = new Texture2D(1, 1);
        t.SetPixel(0, 0, c);
        t.Apply();
        return t;
    }

    void SetupStyles()
    {
        panelTex = Block(new Color(0.1f, 0.1f, 0.1f, 1f));
        trackTex = Block(new Color(0.4f, 0.4f, 0.4f, 1f));
        thumbTex = Block(Color.white);
        btnTex = Block(new Color(0.3f, 0.3f, 0.3f, 1f));

        panelStyle = new GUIStyle(GUI.skin.box);
        panelStyle.normal.background = panelTex;
        panelStyle.padding = new RectOffset(8, 8, 8, 8);

        labelStyle = new GUIStyle(GUI.skin.label);
        labelStyle.normal.textColor = Color.white;

        trackStyle = new GUIStyle(GUI.skin.horizontalSlider);
        trackStyle.normal.background = trackTex;
        trackStyle.fixedHeight = 10;

        thumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb);
        thumbStyle.normal.background = thumbTex;
        thumbStyle.hover.background = thumbTex;
        thumbStyle.active.background = thumbTex;
        thumbStyle.fixedWidth = 10;
        thumbStyle.fixedHeight = 18;

        btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.normal.background = btnTex;
        btnStyle.hover.background = btnTex;
        btnStyle.active.background = thumbTex;
        btnStyle.normal.textColor = Color.white;
        btnStyle.hover.textColor = Color.white;
        btnStyle.active.textColor = Color.black;

        ready = true;
    }

    void OnGUI()
    {
        if (!show || terrain == null) return;
        if (!ready) SetupStyles();

        float s = Mathf.Max(1f, Screen.height / 1080f);
        GUI.matrix = Matrix4x4.Scale(new Vector3(s, s, 1f));

        changed = false;

        GUILayout.BeginArea(new Rect(10, 10, 260, 300), panelStyle);
        GUILayout.Label("TERRAIN  (Tab = hide)", labelStyle);

        terrain.seed = IntSlider("Seed", terrain.seed, 1, 9999);
        terrain.octaves = IntSlider("Octaves", terrain.octaves, 1, 8);
        terrain.persistence = FloatSlider("Persist.", terrain.persistence, 0.1f, 0.9f);
        terrain.lacunarity = FloatSlider("Lacun.", terrain.lacunarity, 1.5f, 3.5f);
        terrain.noiseScale = FloatSlider("Scale", terrain.noiseScale, 1f, 12f);
        terrain.waterLevel = FloatSlider("Water", terrain.waterLevel, 0f, 0.7f);
        terrain.resolution = IntSlider("Res", terrain.resolution, 32, 250);

        GUILayout.Space(6);
        if (GUILayout.Button("NEW SEED", btnStyle))
        {
            terrain.seed = Random.Range(1, 9999);
            changed = true;
        }

        GUILayout.EndArea();

        if (changed) terrain.dirty = true;
    }

    float FloatSlider(string label, float value, float min, float max)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(65));
        float v = GUILayout.HorizontalSlider(value, min, max, trackStyle, thumbStyle);
        GUILayout.Label(v.ToString("0.00"), labelStyle, GUILayout.Width(40));
        GUILayout.EndHorizontal();
        if (!Mathf.Approximately(v, value)) changed = true;
        return v;
    }

    int IntSlider(string label, int value, int min, int max)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, labelStyle, GUILayout.Width(65));
        int v = Mathf.RoundToInt(GUILayout.HorizontalSlider(value, min, max, trackStyle, thumbStyle));
        GUILayout.Label(v.ToString(), labelStyle, GUILayout.Width(40));
        GUILayout.EndHorizontal();
        if (v != value) changed = true;
        return v;
    }
}