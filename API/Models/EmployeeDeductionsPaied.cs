namespace API.Models
{
    public class EmployeeDeductionsPaied
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int? PaymentFileId { get; set; }
        public float? Sanctions { get; set; }
        public float? EWazifi { get; set; }
        public float? EMokamel { get; set; }
        public float? MWazifi { get; set; }
        public float? MMokamel { get; set; }
        public int? Vacation { get; set; }
        public int Mokafaa { get; set; }

        public Employee Employee { get; set; }
    }
}