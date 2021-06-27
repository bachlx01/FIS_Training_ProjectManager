using PM.Common;
using PM.DBSimulator;
using PM.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Repositories
{
    public class ProjectRepository
    {
        public Project GetProject (long Id)
        {
            Project Project = DbSimulator.Projects.Where(p => p.Id == Id).Select(p => new Project()
            {
                Id = p.Id,
                Name = p.Name,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            }).FirstOrDefault();
            if (Project == null)
            {
                return Project;
            }
            List<long> JobIds = DbSimulator.ProjectJobMappings.Where(x => x.ProjectId == Id).Select(x => x.JobId).ToList();
            List<Job> Jobs = JobIds.Select(JobId => new Job()
            {
                Id = JobId,
                Name = DbSimulator.Jobs.Where(j => j.Id == JobId).FirstOrDefault().Name,
                StartDate = DbSimulator.Jobs.Where(j => j.Id == JobId).FirstOrDefault().StartDate,
                EndDate = DbSimulator.Jobs.Where(j => j.Id == JobId).FirstOrDefault().EndDate,
                DependencyJobIds = DbSimulator.Jobs.Where(j => j.Id == JobId)
                .Select(job => new DependencyJob{JobId = job.DependencyJobId,DependencyType = job.DependencyType }).ToList()
        }).ToList();

            Project.Jobs = Jobs;

            return Project;
        }

        public Job GetJob(long JobId)
        {
            Job Job = DbSimulator.Jobs.Where(j => j.Id == JobId).Select(j => new Job
            {
                Id = j.Id,
                Name = j.Name,
                StartDate = j.StartDate,
                EndDate = j.EndDate
            }).FirstOrDefault();
            if (Job == null)
            {
                return Job;
            }
            List<DependencyJob> DependencyJobIds = DbSimulator.Jobs.Where(j => j.Id == JobId).Select(j => new DependencyJob
            {
                JobId = j.DependencyJobId,
                DependencyType = j.DependencyType
            }).ToList();

            Job.DependencyJobIds = DependencyJobIds;
            return Job;
        }
        public Project CreateProject(long ProjectId, string ProjectName)
        {
            ProjectDAO ProjectDAO = DbSimulator.Projects.Where(p => p.Id == ProjectId).FirstOrDefault();
            if (ProjectDAO != null)
            {
                return new Project();
            }
            ProjectDAO = new ProjectDAO()
            {
                Id = ProjectId,
                Name = ProjectName
            };
            DbSimulator.Projects.Add(ProjectDAO);
            return GetProject(ProjectId);
        }

        public Job CreateJob(Job Job)
        {
            DateTime StartDate, EndDate;
            
            if (Job.DependencyJobIds == null)
            {
                StartDate = Job.StartDate;
                EndDate = Job.EndDate;
            }
            else
            {
                List<DateTime> StartDates = new List<DateTime>();
                List<DateTime> EndDates = new List<DateTime>();
                foreach (var DependencyJob in Job.DependencyJobIds)
                {
                    switch (DependencyJob.DependencyType)
                    {
                        case DependencyType.SS:
                            DateTime startTime = DbSimulator.Jobs.Where(j => j.Id == DependencyJob.JobId)
                                .Select(j => j.StartDate).FirstOrDefault();
                            StartDates.Add(startTime.AddDays(DependencyJob.Space));
                            break;

                        case DependencyType.SF:
                            DateTime startTime1 = DbSimulator.Jobs.Where(j => j.Id == DependencyJob.JobId)
                                .Select(j => j.StartDate).FirstOrDefault();
                            EndDates.Add(startTime1.AddDays(DependencyJob.Space));
                            break;

                        case DependencyType.FS:
                            DateTime endTime = DbSimulator.Jobs.Where(j => j.Id == DependencyJob.JobId)
                                .Select(j => j.EndDate).FirstOrDefault();
                            StartDates.Add(endTime.AddDays(DependencyJob.Space));
                            break;

                        case DependencyType.FF:
                            DateTime endTime1 = DbSimulator.Jobs.Where(j => j.Id == DependencyJob.JobId)
                                .Select(j => j.EndDate).FirstOrDefault();
                            EndDates.Add(endTime1.AddDays(DependencyJob.Space));
                            break;
                    }
                }
                StartDate = StartDates.Count < 1 ? Job.StartDate : StartDates.Select(d => d).OrderByDescending(d => d).FirstOrDefault();
                EndDate = EndDates.Count < 1 ? Job.EndDate : EndDates.Select(d => d).OrderByDescending(d => d).FirstOrDefault();
            }
            foreach (var DependencyJob in Job.DependencyJobIds)
            {
                JobDAO JobDAO = new JobDAO()
                {
                    Id = Job.Id,
                    Name = Job.Name,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    DependencyJobId = DependencyJob.JobId,
                    DependencyType = DependencyJob.DependencyType
                };
                
                DbSimulator.Jobs.Add(JobDAO);
            }
            MappingJobToProject(Job.ProjectId, Job.Id);
            UpdateProject(Job.ProjectId);
            return GetJob(Job.Id);   
        }

        public bool UpdateProject(long ProjectId)
        {
            ProjectDAO ProjectDAO = DbSimulator.Projects.Where(p => p.Id == ProjectId).FirstOrDefault();
            if (ProjectDAO == null)
            {
                return false;
            }
            List<long> JobIds = DbSimulator.ProjectJobMappings.Where(m => m.ProjectId == ProjectId).Select(j => j.JobId).ToList();

            ProjectDAO.StartDate = DbSimulator.Jobs.Where(j => JobIds.Contains(j.Id))
                .Select(j => j.StartDate).OrderBy(d => d).FirstOrDefault();
            ProjectDAO.EndDate = DbSimulator.Jobs.Where(j => JobIds.Contains(j.Id))
                .Select(j => j.EndDate).OrderByDescending(d => d).FirstOrDefault();
            return true;
        }
        private bool MappingJobToProject(long ProjectId, long JobId)
        {
            ProjectJobMappingDAO projectJobMappingDAO = new ProjectJobMappingDAO()
            {
                ProjectId = ProjectId,
                JobId = JobId
            };
            DbSimulator.ProjectJobMappings.Add(projectJobMappingDAO);
            return true;
        }
    }
}
