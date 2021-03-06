using Model.Domain.Enum;
using Model.Domain.Entities;

namespace Model.Domain.Responses
{
    public class UserResponse : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
        public IsActive IsActive { get; set; }
        public int? Age { get; set; }
    }
}
