namespace Back_Almazara.Utility
{
    public interface IHashUtility
    {
        string HashPassword(string password);
        string ToBase36(long num);
        long FromBase36(string obfuscated);

    }
}
