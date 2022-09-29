@echo off
@echo Deleting all BIN and OBJ folders
for /d /r . %%d in (bin,obj,node_modules) do @if exist "%%d" rd /s/q "%%d"
@echo BIN and OBJ folders successfully deleted
@echo press any button to close the window.
pause > nul