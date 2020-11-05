using System;

namespace FindAllBonuses
{
    /// <summary>
    /// Global API's /maps response object.
    /// </summary>
    public class MapModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int filesize { get; set; }
        public bool validated { get; set; }
        public int difficulty { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
        public int approved_by_steamdid64 { get; set; }
        public string workshop_url { get; set; }
        public string download_url { get; set; }
    }
}
