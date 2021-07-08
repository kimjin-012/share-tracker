using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StarTracker.Models
{
    public class Star
    {
        [Key]
        public int StarId { get; set; }

        [Required(ErrorMessage="is required")]
        [Display(Name = "Title")]
        public string StarName { get; set; }

        [Required(ErrorMessage = "is required")]
        [Display(Name="Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage="When?...")]
        public DateTime Time {get;set;}
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        /**********************************************************************
        Relationship properties: foreign keys and navigation properties. 
        Navigation properties are null unless .Include is used. 
        "Object reference not set to an instance of an object"
        will be the error if accessed but not included. 
        **********************************************************************/

        public int UserId { get; set; }
        public User CreatedBy { get; set; }
        public List<Comment> Comments { get; set; }
    }
}