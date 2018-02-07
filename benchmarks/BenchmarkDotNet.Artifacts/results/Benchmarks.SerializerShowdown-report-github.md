``` ini

BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical cores and 2 physical cores
Frequency=2742188 Hz, Resolution=364.6723 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host]     : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT


```
|            Method |         Mean |      Error |      StdDev |       Median |
|------------------ |-------------:|-----------:|------------:|-------------:|
| JsonApiDotNetCore |     4.093 us |  0.2806 us |   0.7775 us |     3.774 us |
|             Saule | 2,450.410 us | 77.3559 us | 207.8117 us | 2,361.890 us |
|         Partytime |    16.626 us |  0.4527 us |   1.2392 us |    16.277 us |
