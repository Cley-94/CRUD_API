using Model.Domain.Enum;

namespace Model.Domain.Models
{
    public class FilterRequestModel
    {
        public string Name { get; set; }
        public IsActive IsActive { get; set; }
    }
}
