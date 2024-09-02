# Archive
Starting from 2024 July 29, the application is being maintained by the [Automation & Development Center of the KROK University](https://graph.krok.edu.ua/?culture=en-US) and me as its part.
Students or passersby can freely use the available codebase for their own sake. You can observe the continuation of the project life on the [university schedule page](https://schedule.krok.edu.ua/).

# Smart Timetable - Backend
**C# • ASP.NET Core 8 • PostgreSQL • Entity Framework • xUnit**

This is a REST API project specially designed for unifying data acquired by another university schedule API and uniting it with personalized data from Microsoft educational accounts. API also provides secured notes for study groups and outages for lessons that are obtained by cron job.  

App showcases:
- Microsoft Web API Authentication
- Microsoft Graph usage using on-behalf-of auth flow
- Requests validation
- Distinctive~ three-tier architecture
- Usage of EntityFramework and PostgreSQL for data access via Code-First
- Unit tests large coverage
- Cron job usage for outages obtaining and then parsing

# About Smart Timetable Project
Smart Timetable is an app that provides a schedule designed especially for students of the [KROK University](https://int.krok.edu.ua/en).
The main point of the project is to create a user-friendly interface for students/teachers to acquire schedules.
The integration of the educational process with the Microsoft ecosystem allows to create a personalized features for students.  
It provides not only a plain beautified schedule but also the possibility to access planned lessons in Microsoft Teams and make notes to a lesson that can see everyone in your study group.
The app can access schedules of outages in Kyiv, and provide information about possible outages during any of your planned lessons.  
This schedule, compared to existing ones, provides a user-friendly interface and tries to simplify student navigation as much as possible.
