# DodgeGame
C# UWP game. My first project in the fullstack development course.
## Installation
Download and run Game.sln on Visual Studio.
## Overview
I wanted this game to be based on my favorite childhood game, "Crash Bandicoot". I used the background image, the creatures images and the music from the game. I also used a content dialog to display whether the user won or lost the game.
The game features basic **OOP**; The classes **"Goodie"** (the user) and **"Baddie"** (the user's enemies) both **inherit from their base class**, **"Creature"**. The class **"Game"** contains the game's logic.
## Game Play
In order to start the game, you need to click on *"Start"* on the command bar. Once the game has begun, you need to use the **keyboard arrows (Up, Down, Left and Right)** in order to avoid the "baddies" that are chasing you. You can also use the **spacebar**, which sets you in a random position on the canvas.
**In order to win the game, you need to remain with only one baddie alive.** Baddies die when they collide. You can also use the command bar's buttons, *"Save"* and *"Load"* if you wish to exit the game and come back to it later. The *"Save"* button uses StorageFile, and saves all the baddies's and the goodie's locations. *"Load"* button will always load the last game you saved.

![image](https://user-images.githubusercontent.com/62158246/210216819-4ce597c3-4aae-4d36-95d5-eb50da12186b.png)
