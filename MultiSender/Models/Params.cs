using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MultiSender.Models
{
    class Params
    {
        public static List<Recipient> Recipients  {get;private set;}
        public static List<Dir> Dirs { get; private set; }
        public static string Message { get; private set; }
        public static string Subject { get; private set; }
        public static string XML_File { get { return FullPath("params.xml"); } }

        public static string FullPath(string relFile)
        {
            string p = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            p = p.Replace("\\bin\\Debug", "");
            p = p.Replace("\\bin\\Release", "");
            if (p != "")
            {
                p += "\\" + relFile;
            }
            return p;
        }

        public static void Init()
        {
            Init(XML_File);
        }

        public static void Init(string xmlFile)
        {
            Recipients = new List<Recipient>();
            Dirs = new List<Dir>();
            bool is_message = false;
            using (XmlReader reader = XmlReader.Create(xmlFile))
            {
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name.ToLower() == "recipient")
                            {
                                if (reader.HasAttributes)
                                {
                                    Recipient rec = new Recipient();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name.ToLower() == "name")
                                        {
                                            rec.Name = reader.Value;
                                        }
                                        else if (reader.Name.ToLower() == "email")
                                        {
                                            rec.Email = reader.Value;
                                        }
                                        else if (reader.Name.ToLower() == "regex")
                                        {
                                            rec.Regex = reader.Value;
                                        }
                                    }
                                    Recipients.Add(rec);
                                    reader.MoveToElement();
                                }
                            }
                            else if (reader.Name.ToLower() == "dir")
                            {
                                if (reader.HasAttributes)
                                {
                                    Dir dir = new Dir();
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name.ToLower() == "path")
                                        {
                                            dir.Path = reader.Value;
                                        }
                                        else if (reader.Name.ToLower() == "filter")
                                        {
                                            dir.Filter = reader.Value;
                                        }
                                    }
                                    Dirs.Add(dir);
                                    reader.MoveToElement();
                                }
                            }
                            else if (reader.Name.ToLower() == "message")
                            {
                                if (reader.HasAttributes)
                                {
                                    while (reader.MoveToNextAttribute())
                                    {
                                        if (reader.Name.ToLower() == "subject")
                                        {
                                            Subject = reader.Value;
                                        }
                                    }
                                    is_message = true;
                                    reader.MoveToElement();
                                }
                            }
                            break;
                        case XmlNodeType.Text:
                            if (is_message)
                                Message = reader.Value.Trim();
                            break;
                        case XmlNodeType.XmlDeclaration:
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.Comment:
                        case XmlNodeType.EndElement:
                            if (reader.Name.ToLower() == "message")
                                is_message = false;
                            break;
                    }
                }
            }
        }

    }
}
