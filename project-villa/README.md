# Project Villa

A small card prototype made with Godot 4.7.2 (Mono) and .NET 8.

The project contains the card framework, the HUD, and a sample card
(`Resources/Cards/Testing.tres`) in a deck (`Resources/Cards/StartingDeck.tres`).
The hand deals four copies of it when the game starts.

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
| `Scripts/` | HUD, menus, camera |
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
