#!/bin/bash
cd ~/workspace/
xbuild C#/animestr.sln
#git add *
#git commit -m "updates (auto)"
#git push origin master
mono C#/animestr/bin/Debug/animestr.exe