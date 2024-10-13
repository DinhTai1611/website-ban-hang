using Dapper;
using SV20T1020544.DomainModels;
using System.Data;

namespace SV20T1020544.DataLayers.SQLServer
{
    public class OrderDAL : _BaseDAL, IOrderDAL
    {
        public OrderDAL(string connectingString) : base(connectingString)
        {
        }

        public int Add(Order data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Orders(CustomerId,OrderTime,Deliveryprovince, Deliveryaddress,EmployeeId, Status) 
                            values(@customerID, getdate(),@deliveryProvince, @deliveryAddress,@employeeID, @status); 
                            select @@identity";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    CustomerId = data.CustomerID,
                    Deliveryprovince = data.DeliveryProvince,
                    Deliveryaddress = data.DeliveryAddress,
                    EmployeeId = data.EmployeeID,
                    Status = Constants.ORDER_INIT
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: CommandType.Text);
            }
            return id;

        }

        public int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            int count = 0;
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())

            {
                var sql = @"select count(*)  
                            from Orders as o 
                            left join Customers as c on o.CustomerID = c.CustomerID left join Employees as e on o.EmployeeID = e.EmployeeID 
                            left join Shippers as s on o.ShipperID = s.ShipperID 
                            where (@status = 0 or o.Status = @status)  and (@fromTime is null or o.OrderTime >= @fromTime) and (@toTime is null or o.OrderTime <= @toTime) 
                            and (@searchValue = N''  
                            or c.CustomerName like @searchValue  
                            or e.FullName like @searchValue  
                            or s.ShipperName like @searchValue)";
                //TODO: Hoàn chỉnh code còn thiếu 
                var parameters = new
                {
                    Status = status,
                    FromTime = fromTime,
                    ToTime = toTime,
                    SearchValue = searchValue
                };

                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
            }
            return count;

        }

        public bool Delete(int orderID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from OrderDetails where OrderID = @OrderID;  delete from Orders where OrderID = @OrderID";
                //TODO: Hoàn chỉnh code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public bool DeleteDetail(int orderID, int productID)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from OrderDetails  
                            where OrderID = @OrderID and ProductID = @ProductID";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID,
                    ProductID = productID
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Order? Get(int orderID)
        {
            Order? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select o.*, 
                            c.CustomerName, 
                            c.ContactName as CustomerContactName, 
                            c.Address as CustomerAddress, 
                            c.Phone as CustomerPhone, 
                            c.Email as CustomerEmail, 
                            e.FullName as EmployeeName, 
                            s.ShipperName, 
                            s.Phone as ShipperPhone  
                            from Orders as o 
                            left join Customers as c on o.CustomerID = c.CustomerID
                            left join Employees as e on o.EmployeeID = e.EmployeeID left join Shippers as s on o.ShipperID = s.ShipperID 
                            where o.OrderID = @OrderID";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID
                };
                data = connection.QueryFirstOrDefault<Order>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public OrderDetail? GetDetail(int orderID, int productID)
        {
            OrderDetail? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit 
                            from OrderDetails as od 
                            join Products as p on od.ProductID = p.ProductID  where od.OrderID = @OrderID and od.ProductID = @ProductID";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID,
                    ProductID = productID
                };
                data = connection.QueryFirstOrDefault<OrderDetail>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }

        public IList<Order> List(int page = 1, int pageSize = 0, int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "")
        {
            List<Order> list = new List<Order>();
            if (!string.IsNullOrEmpty(searchValue))
                searchValue = "%" + searchValue + "%";
            using (var connection = OpenConnection())
            {
                var sql = @"with cte as 
                            ( 
                            select row_number() over(order by o.OrderTime desc) as RowNumber,  o.*, 
                            c.CustomerName, 
                            c.ContactName as CustomerContactName, 
                            c.Address as CustomerAddress, 
                            c.Phone as CustomerPhone, 
                            c.Email as CustomerEmail, 
                            e.FullName as EmployeeName, 
                            s.ShipperName, 
                            s.Phone as ShipperPhone  
                            from Orders as o 
                            left join Customers as c on o.CustomerID = c.CustomerID left join Employees as e on o.EmployeeID = e.EmployeeID 
                            left join Shippers as s on o.ShipperID = s.ShipperID 
                            where (@status = 0 or o.Status = @status) 
                            and (@fromTime is null or o.OrderTime >= @fromTime) and (@toTime is null or o.OrderTime <= @toTime) 
                            and (@searchValue = N''  
                            or c.CustomerName like @searchValue  
                            or e.FullName like @searchValue  
                            or s.ShipperName like @searchValue) 
                            ) 
                            select * from cte  
                            where (@pageSize = 0)  
                            or (RowNumber between (@page - 1) * @pageSize + 1 and @page * @pageSize)  order by RowNumber";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    Page = page,
                    PageSize = pageSize,
                    Status = status,
                    FromTime = fromTime,
                    ToTime = toTime,
                    SearchValue = searchValue
                };
                list = connection.Query<Order>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return list;
        }

        public IList<OrderDetail> ListDetails(int orderID)
        {
            List<OrderDetail> list = new List<OrderDetail>();
            using (var connection = OpenConnection())
            {
                var sql = @"select od.*, p.ProductName, p.Photo, p.Unit 
                            from OrderDetails as od 
                            join Products as p on od.ProductID = p.ProductID  where od.OrderID = @OrderID";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                list = connection.Query<OrderDetail>(sql, new { OrderID = orderID }).ToList();
                connection.Close();
            }
            return list;
        }

        public bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails  
                            where OrderID = @OrderID and ProductID = @ProductID) 
                            update OrderDetails  
                            set Quantity = @Quantity,  
                            SalePrice = @SalePrice  
                            where OrderID = @OrderID and ProductID = @ProductID 
                            else 
                            insert into OrderDetails(OrderID, ProductID, Quantity, SalePrice)  values(@OrderID, @ProductID, @Quantity, @SalePrice)";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID,
                    ProductID = productID,
                    Quantity = quantity,
                    SalePrice = salePrice,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        public bool EditAddressDetail(int orderID, string deliveryProvince = "", string deliveryAddress = "")
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from OrderDetails  
                            where OrderID = @orderID) 
                            update Orders  
                            set DeliveryProvince = @deliveryProvince,  
                            DeliveryAddress = @deliveryAddress  
                            where OrderID = @orderID";
                //TODO: Hoàn chỉnh phần code còn thiếu 
                var parameters = new
                {
                    OrderID = orderID,
                    DeliveryProvince = deliveryProvince,
                    DeliveryAddress = deliveryAddress,
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
        public bool Update(Order data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Orders 
                            set CustomerID = @CustomerID, 
                            OrderTime = @OrderTime, 
                            DeliveryProvince = @DeliveryProvince, 
                            DeliveryAddress = @DeliveryAddress, 
                            EmployeeID = @EmployeeID, 
                            AcceptTime = @AcceptTime, 
                            ShipperID = @ShipperID, 
                            ShippedTime = @ShippedTime, 
                            FinishedTime = @FinishedTime, 
                            Status = @Status 
                            where OrderID = @OrderID";
                //TODO: Hoàn chỉnh phần code còn thiếu  
                var parameters = new
                {
                    OrderID = data.OrderID,
                    CustomerID = data.CustomerID,
                    OrderTime = data.OrderTime,
                    DeliveryProvince = data.DeliveryProvince,
                    DeliveryAddress = data.DeliveryAddress,
                    EmployeeID = data.EmployeeID,
                    AcceptTime = data.AcceptTime,
                    ShipperID = data.ShipperID,
                    ShippedTime = data.ShippedTime,
                    FinishedTime = data.FinishedTime,
                    Status = data.Status
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
