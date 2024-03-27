using Microsoft.AspNetCore.Mvc;
using UniversityBookingProject.Models;
using UniversityBookingProject.Data;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace UniversityBookingProject.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly Context _context;

        public BookingController(Context context)
        {
            _context = context;
        }


        //Endpoint for user registration
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (existingUser != null)
            {
                return Conflict("Username already exists.");
            }

            string hashedPassword = BCryptNet.HashPassword(model.Password);

            var newUser = new User
            {
                Username = model.Username,
                PasswordHash = hashedPassword
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = newUser.Id }, newUser);
        }

        //Endpoint user login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
                return Unauthorized("Invalid username or password.");

            if (!BCryptNet.Verify(model.Password, user.PasswordHash))
                return Unauthorized("Invalid username or password.");

            var token = GenerateToken(user);

            return Ok(new {Token = token});
        }

        //Create
        [HttpPost]
        public JsonResult Create(RoomBooking booking)
        {
            if (booking.ID != 0)
            {
                if(booking.Email.ToLower().Contains("admin"))
                    booking.IsAdmin = true;
                else
                    booking.IsAdmin = false;

                _context.RoomBookings.Add(booking);
            } 
            else
            {
                var bookingInDB = _context.RoomBookings.Find(booking.ID);

                if (bookingInDB == null)
                    return new JsonResult(NotFound());

                bookingInDB = booking;
            }
            
            _context.SaveChanges();

            return new JsonResult(Ok(booking));
        }

        //Edit
        [HttpPatch]
        public JsonResult Edit(int id, RoomBooking booking)
        {
            var result = _context.RoomBookings.Find(booking.ID);

            if (result == null)
                return new JsonResult(NotFound());

            result.Email = booking.Email;
            result.PodNumber = booking.PodNumber;
            result.FirstName = booking.FirstName;
            result.LastName = booking.LastName;

            if (booking.Email.ToLower().Contains("admin"))
                result.IsAdmin = true;
            else
                result.IsAdmin = false;

            _context.SaveChanges();

            return new JsonResult(Ok(result));
        }

        //Get
        [HttpGet]
        public JsonResult Get(int id)
        {
            var result = _context.RoomBookings.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            return new JsonResult(Ok(result));
        }

        //Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.RoomBookings.Find(id);

            if (result == null)
                return new JsonResult(NotFound());

            _context.RoomBookings.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());
        }

        //GetAll
        [HttpGet]
        public JsonResult GetAll()
        {
            var result = _context.RoomBookings.ToList();

            return new JsonResult(Ok(result));
        }

        //Generate JWT Token
        private string GenerateToken(User user)
        {
            // Set the secret key for signing the token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sleepy-eepy-hogs"));

            // Generate token parameters
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                    // Add additional claims if needed (e.g., user roles)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            // Create JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
