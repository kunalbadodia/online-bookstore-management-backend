# Online Bookstore Project

## Project Overview
This project is a full-stack Online Bookstore built using React for the frontend and ASP.NET Core for the backend and SSMS for database. The system includes user authentication, product management, and order processing.

## Key Concepts:

- React (Frontend)
- Redux (State Management)
- Routing (Navigation)
- REST API (Data Communication)
- JWT Authentication (Security)
- Form Handling (User Input Management)

## Tech Stack
 - Frontend: React, Redux, React Router DOM, Axios, Bootstrap, Bootstrap Icons
 - Backend: ASP.NET Core, Entity Framework Core
 - Database: MS SQL Server

### Languages Used 
- Reactjs, Bootstrap5, HTML, JavaScript (Frontend)
- C# (Backend) 
- SQL (Database)

# Installation

## Backend Setup (ASP.NET Core)

### 1. Configuration
        Edit `appsettings.json`
         Update the database connection string and JWT settings as follows:

        json
        {
        "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Database=BookEcommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"
        },
        "Jwt": {
            "Key": "YourSecretKeyHere1234567890",
            "Issuer": "BookEcommerceAPI",
            "Audience": "BookEcommerceClient",
            "DurationInMinutes": 60
        }
        }


### 2. Install Required NuGet Packages:
    Run the following commands to install the necessary packages:
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
    ```

### 3. Run Migrations:
Execute the following command to apply database migrations:
```bash
dotnet ef database update
```

### 4. **Start the API:**
Run the following command to start the ASP.NET Core API:
```bash
dotnet run
```

➡️ The API will run at: `https://localhost:7123`

---

## Frontend Setup (React)

1. **Open Project in Visual Studio Code**  
   Navigate to the frontend project folder and open it in VS Code.

2. **Install Dependencies:**
Run the following commands in the terminal:
```bash
cd client
npm install
```

3. **Install Required Libraries:**
Run the following command to install key dependencies:
```bash
npm install axios bootstrap bootstrap-icons react-router-dom redux react-redux redux-thunk redux-devtools-extension
```

4. **Update API Base URL:**
Modify the `axios` instance in your React project to point to the correct API endpoint:

```javascript
const api = axios.create({
  baseURL: 'https://localhost:7123/api',
  headers: {
    'Content-Type': 'application/json'
  }
});
```

---

##  Running the Full Application
1. Start the backend API using `dotnet run`.
2. Start the frontend React application using `npm start`.
3. Access the application at `http://localhost:3000`

---

##  Notes
- Ensure SQL Server is running for the database connection.
- Make sure to create the `BookEcommerceDB` database before running migrations.
- For JWT authentication, ensure your secret key is strong and kept secure.

---
## Website Preview

- HomePage
  
  ![homepage](https://github.com/user-attachments/assets/7318f24e-4c1c-4252-b718-8e356bb93947)

- BookPage

  ![bookpage](https://github.com/user-attachments/assets/c1b095f1-e715-4fde-a49a-bcef64b61259)

- BookCard

  ![bookcard](https://github.com/user-attachments/assets/4865fac0-d5ff-47c1-9e83-56594d9515d3)

- Cart

  ![cart](https://github.com/user-attachments/assets/6e1c72e1-549b-490a-94fd-ad7965e1d5c4)

- checkout

  ![checkout](https://github.com/user-attachments/assets/2373e495-8993-4ae8-ab15-56b7fd943558)

- Order Details and Order History

  ![order details](https://github.com/user-attachments/assets/7b606e05-5179-41fc-9c3f-6e6035f6decd)

  ![order history](https://github.com/user-attachments/assets/6a816c7f-bba2-451b-b77b-4ea02767f872)

- Profile

  ![profile](https://github.com/user-attachments/assets/6ce8ea91-4423-4f2d-8622-f5349e030978)
