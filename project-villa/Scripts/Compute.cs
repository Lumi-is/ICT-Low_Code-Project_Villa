namespace System.Calculations
{
	public static class Compute
	{
		public static float Distance(Godot.Vector2 first, Godot.Vector2 second)
		{
			Godot.Vector2 third = second - first;
			return (float)Math.Sqrt(Math.Pow(third.X,2) + Math.Pow(third.Y,2));
		}
	}
}

