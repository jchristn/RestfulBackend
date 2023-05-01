using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GetSomeInput;
using RestfulBackend.Core;
using RestfulBackend.Core.Services;
using SyslogLogging;
using Timestamps;
using Watson.ORM;
using WatsonWebserver;

namespace RestfulBackend.Server
{
    public static class Program
    {
        #region Public-Members

        #endregion

        #region Private-Members

        private static string _Header = "[RosettaStone] ";
        private static SerializationHelper _Serializer = new SerializationHelper();
        private static string _SettingsFile = "./rosettastone.json";
        private static Settings _Settings = new Settings();
        private static bool _CreateDefaultRecords = false;
        private static LoggingModule _Logging = null;
        private static WatsonORM _ORM = null;
        private static CredentialService _Credentials = null;
        private static UserMasterService _Users = null;
        private static TenantService _Tenants = null;
        private static WatsonWebserver.Server _Server = null;

        #endregion

        #region Entrypoint

        public static void Main(string[] args)
        {
            Welcome();
            InitializeSettings(args);
            InitializeGlobals();

            if (_Settings.EnableConsole)
            {
                RunConsoleWorker();
            }
            else
            {
                EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
                bool waitHandleSignal = false;
                do
                {
                    waitHandleSignal = waitHandle.WaitOne(1000);
                }
                while (!waitHandleSignal);
            }
        }

        #endregion

        #region Public-Methods

        #endregion

        #region Private-Methods

        private static void Welcome()
        {
            Console.WriteLine(
                Environment.NewLine +
                Constants.Logo +
                Constants.ProductName + 
                Environment.NewLine);
        }

        private static void InitializeSettings(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                foreach (string arg in args)
                {
                    if (arg.StartsWith("--config="))
                    {
                        _SettingsFile = arg.Substring(9);
                    }
                    else if (arg.Equals("--setup"))
                    {
                        _CreateDefaultRecords = true;
                    }
                }
            }

            if (!File.Exists(_SettingsFile))
            {
                Console.WriteLine("Settings file '" + _SettingsFile + "' does not exist, creating with defaults");
                File.WriteAllBytes(_SettingsFile, Encoding.UTF8.GetBytes(_Serializer.SerializeJson(_Settings, true)));
            }
            else
            {
                _Settings = _Serializer.DeserializeJson<Settings>(File.ReadAllText(_SettingsFile));
                Console.WriteLine("Loaded settings from file '" + _SettingsFile + "'");
            }
        }

        private static void InitializeGlobals()
        {
            #region Logging

            Console.WriteLine("Initializing logging to " + _Settings.Logging.SyslogServerIp + ":" + _Settings.Logging.SyslogServerPort);

            _Logging = new LoggingModule(
                _Settings.Logging.SyslogServerIp,
                _Settings.Logging.SyslogServerPort,
                _Settings.EnableConsole);

            if (!String.IsNullOrEmpty(_Settings.Logging.LogDirectory)
                && !Directory.Exists(_Settings.Logging.LogDirectory))
            {
                Directory.CreateDirectory(_Settings.Logging.LogDirectory);
                _Settings.Logging.LogFilename = _Settings.Logging.LogDirectory + _Settings.Logging.LogFilename;
            }

            if (!String.IsNullOrEmpty(_Settings.Logging.LogFilename))
            {
                _Logging.Settings.FileLogging = FileLoggingMode.FileWithDate;
                _Logging.Settings.LogFilename = _Settings.Logging.LogFilename;
            }

            #endregion

            #region ORM

            Console.WriteLine("Initializing database");
            _ORM = new WatsonORM(_Settings.Database);

            _ORM.InitializeDatabase();
            _ORM.InitializeTables(new List<Type>
            {
                typeof(Credential),
                typeof(TenantMetadata),
                typeof(UserMaster)
            });

            #endregion

            #region Services

            _Credentials = new CredentialService(_Logging, _ORM);
            _Users = new UserMasterService(_Logging, _ORM);
            _Tenants = new TenantService(_Logging, _ORM);

            #endregion

            #region Default-Records

            if (_CreateDefaultRecords)
            {
                /*
                 * Create records
                 * 
                 */
            }

            #endregion

            #region Webserver

            _Server = new WatsonWebserver.Server(
                _Settings.Webserver.DnsHostname,
                _Settings.Webserver.Port,
                _Settings.Webserver.Ssl,
                DefaultRoute);

            _Server.Start();

            Console.WriteLine("Webserver started on " +
                (_Settings.Webserver.Ssl ? "https://" : "http://") +
                _Settings.Webserver.DnsHostname + ":" +
                _Settings.Webserver.Port);

            #endregion
        }

        private static void RunConsoleWorker()
        {
            bool runForever = true;

            while (runForever)
            {
                string userInput = Inputty.GetString("Command [?/help]:", null, false);

                switch (userInput)
                {
                    case "q":
                        runForever = false;
                        break;
                    case "c":
                    case "cls":
                        Console.Clear();
                        break;
                    case "?":
                        Console.WriteLine("");
                        Console.WriteLine("Available commands:");
                        Console.WriteLine("q         quit, exit the application");
                        Console.WriteLine("cls       clear the screen");
                        Console.WriteLine("?         help, this menu");
                        Console.WriteLine("");
                        break;
                }
            }
        }

        private static async Task DefaultRoute(HttpContext ctx)
        {
            Timestamp ts = new Timestamp();

            #region Variables-and-Query-Values

            Dictionary<string, object> ret = null;

            int maxResults = 10;

            string maxResultsStr = ctx.Request.Query.Elements.Get("results");

            if (!String.IsNullOrEmpty(maxResultsStr))
                maxResults = Convert.ToInt32(maxResultsStr);

            #endregion

            try
            {
                switch (ctx.Request.Method)
                {
                    case HttpMethod.GET:
                        break;
                }

                ret = new Dictionary<string, object>
                {
                    { "Message", Constants.BadRequestError }
                };

                ctx.Response.StatusCode = 400;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.Send(_Serializer.SerializeJson(ret, true));
            }
            catch (Exception e)
            {
                _Logging.Exception(e);

                ret = new Dictionary<string, object>
                {
                    { "Message", Constants.InternalServerError },
                    { "Exception", e }
                };

                ctx.Response.StatusCode = 500;
                ctx.Response.ContentType = "application/json";
                await ctx.Response.Send(_Serializer.SerializeJson(ret, true));
            }
            finally
            {
                ts.End = DateTime.UtcNow;

                _Logging.Debug(
                    _Header +
                    ctx.Request.Method.ToString() + " " +
                    ctx.Request.Url.RawWithoutQuery + " " +
                    ctx.Response.StatusCode + " " +
                    ts.TotalMs + "ms");
            }
        }

        #endregion
    }
}