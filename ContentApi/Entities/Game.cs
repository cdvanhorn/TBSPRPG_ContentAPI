using System;

namespace ContentApi.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid AdventureId { get; set; }
        public string Language { get; set; }
    }
}