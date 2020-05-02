using System;
using System.Collections.Generic;
using System.Text;

namespace Associa.Service.Core.Interfaces
{
   public interface IIdentityToken
    {
        Guid UserId { get; }
        string Role { get; }
    }
}
