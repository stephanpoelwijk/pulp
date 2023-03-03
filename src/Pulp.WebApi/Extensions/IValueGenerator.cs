namespace Pulp.WebApi.Extensions;

internal interface IValueGenerator
{
    bool Bool();
    string Date();
    string DateTime();
    string String(int? minLength, int? maxLength);
    float Float(decimal? min, decimal? max);
    double Double(decimal? min, decimal? max);
    int Int(decimal? min, decimal? max);
    long Long(decimal? min, decimal? max);
}
