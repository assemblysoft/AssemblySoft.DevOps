::@echo Off

echo start custom build

set config=%1
if "%config%" == "" (
   set config=Release
)

set version=
if not "%PackageVersion%" == "" (
   set version=-Version %PackageVersion%
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

REM Package restore
call %NuGet% restore AssemblySoft.DevOps\packages.config -OutputDirectory %cd%\packages -NonInteractive

echo building solution
"%programfiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe" AssemblySoft.DevOps.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=Normal /nr:false

echo creating nuget packages
mkdir Build
call %nuget% pack "AssemblySoft.DevOps\AssemblySoft.DevOps.csproj" -IncludeReferencedProjects -verbosity detailed -o Build -p Configuration=%config% %version%
call %nuget% pack "AssemblySoft.DevOps.Common\AssemblySoft.DevOps.Common.csproj" -IncludeReferencedProjects -verbosity detailed -o Build -p Configuration=%config% %version%

echo complete custom build
