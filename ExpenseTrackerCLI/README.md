﻿# 📝 Task Tracker CLI

**Expense Tracker CLI** is a simple command-line application to manage your expenses efficiently.
It allows you to add, update, delete, list, summarize expenses, and set a budget using a JSON-based storage system.<br>
Sample solution for the [Expense Tracker CLI](https://roadmap.sh/projects/expense-tracker) challenge from [roadmap.sh](https://roadmap.sh/).

## 🚀 Features
✅ Add, update, delete expenses <br>
✅ View all expenses in a structured format<br>
✅ Summarize total expenses<br>
✅ View expenses for a specific month<br>
✅ Set and track a monthly budget<br>
✅ Sort expenses by date, amount, or category<br>
✅ Stores expenses in a JSON file (No database required)<br>

## 📥 Installation & Setup
### 1️⃣ **Clone the repository**
```
git clone https://github.com/rushixdd/Cracked-Backend-Journey.git
cd ExpenseTrackerCLI
```
### 2️⃣ **Build the project**
```
dotnet build
```
### 3️⃣ **Run the application**
```
dotnet run -- add --description "Lunch" --amount 15 --category "Food"
```
## 📌 Commands & Usage
|Command	|Description|Example|
|---------------|-----------|-------|
|expense-tracker add --description "Lunch" --amount 15 --category "Food"|Add a new expense|expense-tracker add --description "Dinner" --amount 10 --category "Food"|
|expense-tracker update --id <ID> --amount <NewAmount>|Update an expense amount|expense-tracker update --id 1 --amount 20|
|expense-tracker delete --id <ID>|Delete an expense (with confirmation prompt)|expense-tracker delete --id 1|
|expense-tracker list|List all expenses|expense-tracker list|
|expense-tracker summary|Show total expenses summary|expense-tracker summary|
|expense-tracker summary --month <1-12>|Show monthly expenses summary|expense-tracker summary --month 3|
|expense-tracker set-budget <Amount>|Set a monthly budget|expense-tracker set-budget 500|

## 🔧 Build a Self-Contained Executable (Windows)
To create a standalone .exe file:<br>
```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o dist
```



## 🛠 Tech Stack
- C#
- .NET (Latest Version)
- JSON for storage
- Command Line Interface (CLI)
## 📜 License
This project is licensed under the MIT License.
