using System.Text;

namespace TestFileBuilder;

public class FileGenerator
{
    public string FilePath { get; set; }
    public int NumberOfLines { get; set; }

    private static readonly Random random = new Random();
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private const int BufferSize = 4096;
    private const int BlockSize = 1000000;
    private static readonly string[] Strings = { "Apple", "Banana", "Cherry", "Something something something" };

    public FileGenerator(string filePath, int numberOfLines)
    {
        FilePath = filePath;
        NumberOfLines = numberOfLines;
        using var fs =File.Create(FilePath, 0);
    }

    public async Task GenerateFileAsync()
    {
        var tasks = new List<Task>();

        for (int i = 0; i < NumberOfLines; i += BlockSize)
        {
            int currentBlockEnd = Math.Min(i + BlockSize, NumberOfLines);
            tasks.Add(WriteBlockAsync(i, currentBlockEnd));
        }
        
        await Task.WhenAll(tasks);
    }

    private async Task WriteBlockAsync(int start, int end)
    {
        var sb = new StringBuilder();
        for (int i = start; i < end; i++)
        {
            sb.AppendLine($"{GenerateNumber()}. {GenerateString()}");
        }
        
        var data = sb.ToString();
        await semaphore.WaitAsync();
        try
        {
            using (var sw = new StreamWriter(new FileStream(FilePath, FileMode.Append, FileAccess.Write, FileShare.None, BufferSize, useAsync: true)))
            {
                await sw.WriteAsync(data);
            }
        }
        finally
        {
            semaphore.Release(); 
        }
    }

    private int GenerateNumber()
    {
        return random.Next(1, 100000);
    }

    private string GenerateString()
    {
        return Strings[random.Next(Strings.Length)];
    }
}

