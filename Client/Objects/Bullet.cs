using Client.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Timer = System.Windows.Forms.Timer;

namespace Client
{
	
	public class Bullet : CollisionTemplate
	{
		public string BulletDirection;

		private int Speed = 20;
		private PictureBox BulletBox = new PictureBox();
		private Timer BulletTimer = new Timer();
		private Form _form;

		public void MakeBullet(Form form, PlayerAdapter player)
		{
			_form = form;
			BulletBox.BackColor = Color.White;
			BulletBox.Size = new Size(5,5);
			BulletBox.Tag = "bullet";
			BulletBox.Left = player.X + 50;
			BulletBox.Top = player.Y + 50;
			BulletBox.BringToFront();
			BulletDirection = player.direction;

			form.Controls.Add(BulletBox);

			Point mousePoint = Control.MousePosition;
			int mouseX = mousePoint.X;
			int mouseY = mousePoint.Y;



			BulletTimer.Interval = Speed;
			BulletTimer.Tick += new EventHandler(BulletTimerEvent);
			BulletTimer.Start();
		}

		private void BulletTimerEvent(object sender, EventArgs e)
		{
			if (BulletDirection == "left")
			{
				BulletBox.Left -= Speed;
			}

			if (BulletDirection == "right")
			{
				BulletBox.Left += Speed;
			}

			if (BulletDirection == "up")
			{
				BulletBox.Top -= Speed;
			}

			if (BulletDirection =="down")
			{
				BulletBox.Top += Speed;
			}

			ProcessCollisions(_form, null);
		}

		private void BulletDispose()
		{
			BulletTimer.Stop();
			BulletTimer.Dispose();
			BulletBox.Dispose();
			BulletTimer = null;
			BulletBox = null;
		}

		protected override bool DetectCollision(Form form, out Control? collider)
		{
			foreach (var picture in form.Controls.OfType<PictureBox>())
			{
				if (picture.Tag is int numericTag && numericTag != Globals.PersonalID && picture.Bounds.IntersectsWith(BulletBox.Bounds))
				{
					collider = picture;
					return true;
				}
			}
			collider = null;

			int[,] colliderMap = Globals.ColliderMap;

			try
			{
				if (colliderMap[BulletBox.Left, BulletBox.Top] == 2)
					return true;
			}
			catch (IndexOutOfRangeException)
			{
				return true;
			}

			return false;
		}

		protected override void InteractWithCollider(Control collider)
		{
			collider.BackColor = Color.Red;
		}

		protected override void ResponseToCollision(object param)
		{
			BulletDispose();
		}
	}


}
