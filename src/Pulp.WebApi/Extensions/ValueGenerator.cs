using Bogus;

namespace Pulp.WebApi.Extensions;

internal class ValueGenerator : IValueGenerator
{
    private static Faker Faker = new Faker();

    public bool Bool() => Faker.Random.Bool();
    public string Date() => Faker.Date.Recent().ToString("yyyy-MM-dd");
    public string DateTime() => Faker.Date.Recent().ToString("yyyy-MM-ddTHH:mm:ssZ");
    public string String(int? minLength, int? maxLength) => Faker.Random.String2(minLength ?? 0, maxLength ?? 50);
    public float Float(decimal? min, decimal? max) => Faker.Random.Float((float?)min ?? 0, (float?)max ?? 50);
    public double Double(decimal? min, decimal? max) => Faker.Random.Double((double?)min ?? 0, (double?)max ?? 50);
    public int Int(decimal? min, decimal? max) => Faker.Random.Int((int?)min ?? 0, (int?)max ?? 50);
    public long Long(decimal? min, decimal? max) => Faker.Random.Long((long?)min ?? 0, (long?)max ?? 50);
}
