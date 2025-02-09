using FluentMigrator;

namespace ApiDbmTeste.FluentMigrator
{
    [Migration(20250207)]
    public class CreateProductTable : Migration
    {
        public override void Up()
        {
          Create.Table("Products")
         .WithColumn("Id").AsInt32().PrimaryKey().Identity()
         .WithColumn("Nome").AsString(100).NotNullable()
         .WithColumn("Descricao").AsString(500).Nullable()
         .WithColumn("Preco").AsDecimal().NotNullable()
         .WithColumn("DataCadastro").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);
            
        }
        public override void Down()
        {
            Delete.Table("Products");
        }

    }
}
