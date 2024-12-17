namespace NotTinderAppMAUI.Models.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int StartupId { get; set; }
    public int Amount { get; set; }
    public DateTime Date { get; set; }
}