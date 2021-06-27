using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.DBSimulator
{
    public class DbSimulator
    {
        public static List<ProjectDAO> Projects = new List<ProjectDAO>() {
            new ProjectDAO {Id = 0001, Name = "First Project", StartDate = new DateTime(2021,6,10), EndDate = new DateTime(2021,6,20)}
        };

        public static List<JobDAO> Jobs = new List<JobDAO>() {
            new JobDAO {Id = 1111, Name = "Job 1", DependencyType = "Single", StartDate = new DateTime(2021,6,10), EndDate = new DateTime(2021,6,15)} ,
            new JobDAO {Id = 1112, Name = "Job 2", DependencyType = "FS", DependencyJobId = 1111, StartDate = new DateTime(2021,6,15), EndDate = new DateTime(2021,6,20)} ,
        };
        public static List<ProjectJobMappingDAO> ProjectJobMappings = new List<ProjectJobMappingDAO>()
        {
            new ProjectJobMappingDAO{ProjectId = 0001, JobId = 1111},
            new ProjectJobMappingDAO{ProjectId = 0001, JobId = 1112}
        };

    }
    public class ProjectDAO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class JobDAO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DependencyType { get; set; }
        public long  DependencyJobId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class ProjectJobMappingDAO
    {
        public long ProjectId { get; set; }
        public long JobId { get; set; }
    }

}
