using System.ComponentModel.DataAnnotations;

namespace Activity.Models
{
    public class Participant
    {
        [Key]
        public int ParticipantId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ActvId { get; set; }
        public Actv Actv { get; set; }
    }
}