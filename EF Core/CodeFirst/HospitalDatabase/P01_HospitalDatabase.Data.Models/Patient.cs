﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {

        public int PatientId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public virtual ICollection<Visitation> Visitations { get; set; }

        public virtual ICollection<PatientMedicament> Prescriptions { get; set; }
        public virtual ICollection<Diagnose> Diagnoses { get;set; }

    }
}
