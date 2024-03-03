using System.Linq;
using System.Linq.Expressions;
using TmpApi.Utilities;

namespace TmpApi.Services;

public interface IHomeService
{
    Task<ServiceResponse<HomeDetail>> Create(HomeCreate model);
}

public interface IServiceResponse
{
    public Dictionary<string, string> Errors { get; set; }
}

public class ServiceResponse<T> : IServiceResponse
    where T : class
{
    public T? Item { get; set; }

    public bool Success => Errors.Count == 0;

    public Dictionary<string, string> Errors { get; set; } = [];
}
public class HomeService : IHomeService
{
    private readonly ApplicationDbContext _dbContext;

    public HomeService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ServiceResponse<HomeDetail>> Create(HomeCreate model)
    {
        var response = ResponseHelper.Create<HomeDetail>();

        // můžu použít first nebo any, ale chci ověřit, že se mi tam nějak nedostal zdvojený název
        var uniqueCheck = _dbContext.Set<Home>().SingleOrDefault(x => x.Name == model.Name);
        if (uniqueCheck == null)
        {
            // ZAPIŠ ERROR
            response.AddError(model.Name, "name is not unique");
        }

        var slugError = string.Empty;
        var forbiddenSymbols = new char[] { '%' };
        for (int i = 0; i < forbiddenSymbols.Length; i++)
        {
            var sForbCheck = model.NameSlug.Contains(forbiddenSymbols[i]);
            if (sForbCheck)
            {
                //ZAPIŠ ERROR
                response.AddError(model.Name, "forbidden characters in slug");
                break;
            }
        }
        if (string.IsNullOrEmpty(slugError))
        {
            var sExistCheck = _dbContext.Set<Home>().SingleOrDefault(x => x.NameSlug == model.NameSlug);
            if (sExistCheck == null)
            {
                response.AddError(model.Name, "slug is not unique");
                // ZAPIŠ ERROR
            }
        }

        var connectionCheck = _dbContext.Set<Home>().FirstOrDefault(x => x.Id == model.ConnectionId);
        if (connectionCheck == null)
        {
            // ZAPIŠ ERROR
            response.AddError(model.Name, "connection does not exist");
        }

        var result = _dbContext.Set<Home>().Select(HomeModelExtensions.ProjectFromEntity).Single();
        response.Item = result;
        return response;
    }
}

public class HomeCreate
{
    public string Name { get; set; } = null!;
    public string NameSlug { get; set; } = null!;
    public Guid ConnectionId { get; set; }
    public List<string> SomeIds { get; set; } = new();
}

public static partial class HomeModelExtensions
{
    public static Home FromCreate(this HomeCreate source)
        => new();
}

public class HomeDetail
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string NameSlug { get; set; } = null!;
    public Guid ConnectionId { get; set; }
    public List<string> SomeIds { get; set; } = new();
}

public static partial class HomeModelExtensions
{
    /// <summary>
    /// Nepotřebuješ Include, selectuje se to přímo v DB.
    /// </summary>
    public static Expression<Func<Home, HomeDetail>> ProjectFromEntity => x => new HomeDetail
    {
        Id = x.Id,
        Name = x.Name,
        SomeIds = /*x.RelatedEntity.Select(xx => x.Id),*/ Enumerable.Empty<string>().ToList(),
    };
}

