using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace CoronaBackend.Data.Models
{
    public class Visitor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }
        [Column(TypeName = "smalldatetime")]
        [SwaggerSchema(ReadOnly = true)]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }
        [Required]
        [MinLength(1), MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        [Column(TypeName = "date")]
        [SwaggerSchema(Format = "date")]
        public DateOnly BirthDate { get; set; }
        [Required]
        [MinLength(1), MaxLength(1000)]
        public string QrCodeString { get; set; }

    }
}
