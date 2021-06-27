using PM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Controllers
{
    public class DTO
    {
    }
    public class ProjectDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<JobDTO> Jobs { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectDTO()
        {

        }
        public ProjectDTO(Project Project)
        {
            this.Id = Project.Id;
            this.Name = Project.Name;
            this.Jobs = Project.Jobs?.Select(j => new JobDTO(j)).ToList();
            this.StartDate = Project.StartDate;
            this.EndDate = Project.EndDate;
        }
    }

    public class JobDTO
    {
        public long ProjectId { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public List<DependencyJobDTO> DependencyJobIds { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public JobDTO() { }
        public JobDTO(Job Job)
        {
            this.ProjectId = Job.ProjectId;
            this.Id = Job.Id;
            this.Name = Job.Name;
            this.DependencyJobIds = Job.DependencyJobIds?.Select(d => new DependencyJobDTO(d)).ToList();
            this.StartDate = Job.StartDate;
            this.EndDate = Job.EndDate;
        }
    }
    
    public class DependencyJobDTO
    {
        public long JobId { get; set; }
        public string DependencyType { get; set; }
        public int Space { get; set; }

        public DependencyJobDTO() { }
        public DependencyJobDTO(DependencyJob DependencyJob) {
            this.JobId = DependencyJob.JobId;
            this.DependencyType = DependencyJob.DependencyType;
            this.Space = DependencyJob.Space;
        }
    }
}
