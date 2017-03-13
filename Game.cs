using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TestGame {

	public class TestGame: Game {
		//...
		private GraphicsDeviceManager graphics;
		private const double timeLimit = 60;
		private MainField field;
		private MainMenu menu;

		public TestGame() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			//graphics.IsFullScreen = true;
			this.IsMouseVisible = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			//...
			base.Initialize();
			//...
			//int w = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, h = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			int w = graphics.PreferredBackBufferWidth, h = graphics.PreferredBackBufferHeight;
			int s = (h < w)? (h): (w);
			s = (4 * s) / 5;
			field = new MainField(this, new Rectangle(w / 2 - s / 2, h / 2 - s / 2, s, s), new Point(8, 8), timeLimit);
			menu = new MainMenu(this, field, w, h);
			//...
			Components.Add(field);
			field.Enabled = field.Visible = false;
			Components.Add(menu);
			//menu.Enabled = menu.Visible = false;
			//...
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			base.LoadContent();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			//...
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
			 || (Keyboard.GetState().IsKeyDown(Keys.Escape)))
				Exit();
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

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			base.Draw(gameTime);
		}

	}
}
