using Model.Domain.Enum;

namespace Model.Domain
{
    public class UserRequestUpdateModel 
    {
        public int Id { get; set; }
        public IsActive IsActive { get; set; }
    }
}
