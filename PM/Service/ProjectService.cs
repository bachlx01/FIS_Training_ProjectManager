using PM.Entities;
using PM.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PM.Service
{
    public class ProjectService
    {
        ProjectRepository ProjectRepository = new ProjectRepository();
        public Project GetProject(long ProjectId)
        {
            return ProjectRepository.GetProject(ProjectId);
        }

        public Project CreateProject(long ProjectId, string ProjectName)
        {
            return ProjectRepository.CreateProject(ProjectId, ProjectName);
        }

        public Job GetJob(long JobId)
        {
            return ProjectRepository.GetJob(JobId);
        }

        public Job CreateJob(Job Job)
        {
            Job NewJob = ProjectRepository.CreateJob(Job);

            return NewJob;
        }
    }
}
