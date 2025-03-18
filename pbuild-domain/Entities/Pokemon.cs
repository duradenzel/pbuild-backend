using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace pbuild_domain.Entities
{
   public class Pokemon
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }

    [ForeignKey("Team")]
    public int TeamId { get; set; }

    [JsonIgnore]
    public Team? Team { get; set; }
}
}