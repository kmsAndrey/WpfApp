namespace AksenovNewTeleTeth.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Direction = c.String(),
                        Color = c.String(),
                        Intensity = c.Int(nullable: false),
                        PointObjectA_Id = c.Int(),
                        PointObjectB_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PointObjects", t => t.PointObjectA_Id)
                .ForeignKey("dbo.PointObjects", t => t.PointObjectB_Id)
                .Index(t => t.PointObjectA_Id)
                .Index(t => t.PointObjectB_Id);
            
            CreateTable(
                "dbo.PointObjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MainObjects", "PointObjectB_Id", "dbo.PointObjects");
            DropForeignKey("dbo.MainObjects", "PointObjectA_Id", "dbo.PointObjects");
            DropIndex("dbo.MainObjects", new[] { "PointObjectB_Id" });
            DropIndex("dbo.MainObjects", new[] { "PointObjectA_Id" });
            DropTable("dbo.PointObjects");
            DropTable("dbo.MainObjects");
        }
    }
}
