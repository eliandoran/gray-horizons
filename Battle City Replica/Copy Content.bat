@echo off
rmdir /s /q %1\Bin\Debug\Content\
mkdir %1\Bin\Debug\Content\
xcopy /E "BattleCity\Content\bin\Windows\*" %1\Bin\Debug\Content\