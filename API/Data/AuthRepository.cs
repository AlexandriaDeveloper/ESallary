namespace API.Data {
    public class AuthRepository {
        private readonly DataContext _context;
        public AuthRepository (DataContext context) {
            this._context = context;

        }
    }
}