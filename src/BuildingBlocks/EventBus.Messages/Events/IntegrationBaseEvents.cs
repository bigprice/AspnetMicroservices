using System;

namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvents
    {
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }

        public IntegrationBaseEvents()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvents(Guid id, DateTime createDate)
        {
            Id = id;
            CreationDate = createDate;
        }
    }
}
