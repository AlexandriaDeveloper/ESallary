using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace API.DTOS {
    public class BankDto {
        
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public int AccountMinLength { get; set; }
        public int AccountMaxLength { get; set; }
        public ICollection<BankBranchDto> Branches { get; set; }
        public BankDto()
        {
            this.Branches = new Collection<BankBranchDto>();
            
        }

    }

    public class BankBranchDto {
        public int? BranchCode { get; set; }
        public string BranchName { get; set; }

        public int BankId { get; set; }
    }
}