
using Client;
using System.Net.Http.Json;
using System.Text.Json;

Dictionary<string, string> cache = new Dictionary<string, string>();
HttpClient client = new HttpClient();

while (true)
{
    // Prompt the user for their name or to end the program
    Console.Write("Please enter your name or type 'end'): ");
    var name = Console.ReadLine();
    if (name?.ToLower() == "end") break;

    // Check if the greeting is already in the cache
    if (cache.TryGetValue(name, out string greeting))
    {
        Console.WriteLine($"From Cache: {greeting}");
        continue;
    }

    try
    {
        var response = await client.GetAsync($"https://localhost:7281/api/greeting?name={name}");

        if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
        {
            Console.WriteLine("Rate limit exceeded. Please try again later.");
            continue;
        }

        response.EnsureSuccessStatusCode();
        string greetingText = await response.Content.ReadAsStringAsync();
        if(greetingText == null || greetingText == null)
        {
            Console.WriteLine($"No result from API");
            return;
        }
        if(name != null)
        {
            cache[name] = greetingText;
        }
        Console.WriteLine($"Greet from API: {greetingText}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}