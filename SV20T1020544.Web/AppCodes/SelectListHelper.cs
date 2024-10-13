using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020544.BusinessLayers;

namespace SV20T1020544
{
    public static class SelectListHelper
    {
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Chọn tỉnh\thành --"
            });
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {

                    Value = item.ProvinceName.ToString(),
                    Text = item.ProvinceName
                });
            }
            return list;
        }
        public static List<SelectListItem> ListOfCategory()
        {
            List<SelectListItem> List = new List<SelectListItem>();

            List.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn Loại Hàng---"

            });

            string searchValue = "";
            int rowCount;
            foreach (var item in CommonDataService.ListOfCategory(out rowCount, 1, 0, searchValue))
            {
                List.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName
                });
            }

            return List;
        }
        public static List<SelectListItem> ListOfCustomer()
        {
            List<SelectListItem> List = new List<SelectListItem>();

            List.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn khách hàng---"

            });

            string searchValue = "";
            int rowCount;
            foreach (var item in CommonDataService.ListOfCustomers(out rowCount, 1, 0, searchValue))
            {
                List.Add(new SelectListItem()
                {
                    Value = item.CustomerID.ToString(),
                    Text = item.CustomerName
                });
            }

            return List;
        }
        public static List<SelectListItem> ListOfShipper()
        {
            List<SelectListItem> List = new List<SelectListItem>();

            List.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn người giao hàng---"

            });

            string searchValue = "";
            int rowCount;
            foreach (var item in CommonDataService.ListOfShippers(out rowCount, 1, 0, searchValue))
            {
                List.Add(new SelectListItem()
                {
                    Value = item.ShipperID.ToString(),
                    Text = item.ShipperName
                });
            }

            return List;
        }
        public static List<SelectListItem> ListOfSupplier()
        {
            List<SelectListItem> List = new List<SelectListItem>();

            List.Add(new SelectListItem()
            {
                Value = "0",
                Text = "---Chọn Nhà Cung Cấp---"

            });

            string searchvalue = "";
            int rowCount = 0;
            foreach (var item in CommonDataService.ListOfSuppliers(out rowCount, 1, 0, searchvalue))
            {
                List.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName
                });
            }

            return List;
        }
    }
}
