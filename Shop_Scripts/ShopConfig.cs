namespace CleanRefactor
{
    public class ShopConfig
    {
        public int BombCost { get; set; } = 100;
        public int BombMaxUses { get; set; } = 3;

        public int ShieldCost { get; set; } = 150;
        public int ShieldMaxUses { get; set; } = 2;

        public int DoubleCoinsCost { get; set; } = 300;
        public int DoubleCoinsMinLevel { get; set; } = 5;
    }
}