using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cloudscribe.Core.Models.Setup
{
    /// <summary>
    /// represents a mismatch between the code version and the schema version (which should be the same)
    /// for a given application. typically a mismatch would indicate a deployment error such as did not deploy
    /// upgrade script or did not run the setup page
    /// basically when using the db schema upgrade system the code version should be kept the same as the highest script
    /// version that is intended to run. The setup page will not run a script that is higher than the code version, 
    /// which allows the developer to work on the upgrade script without fear it will be executed by the setup page
    /// until after he updates the corresponding code version.
    /// This only applies if the application implements an VersionProvider, without that the setup page will execute
    /// any script newer than the current schema version.
    /// The setup page will create and display a list of any found mismatches using a list of VersionMismatch objects
    /// </summary>
    public class VersionMismatch
    {
        public Version CodeVersion { get; set; }
        public Version SchemaVersion { get; set; }
        public string applicationName { get; set; }

    }
}
