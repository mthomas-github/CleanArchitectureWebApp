using System.Data;
using ThirdPartyFreight.Application.Abstractions.Clock;
using ThirdPartyFreight.Application.Abstractions.Data;
using ThirdPartyFreight.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Newtonsoft.Json;

namespace ThirdPartyFreight.Infrastructure.OutBox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages");

        using IDbConnection connection = sqlConnectionFactory.CreateConnection();
        using IDbTransaction transaction = connection.BeginTransaction();

        IReadOnlyList<OutboxMessageResponse> outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (OutboxMessageResponse outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    JsonSerializerSettings)!;

                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(
                    caughtException,
                    "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);

                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(
        IDbConnection connection,
        IDbTransaction transaction)
    {
        string sql = $"""
                      SELECT TOP {_outboxOptions.BatchSize} Id, Content
                      FROM TPF_OutboxMessages WITH (UPDLOCK, ROWLOCK)
                      WHERE ProcessedOnUtc IS NULL
                      ORDER BY OccurredOnUtc
                      """;

        IEnumerable<OutboxMessageResponse> outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(
                       sql,
                       transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(
        IDbConnection connection,
        IDbTransaction transaction,
        OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = @"
            UPDATE TPF_OutboxMessages
            SET ProcessedOnUtc = @ProcessedOnUtc,
                Error = @Error
            WHERE Id = @Id";

        await connection.ExecuteAsync(
            sql,
            new
            {
                outboxMessage.Id,
                ProcessedOnUtc = dateTimeProvider.UtcNow,
                Error = exception?.ToString()
            },
            transaction: transaction);
}

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}
