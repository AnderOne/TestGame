#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace TestGame {

	static class MainClass {
		private static TestGame game;
		[STAThread]
		static void Main() {
			game = new TestGame();
			game.Run();
		}
	}

}
