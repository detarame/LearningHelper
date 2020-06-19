using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("VocabularyWords")]
    public class VocabularyWord
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("Id")]
        public Int16 Key { set; get; }
        public Int16 WordId { get; set; }
        [ForeignKey("WordId")]
        public Word Word { get; set; }
        public Int16 VocabularyId { get; set; }
        [ForeignKey("VocabularyId")]
        public Vocabulary Vocabulary { get; set; }
    }
}
