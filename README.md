# User guide for Greeting API

**Table of Contents**

•	Features

•	Installation

•	Configuration

•	API Reference

•	Database Schema

•	Error Handling

•	Rate Limiting

•	Caching

•	Technologies Used
________________________________________
🚀**Features**

•	✅ "RESTful API Endpoints"

•	✅ "Rate Limiting Support"

•	✅ "Client-side Caching"

•	✅ “Server side Caching”
________________________________________
🛠️**Installation**

Clone the repository

git clone https://github.com/sreemagesh/Greeting.git

cd Greeting

**Install dependencies **

dotnet restore

**Build the project**

dotnet build

**Environment Configuration**

Modify the appsettings.json or set environment variables for the below:

•	Database connection strings

•	Logging options
 

**To host the API on IIS:**
**Publish the Application**
   
dotnet publish -c Release -o ./publish

**Configure IIS Site**
   
o	Open IIS Manager

o	Add new App Pool

o	Add new Web Site
 
**Permissions**

o	Ensure the IIS App Pool identity has read/execute permission on the publish folder.

**Start the Site**

o	Browse the site using your configured hostname or localhost port.
________________________________________
⚙️**Configuration**

Configuration settings can be found in:

•	appsettings.json – Default configuration.

•	appsettings.Development.json – Dev-specific overrides.
________________________________________
📡**API Reference**

•	GET /api/greetings/{name} – Returns a specific greetings.

•	POST /api/greetings/{name} – Creates a new greeting.
________________________________________
🧩 **Database Schema**

•	Greeting

o	Id (int, primary key)

o	Name (string)

o	Greeting (string)

o	CreatedAt (datetime)
________________________________________
❌**Error Handling**

•	400 – Bad Request

•	404 – Not Found

•	429 – Too Many Requests (rate limit exceeded)

•	500 – Internal Server Error
________________________________________
🚦**Rate Limiting**

To prevent abuse, the application uses rate limiting. Rate Limiting is a technique that is used to manage/control how many request a client can make to the API over a specific period of time.

•	5 requests per 1 second

•	HTTP 429 returned if limit is exceeded
________________________________________
⚡**Caching**

Client-side or server-side caching is implemented to improve performance. Caching is a technique that is used to store the data in the memory, the cache data will be used for subsequent calls when the same parameter/key is send from client to API.

In-memory cache (IMemoryCache)

•	Cache invalidation occurs after 5 minutes
________________________________________
🧰 **Technologies Used**

•	.NET Core / ASP.NET Core

•	Entity Framework Core

•	Swagger / Swashbuckle

•	MemoryCache
________________________________________

