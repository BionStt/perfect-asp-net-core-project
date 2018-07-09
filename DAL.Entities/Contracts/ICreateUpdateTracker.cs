using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.Contracts
{
    public interface ICreateUpdateTracker
    {
        DateTime CreatedTime { get; set; }

        DateTime? LastUpdate { get; set; }
    }
}
