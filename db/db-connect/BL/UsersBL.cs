using System.Threading.Tasks;
using AccessCore.Repository;
using DbConnect.Models;

namespace DbConnect.BL
{
    /// <summary>
    /// Users business logic
    /// </summary>
    public class UsersBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="UsersBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        public UsersBL(DataManager dm)
            : base(dm, BlType.UsersBL)
        {
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>created user if everything is ok, otherwise null.</returns>
        public async Task<User> CreateUser(User user)
        {
            var result = await this.dm.OperateAsync<User, object>(
                nameof(DbOperationType.CreateUser),
                user);

            var id = (int)result;

            if (id < 10000)
                return null;

            user.Id = id;
            return user;
        }
    }
}
