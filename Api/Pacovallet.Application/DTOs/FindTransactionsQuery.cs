using Pacovallet.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace Pacovallet.Application.DTOs
{
    public class FindTransactionsQuery
    {
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }

        public string? Type { get; set; } = null;

        [JsonIgnore]
        public TransactionType? TransactionType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Type))
                    return null;
                if (Enum.TryParse<TransactionType>(Type, ignoreCase: true, out var result))
                    return result;
                return null;
            }
        }

        public List<Guid> PersonsId { get; set; } = [];
        public List<Guid> CategoryId { get; set; } = [];
    }
}
