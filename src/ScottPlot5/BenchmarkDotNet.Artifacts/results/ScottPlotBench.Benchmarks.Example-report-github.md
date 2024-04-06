```

BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3296/23H2/2023Update/SunValley3)
Intel Core i7-10700 CPU 2.90GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 8.0.200
  [Host]     : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2
  Job-PJVUAN : .NET 8.0.2 (8.0.224.6711), X64 RyuJIT AVX2

IterationCount=3  IterationTime=200.0000 ms  LaunchCount=1  
WarmupCount=1  

```
| Method           | Count | Mean          | Error          | StdDev        |
|----------------- |------ |--------------:|---------------:|--------------:|
| **AddString**        | **10**    |      **73.04 ns** |      **22.887 ns** |      **1.254 ns** |
| AddStringBuilder | 10    |      62.15 ns |       8.707 ns |      0.477 ns |
| **AddString**        | **100**   |   **2,772.67 ns** |     **334.658 ns** |     **18.344 ns** |
| AddStringBuilder | 100   |     290.31 ns |      58.136 ns |      3.187 ns |
| **AddString**        | **1000**  | **262,216.84 ns** | **296,586.780 ns** | **16,256.922 ns** |
| AddStringBuilder | 1000  |   2,092.89 ns |     167.536 ns |      9.183 ns |
