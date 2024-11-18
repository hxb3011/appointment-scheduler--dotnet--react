using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.IService
{
	public interface IUserService
	{
		IEnumerable<UserViewModel> GetAllUsers();
		UserViewModel GetUserById(uint id);
		bool AddUser(UserViewModel user);
		bool UpdateUser(UserViewModel user);
		bool DeleteUser(uint id);
		int CountUsers();
	}
}
