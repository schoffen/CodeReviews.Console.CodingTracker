# Coding Habit Tracker
This is a coding session tracker console application. It was made with C#, Dapper and Spectre.Console.

### Requirements:
- This application must log daily coding time, using specific format;
- This application must use Spectre.Console library;
- Use separation of concerns;
- Use configuration file for database path and connection strings;
- The user must only input start date time and end date time, the application is responsible for calculating the duration;
- Use Dapper ORM for the data access;
- Follow DRY Principle;
- This READ ME file.

### Optional Requirements:
- Possibility of tracking time via stopwatch;
- Possibility for the user to filter their records by period (day, weeek, year) and ascending or descending order;
- Use unit test. (I didn't use that, but start studying for future projects)

### Features
- Clean console UI using Spectre.Console prompts
- Full CRUD operations with Dapper
- Clear separation between controller, service, and repository layers
- Custom exception handling

### Lessons Learned:
This project helped me better understand responsibility boundaries in a growing codebase. 
Separating concerns is not trivial, especially as new features are added, and I often found myself refactoring code to improve structure and readability.
I also realized that I tend to overcomplicate simple solutions, which taught me the importance of balancing clean architecture with pragmatism. 
Along the way, I learned and practiced concepts such as generic types, interfaces, actions, exception handling, and working with third-party libraries.
