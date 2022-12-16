namespace RaidHelper.Data;

public class TypeModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public List<TypeModel> NotEffectiveAgainst { get; set; } = new();
    public List<TypeModel> SuperEffectiveAgainst { get; set; } = new();
    public List<TypeModel> ResistantTo { get; set; } = new();
    public List<TypeModel> WeakTo { get; set; } = new();

    public override bool Equals(object? obj)
    {
        if (obj is not TypeModel otherTypeModel) return false;
        return Id == otherTypeModel.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}