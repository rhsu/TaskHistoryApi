﻿using System;

namespace TaskHistoryApi.Users
{
	public interface IUser
	{
		int UserId { get; }
		string Username { get; }
		string FirstName { get; }
		string LastName { get; }
		string FullName { get; }
		string Email { get; }
	}
}