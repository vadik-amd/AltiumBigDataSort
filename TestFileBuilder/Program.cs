// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using TestFileBuilder;

Console.WriteLine("Enter the file path:");
var filePath = Console.ReadLine();
filePath = string.IsNullOrEmpty(filePath)
    ? "/Volumes/SSD/11/11.txt"
    : filePath;

Console.WriteLine("Enter the number of lines:");
var lines = Console.ReadLine();
var numberOfLines = int.Parse(string.IsNullOrEmpty(lines) ? "200000000" : lines);

var generator = new FileGenerator(filePath, numberOfLines);

Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

generator.GenerateFileAsync().GetAwaiter().GetResult();

stopwatch.Stop();
TimeSpan ts = stopwatch.Elapsed;

Console.WriteLine("The file was created successfully.");
Console.WriteLine($"Elapsed Time: {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}");