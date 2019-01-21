using PingOmaticCore.Pinger.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Pinger for one address
/// </summary>
namespace PingOmaticCore.Pinger
{
    /// <summary>
    /// Pings an URL, with an interval between the calls
    /// </summary>
    public class Pinger
    {
        #region Properties
        /// <summary>
        /// URL to be called
        /// </summary>
        public Uri Uri { get; private set; }
        /// <summary>
        /// Amount of time between the calls
        /// </summary>
        public TimeSpan Interval { get; private set; }
        /// <summary>
        /// Status of this service
        /// </summary>
        public PingerStatus Status { get; private set; }
        #endregion

        #region Constructors
        public Pinger(Uri uri, TimeSpan interval) =>
                (Uri, Interval) = (uri, interval);

        public Pinger(PingerConfiguration configuration) =>
                (Uri, Interval) = (configuration.Uri, configuration.Interval);
        #endregion


        /// <summary>
        /// Worker thread
        /// </summary>
        private Thread worker;

        /// <summary>
        /// Starts the pinging
        /// </summary>
        public void Start()
        {
            Status = PingerStatus.RUNNING;
            worker = new Thread(() =>
            {
                while (Status == PingerStatus.RUNNING)
                {
                    Ping();
                    Thread.Sleep((int)Interval.TotalMilliseconds);
                }
            });
            worker.Start();
            worker.Name = $"Ping'O'matic {this.Uri}";
        }        

        /// <summary>
        /// Stops to ping
        /// </summary>
        public void Stop()
        {
            Status = PingerStatus.IDLE;
            Task.Delay(20000)
                .ContinueWith((t) => {
                    if (worker.IsAlive)
                        worker.Abort();
                });
        }

        /// <summary>
        /// Pings the address. Asynchronous
        /// </summary>
        /// <returns>Task executing the ping</returns>
        private Task Ping()
        {
            var client = new HttpClient();
            var task = client.GetAsync(Uri);
            task.ContinueWith((s)=>client.Dispose());
            return task;
        }
    }

    /// <summary>
    /// Ping Status
    /// </summary>
    public enum PingerStatus
    {
        /// <summary>
        /// Not Running
        /// </summary>
        IDLE,
        /// <summary>
        /// Running (Oh, no, I would never guess that one ! XD)
        /// </summary>
        RUNNING
    }
}
