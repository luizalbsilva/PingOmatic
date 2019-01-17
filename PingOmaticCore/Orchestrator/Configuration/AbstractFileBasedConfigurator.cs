using PingOmaticCore.Pinger.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PingOmaticCore.Orchestrator.Configuration
{
    public abstract class AbstractFileBasedConfigurator
    {
        #region Properties
        /// <summary>
        /// File location
        /// </summary>
        public string FileLocation { get; private set; }
        #endregion

        /// <summary>
        /// Responsable for notifing file events
        /// </summary>
        private FileSystemWatcher _fileWatcher;

        /// <summary>
        /// Pinger orchestrator to be reconfigured
        /// </summary>
        protected PingerOrchestrator Orchestrator { get; private set; }

        /// <summary>
        /// Auto reloads configuration on file changes
        /// </summary>
        protected bool IsAutoReload { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orchestrator">Orchestrator who will be (re)configured</param>
        /// <param name="fileLocation">Configuration file</param>
        /// <param name="isAutoReload">Auto-reloads on configuration file changes</param>
        protected AbstractFileBasedConfigurator(PingerOrchestrator orchestrator, string fileLocation, bool isAutoReload = true) =>
            (FileLocation,Orchestrator, IsAutoReload) = (fileLocation, orchestrator, isAutoReload);

        /// <summary>
        /// Configure based on the configuration file passed by
        /// </summary>
        public void Configure()
        {
            if ((_fileWatcher == null)&&(IsAutoReload))
            {
                _fileWatcher = new FileSystemWatcher(this.FileLocation);
                _fileWatcher.Changed +=  (s, e) => {
                    ReloadData();
                };
            }
            ReloadData();
        }

        /// <summary>
        /// Reloads data from file
        /// </summary>
        protected void ReloadData() =>
            this.Orchestrator.Reload(ReadData() ?? new List<PingerConfiguration>());

        /// <summary>
        /// Parses data from the file
        /// </summary>
        /// <param name="stream">Stream to be read</param>
        /// <returns></returns>
        internal abstract List<PingerConfiguration> ReadData();
    }
}
