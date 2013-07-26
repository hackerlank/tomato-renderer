using System;
using System.Collections.Generic;

namespace Tomato.Collections
{
	public delegate void ExecutableCollectionItemEventHandler<T>( T item ) where T : class, IExecutableItem;
}