using Microsoft.Extensions.Logging;
using SmartDepoLib.Toolbelt;

namespace SmartDepoLib.Tests;

public class TramDataFixture
{
    public string TramJsonString { get; }

    public TramDataFixture()
    {
        // Load the JSON file once
        var jsonFilePath = "TestData/trams.json";
        TramJsonString = File.ReadAllText(jsonFilePath);
    }
}

public class SmartDepoTests : IClassFixture<TramDataFixture>
{
    private readonly ILogger<SmartDepo> _logger;
    private readonly string _tramJsonString;

    public SmartDepoTests(TramDataFixture fixture)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        _logger = loggerFactory.CreateLogger<SmartDepo>();
        _tramJsonString = fixture.TramJsonString;
    }

    [Fact]
    public void CreateDepo_Creation_ShouldReturnAssortedData()
    {
        // Arrange
        _logger.LogInformation("Test Data: {TramJsonString}", _tramJsonString);

        // Act
        var smartDepo = SmartDepo.CreateDepo(_tramJsonString);

        // Assert
        Assert.NotNull(smartDepo);
        var trams = smartDepo.GetTrams().ToList();

        Assert.Equal(5, trams.Count); // Ensure the correct number of trams is loaded
        Assert.Equal("Tram-1", trams[0].Name); // Check the first tram's name
        Assert.Equal("Mission-Alpha", trams[0].CurrentMission); // Check the first tram's mission
    }

    [Fact]
    public async Task AssignMissionAsync_Interaction_ShouldTriggerMissionAssignedEvent()
    {
        // Arrange
        var smartDepo = SmartDepo.CreateDepo(_tramJsonString);

        var tcs = new TaskCompletionSource<MissionAssignedEventArgs>();
        smartDepo.MissionAssigned += async (sender, args) =>
        {
            tcs.SetResult(args);
            await Task.CompletedTask;
        };

        // Act
        await smartDepo.AssignMissionAsync("Test Mission");

        // Assert
        var eventArgs = await tcs.Task; // Wait for the event to be triggered
        Assert.NotNull(eventArgs);
        Assert.Equal("Test Mission", eventArgs.Mission);
        Assert.Equal(2, eventArgs.Tram.Id);
        Assert.Equal("Tram-2", eventArgs.Tram.Name);
    }

    [Fact]
    public async Task AssignMissionAsync_Interaction_ShouldUpdateCollection()
    {
        // Arrange
        var smartDepo = SmartDepo.CreateDepo(_tramJsonString);

        var tcs = new TaskCompletionSource<MissionAssignedEventArgs>();
        smartDepo.MissionAssigned += async (sender, args) =>
        {
            tcs.SetResult(args);
            await Task.CompletedTask;
        };

        // Act
        await smartDepo.AssignMissionAsync("Test Mission");

        // Wait for the event to be triggered
        var eventArgs = await tcs.Task;

        // Assert: Verify the event was triggered with the correct tram
        Assert.NotNull(eventArgs);
        Assert.Equal("Test Mission", eventArgs.Mission);
        Assert.Equal(2, eventArgs.Tram.Id);
        Assert.Equal("Tram-2", eventArgs.Tram.Name);

        // Assert: Verify the tram collection is updated
        var trams = smartDepo.GetTrams().ToList();

        // Check that the tram with the new mission is updated
        var updatedTram = trams.FirstOrDefault(t => t.Id == 2);
        Assert.NotNull(updatedTram);
        Assert.Equal("Test Mission", updatedTram.CurrentMission);

        // Check that the rest of the collection remains unchanged
        Assert.Equal(5, trams.Count); // Ensure the total count is still correct
        Assert.Equal("Mission-Alpha", trams[0].CurrentMission); // First tram remains unchanged
        Assert.Equal("Tram-1", trams[0].Name); // First tram's name remains unchanged
    }

    [Fact]
    public async Task ResetAsync_ShouldClearAllTramsAndReinitialize()
    {
        // Arrange
        var smartDepo = SmartDepo.CreateDepo(_tramJsonString);

        // Act
        await smartDepo.ResetAsync();

        // Assert
        var tramsAfterReset = smartDepo.GetTrams().ToList();
        Assert.NotNull(tramsAfterReset);
        Assert.Empty(tramsAfterReset);
    }

}
