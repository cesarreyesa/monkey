﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Monkey.HttpAgent.Commands;

namespace Monkey.HttpAgent
{
   class Action
   {
      public string Id { get; set; }

      public Command Command { get; set; }

      public string Arg1 { get; set; }

      public string Arg2 { get; set; }

      public string Arg3 { get; set; }

      public string Arg4 { get; set; }

   }

}
