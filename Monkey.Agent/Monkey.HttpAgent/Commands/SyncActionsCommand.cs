using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monkey.HttpAgent.Commands
{
   class SyncActionsCommand : ICommand
   {
      private readonly string actionsJson;

      public SyncActionsCommand(string actionsJson)
      {
         this.actionsJson = actionsJson;
      }

      public void Run()
      {
         
      }
   }
}
