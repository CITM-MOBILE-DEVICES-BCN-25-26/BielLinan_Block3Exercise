namespace CleanRefactor
{
    public class ShopStatusDto
    {
        public int CurrentCoins { get; set; }
        public int CurrentLevel { get; set; } 
        public bool CanBuyBomb { get; set; }
        public bool CanBuyShield { get; set; }
        public bool CanBuyDoubleCoins { get; set; }
    }
}