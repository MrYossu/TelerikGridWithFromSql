using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Pixata.Extensions;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace TelerikGridWithFromSql.Helpers;

public static class TelerikGridHelper {
  public static async Task<(int, string, List<SqlParameter>)> GetData<T>(this GridReadEventArgs args, DbContext context, string tableName, string defaultColumnForSort, ListSortDirection defaultSort = ListSortDirection.Ascending) where T : class =>
    await GetData<T>(args, context, tableName, defaultColumnForSort, [], defaultSort);

  public static async Task<(int, string, List<SqlParameter>)> GetData<T>(this GridReadEventArgs args, DbContext context, string tableName, string defaultColumnForSort, List<TelerikGridFilterOptions> extraFilters, ListSortDirection defaultSort = ListSortDirection.Ascending) where T : class {
    List<SqlParameter> values = [];

    // Set up SQL for filtering
    string sqlFilters = "";
    string sqlFilterConjunction = "";
    int n = 0;

    // First handle the grid's built-in filters
    foreach (CompositeFilterDescriptor cfd in args.Request.Filters.Cast<CompositeFilterDescriptor>()) {
      foreach (FilterDescriptor fd in cfd.FilterDescriptors.Cast<FilterDescriptor>()) {
        AddValue(values, fd.Member, fd.Operator, fd.Value, n);
        sqlFilters += AddSql(fd.Member, fd.Operator, n, sqlFilterConjunction);
        sqlFilterConjunction = " and";
        n++;
      }
    }

    // Now add in any extra filters
    foreach (TelerikGridFilterOptions option in extraFilters) {
      AddValue(values, option.Member, option.Operator, option.Value, n);
      sqlFilters += AddSql(option.Member, option.Operator, n, sqlFilterConjunction);
      sqlFilterConjunction = " and";
      n++;
    }

    if (!string.IsNullOrWhiteSpace(sqlFilters)) {
      sqlFilters = $" where {sqlFilters}";
    }

    // Paging
    values.Add(new("@Skip", args.Request.Skip));
    values.Add(new("@PageSize", args.Request.PageSize));

    // SQL for sorting
    string sqlSort = $" order by {defaultColumnForSort} {(defaultSort == ListSortDirection.Ascending ? "asc" : "desc")}";
    if (args.Request.Sorts.Any()) {
      SortDescriptor sortDescriptor = (args.Request.Sorts.First())!;
      sqlSort = $" order by {sortDescriptor.Member} " + (sortDescriptor.SortDirection == ListSortDirection.Ascending ? "asc" : "desc");
    }

    // Assemble the final SQL
    string sql = $"select * from {tableName}{sqlFilters} {sqlSort} offset (@Skip) rows fetch next (@PageSize) rows only";

    // Dump the SQL and values
    //Console.WriteLine("\n\n\n");
    //Console.WriteLine("Parameters");
    //foreach (SqlParameter pair in values) {
    //  Console.WriteLine($"  {pair.ParameterName}: {pair.Value}");
    //}
    //Console.WriteLine(sql);

    // Get the data and the total number of rows that match the filters
    args.Data = await context.Set<T>().FromSqlRaw(sql, values.ToArray()).ToObservableCollectionAsync();
    values.Remove(values.Single(v => v.ParameterName == "@Skip"));
    values.Remove(values.Single(v => v.ParameterName == "@PageSize"));
    int matchingRows = await context.Database.SqlQueryRaw<int>($"select count(*) as Value from {tableName}{sqlFilters}", values.ToArray()).SingleAsync();
    args.Total = matchingRows;

    // Return the filter SQL in case the calling code wants to use it (eg to show some totals)
    return (matchingRows, sqlFilters, values);
  }

  private static void AddValue(List<SqlParameter> parameters, string member, FilterOperator op, object value, int n) =>
    parameters.Add(new($"@{member}{n}", op == FilterOperator.Contains
      ? $"%{value}%"
      : value));

  private static string AddSql(string member, FilterOperator op, int n, string sqlFilterConjunction) =>
    $"{sqlFilterConjunction} {member}" + op switch {
      FilterOperator.IsEqualTo => $"=@{member}{n}",
      FilterOperator.Contains => $" like @{member}{n}",
      FilterOperator.IsGreaterThan => $">@{member}{n}",
      FilterOperator.IsGreaterThanOrEqualTo => $">=@{member}{n}",
      FilterOperator.IsLessThan => $"<@{member}{n}",
      FilterOperator.IsLessThanOrEqualTo => $"<=@{member}{n}",
      _ => throw new Exception($"Unknown operator: {op}")
    };
}

public record TelerikGridFilterOptions(string Member, object Value, FilterOperator Operator);