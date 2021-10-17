Description:
This is just a project I put together to do some .net core 3.1 learning. 
1. It uses a .NET EF Core in-memory database for storage. 
2. It has swagger integration, xunit unit tests for the web api endpoints and xunit integration tests for testing the backend. 
3. It has a side-loaded services that consumes messages and stores them in the data store. (TODO: send these messages to the api endpoint instead of directly accessing the data class) 
4. It has a .net core web api that is used to access the data in the datastore.

Endpoints
  - Exams (gets all exams)
  
  - Exams?number=x (gets one exam by exam number)
 
  - Students (gets all students)
  
  - Students?id=x (gets one student by string Id)
  
  - Messages (inserts an event message... this one still needs work.)
 
Run Instructions (other than using visual studio)

Download .net core 3.1
https://dotnet.microsoft.com/download/dotnet/3.1


Using the dotnet tools that were just installed to run the project locally.
1. Open a cmd prompt
2. copy/paste the following "dotnet run --project 'path to LDAPI.csproj'"
  - ex. dotnet run --project ./projects/proj1/proj1.csproj
3. Press enter
4. The project should be starting up 
  - a console window should pop up showing a status for the startup.
  - a browser window should pop up with the address https://localhost:5000/api/students
5. In the console window once the app fully starts up the messages consumer starts outputting event messages for each event that has been parsed and pushed into the in-memory data store.


