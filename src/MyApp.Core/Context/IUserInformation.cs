using System;
using System.Collections.Generic;
using System.Text;

namespace MyApp.Core.Context
{
    /// <summary>
    /// Represents information from the current authenticated user
    /// </summary>
    public interface IUserInformation
    {
        string UserName { get; }
    }
}
