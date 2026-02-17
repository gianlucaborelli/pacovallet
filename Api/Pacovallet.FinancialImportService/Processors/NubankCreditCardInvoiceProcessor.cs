using Microsoft.AspNetCore.Http;
using Pacovallet.Domain.Entities;
using Pacovallet.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pacovallet.FinancialImportService.Processors
{
    public class NubankCreditCardInvoiceProcessor : ICreditCardInvoiceProcessor
    {
        public CreditCardBank Bank => CreditCardBank.Nubank;

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
                    amount < 0 ? TransactionType.Income : TransactionType.Expense,
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
