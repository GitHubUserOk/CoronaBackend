using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System;
using Microsoft.SqlServer.Server;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CoronaBackend.Data.Models
{
    public class Logs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [MaxLength(10)]
        public string? Level { get; set; }

        [MaxLength]
        public string? Message { get; set; }

        [MaxLength]
        public string? StackTrace { get; set; }

        [MaxLength]
        public string? Exception { get; set; }

        [MaxLength(255)]
        public string? Logger { get; set; }

        [MaxLength(255)]
        public string? Url { get; set; }
    }
}
