using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGarage25.Models
{
    /// <summary>
    /// Vehicle types that Vehicle objects can be of
    /// </summary>
    public class VehicleType
    {
        /// <summary>
        /// Id is the unique identifier for the vehicle type
        /// </summary>
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Type is the description for the vehicle type
        /// Type must be a unique string value with 3 to 30 characters
        /// </summary>
        [Required(ErrorMessage = "Type is required."),
            StringLength(30, MinimumLength = 3, ErrorMessage = "Type must be 3 to 30 characters long."),
            Index(IsUnique = true)]
        public string Type { get; set; }
    }
}