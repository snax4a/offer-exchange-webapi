namespace FSH.WebApi.Domain.Common.Contracts;

public interface IAuditableEntity : ICreatedOnInformation
{
    public Guid CreatedBy { get; set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}