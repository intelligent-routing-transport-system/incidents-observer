using incidents_observer.Context;
using System.Threading.Tasks;
using incidents_observer.Repository.Message;

namespace incidents_observer.Repository.UnityOfWork
{
    public class UnityOfWork: IUnityOfWork
    {
        private MessageRepository _messageRepository;
        public AppDbContext _context;
        public UnityOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                return _messageRepository = _messageRepository ?? new MessageRepository(_context);
            }
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
