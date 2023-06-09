using System;

namespace LogAppLibrary
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Borrower { get; set; }
        public int BorrowerQuantity { get; set; }

        public ItemModel() { }
        public ItemModel(string item_name, decimal quantity)
        {
            int quantity_val = Convert.ToInt32(Math.Round(quantity));
            Quantity = quantity_val;

            ItemName = item_name;
        }
        public ItemModel(string item_name, string quantity, string borrower, string borrowerQuantity)
        {
            int quantity_value = 0;
            int.TryParse(quantity, out quantity_value);
            Quantity = quantity_value;

            int borrowerQuantity_value = 0;
            int.TryParse(borrower, out borrowerQuantity_value);
            BorrowerQuantity = quantity_value;

            ItemName = item_name;
            Borrower = borrower;
        }
    }
}
