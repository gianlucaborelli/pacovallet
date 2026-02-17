using Pacovallet.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pacovallet.Domain.Entities
{
    public class Person : Entity
    {
        public required string Name { get; set; }
        public DateTime BirthDate { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.UtcNow.Date;
                var age = today.Year - BirthDate.Year;

                if (BirthDate.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }
    }
}