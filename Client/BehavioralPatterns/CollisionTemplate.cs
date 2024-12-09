namespace Client
{
	public abstract class CollisionTemplate
	{
		public bool ProcessCollisions(Form form, Object? param)
		{
			Control? collider;
			bool collided = DetectCollision(form, out collider);
			if (!collided)
			{
				return false;
			}
			if (collider is not null)
			{
				InteractWithCollider(collider);
			}
			if (param is not null)
			{
				ResponseToCollision(param);
			}

			return collided;
		}

		protected abstract bool DetectCollision(Form form, out Control? collider);
		protected abstract void InteractWithCollider(Control collider);
		protected abstract void ResponseToCollision(Object param);
	}
}
