using incidents_observer.Repository.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace incidents_observer.Repository.UnityOfWork
{
    public interface IUnityOfWork
    {
        IMessageRepository MessageRepository { get; }
        Task Commit();
    }
}
