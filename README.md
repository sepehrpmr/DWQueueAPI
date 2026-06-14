# DWQueue - Distributed Event-Driven Leave Approval System

DWQueue is a modern, scaleable, and decoupled distributed system built with **ASP.NET Core**, **RabbitMQ**, and **Docker**. It leverages an **Event-Driven Architecture (EDA)** to handle employee leave requests and asynchronous background notifications seamlessly.

## 🚀 Architecture Overview

The system is split into independent, autonomous microservices that communicate asynchronously using **MassTransit** over **RabbitMQ**:

1. **DWQueueAPI (Publisher):** A RESTful API that handles HTTP client requests. When a leave request is approved, it publishes a `LeaveApprovedEvent` to RabbitMQ and immediately responds to the client (Stateless & Fast).
2. **RabbitMQ (Message Broker):** Manages the message exchanges and queues using an advanced Exchange-to-Exchange topology provided by MassTransit, ensuring reliable message delivery.
3. **DWNotificationService (Consumer/Worker):** A background worker service that listens to the queue. Upon receiving the event, it processes the data and dispatches an HTML email notification.
4. **MailHog (SMTP Testing Server):** A local email-testing tool that catches all outgoing emails in a beautiful Web UI without sending them to real inboxes.

## 🛠️ Tech Stack

* **Backend:** .NET Core 8.0 / C#
* **Message Broker:** RabbitMQ
* **Bus Provider:** MassTransit (AMQP 0-9-1)
* **Containerization:** Docker & Docker Compose
* **Mail Server (Dev):** MailHog
* **API Documentation:** Swagger / OpenAPI

## ⚙️ Prerequisites

Before running the project, ensure you have the following installed:
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) (Optional, for local development outside Docker)

## 🏎️ Getting Started & Installation

The entire infrastructure is fully containerized. You can spin up the whole environment with a single command.

1. Clone the repository:
   ```bash
   git clone [https://github.com/your-username/DWQueue.git](https://github.com/your-username/DWQueue.git)
   cd DWQueue

   docker compose up --build




   📊 How to Use & Test
Once the containers are up, you can interact with the system using the following interfaces:

Service	URL	Description
Swagger UI	http://localhost:5000/swagger	Trigger leave approval events
RabbitMQ Management	http://localhost:15672	Monitor exchanges, queues, and message rates (Guest/Guest)
MailHog Web UI	http://localhost:8025	View intercepted email notifications in real-time
