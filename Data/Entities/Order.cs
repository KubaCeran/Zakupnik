namespace Zakupnik.Data.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
