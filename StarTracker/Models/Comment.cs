using System;
using System.ComponentModel.DataAnnotations;

namespace StarTracker.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [Required(ErrorMessage = "The Comment is Required to submit")]
        [MinLength(2, ErrorMessage = "Too short")]
        public string Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relationship
        // One User : Many Comment
        public int UserId { get; set; }
        public User User { get; set; }

        // One Post : Many Comment
        public int StarId { get; set; }
        public Star Star { get; set; }
    }
}