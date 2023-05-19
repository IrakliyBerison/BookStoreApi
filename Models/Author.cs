using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Surname { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }        
    }
}
