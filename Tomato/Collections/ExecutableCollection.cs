using System;
using System.Collections.Generic;

namespace Tomato.Collections
{
	public class ExecutableCollection<T> where T : class, IExecutableItem
	{
		protected List<T> Items { get; set; }

		protected Queue<PendingAction> PendingActions { get; set; }

		public bool IsExecuting { get; protected set; }

		public int Count
		{
			get { return Items.Count; }
		}

		public event ExecutableCollectionEventHandler Executing = null;
		public event ExecutableCollectionEventHandler Executed = null;

		public event ExecutableCollectionItemEventHandler<T> ItemAdded = null;
		public event ExecutableCollectionItemEventHandler<T> ItemRemoved = null;
		public event ExecutableCollectionEventHandler ItemsCleared = null;

		public ExecutableCollection()
		{
			Items = new List<T>();

			PendingActions = new Queue<PendingAction>();

			IsExecuting = false;
		}

		public void Execute()
		{
			if( Executing != null )
			{
				Executing();
			}

			IsExecuting = true;

			foreach( T item in Items )
			{
				if( item != null )
				{
					item.Execute();
				}
			}

			IsExecuting = false;

			if( Executed != null )
			{
				Executed();
			}

			// Process pending actions
			while( PendingActions.Count > 0 )
			{
				PendingAction action = PendingActions.Dequeue();
				switch( action.ActionType )
				{
					case ExecutableCollection<T>.PendingActionType.Add:
						{
							Add( action.Item );
						}
						break;

					case ExecutableCollection<T>.PendingActionType.Remove:
						{
							Remove( action.Item );
						}
						break;

					case ExecutableCollection<T>.PendingActionType.Clear:
						{
							Clear();
						}
						break;
				}
			}
		}

		public void Add( T item )
		{
			if( IsExecuting )
			{
				PendingActions.Enqueue( new PendingAction( item, ExecutableCollection<T>.PendingActionType.Add ) );
			}
			else
			{
				Items.Add( item );

				if( ItemAdded != null )
				{
					ItemAdded( item );
				}
			}
		}
		
		public void Add( params T[] items )
		{
			foreach( T item in items )
			{
				Add( item );
			}
		}

		public void Remove( T item )
		{
			if( IsExecuting )
			{
				PendingActions.Enqueue( new PendingAction( item, ExecutableCollection<T>.PendingActionType.Remove ) );
			}
			else
			{
				if( Items.Remove( item ) )
				{
					if( ItemRemoved != null )
					{
						ItemRemoved( item );
					}
				}
			}
		}

		public void Clear()
		{
			if( IsExecuting )
			{
				PendingActions.Enqueue( new PendingAction( ExecutableCollection<T>.PendingActionType.Clear ) );
			}
			else
			{
				// Back up
				List<T> copyCollection = null;
				if( ItemRemoved != null )
				{
					copyCollection = new List<T>( Items );
				}

				// Clear collection
				Items.Clear();

				// Invoke ItemRemoved event handlers
				if( copyCollection != null )
				{
					foreach( T item in copyCollection )
					{
						ItemRemoved( item );
					}
				}

				// Invoke ItemsCleared event handlers
				if( ItemsCleared != null )
				{
					ItemsCleared();
				}
			}
		}

		public bool Contains( T item )
		{
			return Items.Contains( item );
		}

		protected enum PendingActionType
		{
			Add,
			Remove,
			Clear
		}

		protected class PendingAction
		{
			public T Item { get; protected set; }
			public PendingActionType ActionType { get; protected set; }

			public PendingAction( PendingActionType actionType )
			{
				Item = null;
				ActionType = actionType;
			}

			public PendingAction( T item, PendingActionType actionType )
			{
				Item = item;
				ActionType = actionType;
			}
		}
	}
}
