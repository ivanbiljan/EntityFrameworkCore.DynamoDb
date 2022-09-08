using DynamoDb.Linq.Extensions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DynamoDb.Linq;

public class DynamoDbTypeMappingSource : TypeMappingSource
{
    public class DynamoDbTypeMapping : CoreTypeMapping
    {
        public DynamoDbTypeMapping(
            Type clrType,
            ValueComparer? comparer = null,
            ValueComparer? keyComparer = null)
            : base(
                new CoreTypeMappingParameters(
                    clrType,
                    converter: null,
                    comparer,
                    keyComparer))
        {
        }

        protected DynamoDbTypeMapping(CoreTypeMappingParameters parameters)
            : base(parameters)
        {
        }

        public override CoreTypeMapping Clone(ValueConverter? converter)
            => new DynamoDbTypeMapping(Parameters.WithComposedConverter(converter));
    }

    public DynamoDbTypeMappingSource(TypeMappingSourceDependencies dependencies) : base(dependencies)
    {
    }

    protected override CoreTypeMapping? FindMapping(in TypeMappingInfo mappingInfo)
    {
        if (mappingInfo.ClrType is null)
        {
            throw new InvalidOperationException("ClrType is null");
        }
        
        var clrType = mappingInfo.ClrType!;
        if (clrType.IsPrimitive())
        {
            return new DynamoDbTypeMapping(clrType);
        }

        return null;
    }
}