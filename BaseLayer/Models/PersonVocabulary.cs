using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    [Table("PeopleVocabularies")]
    public class PersonVocabulary
    {
        [Key]
        [NotMapped]
        public int Key { set; get; }
        public Int16 PersonId { get; set; }
        [ForeignKey("PersonId")]
        public Person Person { get; set; }
        public Int16 VocabularyId { get; set; }
        [ForeignKey("VocabularyId")]
        public Vocabulary Vocabulary { get; set; }
    }
}
