using SV20T1020544.DomainModels;

namespace SV20T1020544.Web.Models
{
    public class ProductEditModel
    {
        public Product data { get; set; } = new Product();

        public List<ProductAttribute> ListProductAttribute { get; set; }

        public List<ProductPhoto> ListProductPhoto { get; set; }

    }
}
