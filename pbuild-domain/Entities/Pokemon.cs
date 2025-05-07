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

    public int HP { get; set; }

    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int SP_Attack { get; set; }
    public int SP_Defense { get; set; }

    [ForeignKey("Team")]
    public int TeamId { get; set; }

    [JsonIgnore]
    public Team? Team { get; set; }
}
}