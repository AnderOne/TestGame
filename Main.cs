using Microsoft.Xna.Framework;
using System;

namespace TestGame {
	static class MainClass {
		[STAThread]
		static void Main() {
			using (var game = new TestGame())
				game.Run();
		}
	}

}
