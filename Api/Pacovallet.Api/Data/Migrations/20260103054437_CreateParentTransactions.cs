using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pacovallet.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateParentTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions",
                column: "ParentTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Transactions_ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions",
                column: "ParentTransactionId",
                principalSchema: "Pacovallet",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Transactions_ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ParentTransactionId",
                schema: "Pacovallet",
                table: "Transactions");
        }
    }
}
