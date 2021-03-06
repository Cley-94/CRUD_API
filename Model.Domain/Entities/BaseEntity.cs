using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Model.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
    }
}
