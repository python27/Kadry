cd bin/Release/
..\..\..\7z\7z.exe a -tzip ..\..\Releases\Release_%1.zip * -x!PreConfig.json -x!Config.json -x!old/*