using System;
using System.Collections.Generic;
using System.Text;

namespace VT.Business.DTOs
{
    public class UserToTorrentDto : BaseDto
    {
        public UserDto Downloader { get; set; }
        public TorrentDto Torrent { get; set; }
    }
}
