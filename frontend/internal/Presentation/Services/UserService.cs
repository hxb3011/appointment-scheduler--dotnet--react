using System.Diagnostics;
using AppointmentScheduler.Presentation.Models;

namespace AppointmentScheduler.Presentation.Services;

public class UserService
{
    private readonly List<UserModel> _users;
    public UserService()
    {
        _users = new List<UserModel>();
    }
    public IEnumerable<UserModel> GetAllUsers()
    {
        // Dummy users for testing purposes
        if (!_users.Any()) // If no users are in the list, add some for testing
        {
            _users.Add(new UserModel(1, "user1", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(2, "user2", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(3, "user3", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(4, "user4", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(5, "user5", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(6, "user6", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(7, "user7", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(8, "user8", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(9, "user9", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(10, "user10", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(11, "user11", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(12, "user12", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(13, "user13", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(14, "user14", "Password2@", "Nguyen B", 'F', "Address 2"));
            _users.Add(new UserModel(15, "user15", "Password1!", "Nguyen A", 'M', "Address 1"));
            _users.Add(new UserModel(16, "user16", "Password2@", "Nguyen B", 'F', "Address 2"));
        }
        return _users;
    }
    public UserModel GetUserById(uint id)
    {
        var user = _users.Find(x => x.Id == id);
        return user;
    }
    public bool AddUser(UserModel user)
    {
        try
        {
            // Simulate adding a user (In real-world, this could be a database operation)
            _users.Add(user);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while adding user: " + ex.Message);
            return false;
        }
    }
    public bool UpdateUser(UserModel user)
    {
        try
        {
            // Tìm người dùng theo ID
            var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser == null)
            {
                // Nếu không tìm thấy người dùng, trả về false
                return false;
            }
            // Cập nhật thông tin người dùng
            existingUser.UserName = user.UserName;
            existingUser.Password = user.Password;
            existingUser.FullName = user.FullName;
            existingUser.Gender = user.Gender;
            existingUser.Address = user.Address;
            existingUser.RoleId = user.RoleId;
            // Trả về true nếu cập nhật thành công
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating user: " + ex.Message);
            return false;
        }
    }
    public bool DeleteUser(uint id)
    {
        try
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return false;
            }
            _users.Remove(existingUser);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating user: " + ex.Message);
            return false;
        }
    }
    public int CountUsers()
    {
        return _users.Count;
    }
}