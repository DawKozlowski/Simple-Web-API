using System;
using System.Threading.Tasks;
using cw5.Models;

namespace cw5.services{
    public interface IWarehouseService{

        public Task<bool> DoesProductExist(int productId);
        public Task<bool> DoesWarehouseExist(int warehouseId);
        public Task<bool> DoesOrderExist(Order order);
        public Task<bool> DoesOrderDone(Order order);
        public Task UpdateFulfilledAt(Order order);
        public Task<int> InsertProduct(Order order);
        public Task<double> GetTheProductPrice(int productId);
        public Task<int> StoredProcedure(Order order);
    }
}