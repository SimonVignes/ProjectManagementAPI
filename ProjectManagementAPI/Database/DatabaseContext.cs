using System;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagementAPI
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)

		{
		}

		

	}
}

