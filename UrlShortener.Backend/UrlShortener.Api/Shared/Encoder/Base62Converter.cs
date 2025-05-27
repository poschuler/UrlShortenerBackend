using System.Text;

namespace UrlShortener.Api.Shared.Encoder
{
    public class Base62Converter : IBase62Converter
    {
        private const string Base62Characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly int Base = Base62Characters.Length;

        public string Encode(long number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "Number must be non-negative.");
            }

            if (number == 0)
            {
                return Base62Characters[0].ToString();
            }

            var result = new StringBuilder();

            while (number > 0)
            {
                var remainder = (number % Base);
                result.Insert(0, Base62Characters[(int)remainder]);
                number /= Base;
            }

            return result.ToString();
        }
    }
}
