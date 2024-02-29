﻿namespace UniversityBookingProject.Models
{
    public class RoomBooking
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Boolean IsAdmin { get; set; }
        public int RoomNumber { get; set; }
    }
}
