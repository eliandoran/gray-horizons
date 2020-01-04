using System;
using System.IO;

namespace GrayHorizons.Logic
{
    public interface InputOutputAgent
    {
        Stream GetStream(string name, FileMode fileMode);
    }
}

