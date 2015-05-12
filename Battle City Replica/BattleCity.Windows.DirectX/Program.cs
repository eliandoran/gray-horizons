#region Using Statements
using System;
using BattleCity;

#endregion

namespace BattleCity.Windows.DirectX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main ()
        {
            new BattleCityGame ().Run ();
        }
    }
}
