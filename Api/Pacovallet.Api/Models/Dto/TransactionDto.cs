using Pacovallet.Core.Converters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pacovallet.Api.Models.Dto
{
    public class TransactionDto
    {
        public Guid? Id { get; set; }
        public string Description { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

        public TransactionTypeEnum Type { get; set; }

        public Guid CategoryId { get; set; }
        public Guid PersonId { get; set; }

        [JsonConverter(typeof(EmptyStringToListConverter<TransactionDto>))]
        public List<TransactionDto> ChildTransactions { get; set; } = [];

        public bool HasChildren { get; set; } = false;
    }
}
