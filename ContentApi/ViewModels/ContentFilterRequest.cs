namespace ContentApi.ViewModels {
    public class ContentFilterRequest {
        public string Direction { get; set; }
        public ulong? Start { get; set; }
        public long? Count { get; set; }

        public bool IsForward() {
            if(Direction != null && Direction.Length > 0 && Direction.ToLower()[0] == 'f')
                return true;
            return false;
        }

        public bool IsBackward() {
            if(Direction != null && Direction.Length > 0 && Direction.ToLower()[0] == 'b')
                return true;
            return false;
        }
    }
}