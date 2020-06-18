using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VT.Models.Entities
{
    public class UserToTorrent : BaseEntity
    {
        public int DownloaderId { get; set; }
        public int TorrentId { get; set; }

        [ForeignKey("DownloaderId")]
        public User Downloader { get; set; }

        [ForeignKey("TorrentId")]
        public Torrent Torrent { get; set; }
    }
}
