using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.Entities;
using PM.Service;

namespace PM.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        ProjectService ProjectService = new ProjectService();
        
        [Route("get-project"),HttpPost]
        public ActionResult<ProjectDTO> GetProject([FromBody]ProjectDTO ProjectDTO)
        {
            Project Project = ProjectService.GetProject(ProjectDTO.Id);
            if (Project == null)
            {
                return BadRequest();
            }
            return new ProjectDTO(Project);
        }

        [Route("create-project"), HttpPost]
        public ActionResult<ProjectDTO> CreateProject([FromBody] ProjectDTO ProjectDTO)
        {
            Project Project = ProjectService.CreateProject(ProjectDTO.Id, ProjectDTO.Name);

            return new ProjectDTO(Project);
        }

        [Route("get-job"), HttpPost]
        public ActionResult<JobDTO> GetJob([FromBody] JobDTO JobDTO)
        {
            Job Job = ProjectService.GetJob(JobDTO.Id);
            if (Job == null)
            {
                return BadRequest();
            }
            return new JobDTO(Job);
        }

        [Route("create-job"), HttpPost]
        public ActionResult<JobDTO> CreateJob([FromBody] JobDTO JobDTO)
        {
            Job Job = ConvertDTOToEntity(JobDTO);
            Job = ProjectService.CreateJob(Job);
            return new JobDTO(Job);
        }


        private Job ConvertDTOToEntity (JobDTO JobDTO)
        {
            Job Job = new Job();
            Job.ProjectId = JobDTO.ProjectId;
            Job.Id = JobDTO.Id;
            Job.Name = JobDTO.Name;
            Job.DependencyJobIds = JobDTO.DependencyJobIds?.Select(p => new DependencyJob {
                JobId = p.JobId, DependencyType = p.DependencyType, Space = p.Space
            }).ToList();
            Job.StartDate = JobDTO.StartDate;
            Job.EndDate = JobDTO.EndDate;
            return Job;
        }
    }
}
