using System;

namespace Islander
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Islander game = new Islander())
            {
                game.Run();
            }
        }
    }
#endif
}

