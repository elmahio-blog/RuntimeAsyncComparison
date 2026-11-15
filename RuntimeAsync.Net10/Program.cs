using System.Diagnostics;

Console.WriteLine($"Runtime: {System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription}");
Console.WriteLine("Runtime Async comparison demo");
Console.WriteLine();

Console.WriteLine("1) Live stack trace after an asynchronous suspension:");
await FirstAsync();

Console.WriteLine();
Console.WriteLine("2) Small async workload (illustrative timing, not a benchmark):");

const int iterations = 20_000;

// Warm up the JIT before taking the sample.
await RunWorkloadAsync(1_000);

var stopwatch = Stopwatch.StartNew();
var result = await RunWorkloadAsync(iterations);
stopwatch.Stop();

Console.WriteLine($"Completed {iterations:N0} asynchronous yields.");
Console.WriteLine($"Result: {result:N0}");
Console.WriteLine($"Elapsed: {stopwatch.Elapsed.TotalMilliseconds:N2} ms");

static async Task FirstAsync()
{
    await SecondAsync();
}

static async Task SecondAsync()
{
    await ThirdAsync();
}

static async Task ThirdAsync()
{
    await Task.Yield();

    Console.WriteLine("Stack captured inside ThirdAsync:");
    Console.WriteLine(new StackTrace(fNeedFileInfo: false));
}

static async Task<int> RunWorkloadAsync(int iterations)
{
    var total = 0;

    for (var i = 0; i < iterations; i++)
    {
        await DoOneAsync(i);
        total += i;
    }

    return total;
}

static async Task DoOneAsync(int value)
{
    await Task.Yield();
    _ = value;
}
