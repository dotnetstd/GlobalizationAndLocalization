using Microsoft.AspNetCore.Routing.Constraints;

namespace LocalizerCustom.Models
{
    public class CultureRouteConstraint : RegexRouteConstraint
    {
        public CultureRouteConstraint()
            : base(@"^[a-zA-Z]{2}(\-[a-zA-Z]{2})?$") { }
    }
}
