namespace Greeks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var greeks = Geeks.CalculateOptionGreeks(100, 105, 30, 1.5, 2.0, 20);
                Console.WriteLine($"Delta: {greeks.Delta}, Gamma: {greeks.Gamma}, Vega: {greeks.Vega}, Theta: {greeks.Theta}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
