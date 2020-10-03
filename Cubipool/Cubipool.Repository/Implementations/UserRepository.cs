using Cubipool.Entity;
using Cubipool.Repository.Abstractions;
using Cubipool.Repository.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cubipool.Repository.Implementations
{
	public class UserRepository : IUserRepository
	{
		private readonly EFDbContext _context;

		public UserRepository(EFDbContext context)
		{
			this._context = context;
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			return await _context
				.Users
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<User> FindOneByIdAsync(int id)
		{
			return await _context
				.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == id);
		}

		public Task<User> GetOneByStudentCodeAsync(string code)
		{
			return (
					from user in _context.Users
					where user.StudentCode == code
					select user
				).AsNoTracking()
				.FirstOrDefaultAsync();
		}

		public async Task<User> FindOneByStudentCodeAsync(string code)
		{
			return await _context
				.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.StudentCode == code);
		}

		public async Task<User> CreateOneAsync(User user)
		{
			await _context.Users.AddAsync(user);
			await _context.SaveChangesAsync();

			_context.Entry(user).State = EntityState.Detached;
			return user;
		}

		public async Task<User> DeleteOneAsync(User user)
		{
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			_context.Entry(user).State = EntityState.Detached;
			return user;
		}
	}
}