using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Text.RegularExpressions;

namespace MultiSender.ViewModels
{

    public class ActionItem
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Models.Recipient Recipient { get; set; }
        public bool Enabled { get; set; }
    }

    class LogicManager
    {
        internal BoundedVars Bounded { get; private set; }

        internal LogicManager()
        {
            Bounded = new BoundedVars();
            Models.Params.Init();
            Bounded.Recipients = new System.Collections.ObjectModel.ObservableCollection<Models.Recipient>(Models.Params.Recipients);
            BuildActionItemsFromFiles();
            Bounded.Headline = "Welcome";
        }

        internal void MassSend()
        {
            int mail_count = 0;
            foreach (var rec in Models.Params.Recipients)
            {
                bool sent=sendEMailThroughOUTLOOK(rec);
                mail_count = (sent ? mail_count + 1 : mail_count);
            }
            Bounded.Headline = mail_count+" Messages sent";
        }

        internal void LaunchConfig()
        {
            System.Diagnostics.Process.Start(Models.Params.XML_File);
            Bounded.Headline = "Restart the application after editing the config file";
        }
        internal bool AutorunIfHasCommandArg()
        {
            bool has_auto_arg=false;
            string[] args = Environment.GetCommandLineArgs();
            foreach (var item in args)
            {
                has_auto_arg = item.ToLower().Contains("auto");
            }
            if (!has_auto_arg)
                return false;
            MassSend();
            //end:
            System.Windows.Application.Current.Shutdown();
            return true;
        }

        #region Helper Funcs
        private void BuildActionItemsFromFiles()
        {
            foreach (var dir in Models.Params.Dirs)
            {
                DirectoryInfo DI = new DirectoryInfo(dir.Path);
                foreach (FileInfo f in DI.GetFiles(dir.Filter))
                {
                    Bounded.ActionItems.Add(new ActionItem() { FileName = f.Name, FilePath = f.FullName, Enabled = true,
                        Recipient=(from rec in Models.Params.Recipients where RegexMatched(rec.Regex,f.Name) select rec).FirstOrDefault()});
                }
            }
        }
        private bool RegexMatched(string regex,string input)
        {
            try 
	        {
                return Regex.Match(input, regex).Success;
	        }
	        catch (Exception)
	        {
                return false;
	        }
        }
        //method to send email to outlook
        private bool sendEMailThroughOUTLOOK(Models.Recipient Recipient)
        {
            bool success = false;
            try
            {
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();
                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                // Set HTMLBody. 
                //add the body of the email
                oMsg.HTMLBody = Models.Params.Message;
                //Add an attachment.
                int iPosition = (int)oMsg.Body.Length + 1;
                int iAttachType = (int)Outlook.OlAttachmentType.olByValue;
                //now attached the file
                foreach (var item in Bounded.ActionItems)
                {
                    if (item.Recipient == Recipient)
                    {
                        Outlook.Attachment oAttach = oMsg.Attachments.Add(item.FilePath, iAttachType, iPosition, item.FileName);
                    }
                }
                //Subject line
                oMsg.Subject = Models.Params.Subject;
                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(Recipient.Email);
                oRecip.Resolve();
                // Send messages
                if (oMsg.Attachments.Count > 0)
                {
                    oMsg.Send();
                    success = true;
                }
                else
                {
                    oMsg.Delete();
                    success = false;
                }
                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;
            }
            catch (Exception)
            {
                return success=false;
            }
        return success;
        }

        #endregion
    }
}
