using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

string webhook =
    "Webhook URL goes here";

string queueFile =
    @"C:\RunUO\DiscordQueue.txt";

Console.WriteLine("Discord Relay Started");
Console.WriteLine("Watching: " + queueFile);

HttpClient client = new HttpClient();

while (true)
{
    try
    {
        if (!File.Exists(queueFile))
        {
            Console.WriteLine("Queue file not found.");
            await Task.Delay(5000);
            continue;
        }

        string[] lines = File.ReadAllLines(queueFile);

        if (lines.Length == 0)
        {
            await Task.Delay(1000);
            continue;
        }

        foreach (string line in lines)
        {
            if (String.IsNullOrWhiteSpace(line))
                continue;

            Console.WriteLine("Found line: " + line);

            string message = line;

            if (line.Contains("|"))
            {
                string[] split = line.Split('|');

                if (split.Length >= 2)
                    message = split[1];
            }

            string json =
                "{\"content\":\"" +
                message.Replace("\\", "\\\\")
                       .Replace("\"", "\\\"") +
                "\"}";

            HttpResponseMessage response =
                await client.PostAsync(
                    webhook,
                    new StringContent(
                        json,
                        Encoding.UTF8,
                        "application/json"));

            Console.WriteLine(
                "Discord Response: " +
                (int)response.StatusCode +
                " " +
                response.StatusCode);
        }

        File.WriteAllText(queueFile, String.Empty);
    }
    catch (Exception ex)
    {
        Console.WriteLine("ERROR:");
        Console.WriteLine(ex.ToString());
    }

    await Task.Delay(1000);
}