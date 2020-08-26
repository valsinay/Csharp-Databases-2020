﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VaporStore.Data.Models
{
  public  class GameTag
    {
        [Required]
        [Key]
        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }

        public Game Game { get; set; }

        [Required]
        [Key]
        [ForeignKey(nameof(Tag))]
        public  int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
