using UnityEngine;
using UnityEngine.InputSystem;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Mesh")]
    [Range(32, 250)] public int resolution = 100;
    [Range(1f, 12f)] public float noiseScale = 4f;
    public float worldSize = 100f;
    [Range(1f, 60f)] public float heightMultiplier = 25f;

    [Header("Noise")]
    [Range(1, 9999)] public int seed = 1337;
    [Range(1, 8)] public int octaves = 5;
    [Range(0.1f, 0.9f)] public float persistence = 0.5f;
    [Range(1.5f, 3.5f)] public float lacunarity = 2f;

    [Header("Height bands (0 = water line, 1 = highest peak)")]
    [Range(0f, 0.7f)] public float waterLevel = 0.38f;
    [Range(0f, 0.5f)] public float sandHeight = 0.10f;
    [Range(0f, 1f)] public float grassHeight = 0.40f;
    [Range(0f, 1f)] public float rockHeight = 0.65f;

    [Header("Colours")]
    public Color deepWater = new Color(0.04f, 0.12f, 0.31f);
    public Color shallowWater = new Color(0.16f, 0.39f, 0.67f);
    public Color sandColour = new Color(0.87f, 0.82f, 0.63f);
    public Color grassColour = new Color(0.31f, 0.59f, 0.27f);
    public Color forestColour = new Color(0.16f, 0.41f, 0.22f);
    public Color rockColour = new Color(0.45f, 0.42f, 0.40f);
    public Color highRockColour = new Color(0.60f, 0.57f, 0.55f);
    public Color snowColour = Color.white;

   [HideInInspector] public bool dirty = true;
    Mesh mesh;
    Texture2D tex;

    void Start()
    {
        Regenerate();
    }

    // Unity calls this whenever you change a value in the Inspector.
    void OnValidate()
    {
        dirty = true;
    }

    void Update()
    {
        // R = new random seed
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            seed = Random.Range(1, 9999);
            dirty = true;
        }

        if (dirty && Application.isPlaying)
        {
            dirty = false;
            Regenerate();
        }
    }

    void Regenerate()
    {
        // Keep the colour bands in order so none of them disappears.
        grassHeight = Mathf.Max(grassHeight, sandHeight);
        rockHeight = Mathf.Max(rockHeight, grassHeight);

        float[,] map = GenerateHeightMap();
        BuildMesh(map);
        ApplyTexture(map);
    }

    float[,] GenerateHeightMap()
    {
        float[,] map = new float[resolution, resolution];

        System.Random rng = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        for (int o = 0; o < octaves; o++)
            offsets[o] = new Vector2(rng.Next(-10000, 10000), rng.Next(-10000, 10000));

        float min = float.MaxValue, max = float.MinValue;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float amplitude = 1f, frequency = 1f, height = 0f, totalAmp = 0f;

                for (int o = 0; o < octaves; o++)
                {
                    float sx = x / (float)resolution * noiseScale * frequency + offsets[o].x;
                    float sy = y / (float)resolution * noiseScale * frequency + offsets[o].y;
                    height += Mathf.PerlinNoise(sx, sy) * amplitude;
                    totalAmp += amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                }

                height /= totalAmp;
                map[x, y] = height;
                if (height < min) min = height;
                if (height > max) max = height;
            }
        }

        for (int y = 0; y < resolution; y++)
            for (int x = 0; x < resolution; x++)
                map[x, y] = Mathf.InverseLerp(min, max, map[x, y]);

        return map;
    }

    void BuildMesh(float[,] heights)
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null) mf = gameObject.AddComponent<MeshFilter>();

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null) mr = gameObject.AddComponent<MeshRenderer>();
        if (mr.sharedMaterial == null || mr.sharedMaterial.name.StartsWith("Default"))
            mr.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        // Reuse the same mesh each time instead of creating a new one.
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mf.sharedMesh = mesh;
        }
        mesh.Clear();

        Vector3[] vertices = new Vector3[resolution * resolution];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int t = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = y * resolution + x;
                float px = (x / (float)(resolution - 1) - 0.5f) * worldSize;
                float pz = (y / (float)(resolution - 1) - 0.5f) * worldSize;

                float h = Mathf.Max(heights[x, y], waterLevel);

                vertices[i] = new Vector3(px, h * heightMultiplier, pz);
                uvs[i] = new Vector2(x / (float)(resolution - 1), y / (float)(resolution - 1));

                if (x < resolution - 1 && y < resolution - 1)
                {
                    triangles[t++] = i;
                    triangles[t++] = i + resolution;
                    triangles[t++] = i + resolution + 1;

                    triangles[t++] = i;
                    triangles[t++] = i + resolution + 1;
                    triangles[t++] = i + 1;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }

    void ApplyTexture(float[,] heights)
    {
        // Reuse the texture unless the resolution changed.
        if (tex == null || tex.width != resolution)
        {
            if (tex != null) Destroy(tex);
            tex = new Texture2D(resolution, resolution);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;
        }

        Color[] pixels = new Color[resolution * resolution];
        for (int y = 0; y < resolution; y++)
            for (int x = 0; x < resolution; x++)
                pixels[y * resolution + x] = ColourFor(heights[x, y]);

        tex.SetPixels(pixels);
        tex.Apply();
        GetComponent<MeshRenderer>().sharedMaterial.mainTexture = tex;
    }

    Color ColourFor(float h)
    {
        // Below the water line: blend deep to shallow water.
        if (h < waterLevel)
            return Color.Lerp(deepWater, shallowWater, h / Mathf.Max(waterLevel, 0.001f));

        // t = how far up the land we are: 0 at the water line, 1 at the highest peak.
        float t = (h - waterLevel) / Mathf.Max(1f - waterLevel, 0.001f);

        if (t < sandHeight)
            return sandColour;

        if (t < grassHeight)
            return Color.Lerp(grassColour, forestColour, Mathf.InverseLerp(sandHeight, grassHeight, t));

        if (t < rockHeight)
            return Color.Lerp(rockColour, highRockColour, Mathf.InverseLerp(grassHeight, rockHeight, t));

        return snowColour;
    }
}