# Next-Manager backend

## Overview

Next-Manager is a web application that helps teams manage projects efficiently. This system offers features for tracking project progress, managing tasks, and assigning roles to team members. The application is designed to streamline project workflows, enhance collaboration, and provide clear visibility into project status.

## Features

- **Project Tracking**: Create, update, and monitor projects with detailed descriptions and status updates.
- **Task Management**: Add, assign, and prioritize tasks within projects, ensuring that nothing falls through the cracks.
- **Role Management**: Assign roles to team members, ensuring proper access control and task delegation.
- **User Authentication and Authorization**: Secure your application with built-in authentication and role-based access control using ASP.NET Core Identity.
- **Email Notifications**: Send email notifications for important project updates and task assignments.
- **Reporting**: Generate detailed reports on project progress, including user statistics and task completion rates.
- **Responsive Design**: Access the application on any device with a responsive and user-friendly interface.
- **Integration with PostgreSQL**: Robust data storage using PostgreSQL, ensuring data integrity and reliability.

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Azure Account](https://azure.microsoft.com/en-us/free/) (for deployment)

### Installation

1. **Clone the Repository**
   ```sh
   git clone https://github.com/Kubat555/Next-Manager.Backend.git
   cd ProjectManagement

2. **Configure the Database**  
Update your appsettings.json file with your PostgreSQL connection string:
   ```sh
   "ConnectionStrings": {
    "PostgresDbConnection": "Host=your_host;Database=your_db;Username=your_user;Password=your_password"
    }

4. **Migrate the Database**
   ```sh
   dotnet ef database update

5. **Run the Application**
   ```sh
   dotnet run

