using Microsoft.Win32;
using PingOmaticCore;
using PingOmaticCore.Orchestrator.Configuration;
using System;
using System.ServiceProcess;

namespace PingOmatic
{
    public partial class PingOmaticService : ServiceBase
    {

        public PingOmaticService()
        {
            InitializeComponent();

            InitializeService();
        }

        private PingerOrchestrator orchestrator = new PingerOrchestrator();
        private AbstractFileBasedConfigurator configurationManager;

        private void InitializeService()
        {
            var key = Registry.LocalMachine.OpenSubKey(@"\Software\PingOmatic\Location");
            if (key != null)
            {
                var configFile = Convert.ToString(key.GetValue("ConfigurationFile"));
                var reloadOnFileChange = Convert.ToBoolean( key.GetValue("ReloadOnFileChange", "False") );

                configurationManager = new JSONFileConfiguration(orchestrator, configFile, reloadOnFileChange);
                configurationManager.Configure();
            }
        }

        protected override void OnStart(string[] args)
        {
            orchestrator.Start();
        }

        protected override void OnStop()
        {
            orchestrator.Stop();
        }
    }
}
