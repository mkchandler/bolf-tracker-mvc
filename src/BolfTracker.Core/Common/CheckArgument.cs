using System;

namespace BolfTracker
{
    public static class Check
    {
        public static class Argument
        {
            public static void IsNotNull(object parameter, string parameterName)
            {
                if (parameter == null)
                {
                    throw new ArgumentNullException(parameterName);
                }
            }

            public static void IsNotNullOrEmpty(string parameter, string parameterName, string message)
            {
                if (string.IsNullOrWhiteSpace(parameter))
                {
                    throw new ArgumentException(message, parameterName);
                }
            }

            public static void IsNotZeroOrNegative(int parameter, string parameterName)
            {
                if (parameter <= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }

            public static void IsNotNegative(int parameter, string parameterName)
            {
                if (parameter < 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }

            public static void IsNotZeroOrNegative(long parameter, string parameterName)
            {
                if (parameter <= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }

            public static void IsNotNegative(long parameter, string parameterName)
            {
                if (parameter < 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }

            public static void IsNotZeroOrNegative(float parameter, string parameterName)
            {
                if (parameter <= 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }

            public static void IsNotNegative(float parameter, string parameterName)
            {
                if (parameter < 0)
                {
                    throw new ArgumentOutOfRangeException(parameterName);
                }
            }
        }
    }
}
