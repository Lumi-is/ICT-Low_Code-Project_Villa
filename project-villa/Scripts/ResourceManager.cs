public static partial class Resources
{
	static int wood;
	static int stone;
	static int magic;

	public static bool ResourceCalculate(CardData card)
	{
		if (wood >= card.woodCost && stone >= card.stoneCost && magic >= card.magicCost)
		{
			wood -= card.woodCost;
			stone -= card.stoneCost;
			magic -= card.magicCost;
			return true;
		}

		else
		{
			return false;	
		}
	}
}