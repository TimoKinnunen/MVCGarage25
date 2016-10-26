using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCGarage25.Models
{
    /// <summary>
    /// Vehicles in garage
    /// </summary>
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MemberId { get; set; }

        [Display(Name = "Member")]
        virtual public Member Member { get; set; } //owner

        [Required]
        public int VehicleTypeId { get; set; }

        [Display(Name = "Vehicle type")]
        virtual public VehicleType VehicleType { get; set; } //personbil, lastbil

        [Required, Index(IsUnique = true)]
        [StringLength(100)]
        [Display(Name = "Registration number")]
        public string RegistrationNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:g}")] //g Default date & time 10/12/2002 10:11 PM
        [Display(Name = "Checked in")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime StartParkingTime { get; set; }


        [DisplayFormat(NullDisplayText = "Not checked out yet", DataFormatString = "{0:g}")] //g Default date & time 10/12/2002 10:11 PM
        [Display(Name = "Checked out")]
        public DateTime? EndParkingTime { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd\\:hh\\:mm}")]
        [Display(Name = "Parking time dd:hh:mm")]
        public TimeSpan ParkingTime
        {
            get
            {
                if (EndParkingTime == null)
                {
                    return (DateTime.Now - StartParkingTime);
                }
                else
                {
                    return ((DateTime)EndParkingTime - StartParkingTime);
                }
            }
        }

        [Display(Name = "Is checked out")]
        public bool IsCheckedOut
        {
            get
            {
                if (EndParkingTime == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Display(Name = "Parking cost per hour")]
        public int ParkingCostPerHour { get { return 60; } }//hard coded 60 SEK/hour

        [DisplayFormat(DataFormatString = "{0:c}")]
        [Display(Name = "Parking cost")]
        public int ParkingCost
        {
            get { return (int)ParkingTime.TotalMinutes * ParkingCostPerHour / 60; }

        }


        [Range(1, 10, ErrorMessage = "Value for number of wheels must be between 1 and 10.")]
        [Display(Name = "Number of wheels")]
        public int? NumberOfWheels { get; set; }

        [Display(Name = "Brand and model")]
        public string BrandAndModel { get; set; } //Saab 96,Volvo V70

        [Display(Name = "Colour")]
        public string Color { get; set; }
    }
}