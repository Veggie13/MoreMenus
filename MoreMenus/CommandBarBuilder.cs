using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.CommandBars;
using EnvDTE;
using EnvDTE80;

namespace MoreMenus
{
    class CommandBarBuilder
    {
        private class Visitor : MenuNodeDefinition.IVisitor
        {
            private CommandBarBuilder _builder;

            public Visitor(CommandBarBuilder builder, CommandBarPopup parent)
            {
                _builder = builder;
                Parent = parent;
            }

            public CommandBarPopup Parent
            {
                get;
                private set;
            }

            public void Visit(MenuTreeDefinition node)
            {
                var subMenu = _builder.addSubMenu(Parent.Controls, node.Caption);
                var childVisitor = new Visitor(_builder, subMenu);

                foreach (var child in node.Children)
                {
                    child.Accept(childVisitor);
                }
            }

            public void Visit(MenuItemDefinition node)
            {
                var ctrl = _builder.addItem(Parent.Controls, node.Caption);
                _builder._handler.Attach(ctrl, _builder.createAction(node));
            }
        }

        private ActionManager _actionMgr;
        private MenuActionHandler _handler;

        public CommandBarBuilder(ActionManager actionMgr, MenuActionHandler handler)
        {
            _actionMgr = actionMgr;
            _handler = handler;
        }

        public void BuildMenu(CommandBarControls menu, MenuXml rootNode)
        {
            var root = addSubMenu(menu, "MoreMenus");
            var visitor = new Visitor(this, root);
            foreach (var item in rootNode.Items)
            {
                item.Accept(visitor);
            }
        }

        private CommandBarControl addItem(CommandBarControls popup, string name)
        {
            CommandBarControl ctrl = popup.Add(
                MsoControlType.msoControlButton,
                Type.Missing,
                Type.Missing);
            ctrl.Caption = name;
            return ctrl;
        }

        private CommandBarPopup addSubMenu(CommandBarControls popup, string name)
        {
            CommandBarControl ctrl = popup.Add(
                MsoControlType.msoControlPopup,
                Type.Missing,
                Type.Missing);
            ctrl.Caption = name;
            return (CommandBarPopup)ctrl;
        }

        private IMenuAction createAction(MenuItemDefinition node)
        {
            var args = node.Arguments.ToDictionary(arg => arg.Name, arg => (object)arg.Value);
            return _actionMgr.Create(node.Action, args);
        }
    }
}
