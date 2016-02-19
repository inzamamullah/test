﻿using Caliburn.Micro;
using PDS.Witsml.Studio.Models;

namespace PDS.Witsml.Studio.ViewModels
{
    /// <summary>
    /// Manages the data entry for connection details.
    /// </summary>
    public class ConnectionViewModel : Screen
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(typeof(ConnectionViewModel));
        private string _connectionType;

        /// <summary>
        /// Initializes an instance of the ConnectionViewModel.
        /// </summary>
        public ConnectionViewModel(ConnectionTypes connectionType)
        {
            _log.Debug("Creating View Model");

            _connectionType = connectionType.ToString();
            DisplayName = string.Format("{0} Connection", _connectionType);
            Connection = new Connection();
        }

        /// <summary>
        /// Gets and sets the connection details for a connection
        /// </summary>
        public Connection Connection { get; set; }
    }
}
