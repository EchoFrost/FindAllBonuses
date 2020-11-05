using System;

namespace FindAllBonuses
{
    /// <summary>
    /// Global API's /record_filters response object.
    /// </summary>
    public class RecordFilterModel
    {
        public int id { get; set; }
        public int map_id { get; set; }
        public int stage { get; set; }
        public int mode_id { get; set; }
        public int tickrate { get; set; }
        public bool has_teleports { get; set; }
        public DateTime created_on { get; set; }
        public DateTime updated_on { get; set; }
    }
}
