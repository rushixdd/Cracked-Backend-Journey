# 📝 Task Tracker CLI

**Task Tracker CLI** is a simple command-line application to track your tasks efficiently.  
It allows you to **add, update, delete, mark, list, search, and filter tasks** using a JSON-based storage system.

## 🚀 Features
✅ Add, update, delete tasks  
✅ Mark tasks as **todo, in-progress, or done**  
✅ List all tasks or filter by status  
✅ Search tasks by keyword  
✅ Filter tasks by date range  
✅ Sort tasks by creation date (ascending/descending)  
✅ Stores tasks in a **JSON file** (No database required)  

---

## 📥 Installation & Setup
### 1️⃣ **Clone the repository**
```
git clone https://github.com/rushixdd/Cracked-Backend-Journey.git
cd TaskTrackerCLI
```
### 2️⃣ **Build the project**
```
dotnet build
```
### 3️⃣ **Run the application**
```
dotnet run -- add "Complete the .NET project"
```
### 📌 **Commands & Usage**
|Command |Description|Example|
---------|-----------|-------|
|TaskTrackerCLI add "Task description"|Add a new task|TaskTrackerCLI add "Buy groceries"|
|TaskTrackerCLI update \<ID> "New description"|Update an existing task|TaskTrackerCLI update 1 "Buy groceries and cook dinner"|
|TaskTrackerCLI delete \<ID>|Delete a task|TaskTrackerCLIdelete 1|
|TaskTrackerCLI mark-in-progress \<ID>|Mark task as in progress|TaskTrackerCLI mark-in-progress 1|
|TaskTrackerCLI mark-done \<ID>|Mark task as done|TaskTrackerCLI mark-done 1|
|TaskTrackerCLI list|List all tasks|TaskTrackerCLI list|
|TaskTrackerCLI list done|List completed tasks|TaskTrackerCLI list done|
|TaskTrackerCLI search "keyword"|Search tasks by keyword|TaskTrackerCLI search "groceries"|
|TaskTrackerCLI filter --from YYYY-MM-DD --to YYYY-MM-DD|Filter tasks by date|	TaskTrackerCLI filter --from 2025-03-01 --to 2025-03-05|

## 🔧 Build a Self-Contained Executable (Windows)
### To create a standalone .exe file:
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o dist
```
Move the executable to System32 for global access:
```
move dist\TaskTrackerCLI.exe C:\Windows\System32\TaskTrackerCLI.exe
```
Now, you can run TaskTrackerCLI from anywhere in the terminal!


## 🛠 Tech Stack
C#<br>
.NET (Latest Version)<br>
JSON for storage<br>
Command Line Interface (CLI)<br>
## 📜 License
This project is licensed under the MIT License.