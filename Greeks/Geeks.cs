using MathNet.Numerics;

namespace Greeks
{
    public static class Geeks
    {
        const int countOfYearDays = 365;
        public static OptionGreeks CalculateOptionGreeks(double underlyingPrice, double strikePrice, 
            double timeToExpiration, double riskFreeRate, double dividendYield, double volatility)
        {
            ValidateInput(underlyingPrice, strikePrice, timeToExpiration, riskFreeRate, dividendYield, volatility);

            timeToExpiration /= countOfYearDays;
            riskFreeRate = Interest(riskFreeRate);
            dividendYield = Interest(dividendYield);
            volatility = Interest(volatility);

            double d1 = CalculateD1(underlyingPrice, strikePrice, timeToExpiration, riskFreeRate, dividendYield, volatility);
            double d2 = d1 - (volatility)*Math.Sqrt(timeToExpiration); 

            double delta = NormalCDF(d1);
            double gamma = NoramlPDF(d1) / (underlyingPrice * (volatility) * Math.Sqrt(timeToExpiration));
            double vega = underlyingPrice * NoramlPDF(d1) * Math.Sqrt(timeToExpiration) * 0.01;
            double theta = CalculateTheta(underlyingPrice, strikePrice, timeToExpiration, riskFreeRate, dividendYield, volatility, d1, d2);

            return new OptionGreeks { Delta = delta, Gamma = gamma, Theta = theta, Vega =vega }; 
        }

        private static double NoramlPDF(double d1)
        {
            return MathNet.Numerics.Distributions.Normal.PDF(0, 1, d1);
        }

        private static double NormalCDF(double d1)
        {
            return MathNet.Numerics.Distributions.Normal.CDF(0, 1, d1);
        }


        private static double CalculateD1(double underlyingPrice, double strikePrice,
             double timeToExpiration, double riskFreeRate, double dividendYield, double volatility)
        {
            return (Math.Log(underlyingPrice / strikePrice) + ((riskFreeRate) - (dividendYield) + 0.5 * Math.Pow((volatility), 2))
                * timeToExpiration) / ((volatility) * Math.Sqrt(timeToExpiration));
        }

        private static double CalculateTheta(double underlyingPrice, double strikePrice,
             double timeToExpiration, double riskFreeRate, double dividendYield, double volatility, double d1, double d2)
        {
            return -(underlyingPrice * (volatility) * NoramlPDF(d1)) / (2 * Math.Sqrt(timeToExpiration)) -
                riskFreeRate * strikePrice * Math.Exp(-riskFreeRate * timeToExpiration) * NormalCDF(d2);
        }
        private static void ValidateInput(double underlyingPrice, double strikePrice,
             double timeToExpiration, double riskFreeRate, double dividendYield, double volatility)
        {
            if (underlyingPrice <= 0 || strikePrice <= 0 || timeToExpiration <= 0 ||
                riskFreeRate <= 0 || dividendYield <= 0 || volatility <= 0)
            {
                throw new ArgumentException("Invalid input parameters");
            }
        }
        private static double Interest(double d1)
        {

            return d1 / 100;
        }
    }
}
