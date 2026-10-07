# Week 1 API - Weather System for The Lantern Keeper
### What it does:

The games asks a free weather website (in my case Open-Meteo) what the weather is like in London right now. It uses the answer to change how the game looks, sounds and plays. This could be making it slippery because its raining or slower paced because its sunny and mellow.

### How it works:

1. When the game starts, it sends a request to Open-Meteo in the background. This prevents the game from freezing while the request is pending.
2. Open-Meteo replies with the weather that is currently happening the temprerature and the current wind cycle.
3. The game splits the weather options into 5. Clear, overcast, rain, snow or storm.
4. Once the weather is loaded it will change in game.

The request will lookmlike this:

```
https://api.open-meteo.com/v1/forecast?latitude=51.51&longitude=-0.13&current=temperature_2m,weather_code,wind_speed_10m,wind_direction_10m
```
If the request for the weather fails the game will still load just with a clear weather preset. 

---

### How the weather effects the game:
| Weather | What changes |
| --- | --- |
| **Clear** | Glowing dusk sky and crickets. Souls are worth double points |
| **Overcast** | Heavy grey sky. A normal night |
| **Rain** | Rain and rain sounds. The ground is slippery and things fall faster |
| **Snow** | Falling snow and quiet. You move slowly and things fall slowly |
| **Storm** | Lightning and thunder. Things fall fastest and there are more skulls and bats |



The real wind also affects how fast the objects fall and what direction they fall in.

---

### Testing
You can press 1-5 to select the 5 weather options in the live weather feed and play them yourself. If you press R it goes back to the live weather feed.

---