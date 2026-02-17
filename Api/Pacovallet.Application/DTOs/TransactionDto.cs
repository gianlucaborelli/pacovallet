using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.DTOs
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PersonId { get; set; }
        public List<TransactionDto> ChildTransactions { get; set; } = [];
        public bool HasChildren { get; set; } = false;
    }
}