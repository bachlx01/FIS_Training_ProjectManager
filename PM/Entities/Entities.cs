using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Entities
{
    public class Entities
    {
    }
    public class Project
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<Job> Jobs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    public class Job
    {
        public long ProjectId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DependencyJob> DependencyJobIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class DependencyJob
    {
        public long JobId { get; set; }
        public string DependencyType { get; set; }
        public int Space { get; set; }
    }
}
