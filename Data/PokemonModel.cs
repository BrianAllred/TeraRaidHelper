namespace RaidHelper.Data;

public class PokemonModel
{
    public int NationalDexNumber { get; set; }
    public string? Name { get; set; }

    public PokemonModel(int nationalDexNumber, string name)
    {
        NationalDexNumber = nationalDexNumber;
        Name = name;
    }
}