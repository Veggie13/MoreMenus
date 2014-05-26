using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EnvDTE80;
using EnvDTE;

namespace MoreMenus
{
    class TortoiseSvnAction : CommandLineAction
    {
        [SimplePlugins.Factory("TortoiseSVN")]
        public class Factory : SimplePlugins.TypeRegistry<IMenuAction>.IFactory
        {
            public IMenuAction Create(IDictionary<string, object> parms)
            {
                string tsvnPath = Environment.ExpandEnvironmentVariables(@"%ProgramW6432%\TortoiseSVN\bin\TortoiseProc.exe");
                if (!File.Exists(tsvnPath))
                {
                    tsvnPath = Environment.ExpandEnvironmentVariables(@"%ProgramFiles(x86)%\TortoiseSVN\bin\TortoiseProc.exe");
                    if (!File.Exists(tsvnPath))
                    {
                        throw new Exception("Could not find TortoiseSVN.");
                    }
                }

                string parmList = string.Join(" ",
                    parms.Keys.Select(k => string.Format("/{0}:{1}", k, parms[k])));

                var action = new TortoiseSvnAction()
                {
                    Command = tsvnPath,
                    Parameters = parmList
                };
                return action;
            }
        }

        protected override string formatParameters(DTE2 applicationObject)
        {
            var doc = applicationObject.DTE.ActiveDocument;
            var sel = (TextSelection)doc.Selection;

            string path = doc.FullName;
            int lineNum = sel.ActivePoint.Line;

            return Parameters.Replace("{documentPath}", path)
                .Replace("{lineNum}", lineNum.ToString());
        }
    }
}
