using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Monkey.Agent
{
   public partial class ListenerService : ServiceBase
   {
      public ListenerService()
      {
         InitializeComponent();
      }

      protected override void OnStart(string[] args)
      {
         new Process {StartInfo = new ProcessStartInfo(ConfigurationManager.AppSettings["agent.path"])}.Start();
      }

      protected override void OnStop()
      {
      }
   }
}
