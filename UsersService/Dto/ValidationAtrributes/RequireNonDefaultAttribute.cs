using System.ComponentModel.DataAnnotations;

namespace UsersService.Dto.ValidationAtrributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RequireNonDefaultAttribute : ValidationAttribute
    {
        public RequireNonDefaultAttribute()
        : base("This field requires a non-default value.") { }
        
        public override bool IsValid(object? value)
        {
            if(value == null) return false;

            var type = value.GetType();

            return !Equals(value, Activator.CreateInstance(Nullable.GetUnderlyingType(type) ?? type));
        }
    }
}
