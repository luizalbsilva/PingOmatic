using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using PingOmaticCore.Pinger.Configuration;

namespace PingOmaticCore.Orchestrator.Configuration
{
    public class JSONFileConfiguration : AbstractFileBasedConfigurator
    {
        public JSONFileConfiguration(PingerOrchestrator orchestrator, string fileLocation, bool isAutoReload = true) : base(orchestrator, fileLocation, isAutoReload)
        { }

        internal override List<PingerConfiguration> ReadData() =>
            JsonConvert.DeserializeObject<List<PingerConfiguration>>(File.ReadAllText(FileLocation));
    }
}
