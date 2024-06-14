using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Entities.Models;
namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "Suç ve Ceza", Price = 100 },
                new Book { Id = 2, Title = "Yer Altından Notlar", Price = 100 },
                new Book { Id = 3, Title = "Tutunamayanlar", Price = 100 }
            );
        }
    }
}
