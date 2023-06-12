using VtlSoftware.Aspects.Polly21;

namespace VtlSoftware.MultiAspects.ConsoleApp
{
    public class Calculator
    {
        #region Fields

        private static int attempts;

        #endregion

        #region Constructors

        public Calculator()
        {
        }

        #endregion

        #region Public Methods
        [Retry]
        public double Add(double a, double b)
        {
            Thread.Sleep(10);
            attempts++;

            if(attempts <= 3)
            {
                throw new InvalidOperationException();
            }

            return a * b;
        }

        public double Multiply(double a, double b) { return a * b; }

        public double Subtract(double a, double b) { return a - b; }

        #endregion
    }
}
