using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;

// Attach this to an empty GameObject and press Play.
// Put your image in Assets/StreamingAssets/ and set the file name below.
public class RedChannelSum : MonoBehaviour
{
    [Header("Image (must be in Assets/StreamingAssets)")]
    public string imageFileName = "test.png";

    [Header("Settings")]
    public int runs = 20;          // how many times each method is timed (average is logged)
    public int chunkSize = 65536;  // pixels per parallel job chunk

    string resultText = "Running...";

    // ---------- Burst single-thread job ----------
    [BurstCompile]
    struct SingleJob : IJob
    {
        [ReadOnly] public NativeArray<Color32> pixels;
        public NativeArray<long> result;

        public void Execute()
        {
            long sum = 0;
            for (int i = 0; i < pixels.Length; i++) sum += pixels[i].r;
            result[0] = sum;
        }
    }

    // ---------- Burst parallel job (each index = one chunk of pixels) ----------
    [BurstCompile]
    struct ParallelJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<Color32> pixels;
        public NativeArray<long> partialSums;
        public int chunkSize;

        public void Execute(int chunk)
        {
            int start = chunk * chunkSize;
            int end = start + chunkSize;
            if (end > pixels.Length) end = pixels.Length;

            long sum = 0;
            for (int i = start; i < end; i++) sum += pixels[i].r;
            partialSums[chunk] = sum;
        }
    }

    void Start()
    {
        // 1. Load image from disk
        string path = Path.Combine(Application.streamingAssetsPath, imageFileName);
        if (!File.Exists(path))
        {
            resultText = "Image not found: " + path;
            Debug.LogError(resultText);
            return;
        }

        Stopwatch loadTimer = Stopwatch.StartNew();
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);
        Color32[] managedPixels = tex.GetPixels32();
        loadTimer.Stop();

        int pixelCount = managedPixels.Length;
        int chunks = (pixelCount + chunkSize - 1) / chunkSize;

        Debug.Log($"Loaded {imageFileName}: {tex.width}x{tex.height} ({pixelCount} pixels) in {loadTimer.Elapsed.TotalMilliseconds:F2} ms");
        Debug.Log($"CPU threads: {SystemInfo.processorCount}, chunks: {chunks}");

        // 2. Copy pixels into native memory so jobs can use them
        NativeArray<Color32> nativePixels = new NativeArray<Color32>(managedPixels, Allocator.TempJob);
        NativeArray<long> singleResult = new NativeArray<long>(1, Allocator.TempJob);
        NativeArray<long> partials = new NativeArray<long>(chunks, Allocator.TempJob);

        // 3. Time each method (first run is a warm-up so Burst compile time isn't counted)
        long sumMain, sumSingle, sumParallel;

        double msMain = Measure(() =>
        {
            long sum = 0;
            for (int i = 0; i < managedPixels.Length; i++) sum += managedPixels[i].r;
            return sum;
        }, out sumMain);

        double msSingle = Measure(() =>
        {
            new SingleJob { pixels = nativePixels, result = singleResult }.Schedule().Complete();
            return singleResult[0];
        }, out sumSingle);

        double msParallel = Measure(() =>
        {
            new ParallelJob { pixels = nativePixels, partialSums = partials, chunkSize = chunkSize }
                .Schedule(chunks, 1).Complete();
            long total = 0;
            for (int i = 0; i < chunks; i++) total += partials[i];
            return total;
        }, out sumParallel);

        nativePixels.Dispose();
        singleResult.Dispose();
        partials.Dispose();

        // 4. Log results
        bool match = sumMain == sumSingle && sumMain == sumParallel;
        double speedup = msMain / msParallel;

        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Image: {tex.width}x{tex.height} ({pixelCount} pixels)");
        sb.AppendLine($"Load time: {loadTimer.Elapsed.TotalMilliseconds:F2} ms");
        sb.AppendLine($"Average of {runs} runs");
        sb.AppendLine();
        sb.AppendLine($"Main thread (no jobs):   {msMain:F3} ms   sum = {sumMain}");
        sb.AppendLine($"Burst single job:        {msSingle:F3} ms   sum = {sumSingle}");
        sb.AppendLine($"Burst parallel job:      {msParallel:F3} ms   sum = {sumParallel}");
        sb.AppendLine();
        sb.AppendLine($"Sums match: {match}");
        sb.AppendLine($"Parallel speedup vs main thread: {speedup:F1}x");
        resultText = sb.ToString();
        Debug.Log(resultText);

        // 5. Export to CSV
        string csvPath = Path.Combine(Application.persistentDataPath, "red_sum_results.csv");
        StringBuilder csv = new StringBuilder();
        csv.AppendLine("method,avg_ms,red_sum,pixels,cpu_threads");
        csv.AppendLine($"main_thread,{msMain:F3},{sumMain},{pixelCount},{SystemInfo.processorCount}");
        csv.AppendLine($"burst_single_job,{msSingle:F3},{sumSingle},{pixelCount},{SystemInfo.processorCount}");
        csv.AppendLine($"burst_parallel_job,{msParallel:F3},{sumParallel},{pixelCount},{SystemInfo.processorCount}");
        File.WriteAllText(csvPath, csv.ToString());
        Debug.Log("CSV saved to: " + csvPath);
    }

    double Measure(Func<long> run, out long sum)
    {
        sum = run(); // warm-up
        Stopwatch sw = Stopwatch.StartNew();
        for (int r = 0; r < runs; r++) run();
        sw.Stop();
        return sw.Elapsed.TotalMilliseconds / runs;
    }

    // Shows results on screen so you can screenshot / record it
    void OnGUI()
    {
        GUIStyle style = new GUIStyle(GUI.skin.label) { fontSize = 22, wordWrap = true };
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(20, 20, Screen.width - 40, Screen.height - 40), resultText, style);
    }
}
