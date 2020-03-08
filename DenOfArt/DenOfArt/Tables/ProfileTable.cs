using System;

namespace DenOfArt.Tables
{
    public class ProfileTable
    {
        [SQLite.PrimaryKey]
        public Guid ProfileId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string DateOfBirth { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
