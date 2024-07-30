using HealthMed.Domain.Utils;
using Xunit;

namespace HealthMed.UnitTests.Domain;

public class ValidateDocumentTests
{
    [Theory]
    [MemberData(nameof(ObterCpfs))]
    public void ValidateDocument_ValidarCpf(string cpf, bool valido)
    {
        // Act
        var cpfValido = ValidateDocument.IsCpf(cpf);

        //Assert
        Assert.Equal(valido, cpfValido);
    }

    public static IEnumerable<object[]> ObterCpfs()
    {
        yield return new object[] { "94235560271", true };
        yield return new object[] { "68325434848", true };
        yield return new object[] { "101.167.660-50", true };
        yield return new object[] { "67.908.294/0001-25", false };
        yield return new object[] { "53171521000178", false };
        yield return new object[] { "7685AA57674", false };
        yield return new object[] { "12345", false };
        yield return new object[] { "cpfinvalido", false };
    }
}
