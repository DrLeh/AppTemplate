namespace MyApp.Core.Context
{
    public interface ITenantContext
    {
        long TenantId { get; }
    }
}