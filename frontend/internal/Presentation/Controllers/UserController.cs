using Microsoft.AspNetCore.Mvc;
using Models;
using Services.IService;

namespace AppointmentScheduler.Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Hành động này sẽ hiển thị danh sách người dùng
        public IActionResult Index()
        {
            var users = _userService.GetAllUsers();
            return View(users);
        }

        // Hành động để hiển thị form tạo người dùng mới
        public IActionResult Create()
        {
            return View();
        }

        // Hành động POST để tạo người dùng mới
        [HttpPost]
        public IActionResult Create(UserViewModel user)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Id = (uint)(_userService.CountUsers() + 1);

            // Thêm người dùng
            if (_userService.AddUser(user))
            {
                TempData["Success"] = "Thêm mới người dùng thành công.";
                return RedirectToAction(nameof(Index));
            }

            // Xử lý khi có lỗi
            TempData["Error"] = "Đã có lỗi xảy ra khi thêm người dùng.";
            return View(user);
        }

        // Hành động GET để hiển thị form chỉnh sửa người dùng
        public IActionResult Edit(uint id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        // Hành động POST để lưu cập nhật người dùng
        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            // Kiểm tra tính hợp lệ của model
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Cập nhật người dùng
            var existingUser = _userService.GetUserById(user.Id);
            if (existingUser == null)
            {
                TempData["Error"] = "Không tìm thấy người dùng.";
                return RedirectToAction(nameof(Index));
            }

            // Cập nhật thông tin người dùng
            if (_userService.UpdateUser(user))
            {
                TempData["Success"] = "Cập nhật người dùng thành công.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Đã có lỗi xảy ra khi cập nhật người dùng.";
            return View(user);
        }


        [HttpPost]
        public IActionResult Delete(uint id)
        {
            if (_userService.DeleteUser(id))
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
