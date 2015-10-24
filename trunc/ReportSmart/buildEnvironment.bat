cd ..\..\..\CurrentVersion\bin

md ..\..\Releases\NightlyBuild\gfx
md ..\..\Releases\NightlyBuild\locale
copy /y ..\gfx ..\..\Releases\NightlyBuild\gfx\
copy /y ..\locale ..\..\Releases\NightlyBuild\locale\

echo Build environment done.