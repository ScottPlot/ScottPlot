```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
Intel Core i7-10700 CPU 2.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-JVRUTG : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

IterationCount=3  IterationTime=200.0000 ms  LaunchCount=1  
WarmupCount=1  

```
| Method       | Points | Mean       | Error      | StdDev    |
|------------- |------- |-----------:|-----------:|----------:|
| **ScatterLines** | **100**    |   **1.886 ms** |  **0.2302 ms** | **0.0126 ms** |
| **ScatterLines** | **1000**   |  **11.313 ms** |  **5.9995 ms** | **0.3289 ms** |
| **ScatterLines** | **10000**  |  **95.635 ms** |  **7.5858 ms** | **0.4158 ms** |
| **ScatterLines** | **100000** | **933.816 ms** | **83.1581 ms** | **4.5582 ms** |
