# 📝 Personal Blog (ASP.NET Razor Pages)

**Personal Blog** is a simple personal blog built with **ASP.NET Razor Pages** that allows you to **publish, edit, and delete articles**. It includes both a public guest section and an admin section protected by authentication.<br>
Sample solution for the [Personal Blog](https://roadmap.sh/projects/personal-blog) challenge from [roadmap.sh](https://roadmap.sh/).

## 🚀 Features
### 👤 Guest Section
- **Home Page**: View a list of all published articles.
- **Article Page**: Read the full content of each article along with the publication date.

### 🔐 Admin Section
- **Login Page**: Basic login with session-based authentication.
- **Dashboard**: Manage all articles (view, edit, delete).
- **Add Article**: Form to publish a new article.
- **Edit Article**: Form to update existing articles.
- **Delete Article**: Remove articles from the blog.
- **Logout**: Ends the admin session securely.

## 📥 Installation & Setup
### 1️⃣ **Clone the repository**
```
git clone https://github.com/rushixdd/Cracked-Backend-Journey.git
cd PersonalBlog
```
### 2️⃣ **Build the project**
```
dotnet build
```
### 3️⃣ **Run the application**
```
dotnet run
```
### 4️⃣ Access the web app
Once the server starts, open your browser and go to:
```
http://localhost:5000
```
## 🔐 Admin Credentials
You can change the hardcoded credentials in /Pages/Admin/Login.cshtml.cs
```
    private const string AdminUsername = "admin";
    private const string AdminPassword = "password123";
```

## 📌 Usage

🔍 Guest Users
1. Homepage

    - Navigate to the root URL (e.g., /).
    - View the list of all published articles with their titles and dates.

2. View Article

    - Click on any article title from the homepage.
    - You'll be taken to the detailed view showing the full article content.

🔐 Admin Users
First, go to the Login page to authenticate.

1. Login
    - Enter the credentials (admin / password by default).
    - On success, you're redirected to the Admin Dashboard.
2. Dashboard
    - View all articles with options to:
    - Add a new article
    - Edit an existing article
    - Delete any article
3. Add Article
    - Click on the Add Article button.
    - Fill in the title, content, and publication date.
    - Submit to add the article to your blog.
4. Edit Article
    - On the Dashboard, click Edit next to the article you want to update.
    - Update the title/content/date as needed and save changes.
5. Delete Article
    - Click the Delete button next to any article to remove it permanently.
6. Logout
    - Use the Logout link in the dashboard to end your session securely.

## 🛠 Tech Stack
- **Framework**: ASP.NET Razor Pages (.NET 7+)
- **Backend**: C# + Razor PageModel
- **Storage**: JSON files (filesystem-based, no database)
- **Frontend**: HTML + CSS (no JavaScript)
- **Authentication**: Session-based with hardcoded credentials


## 🏗 Future Enhancements
- Add Markdown support for article content
- Rich text editor for content entry
- Tag and category system
- Commenting system
- Search functionality
- SQLite or other DB integration

## 📸 Screenshots


## 📜 License
This project is licensed under the MIT License.
