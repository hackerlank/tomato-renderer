namespace Tomato.Graphics.Console
{
    public enum ConsoleMessageType : ushort
    {
        None,

        List,
        Remove,
        Reset,
        Clear,
        SaveToFile,
        DeleteSaveFile,
        Get,
        Set,
        UserCommand
    };
}
