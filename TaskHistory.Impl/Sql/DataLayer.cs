﻿using System;
using TaskHistory.Api.Sql;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace TaskHistory.Impl.MySql
{
	public class DataLayer : IDataLayer
	{
		public T ExecuteReader<T> (IFromDataReaderFactory<T> factory, 
			string storedProcedureName,
			IEnumerable<ISqlDataParameter> parameters)
		{
			if (factory == null)
				throw new ArgumentNullException ("factory");

			if (parameters == null)
				throw new ArgumentNullException ("parameters");

			if (storedProcedureName == string.Empty || storedProcedureName == null)
				throw new ArgumentNullException ("storedProcedureName");

			using (var connection = new MySqlConnection (ConfigurationManager.AppSettings ["MySqlConnection"]))
			using (var command = new MySqlCommand (storedProcedureName, connection)) 
			{
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.AddRange (CreateMySqlParametersFromSqlDataParams(parameters));
				command.Connection.Open ();

				MySqlDataReader reader = command.ExecuteReader (CommandBehavior.CloseConnection);
				T returnVal = default(T);

				if (reader.Read ()) 
				{
					returnVal = factory.CreateTypeFromDataReader (reader);
				}
				return returnVal;
			}
		}

		public static List<MySqlParameter> CreateMySqlParametersFromSqlDataParams(IEnumerable<ISqlDataParameter> parameters)
		{
			if (parameters == null)
				throw new ArgumentNullException ("parameters");

			var returnVal = new List<MySqlParameter> ();

			foreach (var p in parameters) 
			{
				returnVal.Add (null);
			}

			return returnVal;
		}

		public void ExecuteNonQuery()
		{
		}

		public DataLayer ()
		{
		}
	}
}

