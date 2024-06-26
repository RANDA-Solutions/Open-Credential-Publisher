﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infotekka.ND.ClrExtract.CLR;
using Newtonsoft.Json;

namespace Infotekka.ND.ClrExtract
{
    public static class Convert
    {
        public static ClrRoot ClrFromJson(string JsonData) {
            return JsonConvert.DeserializeObject<ClrRoot>(JsonData);
        }

        public static string JsonFromClr(ClrRoot ClrData) {
            return JsonConvert.SerializeObject(ClrData, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        public static ClrRoot GenerateClr(Guid IssuerId, Guid RecipientId, ITranscriptData Transcript, ICourseData[] Courses) {
            Guid clrId = Guid.NewGuid();
            var recipient = new RecipientType() {
                Type = "id",
                Identity = $"urn:uuid:{RecipientId}"
            };
            DateTime issuedOn = DateTime.UtcNow;

            var publisher = new PublisherType() {
                ID = $"urn:uuid:{IssuerId}",
                Name = Transcript.SchoolName,
                SourcedId = Transcript.StateSchoolId,
                Telephone = Transcript.SchoolPhone,
                Address = new AddressType() {
                    StreetAddress = (Transcript.SchoolAddress1 + " " + Transcript.SchoolAddress2).Trim(),
                    AddressRegion = Transcript.SchoolState,
                    AddressLocality = Transcript.SchoolCity,
                    PostalCode = Transcript.SchoolZip,
                    addressCountry = "USA"
                }
                //ndt:schoolInfo
            };

            List<AssertionType> coreAssertions = new List<AssertionType>();

            //Graduation
            if (Transcript.Graduated) {
                coreAssertions.Add(new AssertionType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    IssuedOn = (DateTime)Transcript.GraduationDate,
                    Recipient = recipient,
                    Achievement = new AchievementType() {
                        ID = $"urn:uuid:{Guid.NewGuid()}",
                        Name = "Graduation",
                        Issuer = publisher,
                        TypeOfAchievement = AchievementTypes.Diploma,
                        Description = "High School Graduation",
                        Requirement = new RequirementType() {
                            ID = $"urn:uuid{Guid.NewGuid()}",
                            Narrative = "Completion of graduation requirements"
                        }
                    }
                });
            }

            //Civics test
            if (Transcript.CivicsTest != null) {
                coreAssertions.Add(new AssertionType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    IssuedOn = issuedOn,
                    Recipient = recipient,
                    Achievement = new AchievementType() {
                        ID = $"urn:uuid:{Guid.NewGuid()}",
                        Name = "Civics Test",
                        Issuer = publisher,
                        TypeOfAchievement = AchievementTypes.Achievement,
                        Description = "Civics Test",
                        Requirement = new RequirementType() {
                            ID = $"urn:uuid{Guid.NewGuid()}",
                            Narrative = "Completion of civics test"
                        }
                    }
                });
            }

            //GPAs and Class Rank
            foreach (var g in Transcript.GPAs) {
                Guid resultId = Guid.NewGuid();
                var uwGpa = new AssertionType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    IssuedOn = issuedOn,
                    Recipient = recipient,
                    Achievement = new AchievementType() {
                        ID = $"urn:uuid:{Guid.NewGuid()}",
                        Name = g.Description,
                        Issuer = publisher,
                        TypeOfAchievement = AchievementTypes.Achievement,
                        Description = g.Description,
                        ResultDescriptions = new ResultDescriptionType[] {
                                new ResultDescriptionType() {
                                    ID = $"urn:uuid:{resultId}",
                                    Name = g.Description,
                                    ResultType = g.GPA != null ? "GradePointAverage" : "ext:Rank"
                                }
                            },
                        Tags = new string[] {
                                g.GPA != null ? "gpa" : "rank"
                            }
                    },
                    Results = new ResultType[] {
                            new ResultType() {
                                ResultDescription = $"urn:uuid:{resultId}",
                                Value = g.GPA != null ? String.Format("{0:0.0000}", g.GPA) : $"{g.ClassRank} of {g.ClassSize}"
                            }
                        }
                };
                coreAssertions.Add(uwGpa);
            }

            //Standard College Admission GPA
            Guid collegeGpaId = Guid.NewGuid();
            coreAssertions.Add(new AssertionType() {
                ID = $"urn:uuid:{Guid.NewGuid()}",
                IssuedOn = issuedOn,
                Recipient = recipient,
                Achievement = new AchievementType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    Name = "Standard College Admission GPA",
                    Issuer = publisher,
                    TypeOfAchievement = AchievementTypes.Achievement,
                    Description = "Based on a 4.0 Scale - Total GPA Points times potential credit, summed, and divided by sum of Potential Credids.",
                    ResultDescriptions = new ResultDescriptionType[] {
                        new ResultDescriptionType() {
                            ID = $"urn:uuid:{collegeGpaId}",
                            Name = "Standard College Admission GPA",
                            ResultType = "GradePointAverage"
                        }
                    },
                    Tags = new string[] {
                        "gpa"
                    }
                },
                Results = new ResultType[] {
                    new ResultType() {
                        ResultDescription = $"urn:uuid:{collegeGpaId}",
                        Value = String.Format("{0:0.0000}", Transcript.GpaWeightedCalc)
                    }
                }
            });

            //NDUS GPA
            Guid ndusGpaId = Guid.NewGuid();
            coreAssertions.Add(new AssertionType() {
                ID = $"urn:uuid:{Guid.NewGuid()}",
                IssuedOn = issuedOn,
                Recipient = recipient,
                Achievement = new AchievementType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    Name = "NDUS Core GPA",
                    Issuer = publisher,
                    TypeOfAchievement = AchievementTypes.Achievement,
                    Description = "NDUS Core GPA",
                    ResultDescriptions = new ResultDescriptionType[] {
                        new ResultDescriptionType() {
                            ID = $"urn:uuid:{ndusGpaId}",
                            Name = "NDUS Core GPA",
                            ResultType = "GradePointAverage"
                        }
                    },
                    Tags = new string[] {
                        "gpa"
                    }
                },
                Results = new ResultType[] {
                    new ResultType() {
                        ResultDescription = $"urn:uuid:{ndusGpaId}",
                        Value = String.Format("{0:0.0000}", Transcript.NdusGpa)
                    }
                }
            });

            //Enrollment ?
            if (Transcript.SchoolEntryDate != null) {
                coreAssertions.Add(new AssertionType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    IssuedOn = (DateTime)Transcript.SchoolEntryDate,
                    Recipient = recipient,
                    Achievement = new AchievementType() {
                        ID = $"urn:uuid:{Guid.NewGuid()}",
                        Name = "Enrollment",
                        Issuer = publisher,
                        TypeOfAchievement = AchievementTypes.SchoolEnrollment,
                        Description = "Enrollment Date"
                    }
                });
            }

            //Exit ?
            if (Transcript.SchoolExitDate != null) {
                coreAssertions.Add(new AssertionType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    IssuedOn = (DateTime)Transcript.SchoolExitDate,
                    Narrative = Transcript.WithdrawalReason,
                    Recipient = recipient,
                    Achievement = new AchievementType() {
                        ID = $"urn:uuid:{Guid.NewGuid()}",
                        Name = "Exit",
                        Issuer = publisher,
                        TypeOfAchievement = AchievementTypes.SchoolExit,
                        Description = "Exit Date / Reason"
                    }
                });
            }

            //NDUS Core Course Credit Totals
            var coreCredits = new AssertionType() {
                ID = $"urn:uuid:{Guid.NewGuid()}",
                Achievement = new AchievementType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    Name = "NDUS Core Course Credit Totals",
                    Issuer = publisher,
                    TypeOfAchievement = AchievementTypes.Achievement,
                    ResultDescriptions = null,
                    Tags = new string[] {
                        "credits"
                    }
                },
                Recipient = recipient,
                IssuedOn = issuedOn,
                Results = null
            };
            Dictionary<string, decimal> CreditsEarnedBySubjectArea = Courses.Where(w => w.CoreCourse).GroupBy(g => g.StateDescriptiveCode).ToDictionary(k => k.Key, v => v.Sum(s => s.CreditsReceived));
            List<ResultDescriptionType> coreCreditResultDesc = new List<ResultDescriptionType>();
            List<ResultType> coreCreditResults = new List<ResultType>();
            foreach (string key in CreditsEarnedBySubjectArea.Keys) {
                Guid coreCreditId = Guid.NewGuid();
                coreCreditResultDesc.Add(new ResultDescriptionType() {
                    ID = $"urn:uuid:{coreCreditId}",
                    Name = key,
                    ResultType = "ext:Credits"
                });
                coreCreditResults.Add(new ResultType() {
                    ResultDescription = $"urn:uuid:{coreCreditId}",
                    Value = string.Format("{0:0.000}", CreditsEarnedBySubjectArea[key])
                });
            }
            coreCredits.Achievement.ResultDescriptions = coreCreditResultDesc.ToArray();
            coreCredits.Results = coreCreditResults.ToArray();
            coreAssertions.Add(coreCredits);

            //High School Course Credit Totals
            Dictionary<string, decimal> CreditsEarnedByGrade = Courses.GroupBy(g => g.GradeLevel).ToDictionary(k => k.Key, v => v.Sum(s => s.CreditsReceived));
            var creditTotals = new AssertionType() {
                ID = $"urn:uuid:{Guid.NewGuid()}",
                Achievement = new AchievementType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    Name = "High School Course Credit Totals",
                    Issuer = publisher,
                    TypeOfAchievement = AchievementTypes.Achievement,
                    ResultDescriptions = null,
                    Tags = new string[] {
                        "credits"
                    }
                },
                Recipient = recipient,
                IssuedOn = issuedOn,
                Results = null
            };
            List<ResultDescriptionType> creditDesc = new List<ResultDescriptionType>();
            List<ResultType> creditResults = new List<ResultType>();
            Guid creditId;
            foreach (string g in CreditsEarnedByGrade.Keys) {
                creditId = Guid.NewGuid();
                creditDesc.Add(new ResultDescriptionType() {
                    ID = $"urn:uuid:{creditId}",
                    Name = $"{g}th",
                    ResultType = "ext:Credits"
                });
                creditResults.Add(new ResultType() {
                    ResultDescription = $"urn:uuid:{creditId}",
                    Value = String.Format("{0:0.000}", CreditsEarnedByGrade[g])
                });
            }
            //Total Earned
            decimal creditsEarned = CreditsEarnedByGrade.Sum(v => v.Value);
            creditId = Guid.NewGuid();
            creditDesc.Add(new ResultDescriptionType() {
                ID = $"urn:uuid:{creditId}",
                Name = "Total Earned",
                ResultType = "ext:Credits"
            });
            creditResults.Add(new ResultType() {
                ResultDescription = $"urn:uuid:{creditId}",
                Value = String.Format("{0:0.000}", creditsEarned)
            });
            //Total Attempted
            decimal creditsAttempted = Courses.Sum(s => s.CreditsAttempted);
            creditId = Guid.NewGuid();
            creditDesc.Add(new ResultDescriptionType() {
                ID = $"urn:uuid:{creditId}",
                Name = "Total Attempted",
                ResultType = "ext:Credits"
            });
            creditResults.Add(new ResultType() {
                ResultDescription = $"urn:uuid:{creditId}",
                Value = String.Format("{0:0.000}", creditsAttempted)
            });
            creditTotals.Achievement.ResultDescriptions = creditDesc.ToArray();
            creditTotals.Results = creditResults.ToArray();
            coreAssertions.Add(creditTotals);

            //Build CLR
            var clr = new ClrRoot() {
                Context = "https://contexts.ward.guru/clr_v1p0.jsonld",  //???
                ID = $"urn:uuid:{clrId}",
                Name = "Student Transcript",
                Partial = true,
                IssuedOn = issuedOn,
                Learner = new LearnerType() {
                    ID = $"urn:uuid:{RecipientId}",
                    Name = $"{Transcript.FirstName} {Transcript.LastName}",
                    SourcedId = Transcript.SourcedID.ToString(), //NOTE: Not sure this is correct?
                    StudentId = Transcript.StateStudentId,
                    Telephone = Transcript.StudentPhone,
                    Address = new AddressType() {
                        StreetAddress = (Transcript.StudentAddress1 + " " + Transcript.StudentAddress2).Trim(),
                        AddressRegion = Transcript.StudentState,
                        AddressLocality = Transcript.StudentCity,
                        PostalCode = Transcript.StudentZip,
                        addressCountry = "USA"
                    }
                    //ndt:studentInfo
                },
                Publisher = publisher,
                Assertions = coreAssertions
                    .Union(Courses.Select(s => courseAssertion(s, publisher, recipient)))
                    .ToArray()
            };

            return clr;
        }

        private static AssertionType courseAssertion(ICourseData Course, PublisherType Publisher, RecipientType Recipient) {
            var ca = new AssertionType() {
                ID = $"urn:uuid:{Guid.NewGuid()}",
                Recipient = Recipient,
                CreditsEarned = Course.CreditsReceived,
                Term = Course.TermName,
                IssuedOn = Course.DateAwarded,
                Achievement = new AchievementType() {
                    ID = $"urn:uuid:{Guid.NewGuid()}",
                    Name = Course.CourseTitle,
                    Issuer = Publisher,
                    TypeOfAchievement = AchievementTypes.Course,
                    CreditsAvailable = Course.CreditsAttempted,
                    FieldOfStudy = Course.StateSubjectDesc,
                    Level = Course.GradeLevel,
                    ResultDescriptions = null,
                    Tags = null
                    //ndt:courseInfo
                },
                Results = null
            };

            Guid resultId = Guid.NewGuid();
            ca.Results = new ResultType[] {
                new ResultType() {
                    ResultDescription = $"urn:uuid:{resultId}",
                    Value = Course.LetterGrade
                }
            };
            ca.Achievement.ResultDescriptions = new ResultDescriptionType[] {
                new ResultDescriptionType() {
                    ID = $"urn:uuid:{resultId}",
                    Name = "Term Grade",
                    ResultType = "LetterGrade"
                }
            };

            List<string> tags = new List<string>();
            if (Course.DualCredit) {
                tags.Add("DC");
            }
            if (Course.ApCourse) {
                tags.Add("AP");
            }
            if (Course.CoreCourse) {
                tags.Add("Core");
            }
            if (Course.CteCourse) {
                tags.Add("CTE");
            }

            ca.Achievement.Tags = tags.ToArray();

            return ca;
        }

        private static class AchievementTypes
        {
            public const string Diploma = "Diploma";
            public const string Achievement = "Achievement";
            public const string Course = "Course";
            public const string SchoolEnrollment = "ext:SchoolEnrollment";
            public const string SchoolExit = "ext:SchoolExit";
        }
    }
}