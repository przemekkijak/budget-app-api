using System.Dynamic;
using System.Globalization;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace BudgetApp.Core.Features.Transactions.Commands;

public class ImportTransactions : IRequest
{
    public IBrowserFile ImportedFile { get; init; }
}

public class ImportTransactionsHandler : IRequestHandler<ImportTransactions>
{
    public async Task Handle(ImportTransactions request, CancellationToken cancellationToken)
    {
        await using var streamData = request.ImportedFile.OpenReadStream(cancellationToken: cancellationToken);
        using var reader = new StreamReader(streamData);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        await csv.ReadAsync();
        csv.ReadHeader();
        var results = new List<ExpandoObject>();
        
        while (await csv.ReadAsync())
        {
            results.Add(csv.GetRecord<dynamic>());
        }
        
        Console.WriteLine("test");
    }
}