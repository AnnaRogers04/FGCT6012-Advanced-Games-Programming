# Week 5

## Approach

For this task I made a Unity script that loads an image and adds up all the red values in it. The image is 4000x4000, which is 16,000,000 pixels. I used the Jobs System with Burst so the adding can run on multiple threads.

The image is loaded from the StreamingAssets folder with File.ReadAllBytes and Texture2D.LoadImage. The pixels are then copied into a NativeArray so the jobs can read them.

For the parallel version I used an IJobParallelFor. The pixels are split into chunks of 65,536. Each job adds up the red values for one chunk and writes the answer into an array of partial sums. When the jobs finish, the main thread adds the partial sums together to get the final number. The job has BurstCompile on it so it runs as fast compiled code.

I also made a single thread Burst job and a normal main thread loop, so I can compare all three. The tools I used were the Jobs System, Burst, the Collections package for NativeArray, a Stopwatch for timing, and OnGUI to show the results on screen.

## Result

To test it, each method runs once as a warm up, because Burst compiles the job the first time it runs. Then each one runs 20 times and the average time is logged. The script also logs the load time, the number of CPU threads and the final sum. It checks that all three sums match and saves everything to a CSV file.

The main thread loop took 8.692 ms. The single Burst job took 9.680 ms. The parallel Burst job took 0.999 ms. All three gave the same sum of 1455577258. My CPU has 24 threads and the image was split into 245 chunks. The parallel job was about 8.7 times faster than the main thread loop. Loading the image took 138.93 ms.


## Reflection

The parallel job was 8.7 times faster, but it was not 24 times faster even though I have 24 threads. Some time is probably lost scheduling the jobs and adding the partial sums at the end.

The single Burst job was slightly slower than the main thread loop, which I did not expect. I think it is because the editor adds safety checks to jobs. I did not test with them turned off, so I am not sure.

These results show that adding up pixels is a good job to split across threads, because each pixel is independent. In a game I would use a parallel job for anything like this that goes over a lot of data. I would not bother for small images, because scheduling the jobs could take longer than the time saved.

With more time I would test different chunk sizes and test in a build instead of the editor, because the editor results may not match the final game. Overall I'm happy with how it turned out and it shows how much faster jobs can be.

<img width="1000" height="500" alt="Screenshot 2026-11-19 234304" src="https://github.com/user-attachments/assets/6d51c372-b59b-4187-b0de-4af056b36d3a" />

<img width="800" height="450" alt="Parallel processing" src="https://github.com/user-attachments/assets/a4f8d185-f2c3-473d-aeb0-8aa2813ebb6e" />

## Decloration
Claude AI was used to help make the code foe unity