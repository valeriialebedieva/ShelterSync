using System.Text.Json;
using ShelterSync.Models;

namespace ShelterSync.Services;

public class AdoptionService
{
    private readonly string _dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "adoption_requests.json");
    private List<AdoptionRequest> _requests = new();
    private int _nextId = 1;

    public AdoptionService()
    {
        LoadOrInitialize();
    }

    private void LoadOrInitialize()
    {
        try
        {
            if (File.Exists(_dataFilePath))
            {
                var json = File.ReadAllText(_dataFilePath);
                _requests = JsonSerializer.Deserialize<List<AdoptionRequest>>(json) ?? new List<AdoptionRequest>();
                if (_requests.Any()) _nextId = _requests.Max(r => r.Id) + 1;
            }
        }
        catch
        {
            _requests = new List<AdoptionRequest>();
        }
    }

    private void Save()
    {
        try
        {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            var json = JsonSerializer.Serialize(_requests, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving adoption requests: {ex.Message}");
        }
    }

    public IEnumerable<AdoptionRequest> GetAll() => _requests.OrderByDescending(r => r.RequestedAt);

    public void Add(AdoptionRequest req)
    {
        req.Id = _nextId++;
        req.RequestedAt = DateTime.UtcNow;
        _requests.Add(req);
        Save();
    }
}
