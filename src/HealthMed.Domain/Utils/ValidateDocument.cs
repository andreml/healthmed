namespace HealthMed.Domain.Utils
{
    public static class ValidateDocument
    {
        public static bool IsCpf(string document)
        {
            try
            {
                int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                string tempdocument;
                string digito;

                int soma = 0;
                int resto;

                document = document.Trim();
                document = document.Replace(".", "").Replace("-", "");

                if (document.Length != 11)
                    return false;

                tempdocument = document[..9];

                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempdocument[i].ToString()) * multiplicador1[i];

                resto = soma % 11;

                resto = (resto < 2) ? 0 : 11 - resto;

                digito = resto.ToString();

                tempdocument += digito;

                soma = 0;

                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempdocument[i].ToString()) * multiplicador2[i];

                resto = soma % 11;

                resto = (resto < 2) ? 0 : 11 - resto;

                digito += resto.ToString();

                return document.EndsWith(digito);
            }
            catch
            {
                return false;
            }

        }
    }
}
