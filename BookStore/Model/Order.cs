namespace BookStore.Model
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Amount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
