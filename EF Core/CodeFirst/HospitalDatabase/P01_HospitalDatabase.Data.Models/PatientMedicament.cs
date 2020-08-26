using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data.Models
{
    public class PatientMedicament
    {
     
        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
