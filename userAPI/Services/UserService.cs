using userAPI.Models;

namespace userAPI.Services
{
    public interface IUserService {
        Task<List<User>> GetAll();
        Task Add(User user);
        Task<User?> Get(int id);
        Task Update(User user);
        Task<List<User>> Search(string text);
    }

    public class UserService : IUserService
    {

        //in-memory users
        static List<User> Users { get; }
        static int nextId = 3;
        static UserService()
        {
            Users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@email.com"},
            new User { Id = 2, Name = "Bob", Email = "bob@email.com"}
        };
        }

        //Using task.run to simulate async behavior
        public async Task<List<User>> GetAll() => await Task.Run(() => Users);

        public async Task Add(User user)
        {
            user.Id = nextId++;
            await Task.Run(() => Users.Add(user));
        }

        public async Task<User?> Get(int id) => await Task.Run(() => Users.FirstOrDefault(i => i.Id == id));

        public async Task Update(User user)
        {
            var index = Users.FindIndex(i => i.Id == user.Id);
            if (index == -1)
                await Task.CompletedTask;
            await Task.Run(() => Users[index] = user);
        }

        public async Task<List<User>> Search(string text) => await Task.Run(() => Users.FindAll(i => i.Name.Contains(text, System.StringComparison.CurrentCultureIgnoreCase)));
    }
}
