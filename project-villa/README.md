# Project Villa

A small card game prototype made with Godot 4.7.2 (Mono) and .NET 8.

The project contains the card framework, the HUD, and a sample card
(`Resources/Cards/Testing.tres`) in a deck (`Resources/Cards/StartingDeck.tres`).

## Requirements

- Godot 4.7.2 .NET (Mono build)
- .NET 8 SDK

## Running

1. Open `project.godot` in Godot.
2. Press **F5**.
3. Click **Play** on the main menu.

The HUD sits at the bottom of the screen with the resource counter, deck counter,
pause and shop buttons. **Shop** opens a menu with three cards; **Buy** adds the
card to your deck, and **Refresh** replaces the selection for resources.

## Project layout

| Folder | Contents |
| --- | --- |
| `CardSystem/` | Card data, hand, drag input, play controller |
| `Scenes/` | Main menu, game scene, HUD, card widget |
| `Scripts/` | HUD, menus, camera, Interactor, etc. |
| `Resources/` | Card and deck assets |
| `Fonts/` | Font files |

## Card system

| File | Purpose |
| --- | --- |
| `CardSystem/Card.cs` | Base card data: name, description, cost, colour, `CanPlay`, `Play` |
| `CardSystem/CardDeck.cs` | A list of cards used by the hand |
| `CardSystem/GainResourceCard.cs` | Adds resources to the HUD |
| `CardSystem/DrawCardsCard.cs` | Draws cards from the deck |
| `CardSystem/CardView.cs` | One card widget and its mouse drag |
| `CardSystem/ShopCardView.cs` | One card in the shop, with its buy button |
| `CardSystem/CardHand.cs` | Hand layout, draw pile, permanent card use |
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
4. Fill in the card fields in the inspector. `ShopPrice` is the resource cost to
   buy the card in the shop.
5. Add the card to `Resources/Cards/StartingDeck.tres` by dragging it into the
   deck's `Cards` array, and to `Scenes/shop_menu.tscn` (`AvailableCardPool`) if
   the shop should sell it.
6. If you make a separate deck instead, open `Scenes/hud.tscn`, select the
   `CardHand` node, and assign your deck to `StartingDeck`.
7. Run the game and drag a card out of the hand.

Cards are paid for automatically before `Play` runs. If `Play` throws, the cost
is refunded.

## Playing a card

Drag a card out of the hand and release the mouse button. The cost is taken from
the HUD's resource counter, the card is permanently consumed, and the hand draws
a replacement from the deck if one is available. The deck counter shows how many
cards are left to draw.

## Shop

`Scenes/shop_menu.tscn` holds the shop settings:

| Field | Purpose |
| --- | --- |
| `CardDisplayPrefab` | Scene used for one shop card (`Scenes/Cards/ShopCardView.tscn`) |
| `AvailableCardPool` | Cards the shop can offer |
| `ShopSize` | How many cards are shown at once |
| `RefreshCost` | Resources spent to reroll the shop |

Buying a card spends its `ShopPrice`, removes it from the shop, and adds it to
your hand if there is room. If your hand is full, the card goes to the draw pile
instead. The deck counter updates immediately.

## Interactor

Handles how objects are interacted with in the game. Searches for objects
which implement IClickable or IDraggable interfaces by doing a physics point
calculation for seeing which Area2D or body type objects the mouse can interact 
with at any given time. 

## Adding new interactables

Look at "ExampleIntaractable" for an idea on how interactor script can be used. 
Explanations below:

To add a new interactable, implement IClickable or IDraggable interfaces to 
script of an object according to need. Note that either an Area2D node or a
body is required for interactor to detect objects. 

- `IClickable` : When implemented, interactor script detects when mouse is being 
hovered over the object, and whether or not the object is clicked. IClickable 
must implement the following methods:
	
	- Sprite2D GetGraphics() = Returns a sprite which the effects of interactor 
	are displayed upon
	
	- void Highlight() = What the object should do when mouse hovers over it
	
	- void OnClickEvent() = What the object should do when player clicks on it
	
	- void Reset() = Reset states of OnClickEvent() and Highlight() to their 
	original ones when not being clicked or hovered over
	
- `IDraggable` : When implemented, interactor script handles dragging and dropping
the object around the scene. IDraggable must implement the following methods:
	
	- Sprite2D GetGraphics() = Returns a sprite which the effects of interactor 
	are displayed upon
	
	- void Drag(Vector2 mousePos) = Handles how object should behave when 
	dragged. Use Interactor.SnapToMouse(this, mousePos) for default 
	functionality. 
	
	- void Release() = Handles how objects should behave when no longer dragged
	by player. 

## WorldGridManager

Namespace: `Grid`

A Static class which handles calculating the nearest cell of the world grid, 
snapping objects onto said grid and instantiating objects such as buildings
to the scene. 

Has three methods:
	
	- Vector2 CalculateSnapPos(Vector2 position) : Returns centre position of 
	the nearest cell of world grid based on a given position.
	
	- void MoveOwner(Node2D someSnappable, Vector2 snapPos) : Moves either
	the owner of the given object or the object itself (considers self to be 
	owner if owner is null) to a given position. Use with CalculateSnapPos to 
	snap object onto the grid 
	
	// Note that the functionality of the aforementioned two methods could be 
	set to one method, but for the sake of flexibility it is seperated into two.
	Nearest cell position can be useful information outside of snapping objects 
	onto the grid.
	
	
	- void InstantiateObject(Vector2 pos, Node n, string path) : Instantiates an
	object (scene) from path "path" to a specified parent node created under the 
	node "n". Object will be instantiated onto position "pos".
	
	
	
	
	
	
