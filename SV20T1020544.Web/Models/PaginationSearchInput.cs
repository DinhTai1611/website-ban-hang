using SV20T1020544.DomainModels;

namespace SV20T1020544.Web.Models
{
    public class PaginationSearchInput
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 0;
        public string SearchValue { get; set; } = "";

        public int RowCount { get; set; }

        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    return 1;
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                    c += 1;
                return c;
            }
        }
    }

    /// <summary>
    /// đầy vào sử dụng tìm kiếm mặt hàng 
    /// </summary>

    public class ProductSearchInput : PaginationSearchInput
    {
        public List<Product>? Data { get; set; }
        public int CategoryID { get; set; } = 0;
        public int SupplierID { get; set; } = 0;
        public int CustomerID { get; set; } = 0;

        public string deliveryProvince { get; set; } = "";
    }
}
