# GlobalCityManager ASP.NET CORE 2.0/2.2.4
Final project from Microsoft Learning ASP.NET Core MVC  2.0 course at edX, code: Microsoft: DEV250x

The original code had two namespaces and I had to delete One, also asyncronous programming wasn't included, no styling, no dependency injection, connection string was written directly on code. I'm fixing all of this, also I will be trying a custom html helper to automaticaly generate a table with the given IEnumerable<Model>, with or without CRUD operations inserted.
  
Advances:
1. I made all the necessary controllers asyncronous before the first commit
2. Connection string moved to appsettings.json
3. Dependency injection for the DBContext
