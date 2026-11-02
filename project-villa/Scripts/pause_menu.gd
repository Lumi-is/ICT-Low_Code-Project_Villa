extends CanvasLayer

@onready var ui: Control = $UI
@onready var resume_button: Button = $UI/MenuButtons/ResumeButton

func _ready() -> void:
	# Hide the menu UI by default on start
	ui.hide()

func _unhandled_input(event: InputEvent) -> void:
	# Check for the default pause action (ESC key or Start button on gamepad)
	if event.is_action_pressed("ui_cancel"):
		toggle_pause()

func toggle_pause() -> void:
	var new_pause_state = !get_tree().paused
	get_tree().paused = new_pause_state
	
	if new_pause_state:
		ui.show()
		resume_button.grab_focus() # Grab focus for keyboard/controller input
	else:
		ui.hide()

func _on_resume_button_pressed() -> void:
	toggle_pause()

func _on_options_button_pressed() -> void:
	print("Options opened!")

func _on_quit_button_pressed() -> void:
	# Unpause the tree before changing scenes or quitting
	get_tree().paused = false
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")
