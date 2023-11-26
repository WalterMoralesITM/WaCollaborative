namespace WaCollaborative.Shared.Entities
{
    public class UserCollaborativeDemand
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public string UserId { get; set; }

        public CollaborativeDemand? CollaborativeDemand { get; set; }
        public int CollaborativeDemandId { get; set; }
    }
}