using client_slave_message_communication.custom_requests;
using client_slave_message_communication.interfaces;
using client_slave_message_communication.model.keyboard_action;
using client_slave_message_communication.model.mouse_action;
using custom_message_based_implementation.model;
using message_based_communication.model;
using message_based_communication.module;
using slave_control_api.ConnectionWrapper;
using System;
using System.Drawing.Printing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using custom_message_based_implementation.proxy;
using message_based_communication.connection;
using window_utility;
using File = custom_message_based_implementation.model.File;

namespace slave_controller
{
    public class SlaveController : BaseRouterModule, ISlave
    {
        public override string CALL_ID_PREFIX => "SLAVE_CALL_ID_";
        protected override string MODULE_ID_PREFIXES => "SLAVE_GIVEN_MODULE_ID_";


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        protected MouseActionHandler mouseActionHandler;

        private DirectoryInfo filesDirectory; // this folder is used to store files retrived from the file servermodule

        public ProxyHelper ServerModuleProxyHelper;

        private FileServermoduleProxy _fileServermoduleProxy;

        public FileServermoduleProxy FileProxy
        {
            get
            {
                if (null == _fileServermoduleProxy)
                {
                    if (null == this.proxyHelper)
                    {
                        throw new Exception("proxyHelper null");
                    }
                    _fileServermoduleProxy = new FileServermoduleProxy(this.proxyHelper, this);
                    
                }
                return _fileServermoduleProxy;
            }
        }

        //state
        public bool IsRunning { get; private set; }
        private PrimaryKey connectedClientPK;
        private IntPtr appWindow;



        public SlaveController(Port forMouseControlApi, Port portForRegistrationToRouter, ModuleType moduleType, message_based_communication.encoding.Encoding customEncoding) : base(portForRegistrationToRouter, moduleType, customEncoding)
        {

            PythonStarter.StartPythonMouseControlApi();
            Thread.Sleep(10); // the mouse control api must be running before the mouseActionHandler can be instanciated

            var pyAutoGui = new PythonAutoGUIWrapper(forMouseControlApi);//TODO FIX, not sure what I mean here anymore

            // FIRST USE THE GetWindowByWindowTitle and GetClassName - when you know the class name, switch to GetWindowByClass
            appWindow = WindowUtils.GetWindowHandle(windowTitleText: new Regex("Paint"));
            this.mouseActionHandler = new MouseActionHandler(new slave_control_api.controlers.MouseControlApi(pyAutoGui), appWindow);

            WindowUtils.PutWindowOnTop(appWindow);

            PythonStarter.StartPythonScreenCapture(appWindow);


            //ensure that a folder is created to store files from the fileserver
            filesDirectory = Directory.CreateDirectory(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "ccfeu-files");
            //make sure the filesDirectory is empty;
            EmptyDirectory(filesDirectory);
            IsRunning = true;
        }

        private void EmptyDirectory(DirectoryInfo dirInfo)
        {
            var files = dirInfo.GetFiles();
            foreach (var item in files)
            {
                item.Delete();
            }
        }

        public void DoKeyboardAction(BaseKeyboardAction action)
        {
            throw new NotImplementedException();
        }

        public void DoMouseAction(BaseMouseAction action)
        {
            mouseActionHandler.QueueMouseCommand(action);
        }

        public Port GetImageProducerConnInfo()
        {
            return new Port() { ThePort = 60254 }; //30303
            throw new NotImplementedException();
        }

        

        public void FetchRemoteFile(string fileName)
        {
            if (null == connectedClientPK)
            {
                Console.WriteLine("can not fetch file before handshake have been called");
                return;
            }
            FileProxy.DownloadFile(
                new FileName(){FileNameProp = fileName}
                , connectedClientPK
                , (file) =>
                {
                    var fs = new FileStream(
                        filesDirectory.FullName + Path.DirectorySeparatorChar + file.FileName.FileNameProp
                        ,FileMode.Create
                        ,FileAccess.Write
                        );
                    fs.Write(file.FileData);
                }
            );

        }

        // this method will only save the files, the terminate myst happen in the handle request after a response have been sent
        public void SaveFilesAndTerminate()
        {
            // this method only needs to save the files to the file servermodule
            if (null == connectedClientPK)
            {
                Console.WriteLine("can save file before handshake have been called");
                return;
            }
            foreach (var file in filesDirectory.GetFiles())
           {
               FileProxy.UploadFile(
                   new File()
                   {
                      FileData = System.IO.File.ReadAllBytes(file.FullName)
                      , FileName = new FileName(){FileNameProp = file.Name + "." + file.Extension }
                   }
                   , connectedClientPK
                   , true
                   , () => { } // empty callback method, as this method returns void
               );
           }
        }

        public override void HandleRequest(BaseRequest message)
        {
            object payload;
            if (message is Handshake _handshake)
            {
                payload = Handshake(_handshake.arg1PrimaryKey);
            }
            else if (message.GetType().IsGenericType)
            {
                var type = message.GetType().GetGenericTypeDefinition();
                var gType = typeof(DoMouseAction<BaseMouseAction>).GetGenericTypeDefinition();
                if (type.Equals(typeof(DoMouseAction<BaseMouseAction>).GetGenericTypeDefinition()))
                {
                    //TODO see if there is a non dynamic way to do this
                    dynamic mouseAction = message;
                    DoMouseAction(mouseAction.arg1MouseAction);
                    payload = null;
                }
                else
                {
                    throw new Exception();
                }
            }
            else if (message is GetImageProducerConnectionInfo _getImageProducer)
            {
                payload = GetImageProducerConnInfo();
            }
            else if (message is FetchRemoteFile _fetchRemoteFile)
            {
                FetchRemoteFile(_fetchRemoteFile.FileName);
                payload = null;
            }
            else if (message is SaveFilesAndTerminate _saveFilesAndTerminate) // this is a special case
            {
                // must send the response and set IsRunning to false;
                SaveFilesAndTerminate();
                payload = null;
                var _response = GenerateResponseBasedOnRequestAndPayload(message, payload);
                SendResponse(_response);
                IsRunning = false; 
                return;
            }
            else
            {
                throw new NotImplementedException();
            }

            var response = GenerateResponseBasedOnRequestAndPayload(message, payload);
            SendResponse(response);
        }

        public Tuple<int, int> Handshake(PrimaryKey pk)
        {
            connectedClientPK = pk;
            return WindowUtils.GetApplicationSize(appWindow);
        }
    }
}
