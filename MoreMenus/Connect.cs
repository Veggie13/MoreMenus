﻿using System;
using System.Linq;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace MoreMenus
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;

            _actionMgr = new ActionManager();
            _handler = new MenuActionHandler(_applicationObject);

            if (connectMode == ext_ConnectMode.ext_cm_AfterStartup)
            {
                object[] contextGUIDS = new object[] { };
                Commands2 commands = (Commands2)_applicationObject.Commands;

                // get popUp command bars where commands will be registered.
                CommandBars cmdBars = (CommandBars)(_applicationObject.CommandBars);
                CommandBar vsBarCodeWindow = cmdBars["Code Window"];

                //This try/catch block can be duplicated if you wish to add multiple commands to be handled by your Add-in,
                //  just make sure you also update the QueryStatus/Exec method to include the new command names.
                try
                {
                    CommandBarBuilder builder = new CommandBarBuilder(_actionMgr, _handler);
                    builder.BuildMenu(vsBarCodeWindow.Controls, MenuXml.Load(@"C:\Corey Derochie\test.xml"));
                }
                catch (System.ArgumentException argEx)
                {
                    System.Diagnostics.Debug.Write("Exception in HintPaths:" + argEx.ToString());
                    //If we are here, then the exception is probably because a command with that name
                    //  already exists. If so there is no need to recreate the command and we can 
                    //  safely ignore the exception.
                }
            }
        }

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
            if (disconnectMode == ext_DisconnectMode.ext_dm_UserClosed)
            {
                try
                {
                    Commands2 commands = (Commands2)_applicationObject.Commands;
                    CommandBars cmdBars = (CommandBars)(_applicationObject.CommandBars);
                    CommandBar codeWindow = cmdBars["Code Window"];
                    var controls = codeWindow.GetControls().Where(ctrl => ctrl.Caption.Equals("SvnAddin")).ToList();
                    foreach (var ctrl in controls)
                    {
                        ctrl.Delete();
                    }
                }
                catch (Exception)
                {
                    
                }
            }
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		private DTE2 _applicationObject;
		private AddIn _addInInstance;
        private ActionManager _actionMgr;
        private MenuActionHandler _handler;
	}

    static class Extensions
    {
        public static IEnumerable<Command> GetCommands(this DTE2 me)
        {
            for (int i = 1; i <= me.Commands.Count; i++)
            {
                yield return me.Commands.Item(i);
            }
        }

        public static IEnumerable<CommandBar> GetCommandBars(this DTE2 me)
        {
            CommandBars bars = (CommandBars)me.CommandBars;
            for (int i = 1; i <= bars.Count; i++)
            {
                yield return bars[i];
            }
        }

        public static IEnumerable<CommandBarControl> GetControls(this CommandBar me)
        {
            for (int i = 1; i <= me.Controls.Count; i++)
            {
                yield return me.Controls[i];
            }
        }
    }
}