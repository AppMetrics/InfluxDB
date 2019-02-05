``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.292 (1809/October2018Update/Redstone5)
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.1.301
  [Host] : .NET Core 2.1.7 (CoreCLR 4.6.27129.04, CoreFX 4.6.27129.04), 64bit RyuJIT
  Core   : .NET Core 2.1.7 (CoreCLR 4.6.27129.04, CoreFX 4.6.27129.04), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|                          Method |     Mean |     Error |    StdDev |   Median | Ratio | RatioSD | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------------------------------- |---------:|----------:|----------:|---------:|------:|--------:|------------:|------------:|------------:|--------------------:|
|    WriteSingleValuePoint_Legacy | 558.0 ns | 11.067 ns | 25.206 ns | 549.2 ns |  1.00 |    0.00 |      0.1478 |           - |           - |               624 B |
|           WriteSingleValuePoint | 288.3 ns |  5.704 ns |  8.881 ns | 286.4 ns |  0.51 |    0.03 |      0.0725 |           - |           - |               304 B |
|                                 |          |           |           |          |       |         |             |             |             |                     |
| WriteMultipleValuesPoint_Legacy | 991.9 ns | 19.413 ns | 31.348 ns | 984.6 ns |  1.00 |    0.00 |      0.2012 |           - |           - |               848 B |
|        WriteMultipleValuesPoint | 352.8 ns |  6.631 ns |  6.202 ns | 353.1 ns |  0.36 |    0.01 |      0.0873 |           - |           - |               368 B |
