using Microsoft.EntityFrameworkCore;
using ProductGallary.Models;

namespace ProductGallary.Reposatories
{
    public class BillReposatory : IReposatory<Bill>
    {
        private readonly Context context;

        public BillReposatory(Context _context)
        {
            context = _context;
        }
        public bool Delete(Guid Id)
        {
            try
            {
                // get Order
                Bill bill = this.GetById(Id);

                // check if order is found
                if (bill == null) return false;
                // remove order from Memory
                this.context.Bills.Remove(bill);
                // Save Changes in the Database
                this.context.SaveChanges();


            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        // This Used To GetAll Bills
        public List<Bill> GetAll()
        {
            List<Bill> bills = this.context.Bills.
                                Include(bill=>bill.Order).
                                ToList();

            // return orders
            return bills;
        }

        //this Used To Get Order By its Id
        public Bill GetById(Guid id)
        {
            Bill bill = context.Bills.
                        Include(bill => bill.Order ).
                        FirstOrDefault(o => o.Id == id);

            return bill;
        }

        // This Is Used To Add Order in The Database
        public bool Insert(Bill item)
        {
            try
            {
                if (item == null)
                    throw new Exception("Bill Is Null");
                //insert order
                this.context.Bills.Add(item);
                this.context.SaveChanges();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool Update(Guid Id, Bill item)
        {
            try
            {
                Bill bill = this.GetById(Id);
                // check if order is found
                if (bill == null) return false;

                //update Data;
                bill.Price= item.Price;
                bill.OrderId = item.OrderId;
              

                // update DataBase

                this.context.SaveChanges();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
