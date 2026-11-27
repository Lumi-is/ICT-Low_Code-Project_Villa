using Godot;

[GlobalClass]
public partial class UiSound : AudioStreamPlayer
{
	public override void _Ready()
	{
		Bus = "Sound effects";
	}

	public void PlaySound()
	{
		Play();
	}
}