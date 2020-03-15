ensure latest bfg is in this folder, named bfg.jar

create a mirror (don't run bfg on existing clones)
```
git clone --mirror https://github.com/swharden/ScottPlot.git
```

remove all folders named "images" from the history
```
java -jar bfg.jar --delete-folders "images" ScottPlot.git
```

clean up
```
cd ScottPlot.git
git reflog expire --expire=now --all
git gc --prune=now --aggressive
```