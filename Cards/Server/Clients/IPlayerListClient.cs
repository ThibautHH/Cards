namespace Cards.Server.Clients
{
    public interface IPlayerListClient
    {
        Task Update(string player);
    }
}