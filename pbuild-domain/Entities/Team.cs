using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pbuild_domain.Entities
{
   public class Team
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    
    
    public List<Pokemon> Pokemons { get; set; } = new List<Pokemon>();
}
}