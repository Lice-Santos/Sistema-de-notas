namespace SafeScribe_cp05.Interface
{
    public interface ITokenBlacklistService
    {
        Task AddToBlacklistAsync(string jti);

        Task<bool> IsBlacklistedAsync(string jti);
    }
}
