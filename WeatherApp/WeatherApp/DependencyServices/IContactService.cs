using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.DependencyServices
{
    public interface IContactService
    {
        Task<IEnumerable<PhoneContact>> GetAllContacts();
    }
}
