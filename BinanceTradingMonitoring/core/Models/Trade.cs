namespace BinanceTradingMonitoring.core.Models
{
    /// <summary>
    /// Represents a trade event.
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// Type of the event.
        /// </summary>
        public string e { get; set; }

        /// <summary>
        /// Event time.
        /// </summary>
        public long E { get; set; }

        /// <summary>
        /// Symbol.
        /// </summary>
        public string s { get; set; }

        /// <summary>
        /// Trade ID.
        /// </summary>
        public long t { get; set; }

        /// <summary>
        /// Price.
        /// </summary>
        public string p { get; set; }

        /// <summary>
        /// Quantity.
        /// </summary>
        public string q { get; set; }

        /// <summary>
        /// Buyer order ID.
        /// </summary>
        public long b { get; set; }

        /// <summary>
        /// Seller order ID.
        /// </summary>
        public long a { get; set; }

        /// <summary>
        /// Trade time.
        /// </summary>
        public long T { get; set; }

        /// <summary>
        /// Is the buyer the maker?
        /// </summary>
        public bool m { get; set; }

        /// <summary>
        /// Is the trade the best price match?
        /// </summary>
        public bool M { get; set; }
    }
}
