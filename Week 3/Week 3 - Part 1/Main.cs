using System;

// The main class for the story driven text game.
class program
{

// This is the entry point where the story starts. The game runs from top to bottom here.

   static void Main()
   {

//This is the intro scene. This will be shown to the player and print the first set of choices for the player to start.

    Console.WriteLine("You awake on a train, it's quiet, and there's nobody around at a quick glance. It has an eerie feel, like something is off...");
    Console.WriteLine("What do you do?");
    Console.WriteLine("");
    Console.WriteLine("");
    Console.WriteLine("1. Look out the window, maybe you can see something?");
    Console.WriteLine("");
    Console.WriteLine("2. Get up and look around, maybe you can see someone and ask what's happened?");
    Console.WriteLine("");
    Console.WriteLine("3. Go back to sleep, everything is ok, surely?");
    Console.WriteLine("");
       

// This is where the player can type between 1-3 to make the first choice for the game.
    string choice = "";

// This while loop will keep asking the player to input a choice until they input a valid choice of 1, 2, or 3.
    while (choice != "1" && choice != "2" && choice != "3")
    {
     choice = Console.ReadLine();
    }
       
       // This is where the game will branch off into different storylines based on the player's choice.`
       //There are 3 choices, choice 1, 2 or 3.
       if (choice == "1")
       {
        Console.WriteLine("You look out the window, you see a pair of eyes looking at you, but you can't tell if it's coming from a reflection inside or if it's outside. You then hear some taps on the glass and back away from the window...");
       }
       else if (choice == "2")
       {
       Console.WriteLine("You get up and look around. There is nobody in the carriage with you, all the luggage is gone, and it's quiet and unsettling. You notice the door to the next carriage in front of you is open, and you look to see if the door to the carriage behind you is open, but it is not. ");   
       }
       else if (choice == "3")
       {
        Console.WriteLine("You decide that the train is empty because you have been travelling for a while and people have left, you are half asleep, so you also think the tiredness isn't helping. You decide to try to get more sleep. Rest is important."); 
        Console.WriteLine("");
        Console.WriteLine("As you close your eyes, you hear disturbing, unintelligible whispers coming from behind your seat. You're scared. What do you do? Do you stay silent?");


//This is the second choice, if the player chooses 3 this is the next branching path for the player to play right now. The player can choose to stay silent or try to go back to sleep. The player will have to type 1 or 2 to make a choice.
    string choice2 = "";
    while (choice2 != "1" && choice2 != "2")
    {
     choice2 = Console.ReadLine();
    }
       
       if (choice2 == "1")
       {
        Console.WriteLine("You choose to stay silent. Fear holds you and keeps you still, your breath becomes shallow as you try to make yourself invisible to the unknown entity.");
        Console.WriteLine("");
           Console.WriteLine("It works, It leaves, your safe....for now.");
       }

       //If the player chooses 2, thhe game is over, the player dies and the game ends. The player will have to restart the game to play again..
       else if (choice2 == "2")
       {
       Console.WriteLine("You try to go back to sleep, It's just a noise, no? It could have been something completely different from what you thought it was. The whispering gets louder, it's not leaving. It looms over you.");   
       Console.WriteLine("");
       Console.WriteLine("You hear a loud scream. It's the last thing you see before darkness...It got to you...You are dead");
       Console.WriteLine("");
       Console.WriteLine("The End");
       }

       }
   } 
}