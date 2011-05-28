using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Monkey.HttpAgent.Commands
{
   class PostgreSQLBackupCommand : ICommand
   {
      private readonly string dbName;
      private readonly string userName;
      private readonly string dumpExePath;
      private readonly string destination;

      public PostgreSQLBackupCommand(string dbName, string destination, string userName, string dumpExePath)
      {
         this.dbName = dbName;
         this.destination = destination;
         this.userName = userName;
         this.dumpExePath = dumpExePath;
      }

      public void Run()
      {
         Console.WriteLine("Haciendo backup de la base de datos {0} a la ruta {1}", dbName, destination);
         var arguments = " -U " + userName + " -f " + destination + " " + dbName;
         var procStartInfo = new System.Diagnostics.ProcessStartInfo(dumpExePath, arguments) {
                                   RedirectStandardOutput = true,
                                   UseShellExecute = false,
                                   CreateNoWindow = true
                                };
         var proc = new System.Diagnostics.Process {StartInfo = procStartInfo};
         proc.Start();
         proc.WaitForExit();
      }
   }
}
