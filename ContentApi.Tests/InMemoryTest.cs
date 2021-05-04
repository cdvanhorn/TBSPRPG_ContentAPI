using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;

namespace ContentApi.Tests
{
    public class InMemoryTest
    {
        protected readonly DbContextOptions<ContentContext> _dbContextOptions;
        protected readonly Guid _testGameId;

        protected InMemoryTest(string dbName)
        {
            _dbContextOptions = new DbContextOptionsBuilder<ContentContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            
            _testGameId = Guid.NewGuid();
        }

        protected IAggregateService MockAggregateService(ICollection<Event> events, List<string> contents)
        {
            var aggregateService = new Mock<IAggregateService>();
            
            //append methods
            aggregateService.Setup(service =>
                service.AppendToAggregate(It.IsAny<string>(), It.IsAny<Event>(), It.IsAny<ulong>())
            ).Callback<string, Event, ulong>((type, evnt, n) =>
            {
                events.Add(evnt);
            });
            
            //aggregate building methods
            aggregateService.Setup(service =>
                service.BuildPartialAggregateLatest(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync((string aggregateId, string aggregateTypeName) => new ContentAggregate()
            {
                Id = _testGameId.ToString(),
                Text = new List<string> { contents.Last() }
            });
            
            aggregateService.Setup(service =>
                service.BuildAggregate(It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync((string aggregateId, string aggregateTypeName) => new ContentAggregate()
            {
                Id = _testGameId.ToString(),
                Text = contents
            });
            
            aggregateService.Setup(service =>
                service.BuildPartialAggregate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ulong>())
            ).ReturnsAsync((string aggregateId, string aggregateTypeName, ulong start) => new ContentAggregate()
            {
                Id = _testGameId.ToString(),
                Text = contents.Skip((int)start).ToList()
            });
            
            aggregateService.Setup(service =>
                service.BuildPartialAggregate(
                    It.IsAny<string>(), 
                    It.IsAny<string>(),
                    It.IsAny<ulong>(),
                    It.IsAny<long>())
            ).ReturnsAsync((string aggregateId,
                string aggregateTypeName,
                ulong start,
                long count) => new ContentAggregate()
            {
                Id = _testGameId.ToString(),
                Text = contents.Skip((int)start).Take((int)count).ToList()
            });

            //reverse aggregate methods
            aggregateService.Setup(service =>
                service.BuildAggregateReverse(
                    It.IsAny<string>(),
                    It.IsAny<string>())
            ).ReturnsAsync((string aggregateId,
                string aggregateTypeName) =>
            {
                contents.Reverse();
                return new ContentAggregate()
                {
                    Id = _testGameId.ToString(),
                    Text = contents
                };
            });
            
            aggregateService.Setup(service =>
                service.BuildPartialAggregateReverse(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<long>())
            ).ReturnsAsync((string aggregateId,
                string aggregateTypeName,
                long start) =>
            {
                start = start < 0 ? 1 - (start * -1) : start;
                contents.Reverse();
                return new ContentAggregate()
                {
                    Id = _testGameId.ToString(),
                    Text = contents.Skip((int)start).ToList()
                };
            });
            
            aggregateService.Setup(service =>
                service.BuildPartialAggregateReverse(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<long>(),
                    It.IsAny<long>())
            ).ReturnsAsync((string aggregateId,
                string aggregateTypeName,
                long start,
                long count) =>
            {
                start = start < 0 ? 1 - (start * -1) : start;
                contents.Reverse();
                return new ContentAggregate()
                {
                    Id = _testGameId.ToString(),
                    Text = contents.Skip((int)start).Take((int)count).ToList()
                };
            });
            
            return aggregateService.Object;
        }

    }
}