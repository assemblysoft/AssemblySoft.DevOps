set Ver=%1
set rubyPath="C:\Ruby193\bin"
set SevenZipPath="C:\Program Files\7-Zip\7z.exe"

set continuity=C:\Development\GI\continuity
set c5Root=C:\Development\GI\continuity\c5
set siteRoot=C:\Development\GI\continuity\c5\lv-installation

set brandFolder=lv

set deployOutput=C:\Development\GI\continuity\deployment\%Ver%\release-packages

call %c5Root%\deploy\automated-deploy.bat %Ver% buildOnly
