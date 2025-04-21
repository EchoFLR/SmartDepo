using SmartDepoLib;
namespace TramPlanner.Server.Services;

public class SmartDepoService
{
    private SmartDepo? _smartDepo;

    public async Task InitializeAsync(string tramJsonString)
    {
        await Task.Run(() => _smartDepo = SmartDepo.CreateDepo(tramJsonString));
    }

    public async Task<IEnumerable<Tram>> GetTramsAsync()
    {
        if (_smartDepo == null)
        {
            throw new InvalidOperationException("SmartDepo is not initialized.");
        }
        return await Task.FromResult(_smartDepo.GetTrams());
    }

    public async Task AssignMissionAsync(string missionName)
    {
        if (_smartDepo == null)
        {
            throw new InvalidOperationException("SmartDepo is not initialized.");
        }
        await Task.Run(() => _smartDepo.AssignMissionAsync(missionName));
    }

    public async Task ResetData()
    {
        if (_smartDepo == null)
        {
            throw new InvalidOperationException("SmartDepo is not initialized.");
        }
        await Task.Run(() => _smartDepo.ResetAsync());
    }
}