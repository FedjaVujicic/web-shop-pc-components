﻿namespace ComponentShopAPI.Dtos
{
    public class RegistrationDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public DateTime Birthday { get; set; }
    }
}
