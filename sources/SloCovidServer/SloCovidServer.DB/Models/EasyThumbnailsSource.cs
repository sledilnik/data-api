using System;
using System.Collections.Generic;

#nullable disable

namespace SloCovidServer.DB.Models
{
    public partial class EasyThumbnailsSource
    {
        public EasyThumbnailsSource()
        {
            EasyThumbnailsThumbnails = new HashSet<EasyThumbnailsThumbnail>();
        }

        public int Id { get; set; }
        public string StorageHash { get; set; }
        public string Name { get; set; }
        public DateTime Modified { get; set; }

        public virtual ICollection<EasyThumbnailsThumbnail> EasyThumbnailsThumbnails { get; set; }
    }
}
