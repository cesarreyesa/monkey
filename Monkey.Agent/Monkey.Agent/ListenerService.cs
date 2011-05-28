using System;
using System.Collections.Generic;
using System.ComponentModel;
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
         
      }

      protected override void OnStop()
      {
      }
   }
}
