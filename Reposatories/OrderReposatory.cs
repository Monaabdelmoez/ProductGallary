using Microsoft.EntityFrameworkCore;
using ProductGallary.Models;

namespace ProductGallary.Reposatories
{
    // this Class Represent Crud Operations On Order
    public class OrderReposatory : IReposatory<Order>
    {
        private readonly Context context;

        public OrderReposatory(Context _context)
        {
            context = _context;
        }

        // this used to delete Order
        public bool Delete(Guid Id)
        {
            try
            {
                // get Order
                Order order = this.GetById(Id);

                // check if order is found
                if (order == null) return false;
                // remove order from Memory
                this.context.Orders.Remove(order);
                // Save Changes in the Database
                this.context.SaveChanges();
                

            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        // This Used To GetAll Orders
        public List<Order> GetAll()
        {
            List<Order> orders = this.context.Orders.
                                Include(or => or.User).
                                Include(or => or.Bill).
                                Include(or => or.Cart).
                                ToList();

            // return orders
            return orders;
        }

        //this Used To Get Order By its Id
        public Order GetById(Guid id)
        {
            Order order = context.Orders.Include(or=>or.User).
                                Include(or => or.Bill).
                                Include(or => or.Cart).
                                FirstOrDefault(o => o.Id == id);

            return order;
        }

        // This Is Used To Add Order in The Database
        public bool Insert(Order item)
        {
            try
            {
                if (item == null)
                    throw new Exception("Order Is Null");
                //insert order
                this.context.Orders.Add(item);
                this.context.SaveChanges();

            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Update(Guid Id, Order item)
        {
            try
            {
                Order order = this.GetById(Id);
                // check if order is found
                if (order == null) return false;

                //update Data;
                order.OrderDate = item.OrderDate;
                order.DeliveryDate = item.DeliveryDate;
                order.IsCanceled = item.IsCanceled;

                // update DataBase

                this.context.SaveChanges();

            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
