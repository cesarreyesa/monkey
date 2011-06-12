using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monkey.HttpAgent.Commands
{
   internal abstract class Command
   {
      public string Name;
      public abstract void Run();
   }
}
