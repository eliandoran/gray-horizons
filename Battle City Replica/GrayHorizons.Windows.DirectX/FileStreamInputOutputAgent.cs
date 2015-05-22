using System;
using GrayHorizons.Logic;
using System.IO;

namespace GrayHorizons.Windows.DirectX
{
    public class FileStreamInputOutputAgent: InputOutputAgent
    {
        #region InputOutputAgent implementation

        public Stream GetStream(string file, FileMode fileMode)
        {
            return new FileStream(file, fileMode);
        }

        #endregion
    }
}

