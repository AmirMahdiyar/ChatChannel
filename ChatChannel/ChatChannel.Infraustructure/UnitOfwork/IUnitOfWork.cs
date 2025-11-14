namespace ChatChannel.Infraustructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public Task<int> Commit(CancellationToken ct);

    }
}
