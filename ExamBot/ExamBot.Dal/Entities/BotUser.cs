namespace ExamBot.Dal.Entities;

public class BotUser
{
    public long BotUserId { get; set; }
    public long ChatId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Assress { get; set; }
}
