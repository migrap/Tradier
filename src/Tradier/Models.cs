using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tradier.Net.Http.Formatting;

namespace Tradier {
    public class SecurityCollection : List<Security> {
    }

    public class Security {
        public string Symbol { get; set; }
    }

    public partial class Quote {
        public string Symbol { get; }
        public string Description { get; }
        public string Exchange { get; }
        public string Type { get; }
        public double Last { get; }
        public double Change { get; }
        public double ChangePercentage { get; }
    }

    public partial class Calendar : List<Day> {
    }

    public class Day {
        public DateTimeOffset Date { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public Hours Premarket { get; set; }
        public Hours Open { get; set; }
        public Hours Postmarket { get; set; }
    }

    public class Hours {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }

    public class Clock {
        public DateTimeOffset Date { get; set; }
        public string Description { get; set; }
        public TimeSpan NextChange { get; set; }
        public string NextState { get; set; }
        public string State { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }

    public partial class History : List<History.Day> {
    }

    public partial class History {
        public class Day {
            public DateTimeOffset Date { get; set; }
            public double Open { get; set; }
            public double High { get; set; }
            public double Low { get; set; }
            public double Close { get; set; }
            public long Volume { get; set; }
        }
    }

    public partial class Expirations : List<DateTimeOffset> {
    }

    public partial class OptionChain : List<Option> {
    }

    public partial class Option {
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string Exch { get; set; }
        public string Type { get; set; }
        //public double Last { get; set; }
        //public double? Change { get; set; }
        //public long Volume { get; set; }
        //public double? Open { get; set; }
        //public double? High { get; set; }
        //public double? Low { get; set; }
        //public double? Close { get; set; }
        public long OpenInterest { get; set; }
        public string Underlying { get; set; }
        public double Strike { get; set; }
        public long ContractSize { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
        public string ExpirationType { get; set; }
        public string OptionType { get; set; }
        public string RootSymbol { get; set; }
    }

    public class Watchlist {
        public string Id { get; set; }
        public string PublicId { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public class Item {
            public string Symbol { get; set; }
            public string Id { get; set; }
        }
    }

    public class Watchlists : List<Watchlist> {
        public Watchlists() {
        }
    }
}



        //"description": "MSFT Aug 21 2015 $57.50 Put",
        //"exch": "Z",
        //"type": "option",
        //"last": 12.15,
        //"change": 0.0,
        //"change_percentage": 0.0,
        //"volume": 0,
        //"average_volume": 0,
        //"last_volume": 30,
        //"trade_date": 1436892537000,
        //"open": null,
        //"high": null,
        //"low": null,
        //"close": null,
        //"prevclose": 12.15,
        //"week_52_high": 0.0,
        //"week_52_low": 0.0,
        //"bid": 10.2,
        //"bidsize": 546,
        //"bidexch": "X",
        //"bid_date": 1439236799000,
        //"ask": 10.65,
        //"asksize": 286,
        //"askexch": "C",
        //"ask_date": 1439236799000,
        //"open_interest": 171,
        //"underlying": "MSFT",
        //"strike": 57.5,
        //"contract_size": 100,
        //"expiration_date": "2015-08-21",
        //"expiration_type": "standard",
        //"option_type": "put",
        //"root_symbol": "MSFT"