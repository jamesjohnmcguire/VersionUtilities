CD %~dp0
CD ..\..
CD VersionUpdate

CALL dotnet publish --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true --runtime linux-x64 -c Release --output Release\linux-x64
CALL dotnet publish --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true --runtime osx-x64 -c Release --output Release\osx-x64
CALL dotnet publish --self-contained true -p:PublishReadyToRun=true -p:PublishSingleFile=true --runtime win-x64 -c Release --output Release\win-x64
