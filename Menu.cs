using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TestGame {

	public class MainMenu: DrawableGameComponent {
		//...
		private Texture2D[] run_image, end_image;
		private SpriteBatch panel;
		private Rectangle[] rect;
		private MainField field;
		private bool focus = false;
		private bool start = true;
		//...
		public MainMenu(Game gm, MainField fld, int W, int H): base(gm) {
			field = fld;
			panel = new SpriteBatch(this.Game.GraphicsDevice);
			run_image = new Texture2D[2];
			end_image = new Texture2D[3];
			rect = new Rectangle[3];
			//TODO: Scaling position of buttons
			//: ...
			int s = ((W < H)? (W): (H)) / 400;
			rect[0] = new Rectangle(W / 2 - 150 * s, H / 2 - 50 * s, 300 * s, 100 * s);
			rect[1] = new Rectangle(W / 2 - 150 * s, (W / 3 + (H - W / 3) / 2) - 50 * s, 300 * s, 100 * s);
			rect[2] = new Rectangle(0, 0, W, W / 3);
			//...
			LoadContent();
		}
		//...
		protected override void LoadContent() {
			base.LoadContent();
			//...
			run_image[0] = Game.Content.Load<Texture2D>("RUN_01");
			run_image[1] = Game.Content.Load<Texture2D>("RUN_02");
			//...
			end_image[0] = Game.Content.Load<Texture2D>("END_01");
			end_image[1] = Game.Content.Load<Texture2D>("END_02");
			end_image[2] = Game.Content.Load<Texture2D>("END_00");
			//...
		}
		//...
		public override void Initialize() {
			base.Initialize();
		}
		//...
		public void setStart(bool _start) {
			start = _start;
		}
		//...
		public override void Draw(GameTime GameTime) {
			panel.Begin();
			base.GraphicsDevice.Clear(Color.AliceBlue);
			int t = (focus)? (1): (0);
			if (start) {
				panel.Draw(run_image[t], rect[0], Color.AliceBlue);
			}
			else {
				panel.Draw(end_image[t], rect[1], Color.AliceBlue);
				panel.Draw(end_image[2], rect[2], Color.AliceBlue);
			}
			panel.End();
			//...
		}
		public override void Update(GameTime gameTime) {
			var mouse = Mouse.GetState();
			if (start) {
				focus = rect[0].Contains(new Point(mouse.X, mouse.Y));
			}
			else {
				focus = rect[1].Contains(new Point(mouse.X, mouse.Y));
			}
			if (!focus) return;
			if (mouse.LeftButton == ButtonState.Pressed) {
				if (start) {
					field.Enabled = field.Visible = true;
					Enabled = Visible = false;
					return;
				}
				start = true;
				//...
			}
		}
		//...
	}
}
