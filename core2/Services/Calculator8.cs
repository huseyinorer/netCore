namespace core2.Services
{
    public class Calculator8 : ICalculate
    {
        public decimal Calculate(decimal amount)
        {
            return amount * 8 / 100;
        }
    }
}
