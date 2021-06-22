using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.Models.Commits
{
    public class CommitInListViewModel
    {
        public string Id { get; set; }

        public string Repository { get; set; }

        public string Description { get; set; }

        public string CreatedOn { get; set; }
    }
}
