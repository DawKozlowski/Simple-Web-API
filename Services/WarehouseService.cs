using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.services{
    public class WarehouseService : IWarehouseService
    {
        
        public async Task<bool> DoesProductExist(int productId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public async Task<bool> DoesWarehouseExist(int warehouseId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM Warehouse WHERE IdWarehouse = @1";
                command.Parameters.AddWithValue("@1", warehouseId);

                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public async Task<bool> DoesOrderExist(Order order)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM Order WHERE IdProduct = @1 AND Amount = @2 AND CreatedAt < @3";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                command.Parameters.AddWithValue("@2", order.Amount);
                command.Parameters.AddWithValue("@3", order.CreatedAt);
                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
        }

        public async Task<bool> DoesOrderDone(Order order)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = (SELECT IdOrder FROM Order WHERE IdProduct = @1 AND Amount = @2 AND CreatedAt < @3)";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                command.Parameters.AddWithValue("@2", order.Amount);
                command.Parameters.AddWithValue("@3", order.CreatedAt);
                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                return dataReader.HasRows;
            }
            
        }

         public async Task UpdateFulfilledAt(Order order)
         {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                await connection.OpenAsync();
                var transaction = await connection.BeginTransactionAsync() as SqlTransaction;

                try{
                    command.Connection = connection;
                    command.Transaction = transaction;
                    
                    command.CommandText = "update Order set FulfiedAt = NOW() where IdProduct = @1 AND Amount = @2 AND CreatedAt < @3";
                    command.Parameters.AddWithValue("@1", order.IdProduct);
                    command.Parameters.AddWithValue("@2", order.Amount);
                    command.Parameters.AddWithValue("@3", order.CreatedAt);
                    await command.ExecuteNonQueryAsync();

                    await transaction.CommitAsync();

                }catch(Exception){
                    await transaction.RollbackAsync();
                    throw;
                }  
            }
         }
        
        public async Task<int> GetIdOrder(Order order)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT IdOrder FROM Order WHERE IdProduct = @1 AND Amount = @2 AND CreatedAt < @3)";
                command.Parameters.AddWithValue("@1", order.IdProduct);
                command.Parameters.AddWithValue("@2", order.Amount);
                command.Parameters.AddWithValue("@3", order.CreatedAt);
                await connection.OpenAsync();

                var dataReader = await command.ExecuteReaderAsync();
                var IdOrder = Int32.Parse(dataReader["IdOrder"].ToString());

                return IdOrder;
            }
        }      

        public async Task<double> GetTheProductPrice(int productId)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "SELECT Price FROM product WHERE IdProduct = @1";
                command.Parameters.AddWithValue("@1", productId);

               await connection.OpenAsync();

                var price = double.Parse((await command.ExecuteScalarAsync()).ToString());
                return price;
            }
        }

         public async Task<int> InsertProduct(Order order)
         {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) values (@1, @2, @3, @4, @5, NOW()); SELECT SCOPE_IDENTITY()";
                command.Parameters.AddWithValue("@1", order.IdWarehouse);
                command.Parameters.AddWithValue("@2", order.IdProduct);
                command.Parameters.AddWithValue("@3", GetIdOrder(order));
                command.Parameters.AddWithValue("@4", order.Amount);
                var Price =order.Amount * GetTheProductPrice(order.IdProduct).Result;
                command.Parameters.AddWithValue("@5", Price);
               
                await connection.OpenAsync();

               
                var Id = Int32.Parse((command.ExecuteScalarAsync()).ToString());
                return Id;
            }
         }
        
        public async Task<int> StoredProcedure(Order order)
        {
            using (var connection = new SqlConnection("connection-string"))
            using(var command = new SqlCommand()){

                command.Connection = connection;
                command.CommandText = "AddProductToWarehouse";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdProduct", order.IdProduct);
                command.Parameters.AddWithValue("@IdWarehouse", order.IdWarehouse);
                command.Parameters.AddWithValue("@Amount", order.Amount);
                command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
                
                await connection.OpenAsync();
                var Id = Int32.Parse((command.ExecuteNonQueryAsync()).ToString());
                return Id;
            }
        }

    }
}