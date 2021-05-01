using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using incidents_observer.Models;

namespace incidents_observer.Repository.Message
{
    public interface IMessageRepository:IRepository<Models.Message>
    {
    }
}
