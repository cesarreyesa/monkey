using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using Monkey.HttpAgent.Commands;

namespace Monkey.HttpAgent
{
   internal class Repository
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
                  action.Command = new PostgreSQLBackupCommand(action.Arg1, action.Arg2, action.Arg3, action.Arg4);
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

      internal void SaveAction(Action action)
      {
         var cn = new SQLiteConnection("Data Source=" + DbName + ";Version=3;");
         cn.Open();

         try
         {
            var current = GetAction(action.Id);
            if(current != null)
            {
               var cmd = new SQLiteCommand("update actions set command = @command, arg1 = @arg1, arg2 = @arg2, arg3 = @arg3, arg4 = @arg4 where id = @id");
               cmd.Parameters.Add(new SQLiteParameter("id", action.Id));
               cmd.Parameters.Add(new SQLiteParameter("command", action.Command.Name));
               cmd.Parameters.Add(new SQLiteParameter("arg1", action.Arg1));
               cmd.Parameters.Add(new SQLiteParameter("arg2", action.Arg2));
               cmd.Parameters.Add(new SQLiteParameter("arg3", action.Arg3));
               cmd.Parameters.Add(new SQLiteParameter("arg4", action.Arg4));
               cmd.ExecuteNonQuery();
            }
            else
            {
               var cmd = new SQLiteCommand("insert into actions (id, command, arg1, arg2, arg3, arg4) values (@id, @command, @arg1, @arg2, @arg3, @arg4)");
               cmd.Parameters.Add(new SQLiteParameter("id", action.Id));
               cmd.Parameters.Add(new SQLiteParameter("command", action.Command.Name));
               cmd.Parameters.Add(new SQLiteParameter("arg1", action.Arg1));
               cmd.Parameters.Add(new SQLiteParameter("arg2", action.Arg2));
               cmd.Parameters.Add(new SQLiteParameter("arg3", action.Arg3));
               cmd.Parameters.Add(new SQLiteParameter("arg4", action.Arg4));
               cmd.ExecuteNonQuery();
            }
         }
         finally
         {
            cn.Close();
         }
      }
   }
}
