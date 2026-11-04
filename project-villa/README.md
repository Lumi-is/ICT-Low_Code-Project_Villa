# Project Villa

A small card prototype made with Godot 4.7.2 (Mono) and .NET 8.

The project currently has the card framework and the HUD, but no card assets —
the hand starts empty on purpose.

## Requirements

- Godot 4.7.2 .NET (Mono build)
- .NET 8 SDK

## Running

1. Open `project.godot` in Godot.
2. Press **F5**.
3. Click **Play** on the main menu.

The HUD sits at the bottom of the screen with the resource counter, deck counter,
pause and shop buttons.

## Project layout

| Folder | Contents |
| --- | --- |
| `CardSystem/` | Card data, hand, drag input, play controller |
| `Scenes/` | Main menu, game scene, HUD, card widget |
| `Scripts/` | HUD, menus, camera |
| `Fonts/` | Font files |

## Card system

| File | Purpose |
| --- | --- |
| `CardSystem/Card.cs` | Base card data: name, description, cost, colour, `CanPlay`, `Play` |
| `CardSystem/CardDeck.cs` | A list of cards used by the hand |
| `CardSystem/GainResourceCard.cs` | Adds resources to the HUD |
| `CardSystem/DrawCardsCard.cs` | Draws cards from the deck |
| `CardSystem/CardView.cs` | One card widget and its mouse drag |
| `CardSystem/CardHand.cs` | Hand layout, draw pile, discard pile |
| `CardSystem/CardPlayController.cs` | Checks the cost and runs the card |

The HUD scene (`Scenes/hud.tscn`) contains the controller at
`CardSystem/CardPlayController` and the hand at
`BottomPanel/HBoxContainer/CardHandSection/CardHand`.

## Adding a card

1. Create a card class in `CardSystem/`:

```csharp
using Godot;

namespace ProjectVilla.Cards;

[GlobalClass]
public partial class HealCard : Card
{
	[Export] public int Amount = 1;

	public override void Play(CardPlayController game)
	{
		game.Hud.AddResources(Amount);
	}
}
```

2. Build the project:

```text
dotnet build
```

3. In Godot, create a folder for cards (for example `Resources/Cards`), right-click
   it in the FileSystem panel, choose **Create New > Resource**, pick `HealCard`,
   and save it.
4. Fill in the card fields in the inspector.
5. Create a **CardDeck** resource, add the card to its `Cards` array, and save it.
6. Open `Scenes/hud.tscn`, select the `CardHand` node, and assign the deck to
   `StartingDeck`.
7. Run the game and drag a card out of the hand.

Cards are paid for automatically before `Play` runs. If `Play` throws, the cost
is refunded.

## Playing a card

Drag a card out of the hand and release the mouse button. The cost is taken from
the HUD's resource counter, the card moves to the discard pile, and the hand
refills to `StartingHandSize`.
