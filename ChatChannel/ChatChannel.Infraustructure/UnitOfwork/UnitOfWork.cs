using ChatChannel.Infraustructure.UnitOfWork;

namespace ChatChannel.Infraustructure.UnitOfwork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _context;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<int> Commit(CancellationToken ct)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
