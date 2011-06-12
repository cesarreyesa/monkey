using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monkey.HttpAgent.Commands
{
   class ExecuteBatCommand : Command
   {
      private readonly string batPath;

      public ExecuteBatCommand(string batPath)
      {
         this.batPath = batPath;
      }

      public override void Run()
      {
         var procStartInfo = new System.Diagnostics.ProcessStartInfo(batPath)
         {
            RedirectStandardOutput = false,
            WindowStyle =System.Diagnostics.ProcessWindowStyle.Normal,
           UseShellExecute = false,
            CreateNoWindow = false, 
         };
         var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
         
         proc.Start();
         proc.WaitForExit();
      }
   }
}
