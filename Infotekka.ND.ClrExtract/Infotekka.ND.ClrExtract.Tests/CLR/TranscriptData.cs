using System;
using System.Collections.Generic;
using System.Text;

namespace Infotekka.ND.ClrExtract.Tests.CLR
{
    class TranscriptData : ITranscriptData
    {
        public string SchoolName { get; set; }

        public string SchoolAddress1 { get; set; }

        public string SchoolAddress2 { get; set; }

        public string SchoolState { get; set; }

        public string SchoolCity { get; set; }

        public string SchoolZip { get; set; }

        public string StateSchoolId { get; set; }

        public string SchoolPhone { get; set; }

        public bool Graduated { get; set; }

        public DateTime? GraduationDate { get; set; }

        public DateTime? CivicsTest { get; set; }

        public IGpaClassRankData[] GPAs { get; set; }

        public decimal GpaWeightedCalc { get; set; }

        public decimal? NdusGpa { get; set; }

        public DateTime? SchoolEntryDate { get; set; }

        public DateTime? SchoolExitDate { get; set; }

        public string WithdrawalReason { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string SourcedId { get; set; }

        public string StateStudentId { get; set; }

        public string StudentPhone { get; set; }

        public string StudentAddress1 { get; set; }

        public string StudentAddress2 { get; set; }

        public string StudentState { get; set; }

        public string StudentCity { get; set; }

        public string StudentZip { get; set; }

        public string Principal { get; set; }

        public IAddressData SchoolAddress { get; set; }

        public IIdentityData[] SchoolIds { get; set; }

        public string MiddleName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public IIdentityData[] StudentIds { get; set; }

        public string HigherEdId { get; set; }

        public IAddressData StudentAddress { get; set; }

        public string DistrictName { get; set; }

        public string DistrictStateId { get; set; }

        public IIdentityData DistrictIds { get; set; }

        public IAddressData DistrictAddress { get; set; }
    }
}
