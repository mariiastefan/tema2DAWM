using Core.Dtos;
using Core.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Project.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private UserService userService {  get; set; }
        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("/register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterDto payload)
        {
            userService.Register(payload);
            return Ok();
        }

        [HttpPost("/login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDto payload)
        {
            var jwtToken = userService.Validate(payload);

            return Ok(new { token = jwtToken });
        }

        [HttpGet("test-auth")]
        public IActionResult TestLogin()
        {

            ClaimsPrincipal user = User;

            var result = "";

            foreach (var claim in user.Claims)
            {
                result += claim.Type + " : " + claim.Value + "\n";
            }



            var hasRole_user = user.IsInRole("User");
            var hasRole_teacher = user.IsInRole("Teacher");

            return Ok(result);
        }

        [HttpGet("students-only")]
        [Authorize(Roles = "Student")]
        public ActionResult<string> HelloStudents()
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;
            if (role == "Student")
            {
                return Ok("Hello students!");
            }
            else
                return BadRequest();
        }

        [HttpGet("teacher-only")]
        [Authorize(Roles = "Teacher")]
        public ActionResult<string> HelloTeachers()
        {
            return Ok("Hello teachers!");
        }


        [HttpGet("/get-all")]
        public ActionResult<List<Student>> GetAll()
        {
            var results = userService.GetAll();

            return Ok(results);
        }

        [HttpGet("/get/{userId}")]
        [AllowAnonymous]
        public ActionResult<User> GetById(int userId)
        {
            var result = userService.GetById(userId);

            if (result == null)
            {
                return BadRequest("User not found");
            }

            return Ok(result);
        }

        [HttpGet("/get-grades/{userId}")]
        [Authorize(Roles = "Teacher, Student")]
        public IActionResult GetGrades(int userId)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return Unauthorized();
            }

            if (role == "Student")
            {
                var id = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
                if (id == null)
                {

                    return BadRequest();
                }

                var grades = userService.GetGradesById(int.Parse(id));
                return Ok(grades);
            }
            else
            {
                var grades = userService.GetGradesForTeacher();

                return Ok(grades);
            }
        }

        //[HttpPatch("edit-name")]
        //public ActionResult<bool> GetById([FromBody] StudentUpdateDto studentUpdateModel)
        //{
        //    var result = userService.EditName(studentUpdateModel);

        //    if (!result)
        //    {
        //        return BadRequest("Student could not be updated.");
        //    }

        //    return result;
        //}

        //[HttpPost("grades-by-course")]
        //public ActionResult<GradesByStudent> Get_CourseGrades_ByStudentId([FromBody] StudentGradesRequest request)
        //{
        //    var result = userService.GetGradesById(request.StudentId, request.CourseType);
        //    return Ok(result);
        //}

        //[HttpGet("{classId}/class-students")]
        //public IActionResult GetClassStudents([FromRoute] int classId)
        //{
        //    var results = userService.GetClassStudents(classId);

        //    return Ok(results);
        //}

        //[HttpGet("grouped-students")]
        //public IActionResult GetGroupedStudents()
        //{
        //    var results = userService.GetGroupedStudents();

        //    return Ok(results);
        //}
    }
}
