#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System;
#endregion

namespace TestGame {

	public class Field: DrawableGameComponent {
		//...
		private class Item {
			//...
			public enum Stat { IS_INIT, IS_FREE, IS_NONE, IS_MOVE, IS_STAT, IS_USED }
			//...
			public Texture2D text; public Point ind, off; public Stat stat;
			//...
			public Item(Point _ind) {
				ind = _ind; off = new Point(0, 0);
				stat = Stat.IS_NONE;
			}
			//...
		}
		//...
		private enum Stat { DO_INIT, DO_FREE, DO_SWAP, DO_BACK, DO_MOVE, DO_WAIT }
		//...
		private const double TIMES_FOR_UPDATE = 0.5;
		private const int STEPS_FOR_UPDATE = 10;
		private const int SCALE_FOR_RESIZE = 1;
		//...
		private SpriteBatch panel; private SpriteFont font;
		private Texture2D[] image; private Item[,] items;
		private Item curr = null, next;
		private Stat stat = Stat.DO_INIT;
		private Random rnd = new Random();
		private Point min, len, num;
		private double timeLimit = 0;
		private double time = 0;
		private int score = 0;
		//...
		public Field(Game gm, Rectangle rec, Point num, double timeLimit): base(gm) {
			//...
			panel = new SpriteBatch(Game.GraphicsDevice);
			image = new Texture2D[5];
			items = new Item[num.X, num.Y];
			//...
			this.timeLimit = timeLimit;
			//...
			min = new Point(rec.X, rec.Y);
			len = new Point(rec.Width, rec.Height);
			len.X /= num.X;
			len.Y /= num.Y;
			this.num = num;
			//...
		}
		//...
		protected override void LoadContent() {
			base.LoadContent();
			/*
			image[0] = Texture2D.FromFile(GraphicsDevice, "Content/BOX_01.PNG");
			image[1] = Texture2D.FromFile(GraphicsDevice, "Content/BOX_02.PNG");
			image[2] = Texture2D.FromFile(GraphicsDevice, "Content/BOX_03.PNG");
			image[3] = Texture2D.FromFile(GraphicsDevice, "Content/BOX_04.PNG");
			image[4] = Texture2D.FromFile(GraphicsDevice, "Content/BOX_05.PNG");
			*/
			image[0] = Game.Content.Load<Texture2D>("BOX_01");
			image[1] = Game.Content.Load<Texture2D>("BOX_02");
			image[2] = Game.Content.Load<Texture2D>("BOX_03");
			image[3] = Game.Content.Load<Texture2D>("BOX_04");
			image[4] = Game.Content.Load<Texture2D>("BOX_05");
			//...
			font = Game.Content.Load<SpriteFont>("FONT");
			//...
		}
		//...
		public override void Initialize() {
			base.Initialize();
			Reset(timeLimit);
		}
		//...
		public void Reset(double timeLimit) {
			for (int ix = 0; ix < num.X; ++ ix) for (int iy = 0; iy < num.Y; ++ iy) {
				items[ix, iy] = new Item(new Point(ix, iy));
				items[ix, iy].stat = Item.Stat.IS_NONE;
			}
			Gener(); stat = Stat.DO_INIT;
			this.timeLimit = timeLimit;
			time = 0;
			curr = null;
			score = 0;
		}
		//...
		public bool isStop() {
			return (timeLimit <= 0);
		}
		//...
		//Generator:
		private void Gener() {
			//...
			Texture2D[] img = image.Clone() as Texture2D[];
			//...
			for (int ix = 0; ix < num.X; ++ ix) for (int iy = 0; iy < num.Y; ++ iy) {
				if (items[ix, iy].stat != Item.Stat.IS_NONE) continue;
				bool[] tmp = {true, true, true, true, true};
				//...
				for (int i = 0; i < img.Length; ++ i) {
					int k = rnd.Next() % img.Length;
					var t = img[k]; img[k] = img[i];
					img[i] = t;
				}
				//...
				Check(img, tmp, ix - 1, iy);
				Check(img, tmp, ix, iy - 1);
				Check(img, tmp, ix + 1, iy);
				Check(img, tmp, ix, iy + 1);
				//...
				for (int i = 0; i < tmp.Length; ++ i) {
					if (!tmp[i]) continue;
					items[ix, iy].text = img[i];
					break;
					//...
				}
				items[ix, iy].stat = Item.Stat.IS_INIT;
			}
			//...
		}
		private void Check(Texture2D[] img, bool[] chk, int ix, int iy) {
			if ((ix < 0) || (iy < 0) || (ix >= num.X) || (iy >= num.Y)) return;
			var it = items[ix, iy];
			for (int i = 0; i < chk.Length; ++ i) {
				if (it.text == img[i]) {
					chk[i] = false;
				}
			}
		}
		//Checker:
		private bool Check() {
			bool evn = true;
			for (int ix = 0; ix < num.X; ++ ix) {
				int iy1 = 0, iy2 = 0;
				for (int iy = 1; iy < num.Y; ++ iy) {
					var it = items[ix, iy];
					if ((it.text != items[ix, iy1].text) || (it.stat == Item.Stat.IS_NONE)) {
						if (iy2 - iy1 > 1) {
							while (iy1 <= iy2) {
								items[ix, iy1 ++].stat = Item.Stat.IS_FREE;
							}
							evn = false;
						}
						if (it.stat == Item.Stat.IS_NONE) iy1 = iy + 1;
						else iy1 = iy;
					}
					else {
						iy2 = iy;
					}
				}
				if (iy2 - iy1 > 1) {
					while (iy1 <= iy2) {
						items[ix, iy1 ++].stat = Item.Stat.IS_FREE;
					}
					evn = false;
				}
			}
			for (int iy = 0; iy < num.Y; ++ iy) {
				int ix1 = 0, ix2 = 0;
				for (int ix = 1; ix < num.X; ++ ix) {
					var it = items[ix, iy];
					if ((it.text != items[ix1, iy].text) || (it.stat == Item.Stat.IS_NONE)) {
						if (ix2 - ix1 > 1) {
							while (ix1 <= ix2) {
								items[ix1 ++, iy].stat = Item.Stat.IS_FREE;
							}
							evn = false;
						}
						if (it.stat == Item.Stat.IS_NONE) ix1 = ix + 1;
						else ix1 = ix;
					}
					else {
						ix2 = ix;
					}
				}
				if (ix2 - ix1 > 1) {
					while (ix1 <= ix2) {
						items[ix1 ++, iy].stat = Item.Stat.IS_FREE;
					}
					evn = false;
				}
			}
			return evn;
		}
		//...
		public override void Draw(GameTime gameTime) {
			//...
			base.GraphicsDevice.Clear(Color.AliceBlue);
			panel.Begin();
			//...
			panel.DrawString(font, "Timing: " + (int) timeLimit + " sec.; " + "Score: " + score, new Vector2(10, 10), Color.Black);
			//...
			int s = Convert.ToInt32(STEPS_FOR_UPDATE * (time / TIMES_FOR_UPDATE));
			//...
			for (int ix = 0; ix < num.X; ++ ix) for (int iy = 0; iy < num.Y; ++ iy) {
				var it = items[ix, iy];
				if (it.stat == Item.Stat.IS_NONE) continue;
				Rectangle r = new Rectangle(min.X + it.ind.X * len.X, min.Y + it.ind.Y * len.Y, len.X, len.Y);
				if (it.stat == Item.Stat.IS_MOVE) {
					r = new Rectangle(r.X + (it.off.X * len.X * s) / STEPS_FOR_UPDATE,
					                  r.Y + (it.off.Y * len.Y * s) / STEPS_FOR_UPDATE, r.Width, r.Height);
				}
				if (it.stat == Item.Stat.IS_INIT) {
					int sx = (len.X * Math.Max(0, (STEPS_FOR_UPDATE - s))) / STEPS_FOR_UPDATE;
					int sy = (len.Y * Math.Max(0, (STEPS_FOR_UPDATE - s))) / STEPS_FOR_UPDATE;
					r = new Rectangle(r.X + sx / 2, r.Y + sy / 2, r.Width - sx, r.Height - sy);
				}
				if (it.stat == Item.Stat.IS_FREE) {
					int sx = (len.X * Math.Min(STEPS_FOR_UPDATE, s)) / STEPS_FOR_UPDATE;
					int sy = (len.Y * Math.Min(STEPS_FOR_UPDATE, s)) / STEPS_FOR_UPDATE;
					r = new Rectangle(r.X + sx / 2, r.Y + sy / 2, r.Width - sx, r.Height - sy);
				}
				panel.Draw(it.text, r, Color.White);
			}
			//Анимация выбора:
			if ((stat == Stat.DO_WAIT) && (curr != null)) {
				var r = new Rectangle(min.X + curr.ind.X * len.X, min.Y + curr.ind.Y * len.Y, len.X, len.Y);
				if (s >= STEPS_FOR_UPDATE / 2) s = STEPS_FOR_UPDATE - s;
				int sx = (len.X * s) / STEPS_FOR_UPDATE;
				int sy = (len.Y * s) / STEPS_FOR_UPDATE;
				r = new Rectangle(r.X - sx / 2, r.Y - sy / 2, r.Width + sx, r.Height + sy);
				panel.Draw(curr.text, r, Color.White);
			}
			//...
			panel.End();
			base.Draw(gameTime);
		}
		//...
		private void SwapText(Item lhs, Item rhs) {
			Texture2D tmp = lhs.text; lhs.text = rhs.text; rhs.text = tmp;
			//...
		}
		//...
		public override void Update(GameTime gameTime) {
			//...
			double sec = gameTime.ElapsedGameTime.TotalSeconds;
			timeLimit = Math.Max(0, timeLimit - sec);
			time += sec;
			//...
			var mouse = Mouse.GetState();
			if (mouse.LeftButton == ButtonState.Pressed) {
				Click(new Point(mouse.X, mouse.Y));
			}
			//...
			if (time < TIMES_FOR_UPDATE) return;
			//...
			switch (stat) {
			//Ожидаем появления новых элементов:
			case Stat.DO_INIT:
				for (int ix = 0; ix < num.X; ++ ix) for (int iy = 0; iy < num.Y; ++ iy) {
					items[ix, iy].stat = Item.Stat.IS_STAT;
				}
				if (!Check()) {
					stat = Stat.DO_FREE;
					break;
				}
				stat = Stat.DO_WAIT;
				break;
			//Уничтожаем ненужные элементы:
			case Stat.DO_FREE:
				bool evn = false;
				for (int ix = 0; ix < num.X; ++ ix) {
					int iy1 = - 1;
					for (int iy = num.Y - 1; iy >= 0; -- iy) {
						var it = items[ix, iy];
						if (it.stat == Item.Stat.IS_FREE) {
							it.stat = Item.Stat.IS_NONE;
							++ score;
							if (iy1 < 0) iy1 = iy;
							continue;
						}
						if (it.stat == Item.Stat.IS_NONE) {
							continue;
						}
						if (iy1 >= 0) {
							it.stat = Item.Stat.IS_MOVE;
							it.off.X = 0;
							it.off.Y = iy1 - iy;
							evn = true;
							-- iy1;
						}
					}
				}
				if (!evn) {
					Gener(); stat = Stat.DO_INIT;
					break;
				}
				stat = Stat.DO_MOVE;
				break;
			//Пытаемся выполнить перестановку:
			case Stat.DO_SWAP:
				SwapText(curr, next);
				if (!Check()) {
					if (curr.stat != Item.Stat.IS_FREE) curr.stat = Item.Stat.IS_USED;
					if (next.stat != Item.Stat.IS_FREE) next.stat = Item.Stat.IS_STAT;
					stat = Stat.DO_FREE;
					curr = null;
					break;
				}
				stat = Stat.DO_BACK;
				break;
			//Откат перестановки:
			case Stat.DO_BACK:
				SwapText(curr, next);
				curr.stat = next.stat = Item.Stat.IS_STAT;
				stat = Stat.DO_WAIT;
				break;
			//Перемещение элементов:
			case Stat.DO_MOVE:
				for (int ix = 0; ix < num.X; ++ ix) for (int iy = num.Y - 1; iy >= 0; -- iy) {
					var it = items[ix, iy];
					if (it.stat != Item.Stat.IS_MOVE) continue;
					var t0 = items[ix + it.off.X, iy + it.off.Y];
					t0.stat = Item.Stat.IS_STAT;
					it.stat = Item.Stat.IS_NONE;
					t0.text = it.text;
					it.text = null;
				}
				if (!Check()) {
					stat = Stat.DO_FREE;
					break;
				}
				Gener();
				stat = Stat.DO_INIT;
				break;
			//Ожидание действия:
			case Stat.DO_WAIT:
				//...
				break;
			}
			time = 0;
			//...
			base.Update(gameTime);
		}
		//...
		public void Click(Point pos) {

			pos.X = (pos.X - min.X) / len.X;
			pos.Y = (pos.Y - min.Y) / len.Y;

			if ((pos.X < 0) || (pos.Y < 0) || (pos.X >= num.X) || (pos.Y >= num.Y)) return;

			if (stat != Stat.DO_WAIT) return;
			//...
			if (curr != null) {
				if (((pos.X == curr.ind.X) && (Math.Abs(pos.Y - curr.ind.Y) == 1))
				 || ((pos.Y == curr.ind.Y) && (Math.Abs(pos.X - curr.ind.X) == 1))) {
					//Пытаемся поменять элементы местами:
					curr.off = new Point(pos.X - curr.ind.X, pos.Y - curr.ind.Y);
					next = items[pos.X, pos.Y];
					next.off = new Point(curr.ind.X - pos.X, curr.ind.Y - pos.Y);
					curr.stat = Item.Stat.IS_MOVE;
					next.stat = Item.Stat.IS_MOVE;
					stat = Stat.DO_SWAP;
					time = 0;
					return;
				}
				curr.stat = Item.Stat.IS_STAT;
			}
			curr = items[pos.X, pos.Y];
			curr.stat = Item.Stat.IS_USED;
			time = 0;
			//...
		}
		//...
	}

}
