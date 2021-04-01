using Microsoft.AspNetCore.Mvc;

namespace ContentApi.ViewModels {
    public class ContentFilterRequest {
        [FromQuery(Name = "direction")]
        public string Direction { get; set; }
        
        [FromQuery(Name = "start")]
        public long? Start { get; set; }
        
        [FromQuery(Name = "count")]
        public long? Count { get; set; }

        public bool IsForward() {
            return !string.IsNullOrEmpty(Direction) && Direction.ToLower()[0] == 'f';
        }
        
        public bool IsBackward() {
            return !string.IsNullOrEmpty(Direction) && Direction.ToLower()[0] == 'b';
        }
    }
}