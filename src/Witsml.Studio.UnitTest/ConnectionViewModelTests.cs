﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.Witsml.Studio.Connections;
using PDS.Witsml.Studio.Properties;
using PDS.Witsml.Studio.ViewModels;

namespace PDS.Witsml.Studio
{
    /// <summary>
    /// Unit tests for the ConnectionViewModel
    /// </summary>
    [TestClass]
    public class ConnectionViewModelTests
    {
        private static readonly string PersistedDataFolderName = Settings.Default.PersistedDataFolderName;
        private static readonly string ConnectionBaseFileName = Settings.Default.ConnectionBaseFileName;

        private ConnectionViewModel _witsmlConnectionVm;
        private ConnectionViewModel _etpConnectionVm;
        private Connection _witsmlConnection;
        private Connection _etpConnection;

        /// <summary>
        /// Sets up the environment for each test.  
        /// ConnectionViewModels and Connections are created 
        /// for ConnectionTypes Witsml and Etp.
        /// In addition the persisence folder is cleard and deleted.
        /// </summary>
        [TestInitialize]
        public void TestSetUp()
        {
            _witsmlConnection = new Connection()
            {
                Name = "Witsml",
                Uri = "http://localhost/Witsml.Web/WitsmlStore.svc",
                Username = "WitsmlUser",
                Password = "WitsmlPassword"
            };

            _etpConnection = new Connection()
            {
                Name = "Etp",
                Uri = "ws://localhost/witsml.web/api/etp",
                Username = "EtpUser",
                Password = "EtpPassword"
            };

            _witsmlConnectionVm = new ConnectionViewModel(ConnectionTypes.Witsml);
            _etpConnectionVm = new ConnectionViewModel(ConnectionTypes.Etp);

            DeletePersistenceFolder();
        }

        /// <summary>
        /// Tests the connection filename for each type of connection has the correct prefix.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilenamePrefix()
        {
            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Witsml.ToString()));

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Etp.ToString()));
        }

        /// <summary>
        /// Tests the connection filename is correctly generated with the 
        /// ConnectionType prefix and the base file name.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilename()
        {
            var witsmlFilename = string.Format("{0}{1}", ConnectionTypes.Witsml, ConnectionBaseFileName);
            var etpFilename = string.Format("{0}{1}", ConnectionTypes.Etp, ConnectionBaseFileName);

            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.AreEqual(witsmlFilename, filename);

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.AreEqual(etpFilename, filename);
        }

        /// <summary>
        /// Tests that a null connection is returned when no connection file is persisted.
        /// </summary>
        [TestMethod]
        public void TestNoConnectionFilePersisted()
        {
            Assert.IsNull(_witsmlConnectionVm.OpenConnectionFile());
            Assert.IsNull(_etpConnectionVm.OpenConnectionFile());
        }

        /// <summary>
        /// Tests the equivalence of a connection before and after file persistence.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilePersistence()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _etpConnectionVm.SaveConnectionFile(_etpConnection);

            var witsmlRead = _witsmlConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_witsmlConnection, witsmlRead);

            var etpRead = _etpConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_etpConnection, etpRead);
        }

        private static void DeletePersistenceFolder()
        {
            var path = string.Format("{0}/{1}", Environment.CurrentDirectory, PersistedDataFolderName);

            // Delete all files in the Persistence Folder
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }

            // Delete the Persistence Folder
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }
    }
}
