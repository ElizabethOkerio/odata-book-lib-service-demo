using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibService.Models
{
    public static class BookLibEdmModel
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Book>("Books");
            builder.EntitySet<Author>("Authors");
            builder.EntitySet<User>("Users");

            return builder.GetEdmModel();
        }
    }
}
