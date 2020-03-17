## Removing cookbook from GIT history 

> ⚠️ **DO NOT DO THIS AGAIN!** The git history was modified on 2019-03-15 to remove the cookbook and it caused
problems for people who were maintaining long-running forks ([#305](https://github.com/swharden/ScottPlot/issues/305))

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