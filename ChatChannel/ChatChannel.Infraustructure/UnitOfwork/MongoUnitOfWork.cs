using ChatChannel.Infraustructure.UnitOfWork;

namespace ChatChannel.Infraustructure.UnitOfwork
{
    public class MongoUnitOfWork : IUnitOfWork
    {
        public async Task<int> Commit(CancellationToken ct)
        {
            return default(int);
        }
    }
}
