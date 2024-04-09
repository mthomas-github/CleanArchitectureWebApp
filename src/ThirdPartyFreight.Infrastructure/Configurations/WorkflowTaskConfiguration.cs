using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ThirdPartyFreight.Domain.WorkflowTask;

namespace ThirdPartyFreight.Infrastructure.Configurations;

public class WorkflowTaskConfiguration : IEntityTypeConfiguration<WorkflowTask>
{
    public void Configure(EntityTypeBuilder<WorkflowTask> builder)
    {
        builder.ToTable("TPF_WorkflowTasks");

        builder.HasKey(workflowTask => workflowTask.Id);
    }
}
