# 🔐 SafeVault - Secure Web Application

## 📌 Project Overview

SafeVault is a secure web application designed to manage sensitive user data such as credentials and financial information. The application focuses on implementing secure coding practices, authentication, and role-based access control while protecting against common web vulnerabilities.

---

## 🚀 Features

* 🔑 User Registration & Login
* 🔐 Password Hashing using **BCrypt.Net**
* 🛡️ Protection against SQL Injection
* 🧼 Input Sanitization to prevent XSS attacks
* 👤 Role-Based Access Control (Admin/User)
* 🧪 Unit Testing using **NUnit**

---

## 🛠️ Technologies Used

* ASP.NET Core Web API
* C#
* MySQL
* Visual Studio Code
* GitHub

---

## 🔒 Security Implementations

### 1. SQL Injection Prevention

* Used parameterized queries instead of string concatenation.
* Ensures user input is treated as data, not executable SQL.

### 2. Cross-Site Scripting (XSS) Protection

* Implemented input sanitization using regex.
* Removed harmful HTML/script tags from user input.

### 3. Authentication

* Secure login system with hashed passwords.
* Passwords stored using BCrypt hashing.

### 4. Authorization (RBAC)

* Admin and User roles implemented.
* Restricted access to admin routes.

---

## 🧪 Testing

Unit tests were implemented to verify application security:

* SQL Injection attack simulation
* XSS attack simulation
* Invalid login attempts
* Unauthorized access checks
* Admin access validation

All tests passed successfully ✔

---

## 📂 Project Structure

```
SafeVault/
│
├── SafeVaultAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── appsettings.json
│
├── SafeVaultTests/
│   └── UnitTest1.cs
```

---

## ⚙️ Setup Instructions

### 1. Clone the Repository

```
git clone https://github.com/yourusername/SafeVault.git
```

### 2. Open in VS Code

Open the project folder in Visual Studio Code.

### 3. Configure Database

* Create database:

```
CREATE DATABASE SafeVault;
```

* Update connection string in `appsettings.json`

### 4. Run API

```
cd SafeVaultAPI
dotnet run
```

### 5. Run Tests

```
cd SafeVaultTests
dotnet test
```

---

## 🤖 Role of Microsoft Copilot

Microsoft Copilot was used to:

* Generate secure code for input validation
* Suggest parameterized queries
* Implement authentication and RBAC
* Help debug vulnerabilities
* Generate unit test cases

---

## 📊 Results

* Successfully prevented SQL Injection and XSS attacks
* Implemented secure authentication and authorization
* All test cases passed
* Application is secure and reliable

---

## 📌 Conclusion

SafeVault demonstrates how secure coding practices, proper validation, and testing can protect web applications from common vulnerabilities. The project highlights the importance of authentication, authorization, and defensive programming in modern web development.

---

## 🔗 GitHub Repository

(Add your link here)
