using System;
using Model.Domain.Enum;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Model.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [MinLength(3, ErrorMessage = "Este campo deve conter pelo menos 3 caracteres!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Sex { get; set; }

        private IsActive isActive = IsActive.Ativo;

        [JsonIgnore]
        public IsActive IsActive { get { return this.isActive; } set { this.isActive = value; } }

        [JsonIgnore]
        public int Age { get; set; }

        public int CalculateAge()
        {
            var today = DateTime.Today;
            var age = today.Year - BirthDate?.Year;

            if (BirthDate?.Date > today.AddYears((int)-age)) age--;

            return (int)age;
        }
    }
}
