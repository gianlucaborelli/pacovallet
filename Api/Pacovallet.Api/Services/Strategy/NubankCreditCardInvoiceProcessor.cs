using Pacovallet.Api.Models;
using System.Drawing;

namespace Pacovallet.Api.Services.Strategy
{
    public class NubankCreditCardInvoiceProcessor : ICreditCardInvoiceProcessor
    {
        public CreditCardBankEnum Bank => CreditCardBankEnum.Nubank;

        public async Task<List<Transaction>> ProcessAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);

            string? line;
            bool header = true;

            List<Transaction> transactions = [];

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (header)
                {
                    header = false;
                    continue;
                }

                var columns = line.Split(',');

                var date = DateTime.Parse(columns[0]);
                var description = columns[1];
                var amount = decimal.Parse(columns[2]) / 100;

                var transaction = new Transaction(
                    description,
                    Math.Abs(amount),
                    amount < 0 ? TransactionTypeEnum.Income : TransactionTypeEnum.Expense,
                    Guid.Empty,
                    Guid.Empty,
                    date
                    );

                transactions.Add(transaction);
            }

            return transactions;
        }
    }
}
