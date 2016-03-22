﻿using System;
using System.Collections.Generic;
using TaskHistoryApi.Labels;
using TaskHistoryApi.Users;
using TaskHistoryImpl.MySql;
using MySql.Data.MySqlClient;
using System.Data;

namespace TaskHistoryImpl.Labels
{
	public class LabelRepo : ILabelRepo
	{
		private readonly LabelFactory _labelFactory;
		private readonly MySqlCommandFactory _mySqlCommandFactory;

		public IEnumerable<ILabel> GetAllLabelsForUser(IUser user)
		{
			if (user == null)
				throw new ArgumentNullException ("user");

			var returnVal = new List<ILabel> ();

			var command = _mySqlCommandFactory.CreateMySqlCommand ("Labels_ForUser_Select");
			command.Parameters.Add (new MySqlParameter ("pUserId", user.UserId));
			command.Connection.Open ();

			MySqlDataReader reader = command.ExecuteReader (CommandBehavior.CloseConnection);

			while (reader.Read ()) 
			{
				ILabel label = CreateLabelFromReader (reader);
				returnVal.Add (label);
			}

			command.Connection.Close ();
			
			return returnVal;
		}

		public ILabel InsertNewLabel (string content)
		{
			var command = _mySqlCommandFactory.CreateMySqlCommand ("Labels_Insert");
			command.Parameters.Add(new MySqlParameter("pContent", content));
			command.Connection.Open ();

			MySqlDataReader reader = command.ExecuteReader (CommandBehavior.CloseConnection);
			ILabel label = null;

			if (reader.Read ()) 
			{
				label = CreateLabelFromReader (reader);
			}

			command.Connection.Close ();

			return label;
		}

		public void DeleteLabel(int labelId)
		{
			
		}

		public void UpdateLabel (ILabel labelDto)
		{
			if (labelDto == null)
				throw new ArgumentNullException ("labelDto");
		}

		private ILabel CreateLabelFromReader(MySqlDataReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException ("reader");

			int labelId = Convert.ToInt32 (reader ["labelId"]);
			string name = reader ["name"].ToString ();
			ILabel label = _labelFactory.CreateLabel (labelId, name);

			return label;
		}

		public LabelRepo (LabelFactory labelFactory, MySqlCommandFactory mySqlCommandFactory)
		{
			_labelFactory = labelFactory;
			_mySqlCommandFactory = mySqlCommandFactory;
		}
	}
}

