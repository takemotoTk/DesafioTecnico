using System.Security.Cryptography;
using System.Text;
using VaccinationCardManagement.Application.ExtensionMethods;
using VaccinationCardManagement.Application.Models;
using VaccinationCardManagement.Domain.Adapter;

namespace VaccinationCardManagement.Application.Services.User;

public class UserService
{
    private readonly IVaccinationCardManagementRepository _repo;
    public UserService(IVaccinationCardManagementRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> AddUserAsync(string name, string email, string password)
    {
        var user = await _repo.FindOne<Domain.Entities.User>(c => c.Email.Equals(email));

        if (user == null)
        {
            var result = await _repo.Add(new Domain.Entities.User
            {
                Name = name,
                Email = email,
                Password = EncryptText(password)
            });

            return result.Id;
        }
        else {
            throw new Exception("This user already exists");
        }
    }

    public async Task<Tuple<UserModel?, bool>> CheckUserValidAsync(string email, string password)
    {
        UserModel? userModel = null;
        bool response = false;
        var result = await _repo.FindOne<Domain.Entities.User>(c=> 
            c.Email.Equals(email) &&
            c.Password.Equals(EncryptText(password)
        ));

        if (result != null) 
        {
            userModel = result.Map<UserModel>();
            response = true;
        }

        return Tuple.Create<UserModel?, bool>(userModel, response);
    }

    private string EncryptText(string text)
    {
        using (var sha256Hash = SHA256.Create())
        {
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
