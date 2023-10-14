using DataAccessLayer.Entities;

namespace BookHub.Models;

public class RatingDetail
{
    public int Id { get; set; }
    public ModelRelated<User> User { get; set; }
    public ModelRelated<Book> Book { get; set; }
    public int Value { get; set; }
    public string? Comment { get; set; }
}