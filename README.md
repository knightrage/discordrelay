Designed for use with RunUO 1.0 but can be used to relay text from any .txt file. Open Project.cs, add your custom Webhook URL and set your .txt file location.

**To build the .exe, run the following command from the project folder:**
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
