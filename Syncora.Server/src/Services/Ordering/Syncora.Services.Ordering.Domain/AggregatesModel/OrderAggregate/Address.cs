using Syncora.SharedKernel.Domain;
using Syncora.SharedKernel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.Services.Ordering.Domain.AggregatesModel.OrderAggregate
{
    public class Address : ValueObject
    {
        public string Street { get; private set; } = default!;
        public string City { get; private set; } = default!;
        public string Zipcode { get; private set; } = default!;

        //for EF Core
        private Address() { }

        public Address(string street, string city, string zipcode)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new DomainException("Street cannot be null or empty");

            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City cannot be null or empty");

            if (string.IsNullOrWhiteSpace(zipcode))
                throw new DomainException("Zipcode cannot be null or empty");

            Street = street;
            City = city;
            Zipcode = zipcode;
        }


        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return City;
            yield return Zipcode;
        }
    }
}
