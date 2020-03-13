namespace API.Helper {
    public class Params {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 30;
        public int PageSize {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

    }

    public class EmployeeParams<T> : Params {
        public int Id { get; set; }
        public string NationalId { get; set; } = "";
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public bool Male { get; set; } = true;
        public bool Female { get; set; } = true;
        public string Collage { get; set; } = "";
        public string Section { get; set; } = "";
        public string Department { get; set; } = "";
        public string Grade { get; set; } = "";
        public string PaymentType { get; set; } = "";
        public bool HasATM { get; set; } = true;
        // public bool HasOrder { get; set; } = true;
        public bool HasBank { get; set; } = true;
        // public bool HasPost { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }

    public class FileParams<T> : Params {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}