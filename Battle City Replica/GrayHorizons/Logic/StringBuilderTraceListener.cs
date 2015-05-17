using System;
using System.Diagnostics;
using System.Text;

namespace GrayHorizons.Logic
{
    public class StringBuilderTraceListener: TextWriterTraceListener
    {
        readonly StringBuilder builder;

        public StringBuilder Builder
        {
            get
            {
                return builder;
            }
        }

        public StringBuilderTraceListener()
        {
            builder = new StringBuilder();
        }

        public override void Write(string message)
        {
            builder.Append(message);
        }

        public override void WriteLine(string message)
        {
            builder.AppendLine(message);
        }
    }
}

