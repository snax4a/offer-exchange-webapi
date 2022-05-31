namespace FSH.WebApi.Domain.Exchange;

public class CountrySubdivision
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string CountryAlpha2Code { get; private set; } = default!;

    private CountrySubdivision()
    {
        // Required by ORM
    }

    public CountrySubdivision(string code, string name, string countryAlpha2Code)
    {
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (countryAlpha2Code.Length != 2) throw new ArgumentException(nameof(countryAlpha2Code));

        Code = code;
        Name = name;
        CountryAlpha2Code = countryAlpha2Code;
    }
}