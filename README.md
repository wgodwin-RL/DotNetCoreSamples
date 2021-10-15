Instructions
Download .net core 3.1
https://dotnet.microsoft.com/download/dotnet/3.1

Running the project locally
Using the dotnet tools that were just installed to run the project locally.
1. Open a cmd prompt
2. copy/paste the following "dotnet run --project 'path to LDAPI.csproj'"
  - ex. dotnet run --project ./projects/proj1/proj1.csproj
3. Press enter
4. The project should be starting up 
  - a console window should pop up showing a status for the startup.
  - a browser window should pop up with the address https://localhost:5000/api/students
5. In the console window once the app fully starts up the messages consumer starts outputting event messages for each event that has been parsed and pushed into the in-memory data store.


