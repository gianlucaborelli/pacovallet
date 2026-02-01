using Pacovallet.Api.Models.Dto;
using Pacovallet.Core.Controller;
using Pacovallet.Core.Models;
using System.Net;

namespace Pacovallet.Api.Models;

public class Transaction : Entity
{
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;

    public TransactionTypeEnum Type { get; set; }

    public Guid CategoryId { get; private set; }
    public Guid PersonId { get; set; }

    public Category Category { get; private set; } = null!;
    public Person Person { get; set; } = null!;
        
    public Guid? ParentTransactionId { get; private set; }
    public Transaction? ParentTransaction { get; private set; }

    public ICollection<Transaction> ChildTransactions { get; private set; }
        = [];

    protected Transaction() { }

    public Transaction(
            string description,
            decimal amount,
            TransactionTypeEnum type,
            Guid categoryId,
            Guid personId,
            DateTime? occurredAt = null 
        )
    {
        Description = description;
        Amount = amount;
        Type = type;
        CategoryId = categoryId;
        PersonId = personId;
        if (occurredAt.HasValue)
            OccurredAt = DateTime.SpecifyKind(occurredAt.Value, DateTimeKind.Utc);
        else
            OccurredAt = DateTime.UtcNow;
    }

    public void SetParentTransaction(Transaction parent)
    {
        if (parent == null)
            return;

        ParentTransaction = parent;
        ParentTransactionId = parent.Id;
    }

    public void SetChildTransactions(ICollection<Transaction> children)
    {
        ChildTransactions = children;
    }


    public void SetCategory(Category category)
    {
        if (Category == category)
            return;

        Category = category;
        CategoryId = category.Id;
    }

    public void SetPerson(Person person)
    {
        if (Person == person)
            return;

        Person = person;
        PersonId = person.Id;
    }

    public bool IsValid(Person person, Category category, out string error)
    {
        if (person.Id != PersonId)
        {
            error = "Person mismatch";
            return false;
        }

        if (category.Id != CategoryId)
        {
            error = "Category mismatch";
            return false;
        }

        if (person.Age < 18 && Type == TransactionTypeEnum.Income)
        {
            error = "Minors cannot register income";
            return false;
        }

        if (Type == TransactionTypeEnum.Expense &&
            category.Purpose == CategoryTypeEnum.Income)
        {
            error = "Category purpose mismatch";
            return false;
        }

        if (Type == TransactionTypeEnum.Income &&
            category.Purpose == CategoryTypeEnum.Expense)
        {
            error = "Category purpose mismatch";
            return false;
        }

        error = string.Empty;
        return true;
    }
}
