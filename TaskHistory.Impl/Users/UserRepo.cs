﻿using System;
using System.Collections.Generic;
using TaskHistory.Api.Sql;
using TaskHistory.Api.Users;
using TaskHistory.Impl.Shared;
using TaskHistory.Impl.Sql;

namespace TaskHistory.Impl.Users
{
	public class UserRepo : BaseRepo, IUserRepo
	{
		const string UserRegisterStoredProcedure = "Users_Insert";
		const string UserValidateStoredProcedure = "User_Validate";

		readonly UserFactory _userFactory;

		public IUser ValidateUsernameAndPassword(string username, string password)
		{
			if (string.IsNullOrEmpty(username))
				throw new ArgumentNullException(nameof(username));

			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException(nameof(password));

			var parameters = new List<ISqlDataParameter>();

			parameters.Add(_dataProxy.CreateParameter("pUsername", username));
			parameters.Add(_dataProxy.CreateParameter("pPassword", password));

			var validatedUser = _dataProxy.ExecuteReader(_userFactory,
									UserValidateStoredProcedure,
									parameters);
			// [TODO] https://github.com/rhsu/TaskHistory/issues/124
			//null means that the user is not valid
			return validatedUser;
		}

		public IUser RegisterUser(UserRegistrationParameters userParams)
		{
			if (userParams == null)
				throw new NullReferenceException("userParams");

			var parameters = CreateDataParameterCollectionFromUserParams(userParams);
			if (parameters == null)
				throw new NullReferenceException("Null returned from CreatingDataParameterCollectionFromUserParams");

			var registeredUser = _dataProxy.ExecuteReader(_userFactory,
									UserRegisterStoredProcedure,
									 parameters);
			// [TODO] https://github.com/rhsu/TaskHistory/issues/124
			// null from dataProxy means that the user is already registered
			return registeredUser;
		}

		IEnumerable<ISqlDataParameter> CreateDataParameterCollectionFromUserParams(
			UserRegistrationParameters userParams)
		{
			if (userParams == null)
				throw new ArgumentNullException(nameof(userParams));

			var returnVal = new List<ISqlDataParameter>();

			returnVal.Add(_dataProxy.CreateParameter("pUsername", userParams.Username));
			returnVal.Add(_dataProxy.CreateParameter("pPassword", userParams.Password));
			returnVal.Add(_dataProxy.CreateParameter("pEmail", userParams.Email));
			returnVal.Add(_dataProxy.CreateParameter("pFirstName", userParams.FirstName));
			returnVal.Add(_dataProxy.CreateParameter("pLastName", userParams.LastName));

			return returnVal;
		}

		public UserRepo(UserFactory userFactory, ApplicationDataProxy dataProxy)
			:base(dataProxy)
		{
			_userFactory = userFactory;
			_dataProxy = dataProxy;
		}
	}
}