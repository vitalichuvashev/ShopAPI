using Shop.Domain;
using Shop.Infrastructure.Interfaces;

namespace Shop.Infrastructure
{
    public class OrderRepository: IOrderRepository
    {
        private readonly DatabaseContext _dbContext;
        public OrderRepository(DatabaseContext dbContext) => _dbContext = dbContext;

        public int AddNew(Order newOrder)
        {
            this._dbContext.Add(newOrder);
            return this.Save();
        }
        public int Update(Order order)
        {
            this._dbContext.Update(order);
            return this.Save();
        }
        private int Save()
        {
            try
            {
                return this._dbContext.SaveChanges();
            }
            catch { throw new DatabaseException(); }
        }
        /// <summary>
        /// Loads order from database by ID
        /// </summary>
        /// <param name="id">Order id</param>
        /// <returns>returns order or null if order didn't found</returns>
        public Order? LoadByID(Guid id)
        {
            return this._dbContext.Orders.FirstOrDefault(order => order.ID == id);
        }
    }
}
