#  Voz del Este - Web Management System for Online Radio

**Voz del Este** is a web platform developed using **ASP.NET MVC with Entity Framework (Database First)** to comprehensively manage an online radio station. The system enables the administration of radio programming, news, users with role-based access, listener comments, sponsors, and the integration of external data such as weather and currency exchange rates.

This project was developed as part of the Software Development course at CEI.

---

## 🚀 Main Features

- 📑 **News Management**  
  Create, edit, categorize, and publish news by topic (Crime, Economy, Sports, Tourism, etc.).

- 🎙️ **Radio Programming Management**  
  Administer radio shows including schedules, broadcast days, and assigned hosts.

- 👥 **User and Role System**  
  Authentication and authorization system with roles: Administrator, Editor, and Client.

- 💬 **Listener Comments**  
  Feedback system allowing listeners to leave comments on each radio program.

- 🌤️ **Real-time Weather**  
  Integration with the OpenWeatherMap API to display current weather and forecasts.

- 💱 **Currency Exchange Rates**  
  CurrencyLayer API integration to display USD and other currency exchange rates.

- 📊 **Permissions Panel**  
  Fine-grained control over role permissions, enabling or disabling access to specific actions.

- 🕵️ **Audit Logging**  
  Automatic tracking of key operations: creation, editing, and deletion of data.

---

## 🧱 Technologies & Tools

- ASP.NET MVC (.NET Framework)
- Entity Framework (Database First)
- SQL Server Express
- HTML5, Bootstrap
- External APIs: OpenWeatherMap, CurrencyLayer
- Claims-based Authentication + Custom Roles

---

##  Project Structure

- `Controllers/`: Business logic per entity (News, RadioPrograms, Users, etc.)
- `Models/`: Database entities + custom ViewModels
- `Views/`: Razor Views for each feature
- `Content/` and `Scripts/`: Static assets (CSS, JS)
- `DAL/`: Database access layer (`VozDelEsteDB.edmx`)
- `App_Start/`: Routing and authentication setup

---

##  Roles & Permissions

| Role        | Access Description                          |
|-------------|---------------------------------------------|
| Admin       | Full access to all system features          |
| Editor      | Manage news and user comments               |
| Client      | View content and submit comments            |

---

##  Project Status

- Actively under development  
- Core features implemented  
- Functional responsive design for desktop  
- Planned improvements for mobile UI/UX

---

## ✍ Authors

- Matías Lanne  
- Lucas Sechous
  
---
