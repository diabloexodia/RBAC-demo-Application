namespace jwtvalidator.Services.Interfaces
{
    public interface IuserService
    {

        public  Task<bool> validate(string username, string password);   

    }
}
