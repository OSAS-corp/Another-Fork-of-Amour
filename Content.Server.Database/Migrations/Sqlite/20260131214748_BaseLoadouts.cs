// SPDX-FileCopyrightText: 2026
// SPDX-License-Identifier: MIT

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Content.Server.Database.Migrations.Sqlite;

[DbContext(typeof(SqliteServerDbContext))]
[Migration("20260131214748_BaseLoadouts")]
/// <inheritdoc />
public partial class BaseLoadouts : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<bool>(
            name: "is_base",
            table: "profile_role_loadout",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "overridden_groups",
            table: "profile_role_loadout",
            type: "TEXT",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "entity_name_overridden",
            table: "profile_role_loadout",
            type: "INTEGER",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "is_base", table: "profile_role_loadout");
        migrationBuilder.DropColumn(name: "overridden_groups", table: "profile_role_loadout");
        migrationBuilder.DropColumn(name: "entity_name_overridden", table: "profile_role_loadout");
    }
}
