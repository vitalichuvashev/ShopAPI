using Shop.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Interfaces
{
    public interface IOrderRepository
    {
        int AddNew(Order newOrder);
        int Update(Order order);

        Order? LoadByID(Guid id);

    }
}
