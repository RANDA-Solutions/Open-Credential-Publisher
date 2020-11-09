using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infotekka.ND.ClrExtract
{
    public interface ITranscriptData
    {
        string SchoolName { get; }

        string SchoolAddress1 { get; }

        string SchoolAddress2 { get; }

        string SchoolState { get; }

        string SchoolCity { get; }

        string SchoolZip { get; }

        string StateSchoolId { get; }

        string SchoolPhone { get; }

        bool Graduated { get; }

        DateTime? GraduationDate { get; }

        DateTime? CivicsTest { get; }  //ND only

        IGpaClassRankData[] GPAs { get; }

        /// <summary>
        /// Standard college admission GPA
        /// </summary>
        decimal GpaWeightedCalc { get; }

        decimal? NdusGpa { get; } //ND only

        DateTime? SchoolEntryDate { get; }

        DateTime? SchoolExitDate { get; }

        string WithdrawalReason { get; }

        string FirstName { get; }

        string LastName { get; }

        /// <summary>
        /// Learner Sourced ID
        /// </summary>
        Guid SourcedID { get; }

        string StateStudentId { get; }

        string StudentPhone { get; }

        string StudentAddress1 { get; }

        string StudentAddress2 { get; }

        string StudentState { get; }

        string StudentCity { get; }

        string StudentZip { get; }
    }
}
