using Godot;

[GlobalClass]
public partial class UiSound : AudioStreamPlayer
{
	public void PlaySound()
	{
		Play();
	}
}