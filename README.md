# 🚀 ChatChannel: Real-Time Support Ecosystem

**ChatChannel** is a professional-grade **Communication Service** designed for seamless interaction between users and support teams. Built with a focus on **Clean Architecture**, it leverages the power of **SignalR** to provide a lightning-fast, real-time experience.



## ✨ Key Features

* **⚡ Real-Time Engine:** Powered by **SignalR** for sub-second latency in message delivery.
* **👥 Dual-Role System:** Optimized workflows for both **Admins** and **Users**.
* **💾 Hybrid Data Access:** Innovative support for both **SQL** and **NoSQL** data layers within the same ecosystem.
* **🔐 Secure by Design:** Robust **JWT-based Authentication** ensuring only authorized roles access sensitive support channels.
* **📩 Offline Persistence:** Messages are stored safely; if an Admin is offline, they can view and reply to the **chat history** later.

---

## 🏗️ Architectural Design

The solution is divided into four strictly decoupled layers to adhere to **Clean Architecture** principles:

1.  **Domain:** Contains the core **Entities**, Business Rules, and Value Objects.
2.  **Application:** Houses **Use Cases**, Interfaces, and DTOs. This is where the business logic orchestration lives.
3.  **Infrastructure:** The implementation layer for **SQL Server**, **MongoDB**, and external services.
4.  **Presentation (API/Web):** The entry point. Manages **SignalR Hubs**, Controllers, and Middleware.

---

## 🛠️ Tech Stack & Infrastructure

This project utilizes a modern, hybrid tech stack to ensure scalability and performance:

| Layer | Technologies | Key Role |
| :--- | :--- | :--- |
| **Backend** | `.NET 8.0`, `C# 12` | Core Logic & API |
| **Real-Time** | `SignalR`, `WebSockets` | Full-duplex live communication |
| **Relational DB** | `MS SQL Server` | Traditional persistent storage option |
| **NoSQL DB** | `MongoDB` | High-speed Chat Logs & Persistence |
| **Security** | `JWT`, `RBAC` | Token-based Auth & Role Management |
| **Pattern** | `Clean Architecture` | Decoupling & Maintainability |

---
