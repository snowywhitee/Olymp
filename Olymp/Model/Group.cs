namespace Olymp.Model;

public record Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Participant[] Participants { get; set; }
}