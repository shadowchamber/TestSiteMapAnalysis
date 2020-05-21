using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TestSiteMapAnalysis.Models
{
    public class SitemapItemResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecGuid { get; set; }

        public Guid SitemapResultRefRecGuid { get; set; }

        public string ItemUrl { get; set; }

        public double RespponseTimeMS { get; set; }

        public virtual SitemapResult SitemapResult { get; set; }
    }
}
