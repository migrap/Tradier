using System.Collections.Generic;

namespace Tradier {
    public class SecurityCollection : List<Security> {
    }
    
    public class Security {
        public string Symbol { get; set; }
    }

    public class Quote {
        public string Symbol { get; }
        public string Description { get; }
        public string Exchange { get; }
        public string Type { get; }
        public double Last { get; }
        public double Change { get; }
        public double ChangePercentage { get; }
    }
}
