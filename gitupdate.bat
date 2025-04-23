@echo off
set /p commitMessage=¬ведите комментарий к коммиту: 
git add .
git commit -m "%commitMessage%"
git push
