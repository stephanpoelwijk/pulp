using Pulp.WebApi.Extensions.Generic;

namespace Pulp.WebApi.Extensions;

internal interface IValueGenerator
{
    bool Bool();
    string Date();
    string DateTime();
    string String(ContentGenerationHints? hints);
    string String(int? minLength, int? maxLength);
    float Float(ContentGenerationHints? hints);
    float Float(decimal? min, decimal? max);
    double Double(ContentGenerationHints? hints);
    double Double(decimal? min, decimal? max);
    int Int(ContentGenerationHints? hints);
    int Int(decimal? min, decimal? max);
    long Long(ContentGenerationHints? hints);
    long Long(decimal? min, decimal? max);
}
