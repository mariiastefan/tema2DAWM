using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository: RepositoryBase<User>
    {
        private readonly AppDbContext dbContext;

        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public User GetByEmail(string email)
        {
            return dbContext.Users.FirstOrDefault(s => s.Email == email);
        }
        public User GetByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(s => s.Username == username);
        }
    }
}
