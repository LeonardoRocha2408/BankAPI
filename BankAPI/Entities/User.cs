using System.ComponentModel.DataAnnotations.Schema;

namespace BankAPI.Entities
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Username { get; set; } = string.Empty;
        [Column("balance")]
        public float Balance { get; set; }
        [Column("password_login")]
        public string PasswordLogin { get; set; } = string.Empty;
        [Column("password_transaction")]
        public string PasswordTransaction { get; set; } = string.Empty;
    }
}
