using ChatChannel.Infraustructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChatChannel.Infraustructure.UnitOfWork.IUnitOfWork;

namespace ChatChannel.Infraustructure.UnitOfwork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChatDbContext _context;

        public UnitOfWork(ChatDbContext context)
        {
            _context = context;
        }

        public async Task<SavingResult> Commit(CancellationToken ct)
        {
            var savedChangedStateCount = await _context.SaveChangesAsync(ct);
            return new SavingResult { ChangesCount = savedChangedStateCount };
        }
    }
}
