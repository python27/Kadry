"C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\MSBuild" kadry.csproj -t:Rebuild -p:Configuration=Release
mkdir ./Releases
mkdir ./Releases/old
xcopy /D /V ./Releases/* ./Releases/old/*
del /P ./Releases/*
ZipPacker.exe