namespace WaCollaborative.Shared.Entities
{
    public class CollaborativeDemand
    {
        public int Id { get; set; }

        public DemandType? DemandType { get; set; }
        public int DemandTypeId { get; set; }

        public EventType? EventType { get; set; }
        public int EventTypeId { get; set; }
        public Product? Product { get; set; }
        public int ProductId { get; set; }
        public ShippingPoint? ShippingPoint { get; set; }
        public int ShippingPointId { get; set; }
        public Status? Status { get; set; }
        public int StatusId { get; set; }
        public ICollection<CollaborativeDemandComponentsDetail>? CollaborativeDemandComponentsDetails { get; set; }
    }
}