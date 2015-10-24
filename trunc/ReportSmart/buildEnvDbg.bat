cd ..\..\..\Debug\bin

md ..\..\Releases\Debug\gfx
md ..\..\Releases\Debug\locale
copy /y ..\gfx ..\..\Releases\Debug\gfx\
copy /y ..\locale ..\..\Releases\Debug\locale\
copy /y ..\bin\DotNetAsmInfo.dll ..\..\Releases\Debug\bin\

echo Build environment done.

pause