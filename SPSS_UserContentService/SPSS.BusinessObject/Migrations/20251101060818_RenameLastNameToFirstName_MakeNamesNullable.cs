using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPSS.BusinessObject.Migrations
{
	public partial class RenameLastNameToFirstName_MakeNamesNullable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			// 1) đổi tên cột LastName -> FirstName
			migrationBuilder.RenameColumn(
				name: "LastName",
				table: "Users",
				newName: "FirstName");

			// 2) làm SurName nullable
			migrationBuilder.AlterColumn<string>(
				name: "SurName",
				table: "Users",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);

			// 3) làm FirstName (trước là LastName) nullable
			migrationBuilder.AlterColumn<string>(
				name: "FirstName",
				table: "Users",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			// Khi rollback: đặt NOT NULL trở lại. Lưu ý: nếu có giá trị NULL trong cột,
			// Down sẽ fail trừ khi bạn đặt defaultValue hoặc cập nhật dữ liệu trước.
			migrationBuilder.AlterColumn<string>(
				name: "FirstName",
				table: "Users",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.AlterColumn<string>(
				name: "SurName",
				table: "Users",
				type: "nvarchar(100)",
				maxLength: 100,
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(100)",
				oldMaxLength: 100,
				oldNullable: true);

			migrationBuilder.RenameColumn(
				name: "FirstName",
				table: "Users",
				newName: "LastName");
		}
	}
}
