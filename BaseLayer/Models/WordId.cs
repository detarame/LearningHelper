using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    [Table("Words")]
    public class WordId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public Int16 Id { get; set; }
        // related table
        public List<Word> wordValue { get; set; }
        public WordId()
        {
            wordValue = new List<Word>();
        }
    }
}
