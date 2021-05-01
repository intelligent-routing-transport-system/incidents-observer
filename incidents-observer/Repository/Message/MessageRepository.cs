using incidents_observer.Context;

namespace incidents_observer.Repository.Message
{
    public class MessageRepository:Repository<Models.Message>,IMessageRepository
    {
        public MessageRepository(AppDbContext context): base(context)
        {

        }
    }
}
