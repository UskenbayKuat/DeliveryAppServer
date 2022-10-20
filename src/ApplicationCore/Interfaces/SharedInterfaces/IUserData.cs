using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationCore.Interfaces.SharedInterfaces
{
    public interface IUserData
    {
        public Task<ActionResult> SendUser(string userId, CancellationToken cancellationToken);
    }
}