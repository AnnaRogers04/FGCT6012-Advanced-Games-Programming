# Week 4

Game link:[https://lunarlynx-games.itch.io/lantern](https://lunarlynx-games.itch.io/lantern)

Survey Link:[https://annarogers04.github.io/QA-Telemetry/](https://annarogers04.github.io/QA-Telemetry/)


##  Approach
I made a playtest survey and a session logger as one web page. Testers play my game The Lantern Keeper, which is on itch.io, then fill in the survey. My FMP is not built yet, so I used this game to test the system.

The survey starts with consent. Testers tick three boxes: to take part, to let me store their answers, and to confirm they are 18 or over. The text says what I collect, why, how long I keep it and how to withdraw, to follow UK GDPR. If they say no, nothing is saved.

Next it asks for the game version, platform and hardware, so I know exactly what was tested.

For numbers, I ask for 1 to 5 ratings on enjoyment, controls, visuals, sound and clear instructions. There is also a difficulty score out of 10, how smooth it felt, an optional FPS number and how many times they got stuck. For words, I ask what they liked, what was confusing, any bugs, how bad the worst bug was, and any ideas.

The technical feature is a JavaScript session logger. Each session gets an ID. The logger records timestamped events like consent given, sections viewed, ratings set and survey submitted. It does not record what people type in the comment boxes, only that they wrote something. Everything is saved with the answers as JSON in the browser. It also sends the data to Supabase, so every tester's answers are in one table.

## Result

The system works. The Results tab shows average scores as bars, the worst bug levels and all the comments. I can export everything as JSON or CSV, and import files from other testers.

I tested it with 3 people. All rated every part 4 or 5 out of 5. One said changing modes took too many taps. One wanted more guidance at the start. One said the start screen was crowded and the perk and health icons were too small.

Tools used: HTML, CSS, JavaScript, browser localStorage, Supabase, itch.io and GitHub.

<img width="2000" height="400" alt="Screenshot 2026-11-12 210837" src="https://github.com/user-attachments/assets/53ed9136-042f-498b-a6d9-65616ff93a64" />

*figure 1* Shows the survey results.

<img width="800" height="450" alt="Survey" src="https://github.com/user-attachments/assets/2094d489-77e9-4770-b7eb-9835e06dfa70" />

*Figure 2* Shows my survey.

## Reflection

This data can help my QA in a few ways. Low scores on controls or clear instructions show me what to fix first. The "what was confusing" answers show me where players get lost. The version and platform fields show if a bug only happens on one setup. The "times stuck" number points to the hardest parts.

Next I would log data from inside the game itself, like where players stop or fail, and join it to the survey by session ID. I want to use this same system for my FMP, where I would also ask how unsettled players feel.

## Decloration
Claude AI was used to help make the survey