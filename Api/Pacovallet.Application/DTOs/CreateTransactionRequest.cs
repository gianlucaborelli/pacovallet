using Pacovallet.Domain.ValueObjects;

namespace Pacovallet.Application.DTOs
{
    public class CreateTransactionRequest
    {
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? OccurredAt { get; set; }
        public TransactionType Type { get; set; }
        public Guid CategoryId { get; set; }
        public Guid PersonId { get; set; }
        public List<CreateTransactionRequest> ChildTransactions { get; set; } = [];
    }
}