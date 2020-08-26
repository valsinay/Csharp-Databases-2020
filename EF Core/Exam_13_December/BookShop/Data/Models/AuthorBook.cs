using BookShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BookShop.Data
{
  public  class AuthorBook
    {
        [Key]
        [ForeignKey("Author")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [Key]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
