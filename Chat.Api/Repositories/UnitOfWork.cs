using Chat.Api.Context;

namespace Chat.Api.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {

        private readonly ChatDbContext _context;

        public UnitOfWork(ChatDbContext context, IUserRepository userRepository)
        {
            _context = context;

        }


        private IUserRepository? _userRepository { get; set; }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository==null)
                {
                    return new UserRepository(_context);
                }
                return _userRepository;
            }
        }

        private IChatRepository? _chatRepository { get; set; }

        public IChatRepository ChatRepository
        {
            get
            {
                if (_chatRepository==null)
                {
                    return new ChatRepository(_context);
                }

                return _chatRepository;
            }
        }



        private IUserChatRepository? _userChatRepository { get; set; }

        public IUserChatRepository UserChatRepository
        {
            get
            {
                if (_userChatRepository==null)
                {
                    return new UserChatRepository(_context);
                }
                return _userChatRepository;
            }
        }



        private IMessageRepository? _messageRepository{ get; set; }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (_messageRepository==null)
                {
                    return new MessageRepository(context:_context);
                }
                return _messageRepository;
            }
        }
    }
}
