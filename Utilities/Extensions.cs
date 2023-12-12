using System.Web;
using PokeApiNet;
using RaidHelper.Data;

namespace RaidHelper.Utilities;

public static class Extensions
{
    public static Task<NamedApiResourceList<T>> GetNextResourcePageAsync<T>(this PokeApiClient client, string page)
        where T : NamedApiResource
    {
        var uri = new Uri(page);
        var queryParams = HttpUtility.ParseQueryString(uri.Query);
        var offset = int.Parse(queryParams.Get("offset") ?? "0");
        var limit = int.Parse(queryParams.Get("limit") ?? "20");
        return client.GetNamedResourcePageAsync<T>(limit, offset);
    }

    public static TypeModel ToModel(this PokeApiNet.Type type)
    {
        var name = type.Names.First(name => name.Language.Name == "en").Name;
        return new TypeModel { Name = name, Id = type.Id };
    }

    public static IEnumerable<TypeModel> ToModels(this IEnumerable<PokeApiNet.Type> types)
    {
        foreach (var type in types)
        {
            var name = type.Names.First(name => name.Language.Name == "en").Name;
            yield return new TypeModel { Name = name, Id = type.Id };
        }
    }
}