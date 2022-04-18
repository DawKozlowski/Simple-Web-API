// //Przykładowy interfejs
// public interface IWarehouseService{

//     public Task<bool> DoesProductExist(int productId);
//     public Task<bool> DoesWarehouseExist(int productId);
//     public Task<int> GetTheValidOrderId(int warehouseId, int productId, int amount, DateTime createdAt);
//     public Task<bool> CompeleteTheOrder(int orderId, int warehouseId, int productId, int amount, int createdAt);
//     public Task<double> GetTheProductPrice(int productId);
//     public Task<int> StoredProcedure(int warehouseId, int productId, int amount, DateTime createdAt);
// }

// //Przykładowy DTO
// public class WarehousePost{

//     [Required]
//     public int IdProduct { get; set; }
//     [Required]
//     public int IdWarehouse { get; set; }
//     [Required]
//     [Range(0, int.MaxValue)]
//     public int Amount { get; set; }
//     [Required]
//     public DateTime CreatedAt { get; set; }
// }

// //Przzykład sprawdzenia czy element w bazie istnieje
// public async Task<bool> DoesAnimalExist(int animalId){

//     using (var connection = new SqlConnection("connection-string"))
//     using(var command = new SqlCommand()){

//         command.Connection = connection;
//         command.CommandText = "select 1 from animal where idanimal = @1";
//         command.Parameters.AddWithValue("@1", animalId);

//         await connection.OpenAsync();

//         var dataReader = await command.ExecuteReaderAsync();
//         return dataReader.HasRows;
//     }
// }

// //Przykład zwrócenia pojedynczej wartosci
// public async Task<double> GetAnimalWeight(int animalId){

//     using (var connection = new SqlConnection("connection-string"))
//     using(var command = new SqlCommand()){

//         command.Connection = connection;
//         command.CommandText = "select weight from animal where idanimal = @1";
//         command.Parameters.AddWithValue("@1", animalId);

//         await connection.OpenAsync();

//         var weight = double.Parse((await command.ExecuteScalarAsync()).ToString());
//         return weight;
//     }
// }

// //Przykład transakcji
// public async Task UpdateAnimalInfo(int animalId, string animalName, int houseId){

//     using (var connection = new SqlConnection("connection-string"))
//     using(var command = new SqlCommand()){

//         await connection.OpenAsync();
//         var transaction = await connection.BeginTransactionAsync() as SqlTransaction;

//         try{
//             command.Connection = connection;
//             command.Transaction = transaction;
            
//             command.CommandText = "update animalHouse set idhouse = @1 where idanimal = @2";
//             command.Parameters.AddWithValue("@1", houseId);
//             command.Parameters.AddWithValue("@2", animalId);
//             await command.ExecuteNonQueryAsync();

//             command.CommandText = "update animal set animalname = @1 where idanimal = @2";
//             command.Parameters.AddWithValue("@1", animalName);
//             command.Parameters.AddWithValue("@2", animalId);
//             await command.ExecuteNonQueryAsync();

//             await transaction.CommitAsync();

//         }catch(Exception){
//             await transaction.RollbackAsync();
//             throw;
//         }  
//     }
// }

// //Wywołanie procedury składowanej
// public async Task StoredProcedure(int animalId, string animalName, int houseId){
    
//     using (var connection = new SqlConnection("connection-string"))
//     using(var command = new SqlCommand()){

//         command.Connection = connection;
//         command.CommandText = "UpdateAnimalInfo";
//         command.CommanType = System.Data.CommandType.StoredProcedure;
//         command.Parameters.AddWithValue("@idanimal", animalId);
//         command.Parameters.AddWithValue("@animalname", animalName);
//         command.Parameters.AddWithValue("@idhouse", houseId);

//         await connection.OpenAsync();
//         await command.ExecuteNonQueryAsync();
//     }
// }