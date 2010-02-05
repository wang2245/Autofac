﻿using System;
using NHibernate.Bytecode.Lightweight;
using NHibernate.Properties;
using Autofac;

namespace AutofacContrib.NHibernate
{
	public class AutofacReflectionOptimizer : ReflectionOptimizer
	{
		private readonly IContainer container;

		public AutofacReflectionOptimizer(IContainer container, Type mappedType, IGetter[] getters, ISetter[] setters)
			: base(mappedType, getters, setters)
		{
			this.container = container;
		}

		public override object CreateInstance()
		{
			if (container.IsRegistered(mappedType))
			{
				return container.Resolve(mappedType);
			}
			else
			{
				return container.IsRegistered(mappedType.FullName, mappedType)
					? container.Resolve(mappedType.FullName, mappedType)
					: base.CreateInstance();
			}
		}

		protected override void ThrowExceptionForNoDefaultCtor(Type type) { }
	}
}