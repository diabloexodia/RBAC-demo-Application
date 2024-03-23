using jwtvalidator.Services.Interfaces;
using jwtvalidator.Repository.Interfaces;
using jwtvalidator.Models;
namespace jwtvalidator.Services
{
    
    public class userService : IuserService
    {
        private readonly IloginModel ll;

        public userService(IloginModel l)

        {
            ll = l;
        }
        public async Task<bool> validate(string username, string password)
        {
           loginModelForm userForm = new loginModelForm();
           
            userForm = ll.getuser(username);
            if (userForm == null) { 
            
                return false;
            }
            else if(userForm.password == password)
            {
                return true;
            }
            return false;
        }
    }
}
