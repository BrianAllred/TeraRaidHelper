using PokeApiNet;
using RaidHelper.Utilities;
using Type = PokeApiNet.Type;

namespace RaidHelper.Data;

public class PokemonService
{
    private readonly PokeApiClient client = new();

    public async Task<TypeModel[]> GetTypesAsync()
    {
        var typesPage = await client.GetNamedResourcePageAsync<Type>();
        var types = (await client.GetResourceAsync(typesPage.Results)).Where(type => type.Pokemon.Count > 0).Select(type => type.ToModel());
        while (!string.IsNullOrEmpty(typesPage.Next))
        {
            typesPage = await client.GetNextResourcePageAsync(typesPage);
            types = types.Concat((await client.GetResourceAsync(typesPage.Results)).Where(type => type.Pokemon.Count > 0).Select(p => p.ToModel()));
        }

        return types.ToArray();
    }

    public async Task<TypeModel> GetDamageRelationsAsync(TypeModel model)
    {
        if (model.Id == 0 || string.IsNullOrEmpty(model.Name))
        {
            return model;
        }

        var type = await client.GetResourceAsync<Type>(model.Id);
        var notEffectiveAgainst = await client.GetResourceAsync(type.DamageRelations.NoDamageTo);
        notEffectiveAgainst.AddRange(await client.GetResourceAsync(type.DamageRelations.HalfDamageTo));

        var superEffectiveAgainst = await client.GetResourceAsync(type.DamageRelations.DoubleDamageTo);

        var resistantTo = await client.GetResourceAsync(type.DamageRelations.HalfDamageFrom);
        resistantTo.AddRange(await client.GetResourceAsync(type.DamageRelations.NoDamageFrom));

        var weakTo = await client.GetResourceAsync(type.DamageRelations.DoubleDamageFrom);

        model.NotEffectiveAgainst = notEffectiveAgainst.ToModels().ToList();
        model.ResistantTo = resistantTo.ToModels().ToList();
        model.SuperEffectiveAgainst = superEffectiveAgainst.ToModels().ToList();
        model.WeakTo = weakTo.ToModels().ToList();

        return model;
    }

    public async Task<TypeModel[]> GetRecommendedTypes(TypeModel teraType, TypeModel naturalTypeOne, TypeModel naturalTypeTwo)
    {
        var offensiveTypes = await GetGoodOffensiveTypes(teraType);
        var defensiveTypes = await GetGoodDefensiveTypes(naturalTypeOne, naturalTypeTwo);

        return offensiveTypes.Concat(defensiveTypes).DistinctBy(type => type.Id).ToArray();
    }

    public async Task<TypeModel[]> GetNotRecommendedTypes(TypeModel teraType, TypeModel naturalTypeOne, TypeModel naturalTypeTwo)
    {
        var offensiveTypes = await GetBadOffensiveTypes(teraType);
        var defensiveTypes = await GetBadDefensiveTypes(naturalTypeOne, naturalTypeTwo);

        return offensiveTypes.Concat(defensiveTypes).DistinctBy(type => type.Id).ToArray();
    }

    public async Task<TypeModel[]> GetGoodOffensiveTypes(TypeModel teraType)
    {
        teraType = await GetDamageRelationsAsync(teraType);

        return teraType.WeakTo.ToArray();
    }

    public async Task<TypeModel[]> GetBadOffensiveTypes(TypeModel teraType)
    {
        teraType = await GetDamageRelationsAsync(teraType);

        return teraType.ResistantTo.ToArray();
    }

    public async Task<TypeModel[]> GetGoodDefensiveTypes(TypeModel naturalTypeOne, TypeModel naturalTypeTwo)
    {
        naturalTypeOne = await GetDamageRelationsAsync(naturalTypeOne);
        naturalTypeTwo = await GetDamageRelationsAsync(naturalTypeTwo);

        var safeTypes = naturalTypeOne.NotEffectiveAgainst.Concat(naturalTypeTwo.NotEffectiveAgainst).DistinctBy(type => type.Id);
        var unsafeTypes = naturalTypeOne.SuperEffectiveAgainst.Concat(naturalTypeTwo.SuperEffectiveAgainst).DistinctBy(type => type.Id);

        return safeTypes.Except(unsafeTypes).ToArray();
    }

    public async Task<TypeModel[]> GetBadDefensiveTypes(TypeModel naturalTypeOne, TypeModel naturalTypeTwo)
    {
        naturalTypeOne = await GetDamageRelationsAsync(naturalTypeOne);
        naturalTypeTwo = await GetDamageRelationsAsync(naturalTypeTwo);

        var types = naturalTypeOne.SuperEffectiveAgainst.Concat(naturalTypeTwo.SuperEffectiveAgainst).DistinctBy(type => type.Id).ToArray();
        return types;
    }
}