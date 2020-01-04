namespace GrayHorizons.Logic
{
    using System.IO;

    public interface IInputOutputAgent
    {
        Stream GetStream(string name, FileMode fileMode);
    }
}

