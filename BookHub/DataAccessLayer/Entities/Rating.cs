using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities;

public class Rating : BaseEntity
{
    public int UserId { get; set; }
    [ForeignKey("UserId")] 
    public virtual User User { get; set; }
    
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public virtual Book Book { get; set; }
    
    public int Value { get; set; }
    
    public string? Comment { get; set; }
}