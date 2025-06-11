using Microsoft.EntityFrameworkCore;

namespace Greeting.api.Model
{
    public class Greetings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Greeting { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
