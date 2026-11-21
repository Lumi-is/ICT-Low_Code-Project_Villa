using ProjectVilla.Cards;

public static partial class ResourceManager
{
	public static int wood;
	public static int stone;
	public static int magic;
	public static int food;

	public static bool ResourceCalculate(Card card)
	{
		if (wood >= card.woodCost && stone >= card.stoneCost && magic >= card.magicCost && food >= card.foodCost)
		{
			wood -= card.woodCost;
			stone -= card.stoneCost;
			magic -= card.magicCost;
			food -= card.foodCost;
			return true;
		}

		else
		{
			return false;	
		}
	}
}