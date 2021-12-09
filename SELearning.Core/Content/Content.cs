#nullable disable
namespace SELearning.Core.Content;

public class Content
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string VideoLink { get; set; }
    
    public int Rating { get; set; }
    
    public User.User Author { get; set; }

    public Section Section { get; set; }
    
    public Content(string title, string description, string videoLink, int rating) {
        Title = title;
        Description = description;
        VideoLink = videoLink;
        Rating = rating;
    }

    public Content(string title, string description, string videoLink, int? rating, User.User author, Section section) {
        Title = title;
        Description = description;
        VideoLink = videoLink;
        Rating = rating ?? 0;
        Author = author;
        Section = section;
    }
}
