#!/bin/bash
xbuild C#/animestr.sln
mono C#/animestr/bin/Debug/animestr.exe
git add *
git commit -m "updates (auto)"
git push origin master