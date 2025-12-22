using System.ComponentModel.DataAnnotations;

namespace Pacovallet.Api.Models.Dto
{
    public class CreateTransactionRequest
    {
        [Required]
        public required string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public TransactionTypeEnum Type { get; set; }

        public Guid CategoryId { get; set; }
        public Guid PersonId { get; set; }
    }
}
