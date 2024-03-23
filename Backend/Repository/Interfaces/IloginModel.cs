using jwtvalidator.Models;
namespace jwtvalidator.Repository.Interfaces
{
    public interface IloginModel
    {
        public loginModelForm getuser(string username);
    }
}
