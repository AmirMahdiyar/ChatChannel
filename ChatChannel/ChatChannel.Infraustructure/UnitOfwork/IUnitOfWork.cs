using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatChannel.Infraustructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public Task<SavingResult> Commit(CancellationToken ct);
        public class SavingResult
        {
            public int ChangesCount { get; set; }
            public bool IsSucceeded => this.ChangesCount > 0;

            public void ThrowIfNoChanges<TExeption>() where TExeption : Exception, new()
            {
                if (!IsSucceeded)
                    throw new TExeption();
            }
        }

    }
}
