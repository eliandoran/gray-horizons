#region Using Statements
using System;
using GrayHorizons;

#endregion

namespace GrayHorizons.Windows.DirectX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main ()
        {
            new GrayHorizonsGame ().Run ();
        }
    }
}
