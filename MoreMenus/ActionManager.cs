using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreMenus
{
    class ActionManager
    {
        private SimplePlugins.Registry _registry = new SimplePlugins.Registry();

        public ActionManager()
        {
            _registry.Load(GetType().Assembly);
        }
        
        public ActionManager(string path)
        {
            _registry.LoadDirectory(path);
            _registry.Load(GetType().Assembly);
        }

        public IMenuAction Create(string name, IDictionary<string, object> args)
        {
            return _registry.Create<IMenuAction>(name, args);
        }
    }
}
