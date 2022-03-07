namespace SharedLib.Security
{
    public interface IClaimsAccessor
    {
        bool TryGetValue(string type, out string value);
    }
}
