using System.ComponentModel.DataAnnotations;

namespace Domain.Interfaces;

public interface IHasId
{
    [Key]
    public Guid Id { get; set; }
}