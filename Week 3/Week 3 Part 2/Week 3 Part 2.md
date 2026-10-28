# Week 3 part 2

  
##  Approach

For this task I chose to make a terrain generator where you can see the changes in real time with sliders.

The land comes from a height map made with Perlin noise. I put a few layers of noise on top of each other, called octaves. Each layer is smaller and weaker than the last. This gives big mountains with smaller bumps on top. Persistence and lacunarity control how the layers change. The seed number picks the random start, so the same seed always makes the same land.

The height map is used to build the 3D mesh and a colour texture. The mesh uses the height for each point. The texture colours each point by its height, so low is water, then sand, grass, rock and snow. Anything below the water level is flattened to make lakes.

It is procedural because I don't place or paint anything by hand. The computer makes the land and the colours from the seed and the settings. Changing one number makes a new land.

## Result

The result lets you make your own unique terrain using the sliders. You can change the seed, octaves, persistence, lacunarity, noise scale, water level and resolution. Pressing R gives a new random seed. You can fly around the land with the camera to look at it from different angles.

I think what I have made is very effective and works really well. As seen in the gif below, there are quite a lot of adjustments that can be made to the land and the water. Changing the octaves changes how rough the land is. Changing the water level makes the lakes bigger or smaller. Every seed gives a different land.


<img width="800" height="450" alt="Terrain Generation" src="https://github.com/user-attachments/assets/1773304b-1536-464f-bfe3-ccb786a80a81" />

## Reflection

I think it could be refined more. Right now the land is coloured by height, which looks quite flat. With more time I would use materials and textures instead of colours to give it a more realistic look. For example, grass, sand and rock textures would look better than plain colours.

I also think the water could look better. At the moment it is just the land flattened and coloured blue, so it has no waves and you can't see through it. I would make a proper water material so it looks more like real water.

The slider panel is simple but it does the job. I could add more sliders to it, like the height bands, so more of the system can be changed in the build.

Overall I'm happy with how it turned out and I think it shows what a procedural system can do. If I had more time I would carry on and make it look more realistic.