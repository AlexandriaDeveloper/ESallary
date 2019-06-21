namespace API.Helper {

    public static class EmployeeATMFileConst {
        public static string EmployeeCode {
            get { return "EmpATMCode"; }
        }
        public static string Active {
            get { return "Active"; }
        }
    }
    public static class EmployeeBankFileConst {
        public static string EmployeeCode {
            get { return "EmpBankCode"; }
        }
        public static string BankCode {
            get { return "BankCode"; }
        }
        public static string BankName {
            get { return "BankName"; }
        }
        public static string BranchCode {
            get { return "BranchCode"; }
        }
        public static string BranchName {
            get { return "BranchName"; }
        }
        public static string Active {
            get { return "Active"; }
        }

    }
    public static class PaymentOrderConst {
        public static string OrderBankName {
            get { return "OrderBankName"; }
        }
        public static string OrderBranchName {
            get { return "OrderBranchName"; }
        }
        public static string OrderAccountNum {
            get { return "OrderAccountNum"; }
        }
        public static string Active {
            get { return "Active"; }
        }

    }
    public static class PersonalPostConst {
        public static string PersonalPostAddress {
            get { return "PersonalPostAddress"; }
        }
        public static string PersonalPostPhone {
            get { return "PersonalPostPhone"; }
        }
        public static string Active {
            get { return "Active"; }
        }

    }

    public static class PaymentTypeConst {
        public static string ATM {
            get { return "2-اخرى بطاقات حكومية"; }
        }
        public static string Bank {
            get { return "3-مرتب تحويلات بنكية"; }
        }
        public static string PaymentOrder {
            get { return "أمر دفع"; }
        }
        public static string PersonalPost {
            get { return "حواله بريديه أشخاص"; }
        }
        public static string InternalPost {
            get { return "حواله صراف"; }
        }
    }
    public static class SheetHeader {
        public static string[] Names {
            get { return new string[] { "الموظف", "الإسم", "الأسم", "الاسم", "اسم الموظف" }; }
        }
        public static string[] Codes {
            get { return new string[] { "كود", "كود الموظف", "رقم الموظف بجهته الأصلية", "الكود" }; }
        }
        public static string[] Net {
            get { return new string[] { "المرتب", "صافي المبلغ", "صافى المبلغ", "الصافي", "الصافى" }; }
        }

    }
    public enum AccountStatus {
        Credit,
        Debit,
        None
    }

    public enum FileState{
        New,
        Saved,
        Edited,
        Deleted
    }
}