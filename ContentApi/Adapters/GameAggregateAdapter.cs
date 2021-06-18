using System;
using ContentApi.Entities;
using TbspRpgLib.Aggregates;

namespace ContentApi.Adapters
{
    public interface IGameAggregateAdapter
    {
        Game ToEntity(GameAggregate aggregate);
    }
    
    public class GameAggregateAdapter : IGameAggregateAdapter
    {
        public Game ToEntity(GameAggregate aggregate)
        {
            return new Game()
            {
                Id = Guid.Parse(aggregate.Id),
                AdventureId = Guid.Parse(aggregate.AdventureId),
                Language = aggregate.Settings.Language
            };
        }
    }
}