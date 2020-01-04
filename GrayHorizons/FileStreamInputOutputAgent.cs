namespace GrayHorizons
{
    using System;
    using System.IO;
    using GrayHorizons.Logic;

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

