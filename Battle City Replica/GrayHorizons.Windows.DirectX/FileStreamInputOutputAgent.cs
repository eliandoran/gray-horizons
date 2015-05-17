using System;
using GrayHorizons.Logic;
using System.IO;

namespace GrayHorizons.Windows.DirectX
{
    public class FileStreamInputOutputAgent: InputOutputAgent
    {
        #region InputOutputAgent implementation

        public Stream GetStream (string file, bool readOnly)
        {
            FileMode fileMode = (readOnly ? FileMode.Open : FileMode.Create);
            FileStream fs = new FileStream (file, fileMode);

            return fs;
        }

        #endregion
    }
}

