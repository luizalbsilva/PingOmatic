using System;
using System.Collections.Generic;
using System.Text;

namespace PingOmaticCore.Pinger.Configuration
{
    /// <summary>
    /// Pinger's configuration data
    /// </summary>
    [Serializable]
    public class PingerConfiguration
    {
        #region Properties
        /// <summary>
        /// URL to be pinged
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// Time interval between calls
        /// </summary>
        public TimeSpan Interval { get; set; }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="uri">URL to be pinged</param>
        /// <param name="between">Time interval between calls</param>
        public PingerConfiguration(Uri uri, TimeSpan between) =>
            (Uri, Interval) = (uri, between);
    }
}
