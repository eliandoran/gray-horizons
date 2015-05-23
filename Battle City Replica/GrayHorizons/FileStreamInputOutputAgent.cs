using System;
using GrayHorizons.Logic;
using System.IO;

namespace GrayHorizons
{
    public class FileStreamInputOutputAgent: IInputOutputAgent
    {
        #region InputOutputAgent implementation

        public Stream GetStream(string file, FileMode fileMode)
        {
            return new FileStream(file, fileMode);
        }

        #endregion
    }
}

