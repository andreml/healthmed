using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthMed.Application.ViewModels
{
    public class DoctorViewModel
    {
        public ICollection<Doctor> Doctors { get; set; } = default!;
    }

    public class Doctor
    {
        public string Name { get; set; }
        public string Crm { get; set; }
        public string Email { get; set; }
    }
}
