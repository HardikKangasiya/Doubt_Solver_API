using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StackflowAPI.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }
        public DateTime PostedAt { get; set; }

        // This is the foreign key
        [ForeignKey("PostedBy")]
        public string PostedById { get; set; }

        // Navigation property
        [JsonIgnore]
        public virtual ApplicationUser PostedBy { get; set; }
    }

}