1. Створіть локальну бд PostgreSQL та заповніть її даними з файлу DB_new_. Ви можете використовувати будь-який інструмент для роботи з PostgreSQL, наприклад, pgAdmin або командний рядок psql.



2. створіть файл з назвою "appsettings.json" у кореневій папці вашого проекту і додайте туди:

{
    "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Database=назваБд;Username=postgres;Password=пароль"
    },
    "LLM": {
        "ApiKey": "YOUR_API_key",
        "Endpoint": "https://api.groq.com/openai/v1/chat/completions"
    },
    "Logging": {
        "LogLevel": {
            "Default": "Information",
            "Microsoft.AspNetCore": "Warning"
        }
    },
    "AllowedHosts": "*"
}

3. Запустіть проект через run або натиснувши F5.