using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
    public class Actv
    {        
        [Key]
        public int ActvId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage="Must be at least 2 characters in length.")]
        public string Title { get; set; }
        [DataTypeAttribute(DataType.Date)]
        public DateTime Date { get; set; }
        [DataTypeAttribute(DataType.Time)]
        public DateTime Time { get; set; }
        public int Duration { get; set; }
        public string DurationType { get; set; }
        [Required]
        [MinLength(10, ErrorMessage="Must be at least 10 characters in length.")]
        public string Description { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<Participant> Participants { get; set; }



    }
}