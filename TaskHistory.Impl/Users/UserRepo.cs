﻿using System;
using TaskHistory.Api.Users;
using TaskHistory.Impl.MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace TaskHistoryImpl.Users
{
	public class UserRepo : IUserRepo
	{
		private readonly UserFactory _userFactory;

		public IUser ValidateUsernameAndPassword (string username, string password)
		{
			using (var connection = new MySqlConnection (ConfigurationManager.AppSettings ["MySqlConnection"]))
			using (var command = new MySqlCommand ("User_Validate", connection)) 
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add (new MySqlParameter ("pUsername", username));
				command.Parameters.Add (new MySqlParameter ("pPassword", password));
				command.Connection.Open ();

				using (var reader = command.ExecuteReader (CommandBehavior.CloseConnection)) 
				{
					IUser user = null;

					if (reader.Read ()) 
					{
						user = _userFactory.CreateUser (reader);
					}

					return user;
				}
			}
		}

		// TODO Too many parameters https://github.com/rhsu/TaskHistory/issues/82
		public IUser RegisterUser (string username, string password, string firstName, string lastName, string email)
		{
			using (var connection = new MySqlConnection (ConfigurationManager.AppSettings ["MySqlConnection"]))
			using (var command = new MySqlCommand("Users_Insert", connection))
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add (new MySqlParameter ("pUsername", username));
				command.Parameters.Add (new MySqlParameter ("pPassword", password));
				command.Parameters.Add (new MySqlParameter ("pFirstName", firstName));
				command.Parameters.Add (new MySqlParameter ("pLastName", lastName));			
				command.Parameters.Add (new MySqlParameter ("pEmail", email));
				command.Connection.Open ();

				using (MySqlDataReader reader = command.ExecuteReader (CommandBehavior.CloseConnection)) 
				{
					IUser user = null;

					if (reader.Read ()) 
					{
						user = _userFactory.CreateUser (reader);
					}

					return user;
				}
			}
		}

		public UserRepo (UserFactory userFactory)
		{
			_userFactory = userFactory;
		}
	}
}

