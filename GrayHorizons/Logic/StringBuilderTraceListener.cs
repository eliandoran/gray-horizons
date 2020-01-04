namespace GrayHorizons.Logic
{
    using System.Diagnostics;
    using System.Text;

    public class StringBuilderTraceListener: TextWriterTraceListener
    {
        readonly StringBuilder builder;
        bool skip;

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
            if (skip)
            {
                skip = false;
                return;
            }

            builder.AppendLine(message);
            skip = true;
        }
    }
}

