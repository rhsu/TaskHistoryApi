﻿using System;
using TaskHistory.Api.Users;

namespace TaskHistory.Impl.Users
{
	public class AdminUserProvider : IAdminUserProvider
	{
		const string _default_name = "admin";
		const string _default_password = "password";

		UserFactory _userFactory;

		public AdminUserProvider(UserFactory userFactory)
		{
			_userFactory = userFactory;
		}

		IUser BuildAdminUser()
		{
			IUser user = _userFactory.Build(-1, "admin", "admin", "admin", "admin");
			if (user == null)
				throw new NullReferenceException("null user returned");

			return user;
		}

		public IUser AuthenticateAdminUser(string name, string password)
		{
			if (string.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (string.IsNullOrEmpty(password))
				throw new ArgumentNullException(nameof(password));

			//1 check for static value
			//1 name = admin and password = password
			// TODO for now this is probably OK
			// Need to make sure that admin is not a valid username when registering
			if ((name == _default_name) && (password == _default_password))
			{
				return BuildAdminUser();
			}

			// TODO
			//2 check for webconfig values

			// TODO
			//3 check for database values (maybe certain users are admins)

			// TODO for number 3 need to make sure that users have Roles 
			// This requires more digging into MVC

			// throw new AuthenticationException("Invalid Username and Password");
			return null;
		}
	}
}