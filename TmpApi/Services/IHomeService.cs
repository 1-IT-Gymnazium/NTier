using System.Linq;

namespace TmpApi.Services;

public interface IHomeService
{
    Task<ServiceResponse<HomeDetail>> Create(HomeCreate model);
}

public class ServiceResponse<T>
    where T : class
{
    public T? Item { get; set; }

    public bool Success => Errors.Any();

    public Dictionary<string, string> Errors { get; set; } = [];
}

public class HomeService : IHomeService
{
    private readonly List<HomeDetail> _dbContext = null!;

    public HomeService()
    {
        this._dbContext = new();
    }

    public async Task<ServiceResponse<HomeDetail>> Create(HomeCreate model)
    {
        var response = new ServiceResponse<HomeDetail>();
        // můžu použít first nebo any, ale chci ověřit, že se mi tam nějak nedostal zdvojený název
        var uniqueCheck = _dbContext.SingleOrDefault(x => x.Name == model.Name);
        if (uniqueCheck == null)
        {
            // ZAPIŠ ERROR
            response.Errors.Add(nameof(model.Name), "name is not unique");
        }

        var slugError = string.Empty;
        var forbiddenSymbols = new char[] { '%' };
        for (int i = 0; i < forbiddenSymbols.Length; i++)
        {
            var sForbCheck = model.NameSlug.Contains(forbiddenSymbols[i]);
            if (sForbCheck)
            {
                //ZAPIŠ ERROR
                slugError = "text";
                break;
            }
        }
        if (string.IsNullOrEmpty(slugError))
        {
            var sExistCheck = _dbContext.SingleOrDefault(x => x.NameSlug == model.NameSlug);
            if (sExistCheck == null)
            {
                slugError = "text";
                // ZAPIŠ ERROR
            }
        }

        var connectionCheck = _dbContext.FirstOrDefault(x => x.Id == model.ConnectionId);
        if (connectionCheck == null)
        {
            // ZAPIŠ ERROR
        }

        return null;
    }
}

public class HomeCreate
{
    public string Name { get; set; } = null!;
    public string NameSlug { get; set; } = null!;
    public Guid ConnectionId { get; set; }
    public List<string> SomeIds { get; set; } = new();
}
public class HomeDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameSlug { get; set; } = null!;
    public Guid ConnectionId { get; set; }
    public List<string> SomeIds { get; set; } = new();
}
