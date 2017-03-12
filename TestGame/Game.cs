#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
#endregion

namespace TestGame {

	public class TestGame: Game {
		//...
		private GraphicsDeviceManager graphics;
		private Field field;
		private const double timeLimit = 60;
		private MainMenu menu;
		private SpriteFont font;
		private double start_time, curr_time;

		public TestGame() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			//graphics.IsFullScreen = true;
		}

		protected override void Initialize() {
			//...
			base.Initialize();
			//...
			//paint = menu;
			start_time = 0;
			//...
			//int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			int w = graphics.PreferredBackBufferWidth, h = graphics.PreferredBackBufferHeight;
			int s = (h < w)? (h): (w);
			s = (4 * s) / 5;
			field = new Field(this, new Rectangle(w / 2 - s / 2, h / 2 - s / 2, s, s), new Point(8, 8), timeLimit);
			menu = new MainMenu(this, field, w, h);
			//...
			Components.Add(field);
			field.Enabled = field.Visible = false;
			Components.Add(menu);
			//menu.Enabled = menu.Visible = false;
			//...
		}

		protected override void LoadContent() {
			base.LoadContent();
		}

		protected override void Update(GameTime gameTime) {
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit();
			}
			//...
			if (field.isStop()) {
				field.Enabled = field.Visible = false;
				menu.setStart(false);
				menu.Enabled = menu.Visible = true;
				field.Reset(timeLimit);
			}
			//...
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
		}
		//...
	}
}
