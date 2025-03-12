namespace OrderAPIV1.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Cancelled
    }
    public class Order
    {
        public int OrderId { get; set; }
        public string OrderName { get; set; }
        public string OrderDescription { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime OrderDate { get; set; }        
        public long OrderAmount { get; set; }
    }
}
