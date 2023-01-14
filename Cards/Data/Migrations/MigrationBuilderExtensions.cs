using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace Cards.Data.Migrations
{
	public static class MigrationBuilderExtensions
	{
		public static OperationBuilder<SqlOperation> CreateSchema(
			this MigrationBuilder migrationBuilder, string schema)
		{
			if (string.IsNullOrEmpty(schema) || !schema.All(char.IsLetterOrDigit))
				throw new ArgumentException("Invalid schema name", nameof(schema));
			return migrationBuilder.Sql($@"CREATE SCHEMA [{schema}];");
		}
	}
}