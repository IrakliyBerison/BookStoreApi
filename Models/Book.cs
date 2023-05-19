using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreApi.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Author? Author { get; set; }
        public BookState? State { get; set; }
    }
}
