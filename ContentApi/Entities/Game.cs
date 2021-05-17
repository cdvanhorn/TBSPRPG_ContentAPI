using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContentApi.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public ICollection<Content> Contents { get; set; }
        [NotMapped]
        public Guid AdventureId { get; set; }
    }
}