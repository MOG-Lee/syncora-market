using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncora.SharedKernel.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }

        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public void AddDomainEvent(DomainEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            if (Id == Guid.Empty || other.Id == Guid.Empty)
                return false;

            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + Id).GetHashCode();
        }
    }
}
