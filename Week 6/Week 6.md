# Week 6 AI NPC

## Approach

For this task I planned an AI NPC in Unity that uses a local AI model. The NPC is called Old Pip. He is a caretaker in an abandoned farmhouse and he is hiding something about the cellar. The player walks up to him, presses E and types what they want to say. He is meant to talk back.

The model was meant to run on my own computer through Ollama, using the llama3.2:3b model. Unity does not run the model. Unity sends a message to Ollama and Ollama sends the answer back. I wrote the script to do this with UnityWebRequest, and the data goes back and forth as JSON.

Every time the player sends a message, the script sends the NPC's character description, a line about what is happening in the game, the last 10 messages, and the new message. The model gives back a short reply and the script shows it on screen.

I also added a dread number that goes up by one with every message. Each level adds a new rule to the character description. At the start he is friendly and warns you about the cellar. Later he is scared and whispering. This is how the AI was meant to change the gameplay.

After every reply the script saves a line to a CSV file. It saves the model name and version, the response time, the number of tokens made, the tokens per second, and my platform, CPU, GPU and RAM.

The tools I used were Unity, Ollama, llama3.2:3b, UnityWebRequest, JsonUtility and OnGUI for the chat box.

## Result

I did not get this working in time. I installed Ollama, but I could not get the model to download and run on my computer. I typed the commands in the wrong place a few times and I did not know how to fix it.

Because of this I could not test the NPC in Unity, so I have no gif, screenshots or telemetry data. The code is in the project, but I have not seen it run, so I do not know if it works.

## Reflection

The main thing I learned is that I should have set up Ollama before writing anything else. I wrote the script first and left the setup until last, so when it went wrong I had no time left to fix it.

Getting a local model running was harder than I expected. It needs the program installed, the model downloaded and Ollama running in the background, all before Unity can use it. If I did this again I would test it with a simple command first, then connect Unity once I knew it worked.

With more time I would get the model running, test the NPC, and collect the response times from the CSV. I would also compare the dread levels to see if the NPC really does get more unsettling.

## Declaration

Claude AI was used to help me make the script.
