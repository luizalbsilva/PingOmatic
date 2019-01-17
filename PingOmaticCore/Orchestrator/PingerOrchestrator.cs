using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PingOmaticCore.Pinger.Configuration;
using PingOmaticCore.Pinger;

namespace PingOmaticCore.Orchestrator
{
    /// <summary>
    /// Call the same methods for all pingers. Creates each one depending on the configuration passed by.
    /// </summary>
    public class PingerOrchestrator 
    {
        /// <summary>
        /// Destructor. Release resources.
        /// </summary>
        ~PingerOrchestrator() => 
            Stop();

        #region Properties
        /// <summary>
        /// List of all pingers
        /// </summary>
        private List<Pinger.Pinger> _pingers = new List<Pinger.Pinger>();

        /// <summary>
        /// List of all pingers
        /// </summary>
        public List<Pinger.Pinger> Pingers { get => _pingers.ToList(); }

        /// <summary>
        /// Status the pingers should have
        /// </summary>
        public PingerStatus Status { get; set; } = PingerStatus.IDLE;
        #endregion

        #region Public Methods
        /// <summary>
        /// Add all pingers to the internal list
        /// </summary>
        /// <param name="dados"></param>
        public void AddAll(ICollection<PingerConfiguration> dados)
        {
            var newPingers = dados.Select(c => new Pinger.Pinger(c));
            _pingers.AddRange(newPingers);
            if (Status == PingerStatus.RUNNING)
                Parallel.ForEach(newPingers, pinger => pinger.Start());
        }

        /// <summary>
        /// Add all pingers to the internal list
        /// </summary>
        /// <param name="dados"></param>
        public void Reload(ICollection<PingerConfiguration> dados)
        {
            var reload = Status == PingerStatus.RUNNING;
            if (reload)
                this.Stop();

            this._pingers = dados.Select(c => new Pinger.Pinger(c))
                .ToList();

            if (reload)
                this.Start();
        }

        /// <summary>
        /// Starts all pingers
        /// </summary>
        public void Start()
        {
            Parallel.ForEach( _pingers, p => p.Start());
            Status = PingerStatus.RUNNING;
        }

        /// <summary>
        /// Stops all pingers
        /// </summary>
        public void Stop() 
        {
            Parallel.ForEach(_pingers, p => p.Stop());
            Status = PingerStatus.IDLE;
        }
        #endregion
    }
}
