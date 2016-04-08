﻿using System;
using TaskHistory.Api.Users;
using TaskHistory.ViewModel.Users;

namespace TaskHistory.Orchestrator
{
	public class HomeOrchestrator
	{
		private IUserRepo _userRepo;

		public IUser OrchestrateRegisterUser(UserRegisterViewModel vmUserRegister)
		{
			if (vmUserRegister == null)
				throw new ArgumentNullException ("vmUserRegister");

			IUser newUser = _userRepo.RegisterUser (vmUserRegister.Username, 
				                vmUserRegister.Password, 
				                vmUserRegister.FirstName,
								vmUserRegister.LastName,
				                vmUserRegister.Email);

			return newUser;
		}

		public HomeOrchestrator (IUserRepo userRepo)
		{
			_userRepo = userRepo;
		}
	}
}

