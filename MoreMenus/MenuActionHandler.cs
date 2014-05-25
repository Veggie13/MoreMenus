using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace MoreMenus
{
    class MenuActionHandler
    {
        private DTE2 _applicationObject;
        private List<CommandBarEvents> _events = new List<CommandBarEvents>();
        private Dictionary<CommandBarControl, IMenuAction> _actions = new Dictionary<CommandBarControl, IMenuAction>();

        public MenuActionHandler(DTE2 appObj)
        {
            _applicationObject = appObj;
        }

        public void Attach(CommandBarControl ctrl, IMenuAction action)
        {
            var events = (CommandBarEvents)_applicationObject.Events.get_CommandBarEvents(ctrl);
            events.Click += new _dispCommandBarControlEvents_ClickEventHandler(events_Click);
            _events.Add(events);
            _actions[ctrl] = action;
        }

        private void events_Click(object commandBarControl, ref bool Handled, ref bool CancelDefault)
        {
            var ctrl = (CommandBarControl)commandBarControl;
            _actions[ctrl].Fire(_applicationObject);
        }
    }
}
