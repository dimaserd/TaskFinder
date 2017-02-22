

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoStu.Logic.Entities
{
    public class SubjectSection
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        [Required]
        [ForeignKey("FromSubject")]
        public int SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject FromSubject { get; set; }
    }
}
