namespace BusinessLayer.Models;

public class AuthorDetail
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<ModelRelated> Books { get; set; }
}