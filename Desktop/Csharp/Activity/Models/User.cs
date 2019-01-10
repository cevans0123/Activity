using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Activity.Models
{
    public class User
    {
        [Key]
        
        public int UserId {get; set;}
        
        [Display(Name = "First Name")]
        [Required(ErrorMessage="First name required.")]
        [MinLength(2, ErrorMessage="Must be at least 2 characters in length")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage="non-numeric characters")]
        public string  FirstName {get; set;}
        [Display(Name = "Last Name")]
        [Required(ErrorMessage="Last name required.")]
        [MinLength(2, ErrorMessage="Must be at least 2 characters in length")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage="non-numeric characters")]
        public string  LastName {get; set;}
        [Display(Name = "Email")]
        [Required(ErrorMessage="Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string  Email {get; set;}
        [Display(Name = "Password")]
        [Required(ErrorMessage="Password required.")]
        [MinLength(8, ErrorMessage="Must be at least 8 characters in length")]
        [DataType(DataType.Password)]
        public string  Password {get; set;}
        [Display(Name = "ConfirmPW")]
        [Required(ErrorMessage="required.")]
        [NotMapped]
        [Compare("Password", ErrorMessage="Confirm password must match Password")]
        [DataType(DataType.Password)]
        public string  ConfirmPW {get; set;}
 

    }
}