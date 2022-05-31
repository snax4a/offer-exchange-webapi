namespace FSH.WebApi.Domain.Exchange;

public class CountrySubdivision
{
    public string CountryAlpha2Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    private CountrySubdivision()
    {
        // Required by ORM
    }

    public CountrySubdivision(string countryAlpha2Code, string name, string code)
    {
        if (countryAlpha2Code.Length != 2) throw new ArgumentException(nameof(countryAlpha2Code));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

        CountryAlpha2Code = countryAlpha2Code;
        Name = name;
        Code = code;
    }
}