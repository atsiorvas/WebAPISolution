using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Interface {
    public interface ISaveChangesWarper : IDisposable {
        Task<bool> SaveChangesAsync();
    }
}