using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;

namespace MoreMenus
{
    class CommandLineAction : IMenuAction
    {
        [SimplePlugins.Factory("CommandLine", Description = "")]
        public class Factory : SimplePlugins.TypeRegistry<IMenuAction>.IFactory
        {
            public IMenuAction Create(IDictionary<string, object> parms)
            {
                var action = new CommandLineAction()
                {
                    Command = (string)parms["Command"],
                    Parameters = (string)parms["Parameters"]
                };
                return action;
            }
        }

        public string Command
        {
            get;
            set;
        }

        public string Parameters
        {
            get;
            set;
        }

        public void Fire(DTE2 applicationObject)
        {
            System.Diagnostics.Process.Start(Command, Parameters);
        }
    }
}
