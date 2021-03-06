﻿using System;

namespace DenOfArt.Tables
{
    public class RegUserTable
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
