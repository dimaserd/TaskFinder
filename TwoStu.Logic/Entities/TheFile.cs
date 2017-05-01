using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class TheFile
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] FileData { get; set; }

        public string FileMymeType { get; set; }

        [ForeignKey("FromTaskSolutionVersion")]
        public string TaskSolutionId { get; set; }

        [JsonIgnore]
        public virtual TaskSolution FromTaskSolutionVersion { get; set; }
    }
}
