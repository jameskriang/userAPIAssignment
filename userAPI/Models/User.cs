using Swashbuckle.AspNetCore.Annotations;

namespace userAPI.Models
{
    public class User
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
