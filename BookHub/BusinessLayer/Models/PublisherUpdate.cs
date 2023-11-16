using BusinessLayer.Models;

namespace BookHub.Models;

public class PublisherUpdate
{
    public string Name { get; set; }
    public virtual ICollection<ModelRelated> Books { get; set; } = new List<ModelRelated>();
}