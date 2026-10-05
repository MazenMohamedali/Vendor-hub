using System;
using System.Collections.Generic;
using System.Text;

namespace VendorHub.Application.Common.Models
{
    public sealed record Error(string code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");
    }
}
