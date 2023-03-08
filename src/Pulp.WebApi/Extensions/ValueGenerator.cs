using Bogus;
using Pulp.WebApi.Extensions.Generic.Models;

namespace Pulp.WebApi.Extensions;

internal class ValueGenerator : IValueGenerator
{
    private static Faker Faker = new Faker();

    public bool Bool() => Faker.Random.Bool();
    public string Date() => Faker.Date.Recent().ToString("yyyy-MM-dd");
    public string DateTime() => Faker.Date.Recent().ToString("yyyy-MM-ddTHH:mm:ssZ");
    public string String(int? minLength, int? maxLength) => Faker.Random.String2(minLength ?? 0, maxLength ?? 50);
    public string String(ContentGenerationHints? hints) => String(hints?.MinLength, hints?.MaxLength);
    public float Float(decimal? min, decimal? max) => Faker.Random.Float((float?)min ?? 0, (float?)max ?? 50);
    public float Float(ContentGenerationHints? hints) => Float(hints?.Min, hints?.Max);
    public double Double(decimal? min, decimal? max) => Faker.Random.Double((double?)min ?? 0, (double?)max ?? 50);
    public double Double(ContentGenerationHints? hints) => Double(hints?.Min, hints?.Max);
    public int Int(decimal? min, decimal? max) => Faker.Random.Int((int?)min ?? 0, (int?)max ?? 50);
    public int Int(ContentGenerationHints? hints) => Int(hints?.Min, hints?.Max);
    public long Long(decimal? min, decimal? max) => Faker.Random.Long((long?)min ?? 0, (long?)max ?? 50);
    public long Long(ContentGenerationHints? hints) => Long(hints?.Min, hints?.Max);
}
