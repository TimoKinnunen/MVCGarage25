using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGarage25.Models
{
    /// <summary>
    /// Members of garage with or without parked vehicles in garage
    /// </summary>
    //[DisplayColumn("FullName", "FullName", false)]
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required, Index("IX_MemberFullName", 1, IsUnique = true)]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required, Index("IX_MemberFullName", 2, IsUnique = true)]
        [StringLength(100)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Full name")]
        public string FullName { get { return LastName + ", " + FirstName; } }

        [Display(Name = "Member's parked vehicles")]
        virtual public List<Vehicle> ParkedVehicles { get; set; }
    }
}