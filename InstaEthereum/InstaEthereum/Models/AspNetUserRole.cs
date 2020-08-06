using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InstaEthereum.Models
{
    [Table("aspnetuserroles")]
    public class AspNetUserRole 
    {
        [Key]
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}