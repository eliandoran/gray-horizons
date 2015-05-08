#region Using Statements
using System;
using BattleCity;

#endregion

namespace BattleCity.DirectX
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            new BattleCityGame ().Run ();
        }
    }
}
