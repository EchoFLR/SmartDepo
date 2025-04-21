# 🚋 Smart Depo Project

![.NET](https://img.shields.io/badge/.NET-6.0-blue)
![Blazor](https://img.shields.io/badge/Blazor-WebAssembly-purple)
![License](https://img.shields.io/badge/License-MIT-green)

A project for managing trams and their missions efficiently using a **Smart Depo** library, a **Backend API**, and a **Frontend UI**.

---

## 📚 Smart Depo
The **Smart Depo** library is the core of this project, designed to handle tram management tasks such as:
- 🚀 Assigning missions to trams.
- 🔄 Resetting the tram depot to its initial state.
- ✅ Validating tram data using JSON schemas.

The library is built with extensibility and scalability in mind, making it suitable for various tram management scenarios.

---

## 🖥️ Backend
The **Backend** is implemented using **ASP.NET Core** and serves as the API layer for the Smart Depo system. It provides the following endpoints:

### API Endpoints:
| **Endpoint**                   | **Method** | **Description**                                                                |
|--------------------------------|------------|--------------------------------------------------------------------------------|
| `/api/SmartDepo/initialize`    | `POST`     | Initializes the tram depot with tram data provided in JSON format.             |
| `/api/SmartDepo/trams`         | `GET`      | Retrieves the list of all trams in the depot.                                  |
| `/api/SmartDepo/assignMission` | `POST`     | Assigns a mission to a tram. The backend determines which tram to assign it to.|
| `/api/SmartDepo/reset`         | `POST`     | Resets the tram depot by clearing all trams and optionally reinitializing them.|

### Swagger API Documentation
The backend includes **Swagger** for interactive API documentation. Once the backend is running, you can access the Swagger UI at:
- **URL**: [`http://localhost:5076/swagger`](http://localhost:5076/swagger/index.html)

Swagger provides a user-friendly interface to explore and test the available API endpoints.

---

## 🌐 Frontend
The **Frontend** is built using **Blazor WebAssembly** and provides a user-friendly interface for interacting with the Smart Depo system. Key features include:

- **Tram Data Display**:
  - Displays a list of all trams, including their names, IDs, and current missions.
- **Mission Assignment**:
  - Allows users to assign a mission to a tram using a simple text input.
- **Reset Depot**:
  - Provides a button to reset the tram depot to its initial state.
- **Initialize Depot**:
  - Allows users to initialize the depot by pasting tram data in JSON format.

The frontend communicates with the backend via RESTful APIs and dynamically updates the UI based on the backend's responses.

---

## 🛠️ How To

### Prerequisites
- **.NET SDK**: Install the latest version of the .NET SDK from [dotnet.microsoft.com](https://dotnet.microsoft.com/).
- **Git**: Ensure Git is installed for cloning the repository.

### Steps to Start the App (Unix Based)

1. **Clone the Repository**:
   git clone <repository-url>
   cd <repository-folder>
2. **Start the Backend**:
```bash
   cd TramPlanner.Server
   dotnet run
```   
   The backend will start at http://localhost:5076 (or the configured port).
3. **Start the Frontend**:
```bash
   cd ../TramPlanner.Server
   dotnet run
```
   The frontend will start and can be accessed in your browser.
4. **Initialize the Depot**:
   Paste tram data in JSON format into the provided textarea in the frontend.
   Click the "Start" button to initialize the depot.
5. **Assign a Mission**:
   Enter a mission in the text input field and click "Assign Mission."
   The mission will be assigned to a tram, and the tram data will update dynamically.
6. **Reset the Depot**:
   Click the "Reset Data" button to clear all trams and reinitialize the depot.

## 📝 Example JSON for Initialization
```json
[
    { "Id": 1, "Name": "Tram-1", "CurrentMission": null },
    { "Id": 2, "Name": "Tram-2", "CurrentMission": "Delivery" },
    { "Id": 3, "Name": "Tram-3", "CurrentMission": null }
]
```

## 📄 License
This project is licensed under the MIT License.

## 📫 Contact
For any questions or feedback, feel free to reach out via GitHub issues or email!