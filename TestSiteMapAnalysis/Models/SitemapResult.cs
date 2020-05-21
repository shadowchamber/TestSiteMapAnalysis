using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestSiteMapAnalysis.Models
{
    public class SitemapResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecGuid { get; set; }

        public string Url { get; set; }

        public DateTime AnalysisDate { get; set; }

        public ICollection<SitemapItemResult> ItemResults { get; set; }
    }
}
