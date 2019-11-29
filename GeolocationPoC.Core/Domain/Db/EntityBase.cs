using System.ComponentModel.DataAnnotations;

namespace GeolocationPoC.Core.Domain.Db
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}
