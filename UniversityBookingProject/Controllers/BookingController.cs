using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityBookingProject.Models;
using UniversityBookingProject.Data;

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

        //Create and Edit
        [HttpPost]
        public JsonResult CreateEdit(RoomBooking booking)
        {
            if (booking.ID != 0)
            {
                _context.RoomBookings.Add(booking);
            } 
            else
            {
                var bookingInDB = _context.RoomBookings.Find(booking.ID);

                if (bookingInDB == null )
                    return new JsonResult(NotFound());

                bookingInDB = booking;
            }

            _context.SaveChanges();

            return new JsonResult(Ok(booking));
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
    }
}
