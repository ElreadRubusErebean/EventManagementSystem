using EventManagmentSystem.Models.DbModel;

namespace EventManagmentSystem.Models.ViewModel
{
    public class AdminViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Event> Events { get; set; }
    }
}
