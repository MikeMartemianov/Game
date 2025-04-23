@echo off
set /p commitMessage=Paste commit for game: 
git add .
git commit -m "%commitMessage%"
git push
