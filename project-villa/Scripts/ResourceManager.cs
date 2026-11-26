using ProjectVilla.Cards;

public static partial class ResourceManager
{
	public enum ResourceType
	{
		Wood,
		Stone,
		Magic,
		Food
	}

	private const int StartingAmount = 100;
	private static readonly int[] _amounts = new int[4];

	static ResourceManager()
	{
		Reset();
	}

	public static int Wood => _amounts[(int)ResourceType.Wood];

	public static int Stone => _amounts[(int)ResourceType.Stone];

	public static int Magic => _amounts[(int)ResourceType.Magic];

	public static int Food => _amounts[(int)ResourceType.Food];

	public static void Reset()
	{
		for (int i = 0; i < _amounts.Length; i++)
		{
			_amounts[i] = StartingAmount;
		}
	}

	public static int Get(ResourceType type)
	{
		return _amounts[(int)type];
	}

	public static void Set(ResourceType type, int amount)
	{
		if (amount >= 0)
		{
			_amounts[(int)type] = amount;
		}
	}

	public static void Add(ResourceType type, int amount)
	{
		Set(type, Get(type) + amount);
	}

	public static bool CanAfford(ResourceType type, int amount)
	{
		return amount <= 0 || Get(type) >= amount;
	}

	public static bool TrySpend(ResourceType type, int amount)
	{
		if (!CanAfford(type, amount))
		{
			return false;
		}

		Set(type, Get(type) - amount);
		return true;
	}

	public static bool CanPlayCard(Card card)
	{
		return card != null
			&& CanAfford(ResourceType.Wood, card.WoodCost)
			&& CanAfford(ResourceType.Stone, card.StoneCost)
			&& CanAfford(ResourceType.Magic, card.MagicCost)
			&& CanAfford(ResourceType.Food, card.FoodCost);
	}

	public static bool SpendCardCost(Card card)
	{
		if (!CanPlayCard(card))
		{
			return false;
		}

		TrySpend(ResourceType.Wood, card.WoodCost);
		TrySpend(ResourceType.Stone, card.StoneCost);
		TrySpend(ResourceType.Magic, card.MagicCost);
		TrySpend(ResourceType.Food, card.FoodCost);
		return true;
	}
}
