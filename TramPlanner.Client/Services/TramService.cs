using System.Net.Http.Json;
using SmartDepoLib;

namespace TramPlanner.Client.Services;

public class TramService
{
    private readonly HttpClient _httpClient;

    public TramService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Initialize SmartDepo with tram data
    public async Task<string> InitializeDepoAsync(string tramJsonString)
    {
        // Ensure tramJsonString is not null or empty
        if (string.IsNullOrWhiteSpace(tramJsonString))
        {
            throw new ArgumentException("Tram JSON string cannot be null or empty.", nameof(tramJsonString));
        }

        // Send the POST request with the JSON string
        var response = await _httpClient.PostAsJsonAsync("api/SmartDepo/initialize", tramJsonString);

        // Ensure the response is successful
        response.EnsureSuccessStatusCode();

        // Return the HTTP status code
        return response.StatusCode.ToString();
    }

    public async Task<IEnumerable<Tram>> GetTramsAsync()
    {
        return await _httpClient.GetFromJsonAsync<IEnumerable<Tram>>("api/SmartDepo/trams");
    }

    public async Task<string> AssignMissionAsync(string newMission)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/SmartDepo/assignMission/", newMission);
        response.EnsureSuccessStatusCode();
        return response.StatusCode.ToString();
    }
    
     public async Task<string> ResetDataAsync()
    {
        var response = await _httpClient.PostAsync("api/SmartDepo/reset", null);
        response.EnsureSuccessStatusCode();
        return response.StatusCode.ToString();
    }

}