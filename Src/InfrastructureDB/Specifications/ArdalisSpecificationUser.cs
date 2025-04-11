using Ardalis.Specification;
using Kurier.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDB.Specifications
{
    internal class ArdalisSpecificationUser : SingleResultSpecification<User>
    {
        public ArdalisSpecificationUser(string email, string password)
        {
            Query
                .Where(specification => specification.Email == email && specification.Password == password)
                .Include(specification => specification.Orders);
        }
    }
}
