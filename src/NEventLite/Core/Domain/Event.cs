﻿using System;

namespace NEventLite.Core.Domain
{
    public abstract class Event<TAggregate> : Event<TAggregate, Guid, Guid> where TAggregate : AggregateRoot<Guid, Guid>
    {
        protected Event(Guid id, Guid aggregateId) : base(id, aggregateId)
        {
        }

        protected Event(Guid id, Guid aggregateId, int targetVersion) : base(id, aggregateId, targetVersion)
        {
        }

        protected Event(Guid id, Guid aggregateId, int targetVersion, int eventSchemaVersion, string correlationId) : 
            base(id, aggregateId, targetVersion, eventSchemaVersion, correlationId)
        {
        }
    }

    public abstract class Event<TAggregate, TAggregateKey, TEventKey> : IEvent<TAggregate, TAggregateKey, TEventKey> where TAggregate : AggregateRoot<TAggregateKey, TEventKey>
    {
        protected Event(TEventKey id, TAggregateKey aggregateId) : this(id, aggregateId, (int) StreamState.NoStream)
        {
        }

        protected Event(TEventKey id, TAggregateKey aggregateId, int targetVersion) : this(id, aggregateId, targetVersion, 0,
            string.Empty)
        {
        }

        protected Event(TEventKey id, TAggregateKey aggregateId, int targetVersion, int eventSchemaVersion, string correlationId)
        {
            this.Id = id;
            this.AggregateId = aggregateId;
            this.TargetVersion = targetVersion;
            this.EventSchemaVersion = eventSchemaVersion;
            this.CorrelationId = correlationId;
        }

        public TEventKey Id { get; set; }
        public TAggregateKey AggregateId { get; set;  }
        public int TargetVersion { get; set;  }
        public DateTimeOffset EventCommittedTimestamp { get; set; }
        public int EventSchemaVersion { get; set; }
        public string CorrelationId { get; set; }
    }
}