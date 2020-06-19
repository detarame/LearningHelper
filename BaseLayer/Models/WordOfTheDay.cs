using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("WordsOfTheDay")]
    public class WordOfTheDay
    {
        [Key]
        [Column("Id")]
        public Int16 Id { get; set; }
        public Int16 WordId { get; set; }
        [ForeignKey("WordId")]
        public virtual Word WordReference { get; set; }
        public Int16 PersonId { get; set; }
        [ForeignKey("PersonId")]
        public virtual Person Language { get; set; }
        [Column("AddingDate")]
        public DateTime AddingDate { get; set; }

    }
}
