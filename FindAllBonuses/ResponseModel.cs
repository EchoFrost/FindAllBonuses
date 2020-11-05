namespace FindAllBonuses
{
    /// <summary>
    /// Model for storing calculated/response data for the application user.
    /// </summary>
    public class ResponseModel
    {
        // The map's map_id.
        public int MapId { get; set; }

        // The map's name.
        public string MapName { get; set; }

        // The number of bonuses the map has.
        public int NumberOfBonuses { get; set; }
    }
}
