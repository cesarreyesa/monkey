using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Text;

namespace Monkey.HttpAgent.Commands
{
   class SyncActionsCommand : Command
   {
      private readonly string actionsJson;

      private static readonly Repository Repository = new Repository();

      public SyncActionsCommand(string actionsJson)
      {
         this.actionsJson = actionsJson;
      }

      public override void Run()
      {
         var actions = new JsonSerializer<List<Action>>().DeserializeFromString(actionsJson);
         foreach (var action in actions)
         {
            Repository.SaveAction(action);
         }
      }
   }
}
