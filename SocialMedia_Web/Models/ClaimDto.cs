namespace SportifMedia_Web.Models
{
    public class ClaimDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }
        public string UserName { get; set; }
        public string ClaimName { get; set; }
    }
}
