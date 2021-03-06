﻿using System;
using Ninject;
using TaskHistory.Api.Terminal;
using TaskHistory.Impl.Terminal;

namespace TaskHistory.Bindings
{
	public class TerminalModule : IModule
	{
		public void Bind(IKernel kernel)
		{
			if (kernel == null)
				throw new ArgumentNullException(nameof(kernel));

			kernel.Bind<ITerminalInterpreter>()
				  .To<TerminalInterpreter>();

			kernel.Bind<ITerminalProxyRepo>()
				  .To<TerminalProxyRepo>();

			kernel.Bind<IRegisteredObjectRepoProxy>()
				  .To<RegisteredObjectRepoProxy>();

			kernel.Bind<ITerminalObjectMapper>()
				  .To<TerminalObjectMapper>();

			kernel.Bind<ITerminalObjectFactory>()
				  .To<TerminalObjectFactory>();
		}
	}
}
