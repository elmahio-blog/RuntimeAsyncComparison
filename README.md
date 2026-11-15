# .NET 10 vs .NET 11 Runtime Async console demo

Both projects use the exact same `Program.cs`. Neither project requires a third-party NuGet package.

## Projects

- `RuntimeAsync.Net10`: targets `net10.0`; uses the traditional compiler-generated async state-machine implementation.
- `RuntimeAsync.Net11`: targets `net11.0`; opts the application's eligible async methods into Runtime Async with `<Features>$(Features);runtime-async=on</Features>`.

## Run

Install the .NET 10 and .NET 11 SDKs, then run each project:

```bash
dotnet run --project RuntimeAsync.Net10
dotnet run --project RuntimeAsync.Net11
```

## What to observe

1. Both applications perform the same nested `async`/`await` calls and capture a live `StackTrace` after an asynchronous suspension.
2. Both run the same small `Task.Yield()` workload and print elapsed time.
3. Compare the live stack traces: Runtime Async can show cleaner async call stacks without the compiler-generated state-machine infrastructure.

## Important interpretation

- The source code is intentionally identical. Runtime Async changes how eligible async methods are implemented, not the C# programming model.
- .NET 11 Runtime Async is opt-in for application code in this example. ASP.NET Core shared-framework-only libraries targeting .NET 11 are already built with Runtime Async.
- The stopwatch output is only a demonstration. It is not a reliable performance benchmark: results depend on hardware, runtime version, JIT warm-up, background load, and other factors. Do not promise that the .NET 11 run will always be faster.
- Runtime Async does not remove NuGet dependencies. These examples simply need no third-party packages in either version.
