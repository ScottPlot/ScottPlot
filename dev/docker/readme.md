The goal of this folder is to be able to run the ScottPlot tests in a Docker container

```
docker compose up
```

This is close to working, but NUnit3 chokes 😩

```
System.IO.FileNotFoundException: 
  Could not load file or assembly 'Microsoft.Bcl.AsyncInterfaces, Culture=neutral, PublicKeyToken=null'. 
  The system cannot find the file specified.
```