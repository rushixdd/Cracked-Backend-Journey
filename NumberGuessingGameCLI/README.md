# 🎯 Number Guessing Game CLI

**Number Guessing Game CLI** is a fun and interactive command-line game where players guess a randomly generated number within a set number of attempts. The game includes multiple difficulty levels, hints, a timer, and a high-score tracker to enhance the experience.<br>
Sample solution for the [Number Guessing Game CLI](https://roadmap.sh/projects/number-guessing-game) challenge from [roadmap.sh](https://roadmap.sh/).

## 🚀 Features
✅ Random number generation between 1-100<br>
✅ Three difficulty levels (Easy, Medium, Hard)<br>
✅ Hint system to guide the player<br>
✅ Timer to track how long it takes to guess correctly<br>
✅ High Score Tracker (minimum attempts per difficulty)<br>
✅ Option to play multiple rounds<br>
✅ CLI-based interface for fast and efficient gameplay<br>

## 📥 Installation & Setup
### 1️⃣ **Clone the repository**
```
git clone https://github.com/rushixdd/Cracked-Backend-Journey.git
cd NumberGuessingGameCLI
```
### 2️⃣ **Build the project**
```
dotnet build
```
### 3️⃣ **Run the application**
```
dotnet run
```
## 🎮 How to Play
1️⃣ The game starts with a welcome message and instructions.<br>
2️⃣ Players select a difficulty level:<br>
&ensp; - Easy → 10 chances<br>&ensp; - Medium → 5 chances<br>&ensp; - Hard → 3 chances

3️⃣ The game begins, and the player enters a guess.<br>
4️⃣ The game provides hints (higher or lower).<br>
5️⃣ If correct, the game displays the number of attempts & time taken.<br>
6️⃣ If wrong, the player loses an attempt and tries again.<br>
7️⃣ The game ends when the number is guessed or attempts run out.<br>

## 📌 Commands & Usage
|Command	|Description|Example|
|---------------|-----------|-------|
|dotnet run|Start the game|dotnet run|
|Enter a number|Make a guess|50|
|Y or N|Play again after a round|Y|

## 🏆 High Score System
- The game records the lowest number of attempts required to guess the number for each difficulty level.
- Players can try to beat their own best scores in subsequent rounds.

## 🛠 Tech Stack
- C#
- .NET (Latest Version)
- Command Line Interface (CLI)
## 📜 License
This project is licensed under the MIT License.
