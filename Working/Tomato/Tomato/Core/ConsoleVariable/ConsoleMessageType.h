namespace Tomato
{
	struct ConsoleMessageType
	{
		enum Type
		{
			None,

			List,
			Remove,
			Reset,
			Clear,
			SaveFile,
			DeleteFile,
			Get,
			Set,
			UserCommand,

			FORCEDWORD = 0x7FFFFFFF
		};
	};
}