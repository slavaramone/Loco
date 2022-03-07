using Microsoft.EntityFrameworkCore.Migrations;
using System.Linq;

namespace Management.Db.Migrations
{
	public partial class ActivateLocos : Migration
    {
		private readonly ManagementDbContext _db;

		public ActivateLocos(ManagementDbContext db)
		{
			_db = db;
		}

		protected override void Up(MigrationBuilder migrationBuilder)
        {
			var locos = _db.Locos.ToList();
			foreach (var loco in locos)
			{
				loco.IsActive = true;
			}
			_db.SaveChanges();
		}

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
