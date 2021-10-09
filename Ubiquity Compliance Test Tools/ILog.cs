using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using SKYNET.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYNET.LOG
{

    public class ILog
    {
        private static Mutex mutexFile;
        public static string fileName = "SkynetServer.log";
        static Process currentProcess;
        public string Message { get; set; } = "";
        public bool IsDebugEnabled = true;

        public void Save(Exception ex)
        {
            string returns = "";

            if (fileName == "")
                fileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().ProcessName) + ".log";

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(ex.Message);
                if (ex.InnerException != null)
                    stringBuilder.Append("\r\n").Append((object)ex.InnerException);
                if (ex.StackTrace != null)
                    stringBuilder.Append("\r\n").Append(ex.StackTrace);
                returns = string.Format($"{(object)stringBuilder.ToString()}:");
                AppendFile(returns, Path.Combine(Application.StartupPath, fileName));
            }
            catch { }
        }
        public static void AppendFile(string s, string fname)
        {
            string path = Path.Combine(Application.StartupPath, fname);
            StreamWriter streamWriter = null;
            try
            {
                mutexFile = LogMutex.mutexFile;
                mutexFile.WaitOne();
                FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write);
                streamWriter = new StreamWriter(stream);
                streamWriter.BaseStream.Seek(0L, SeekOrigin.End);
                streamWriter.WriteLine(Conversions.ToString(DateAndTime.Now) + ":" + s);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                Exception ex2 = ex;
                streamWriter?.Close();
                ProjectData.ClearProjectError();
            }
            finally
            {
                mutexFile.ReleaseMutex();
            }
        }
        public class LogMutex
        {
            public static Mutex mutexFile = new Mutex(false, "LogMutex");
        }


        public void Info(object mess)
        {
            Print(mess.ToString(), MessageType.INFO);
        }

        public void Warn(object mess)
        {
            Print(mess.ToString(), MessageType.WARN);
        }

        public void Error(object mess, Exception ex4)
        {
            Print(mess.ToString(), MessageType.ERROR);
        }

        public void Error(object mess)
        {
            Print(mess.ToString(), MessageType.ERROR);
        }
        public void Debug(object mess)
        {
            Print(mess.ToString(), MessageType.DEBUG);
        }
        private string HeadMessage;
        public ILog(string Head)
        {
            this.HeadMessage = Head;
        }

        public ILog()
        {
        }


        public void Print(string mess, MessageType mtype)
        {
            frmMain.frm.Write(mess, mtype);
        }
        public void Test()
        {
        }
        public static string GetExePatch()
        {
            try
            {
                currentProcess = Process.GetCurrentProcess();
                return new FileInfo(currentProcess.MainModule.FileName).FullName;
            }
            finally { currentProcess = null; }
        }
        public bool IsWarnEnabled()
        { return true; }

        public void MoreInfo(object v, Exception ex8 = null)
        {
            //throw new NotImplementedException();
        }


    }
    public enum MessageType
    {
        INFO, WARN, ERROR, DEBUG
    }
    public enum TypeMessage
    {
        Alert,
        Normal,
        YesNo
    }
    public class ErrorMessage
    {
        public ErrorMessage(int id, Exception error)
        {
            ID = id;
            Error = error;
        }
        public int ID;
        public Exception Error;
        public static List<ErrorMessage> ListError = new List<ErrorMessage>();

        public static int Add(Exception error)
        {
            int id = modCommon.RandomID();
            ListError.Add(new ErrorMessage(id, error));
            return id;
        }
        public static Exception Get(int id)
        {
            Exception error = null;
            foreach (ErrorMessage item in ListError)
            {
                if (item.ID == id)
                {
                    error = item.Error;
                }
            }
            return error;
        }
    }

}