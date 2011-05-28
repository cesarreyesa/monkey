using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Monkey.HttpAgent.Commands;

namespace Monkey.HttpAgent
{
   internal class Respository
   {
      private const string DbName = "monkey.db";

      internal void CreateDbIfNotExists()
      {
         if (!File.Exists(DbName))
         {
            SQLiteConnection.CreateFile(DbName);
         }
      }

      internal Action GetAction(string actionName)
      {
         var cn = new SQLiteConnection("Data Source=" + DbName + ";Version=3;");
         cn.Open();

         var reader = new SQLiteCommand("select id, command, arg1, arg2 from actions where id = '" + actionName + "'", cn).ExecuteReader();

         Action action = null;
         try
         {
            if (reader.Read())
            {
               action = new Action();
               action.Id = reader.GetString(reader.GetOrdinal("id"));
               action.Arg1 = reader.GetString(reader.GetOrdinal("arg1"));
               action.Arg2 = reader.GetString(reader.GetOrdinal("arg2"));
               //action.Arg3 = reader.GetString(reader.GetOrdinal("arg3"));

               if (reader.GetString(reader.GetOrdinal("command")) == "pgdb-backup")
               {
                  action.Command = new PostgreSQLBackupCommand(action.Arg1, action.Arg2);
               }
               action.Command.Run();
            }
         }
         finally
         {
            cn.Close();
         }
         return action;
      }
   }
}
